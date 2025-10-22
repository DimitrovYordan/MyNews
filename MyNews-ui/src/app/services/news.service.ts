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
        const params = new HttpParams({
            fromObject: {
                sectionIds: sectionIds.map(x => x.toString()),
                sourceIds: sourceIds.map(x => x.toString())
            }
        });
        
        return this.http.get<NewsItem[]>(`${environment.apiUrl}/api/news`, { params });
    }
}