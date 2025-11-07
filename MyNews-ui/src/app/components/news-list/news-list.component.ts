import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";
import { FormsModule } from '@angular/forms';
import { CdkDragDrop, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { CdkAccordionModule } from "@angular/cdk/accordion";

import { TranslateModule } from "@ngx-translate/core";

import { NewsService } from "../../services/news.service";
import { UserNewsService } from "../../services/user-news.service";
import { UserPreferencesService } from "../../services/user-preferences.service";
import { SectionService } from "../../services/section.service";
import { NewsItem } from "../../interfaces/news-item";
import { SectionWithNews } from "../../interfaces/section-with-news";
import { GroupedNews } from "../../interfaces/grouped-news";
import { NamesUtilsService } from "../../shared/names-utils.service";
import { LanguageService } from "../../services/language.service";

@Component({
    selector: 'app-news-list',
    standalone: true,
    imports: [CommonModule, FormsModule, DragDropModule, CdkAccordionModule, TranslateModule],
    templateUrl: './news-list.component.html',
    styleUrls: ['./news-list.component.scss'],
})
export class NewsListComponent implements OnInit {
    public sectionsWithNews: (SectionWithNews & { groupedNews: GroupedNews[] })[] = [];
    public news: NewsItem[] = [];
    public selectedSections: number[] = [];
    public selectedSources: number[] = [];
    public errorMessage: string = '';
    public searchTerm: string = '';
    public isLoading: boolean = false;
    public currentLang: string = 'DEFAULT';

    constructor(
        private newsService: NewsService,
        private userNewsService: UserNewsService,
        private userPreferencesService: UserPreferencesService,
        private sectionService: SectionService,
        public namesUtilsService: NamesUtilsService,
        private languageService: LanguageService,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.currentLang = this.languageService.getLanguage();
        this.languageService.language$.subscribe(lang => {
            this.currentLang = lang;
        });

        this.isLoading = true;
        this.loadUserPreferencesAndFetchNews();
    }

    goBack() {
        this.router.navigate(['/sections']);
    }

    dropSection(event: CdkDragDrop<any[]>) {
        moveItemInArray(this.sectionsWithNews, event.previousIndex, event.currentIndex);

        const newOrder = this.sectionsWithNews.map(s => s.sectionId);

        this.userPreferencesService.updateSectionsOrder(newOrder).subscribe({
            next: (res: any) => {
                this.reorderSections(res.sections ?? newOrder);
            },
            error: err => console.error('Failed to save section order', err)
        });
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

    onLanguageChange(event: Event) {
        const select = event.target as HTMLSelectElement;
        if (select) {
            this.languageService.setLanguage(select.value);
        }
    }

    getTitle(news: NewsItem): string {
        const lang = this.languageService.getLanguage();
        const translation = news.translations?.find(t => t.languageCode === lang);

        return translation?.title || news.title;
    }

    getSummary(news: NewsItem): string {
        const lang = this.languageService.getLanguage();
        const translation = news.translations?.find(t => t.languageCode === lang);

        return translation?.summary || news.summary;
    }

    getFilteredItems(source: GroupedNews): { unread: NewsItem[], read: NewsItem[], count: number } {
        if (!this.searchTerm.trim()) {
            return { unread: source.unread, read: source.read, count: source.unread.length };
        }

        const lower = this.searchTerm.toLowerCase();
        const currentLang = this.languageService.getLanguage().toUpperCase();

        const matcheSearch = (item: NewsItem): boolean => {
            const currentTranslation = item.translations?.find(
                t => t.languageCode?.toUpperCase() === currentLang
            );
            if (currentTranslation) {
                if (
                    (currentTranslation.title && currentTranslation.title.toLowerCase().includes(lower)) ||
                    (currentTranslation.summary && currentTranslation.summary.toLowerCase().includes(lower))
                ) {
                    return true;
                }
            }

            if (item.title.toLowerCase().includes(lower) ||
                item.summary.toLowerCase().includes(lower)
            ) {
                return true;
            }

            if (item.translations && item.translations.length > 0) {
                return item.translations.some(t =>
                    (t.languageCode?.toUpperCase() !== currentLang) &&
                    (
                        (t.title && t.title.toLowerCase().includes(lower)) ||
                        (t.summary && t.summary.toLowerCase().includes(lower))
                    )
                );
            }

            return false;
        }

        const unread = source.unread.filter(matcheSearch);
        const read = source.read.filter(matcheSearch);
        const count = unread.length + read.length;

        return { unread, read, count };
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

    private reorderSections(sectionOrder: number[]): void {
        if (!sectionOrder || sectionOrder.length === 0) return;

        const orderMap = new Map<number, number>();
        sectionOrder.forEach((id, index) => orderMap.set(id, index));

        this.sectionsWithNews.sort((a, b) => {
            const aIndex = orderMap.get(a.sectionId) ?? Number.MAX_SAFE_INTEGER;
            const bIndex = orderMap.get(b.sectionId) ?? Number.MAX_SAFE_INTEGER;
            return aIndex - bIndex;
        });
    }

    private loadUserPreferencesAndFetchNews(): void {
        this.isLoading = true;

        this.userPreferencesService.getUserSections().subscribe({
            next: sections => {
                this.selectedSections = sections;
                this.userPreferencesService.getUserSources().subscribe({
                    next: sources => {
                        this.selectedSources = sources;
                        this.fetchNews();
                    },
                    error: () => {
                        this.errorMessage = 'Failed to load sources.';
                        this.isLoading = false;
                    }
                });
            },
            error: () => {
                this.errorMessage = 'Failed to load sections.';
                this.isLoading = false;
            }
        });
    }

    private fetchNews(): void {
        this.isLoading = true;
        
        this.newsService.getNews(this.selectedSections, this.selectedSources).subscribe({
            next: (data: NewsItem[]) => {
                const sectionMap = new Map<number, NewsItem[]>();

                data.forEach(item => {
                    if (!sectionMap.has(item.sectionId)) {
                        sectionMap.set(item.sectionId, []);
                    }
                    sectionMap.get(item.sectionId)!.push(item);
                });

                this.sectionsWithNews = Array.from(sectionMap.entries()).map(([sectionId, items]) => {
                    const groupedMap = items.reduce((acc, item) => {
                        if (!acc[item.sourceName || item.sourceUrl]) acc[item.sourceName || item.sourceUrl] = [];
                        acc[item.sourceName || item.sourceUrl].push(item);
                        return acc;
                    }, {} as Record<string, NewsItem[]>);

                    const groupedNews: GroupedNews[] = Object.entries(groupedMap).map(([key, items]) => {
                        const unread = items.filter(i => !i.isRead);
                        const read = items.filter(i => i.isRead);
                        return { key, unread, read, isOpen: false, openItemId: null };
                    });

                    const sectionFromAll = this.sectionService.allSections?.find(s => s.id === sectionId);
                    const sectionNameFormatted = sectionFromAll
                        ? this.namesUtilsService.formatSectionName(sectionFromAll.name)
                        : 'Unknown Section';

                    return {
                        sectionId,
                        sectionName: sectionNameFormatted,
                        news: items,
                        groupedNews
                    } as SectionWithNews & { groupedNews: GroupedNews[] };
                });

                this.sectionsWithNews.sort((a, b) => {
                    const indexA = this.selectedSections.indexOf(a.sectionId);
                    const indexB = this.selectedSections.indexOf(b.sectionId);
                    return indexA - indexB;
                });

                this.isLoading = false;
            },
            error: (error) => {
                this.errorMessage = 'Failed to load news.';
                this.isLoading = false;
            }
        });
    }
}