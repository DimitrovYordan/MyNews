import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class UserNewsService {
    private apiUrl = 'http://localhost:5271/api/news';

    constructor(private http: HttpClient) { }

    markAsRead(newsItemId: string): Observable<void> {
        return this.http.post<void>(`${this.apiUrl}/mark-as-read/${newsItemId}`, {});
    }

    markLinkClicked(newsItemId: string): Observable<void> {
        return this.http.post<void>(`${this.apiUrl}/mark-link-clicked/${newsItemId}`, {});
    }


    getReadNewsIds(): Observable<string[]> {
        return this.http.get<string[]>(`${this.apiUrl}/read-news`);
    }
}