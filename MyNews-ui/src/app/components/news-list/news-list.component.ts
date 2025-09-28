import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";

import { NewsService } from "../../services/news.service";
import { UserNewsService } from "../../services/user-news.service";
import { UserSectionService } from "../../services/user-section.service";
import { NewsItem } from "../../interfaces/news-item";
import { SectionWithNews } from "../../interfaces/section-with-news";
import { GroupedNews } from "../../interfaces/grouped-news";
import { SectionsNamesUtilsService } from "../../shared/sections-names-utils.service";

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
                if (s.openItemId) {
                    const openItem = [...s.unread, ...s.read].find(x => x.id === s.openItemId);
                    if (openItem && !openItem.isRead) {
                        openItem.isRead = true;
                        s.unread = s.unread.filter(x => x.id !== openItem.id);
                        s.read = [openItem, ...s.read];

                        this.userNewsService.markAsRead(openItem.id).subscribe();
                    }
                    s.openItemId = null;
                }
                s.isOpen = false;
            });
        });

        if (!isCurrentlyOpen) {
            source.isOpen = true;
        }
    }

    toggleNewsItem(source: GroupedNews, item: NewsItem) {
        if (source.openItemId === item.id) {
            if (!item.isRead) {
                item.isRead = true;
                source.unread = source.unread.filter(x => x.id !== item.id);
                source.read = [item, ...source.read];

                this.userNewsService.markAsRead(item.id).subscribe();
            }

            source.openItemId = null;
            return;
        }

        if (source.openItemId) {
            const prevItem = [...source.unread, ...source.read].find(x => x.id === source.openItemId);
            if (prevItem && !prevItem.isRead) {
                prevItem.isRead = true;
                source.unread = source.unread.filter(x => x.id !== prevItem.id);
                source.read = [prevItem, ...source.read];

                this.userNewsService.markAsRead(prevItem.id).subscribe();
            }
        }

        source.openItemId = item.id;
    }

    onArticleLinkClick(event: MouseEvent, item: NewsItem) {
        event.preventDefault();

        if (!item.isRead) {
            item.isRead = true;
            const source = this.findSourceByItem(item);
            if (source) {
                source.unread = source.unread.filter(x => x.id !== item.id);
                source.read = [item, ...source.read];
            }
            this.userNewsService.markAsRead(item.id).subscribe({
                next: () => this.userNewsService.markLinkClicked(item.id).subscribe()
            });
        } else {
            this.userNewsService.markLinkClicked(item.id).subscribe();
        }

        window.open(item.link, '_blank');
    }

    private findSourceByItem(item: NewsItem): GroupedNews | undefined {
        for (const section of this.sectionsWithNews) {
            for (const source of section.groupedNews) {
                if ([...source.unread, ...source.read].some(x => x.id === item.id)) {
                    return source;
                }
            }
        }
        return undefined;
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

                    const groupedNews: GroupedNews[] = Object.entries(groupedMap).map(([key, items]) => {
                        const unread = items.filter(i => !i.isRead);
                        const read = items.filter(i => i.isRead);

                        return {
                            key,
                            unread,
                            read,
                            isOpen: false,
                            openItemId: null
                        };
                    });

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