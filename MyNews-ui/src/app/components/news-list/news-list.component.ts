import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";

import { NewsService } from "../../services/news.service";
import { AuthService } from "../../services/auth.service";
import { NewsItem } from "../../interfaces/news-item";
import { SectionWithNews } from "../../interfaces/section-with-news";
import { GroupedNews } from "../../interfaces/grouped-news";
import { SectionsNamesUtilsService } from "../../shared/sections-names-utils.service";
import { UserNewsService } from "../../services/user-news.service";
import { UserSectionService } from "../../services/user-section.service";

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
    sectionsWithNews: (SectionWithNews & { groupedNews: GroupedNews[] })[] = [];

    constructor(
        private newsService: NewsService,
        private authService: AuthService,
        private userNewsService: UserNewsService,
        private userSectionService: UserSectionService,
        private router: Router,
        public sectionName: SectionsNamesUtilsService
    ) { }

    ngOnInit(): void {
        this.userSectionService.getUserSections().subscribe(userSections => {
            this.selectedSections = userSections;

            if (this.selectedSections.length === 0) {
                this.errorMessage = 'No sections selected. Please select sections first.';
                return;
            }

            this.fetchNews();
        });
    }

    goBack() {
        this.router.navigate(['/sections']);
    }

    toggleSource(sectionId: number, source: GroupedNews) {
        const isCurrentlyOpen = source.isOpen;

        this.sectionsWithNews.forEach((section) => {
            section.groupedNews.forEach((s) => {
                s.isOpen = false;
                s.openItemId = null;
            });
        });

        if (!isCurrentlyOpen) {
            source.isOpen = true;
        }
    }

    toggleNewsItem(source: GroupedNews, item: NewsItem) {
        if (source.openItemId === item.id) {
            source.openItemId = null;
        } else {
            source.openItemId = item.id;
            this.markNewsAsRead(item);
        }

        this.userNewsService.markAsClicked(item.id).subscribe();
    }

    private markNewsAsRead(item: NewsItem): void {
        this.userNewsService.markAsRead(item.id.toString()).subscribe({
            next: () => {
                item.isRead = true;
            },
            error: (err) => {
                console.log('Failed to mark news as read', err);
            }
        })
    }

    private fetchNews(): void {
        this.isLoading = true;

        this.newsService.getNewsBySections(this.selectedSections).subscribe({
            next: (data: SectionWithNews[]) => {
                if (!data || data.length === 0) {
                    this.errorMessage = 'No news available in the selected sections.';
                    this.isLoading = false;
                    return;
                }

                this.sectionsWithNews = data.map((section) => {
                    const groupedMap = section.news.reduce((acc, item) => {
                        if (!acc[item.sourceUrl]) {
                            acc[item.sourceUrl] = [];
                        }

                        acc[item.sourceUrl].push(item);
                        return acc;
                    }, {} as Record<string, NewsItem[]>);

                    const groupedNews: GroupedNews[] = Object.entries(groupedMap).map(
                        ([key, items]) => ({
                            key,
                            items,
                            isOpen: false,
                            openItemId: null
                        })
                    );

                    return { ...section, groupedNews };
                });
                console.log('Sections with news:', this.sectionsWithNews);
                this.isLoading = false;
            },
            error: () => {
                this.errorMessage = 'Failed to load news.';
                this.isLoading = false;
            }
        });
    }
}