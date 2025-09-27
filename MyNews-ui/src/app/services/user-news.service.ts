import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class UserNewsService {
    private apiUrl = 'http://localhost:5271/api/news';

    constructor(private http: HttpClient) { }

    markInteraction(newsItemId: string, clickedLink: boolean = false): Observable<void> {
        return this.http.post<void>(
            `${this.apiUrl}/mark-interaction/${newsItemId}?clickedLink=${clickedLink}`,
            {}
        );
    }

    getReadNewsIds(): Observable<string[]> {
        return this.http.get<string[]>(`${this.apiUrl}/read-news`);
    }
}