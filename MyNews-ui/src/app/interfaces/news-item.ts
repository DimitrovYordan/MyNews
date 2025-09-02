export interface NewsItem {
    id: number;
    title: string;
    content: string;
    publishedAt: string;
    sectionId: number;
    sectionName?: string;
    sourceId: number;
    sourceName?: string;
}