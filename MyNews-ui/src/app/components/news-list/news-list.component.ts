import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";

import { NewsItem } from "../../interfaces/news-item";
import { NewsService } from "../../services/news.service";
import { AuthService } from "../../services/auth.service";
import { SectionWithNews } from "../../interfaces/section-with-news";

@Component({
    selector: 'app-news-list',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './news-list.component.html',
    styleUrls: ['./news-list.component.scss'],
})
export class NewsListComponent implements OnInit {

    news: NewsItem[] = [];
    errorMessage: string = '';
    isLoading: boolean = false;

    selectedSections: number[] = [];
    sectionsWithNews: SectionWithNews[] = [];

    constructor(private newsService: NewsService, private authService: AuthService, private router: Router) { }

    ngOnInit(): void {
        this.selectedSections = this.authService.getSelectedSections();

        this.fetchNews();
    }

    formatSectionName(name: string): string {
        return name.replace(/_/g, ' ');
    }

    goBack() {
        this.router.navigate(['/sections']);
    }

    private fetchNews(): void {
        this.isLoading = true;

        this.newsService.getNewsBySections(this.selectedSections).subscribe({
            next: (data: SectionWithNews[]) => {
                console.log('All news fetched:', data);
                this.sectionsWithNews = data;
                this.isLoading = false;
            },
            error: (err) => {
                console.error('Error fetching news', err);
                this.errorMessage = 'Failed to load news.';
                this.isLoading = false;
            }
        });
    }
}