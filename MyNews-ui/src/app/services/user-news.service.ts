import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class UserNewsService {
    private apiUrl = 'http://localhost:5271/api/users';

    constructor(private http: HttpClient) { }

    markAsRead(newsId: string): Observable<void> {
        return this.http.post<void>(`${this.apiUrl}/mark-as-read`, { newsId });
    }

    markAsClicked(newsId: string): Observable<void> {
        return this.http.post<void>(`${this.apiUrl}/mark-clicked`, { newsId });
    }

    getReadNewsIds(): Observable<string[]> {
        return this.http.get<string[]>(`${this.apiUrl}/read-news`);
    }
}