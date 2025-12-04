import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { environment } from "../../environments/environment";
import { NewsItem } from "../interfaces/news-item";

@Injectable({
    providedIn: 'root'
})
export class NewsService {
    constructor(private http: HttpClient) { }

    getNews(sectionIds: number[], sourceIds: number[]): Observable<NewsItem[]> {
        const body = { sectionIds, sourceIds };

        return this.http.post<NewsItem[]>(`${environment.apiUrl}/api/news/filter`, body);
    }
}