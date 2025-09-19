import { Pipe, PipeTransform } from "@angular/core";

import { NewsItem } from "../interfaces/news-item";
import { GroupedNews } from "../interfaces/grouped-news";
import { SectionsNamesUtilsService } from "./sections-names-utils.service";

@Pipe({
    name: 'groupBy',
    standalone: true
})
export class GroupByPipe implements PipeTransform {

    constructor(private sectionNameService: SectionsNamesUtilsService) { }

    transform<T>(items: NewsItem[] = [], field: keyof NewsItem): GroupedNews[] {
        const map = new Map<string, NewsItem[]>();

        items.forEach(item => {
            const value = item[field] as unknown as string;
            if (!value) {
                return;
            }

            const domain = this.sectionNameService.getDomain(value);

            if (!map.has(domain)) {
                map.set(domain, []);
            }

            map.get(domain)!.push(item);
        });

        return Array.from(map.entries()).map(([key, items]) => ({ key, items, isOpen: false }));
    }
}