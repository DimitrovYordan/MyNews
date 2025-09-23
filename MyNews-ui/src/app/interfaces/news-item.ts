export interface NewsItem {
    id: number;
    title: string;
    content: string;
    publishedAt: string;
    sectionId: number;
    sectionName?: string;
    sourceUrl: string;
    sourceName?: string;
    isRead?: boolean;
}