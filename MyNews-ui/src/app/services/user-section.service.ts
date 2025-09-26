import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class UserSectionService {
    private apiUrl = 'http://localhost:5271/api/userpreferences';

    constructor(private http: HttpClient) { }

    getUserSections(): Observable<number[]> {
        return this.http.get<number[]>(`${this.apiUrl}/sections`);
    }

    saveUserSections(sectionIds: number[]): Observable<void> {
        return this.http.post<void>(`${this.apiUrl}/sections`, sectionIds);
    }
}