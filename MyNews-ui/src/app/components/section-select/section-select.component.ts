import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";

import { Section } from "../../interfaces/section";
import { SectionService } from "../../services/section.service";
import { NewsService } from "../../services/news.service";
import { SectionWithNews } from "../../interfaces/section-with-news";
import { SignupComponent } from "../../auth/signup/signup.component";
import { LoginComponent } from "../../auth/login/login.component";
import { ForgotPasswordComponent } from "../../auth/forgot-password/forgot-password.component";

@Component({
    selector: 'app-section-select',
    standalone: true,
    imports: [CommonModule, LoginComponent, SignupComponent, ForgotPasswordComponent],
    templateUrl: './section-select.component.html',
    styleUrls: ['section-select.component.scss'],
})
export class SectionSelectComponent implements OnInit {
    sections: Section[] = [];
    selectedSections: number[] = [];
    sectionWithNews: SectionWithNews[] = [];
    showSections = true;
    isLoading = false;
    isAllSelected = false;
    showLogin = false;
    showSignup = false;
    showForgotPassword = false;

    constructor(
        private sectionService: SectionService,
        private newsService: NewsService
    ) { }

    ngOnInit(): void {
        // load all sections
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

    openForgotPassword() {
        this.showSignup = false;
        this.showForgotPassword = true;
    }

    backToSignup() {
        this.showForgotPassword = false;
        this.showSignup = true;
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
                next: (data) => {
                    this.sectionWithNews = data.map(sw => ({
                        sectionId: sw.sectionId,
                        sectionName: sw.sectionName,
                        news: sw.news ?? []
                    }));
                    this.isLoading = false;
                },
                error: () => {
                    this.sectionWithNews = sectionsToLoad.map(id => ({
                        sectionId: id,
                        sectionName: this.sections.find(s => s.id === id)?.name ?? 'Unknown',
                        news: []
                    }));
                    this.isLoading = false;
                }
            });
        } else {
            this.sectionWithNews.forEach((sw) => (sw.news = []));
            this.isLoading = false;
        }
    }
}