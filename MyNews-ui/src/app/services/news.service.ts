import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { NewsItem } from "../interfaces/news-item";

@Injectable({
    providedIn: 'root'
})
export class NewsService {
    private apiUrl = 'http://localhost:5271/api/news';

    constructor(private http: HttpClient) {}

    getNews(): Observable<NewsItem[]> {
        return this.http.get<NewsItem[]>(this.apiUrl);
    }
}