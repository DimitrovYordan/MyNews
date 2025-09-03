import { Component, OnInit } from "@angular/core";
import { DatePipe } from "@angular/common";

import { Section } from "../../interfaces/section";
import { NewsItem } from "../../interfaces/news-item";
import { SectionService } from "../../services/section.service";
import { NewsService } from "../../services/news.service";
import { NewsListComponent } from "../news-list/news-list.component";

@Component({
    selector: 'app-section-select',
    standalone: true,
    imports: [DatePipe],
    templateUrl: './section-select.component.html',
    styleUrls: ['section-select.component.scss'],
})
export class SectionSelectComponent implements OnInit {
    sections: Section[] = [];
    selectedSections: number[] = [];
    news: NewsItem[] = [];
    showSections = true;

    constructor(
        private sectionService: SectionService, 
        private newsService: NewsService
    ) {}

    ngOnInit(): void {
        // load all sections
        this.sectionService.getSections().subscribe(data => {
            this.sections = data;
        });
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

    toggleSelection(id: number): void {
        if (this.selectedSections.includes(id)) {
            this.selectedSections = this.selectedSections.filter(s => s !== id);
        }
        else {
            this.selectedSections.push(id);
        }
    }

    toggleAll(): void {
        if(this.selectedSections.length === this.sections.length) {
            this.selectedSections = [];
        } else {
            this.selectedSections = this.sections.map(s => s.id);
        }
    }

    saveSelection(): void {
        this.showSections = false;

        this.loadNews();
    }

    get selectAllButtonText(): string {
        return this.selectedSections.length === this.sections.length ? 'Unselect All' : 'Select All';
    }

    trackById(index: number, item: any): number {
        return item.id;
    }

    private loadNews(): void {
        this.newsService.getNewsBySections(this.selectedSections).subscribe({
            next: (data) => {
                this.news = data ?? [];
            }, error: () => {
                this.news = [];
            }
        });
    }
}