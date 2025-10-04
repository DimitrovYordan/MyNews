import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { NewsItem } from "../interfaces/news-item";
import { SectionWithNews } from "../interfaces/section-with-news";

@Injectable({
    providedIn: 'root'
})
export class NewsService {
    private apiUrl = 'http://localhost:5271/api/news';

    constructor(private http: HttpClient) { }

    getAllNews(): Observable<NewsItem[]> {
        return this.http.get<NewsItem[]>(this.apiUrl);
    }

    getNewsBySections(sectionIds: number[]): Observable<SectionWithNews[]> {
        return this.http.post<SectionWithNews[]>(`${this.apiUrl}/by-sections`, sectionIds, {
            headers: { 'Content-Type': 'application/json' }
        });
    }
}