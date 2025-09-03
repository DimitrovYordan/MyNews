import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";

import { NewsItem } from "../../interfaces/news-item";
import { NewsService } from "../../services/news.service";

@Component({
    selector: 'app-news-list',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './news-list.component.html',
    styleUrls: ['./news-list.component.scss'],
})
export class NewsListComponent implements OnInit{

    news: NewsItem[] = [];
    errorMessage = '';

    constructor(private newsService: NewsService){}

    ngOnInit(): void {
        this.newsService.getAllNews().subscribe({
            next: (data) => {
                console.log('News fetched from API:', data);
                this.news = data;
            },
            error: (err) => console.error('Error fetching news', err)
        });
    }
}