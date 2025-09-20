import { NewsItem } from "./news-item";

export interface GroupedNews {
    key: string;
    items: NewsItem[];
    isOpen: boolean;
    openItemId?: number | null;
}