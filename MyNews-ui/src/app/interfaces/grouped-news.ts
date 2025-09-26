import { NewsItem } from "./news-item";

export interface GroupedNews {
    key: string;
    items: NewsItem[];
    // unread: NewsItem[];
    // read: NewsItem[];
    isOpen: boolean;
    openItemId?: string | null;
}