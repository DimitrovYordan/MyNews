import { NewsItem } from "./news-item";

export interface SectionWithNews {
    sectionId: number;
    sectionName: string;
    sourceUrl: string;
    news: NewsItem[];
}