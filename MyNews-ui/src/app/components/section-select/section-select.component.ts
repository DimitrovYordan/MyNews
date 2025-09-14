import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SectionService } from "../../services/section.service";
import { NewsService } from "../../services/news.service";
import { Section } from "../../interfaces/section";

@Component({
    selector: 'app-section-select',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './section-select.component.html',
    styleUrls: ['section-select.component.scss'],
})
export class SectionSelectComponent implements OnInit {
    sections: Section[] = [];
    selectedSections: number[] = [];
    showSections: boolean = true;
    isLoading: boolean = false;
    isAllSelected: boolean = false;

    constructor(
        private sectionService: SectionService,
        private newsService: NewsService,
    ) { }

    ngOnInit(): void {
         this.sectionService.getSections().subscribe(data => {
            this.sections = data;
        });
    }

    toggleSelection(id: number): void {
        if (this.selectedSections.includes(id)) {
            this.selectedSections = this.selectedSections.filter(s => s !== id);
        }
        else {
            this.selectedSections.push(id);
        }
    }

    toggleAll(): void {
        if (this.isAllSelected) {
            this.selectedSections = [];
            this.isAllSelected = false;
        } else {
            this.selectedSections = this.sections.map(s => s.id);
            this.isAllSelected = true;
        }
    }

    saveSelection(): void {
        this.showSections = false;

        this.loadNews();
    }

    onCheckboxChange(section: Section, event: Event) {
        const input = event.target as HTMLInputElement;
        if (input.checked) {
            if (!this.selectedSections.includes(section.id)) {
                this.selectedSections.push(section.id)
            }
        } else {
            this.selectedSections = this.selectedSections.filter(s => s !== section.id)
        }
    }

    selectAll(): void {
        this.selectedSections = this.sections.map(s => s.id);
    }

    get selectAllButtonText(): string {
        return this.selectedSections.length === this.sections.length ? 'Unselect All' : 'Select All';
    }

    private loadNews(): void {
        this.isLoading = true;
        const sectionsToLoad = this.selectedSections.length ? this.selectedSections : this.sections.map(s => s.id);

        if (this.selectedSections.length > 0) {
            this.newsService.getNewsBySections(sectionsToLoad).subscribe({
                next: () => {
                    this.isLoading = false;
                },
                error: () => {
                    this.isLoading = false;
                }
            });
        } else {
            this.isLoading = false;
        }
    }
}