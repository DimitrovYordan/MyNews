import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";

import { NewsItem } from "../../interfaces/news-item";
import { NewsService } from "../../services/news.service";
import { AuthService } from "../../services/auth.service";

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

    constructor(private newsService: NewsService, private authService: AuthService) { }

    ngOnInit(): void {
        this.selectedSections = this.authService.getSelectedSections();

        this.fetchNews();
    }

    private fetchNews(): void {
        this.isLoading = true;

        this.newsService.getAllNews().subscribe({
            next: (data: NewsItem[]) => {
                console.log('All news fetched:', data);

                if (this.selectedSections.length > 0) {
                    this.news = data.filter(n => this.selectedSections.includes(n.sectionId));
                } else {
                    this.news = data;
                }

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