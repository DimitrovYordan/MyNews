import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";

import { NewsItem } from "../../interfaces/news-item";
import { NewsService } from "../../services/news.service";
import { AuthService } from "../../services/auth.service";
import { SectionWithNews } from "../../interfaces/section-with-news";
import { SectionsNamesUtilsService } from "../../shared/sections-names-utils.service";
import { GroupByPipe } from "../../shared/group-by-pipe.service";

@Component({
    selector: 'app-news-list',
    standalone: true,
    imports: [CommonModule, GroupByPipe],
    templateUrl: './news-list.component.html',
    styleUrls: ['./news-list.component.scss'],
})
export class NewsListComponent implements OnInit {

    news: NewsItem[] = [];
    errorMessage: string = '';
    isLoading: boolean = false;

    selectedSections: number[] = [];
    sectionsWithNews: SectionWithNews[] = [];
    openSources: Record<string, boolean> = {};

    constructor(
        private newsService: NewsService,
        private authService: AuthService,
        private router: Router,
        public sectionName: SectionsNamesUtilsService
    ) { }

    ngOnInit(): void {
        this.selectedSections = this.authService.getSelectedSections();

        this.fetchNews();
    }

    goBack() {
        this.router.navigate(['/sections']);
    }

    toggleSourceDropdown(key: string) {
        this.openSources[key] = !this.openSources[key];
    }

    private fetchNews(): void {
        this.isLoading = true;

        this.newsService.getNewsBySections(this.selectedSections).subscribe({
            next: (data: SectionWithNews[]) => {
                this.sectionsWithNews = data;
                this.isLoading = false;
            },
            error: (err) => {
                this.errorMessage = 'Failed to load news.';
                this.isLoading = false;
            }
        });
    }
}