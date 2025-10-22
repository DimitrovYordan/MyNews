import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { environment } from "../../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class UserPreferencesService {
    constructor(private http: HttpClient) { }

    getUserSections(): Observable<number[]> {
        return this.http.get<number[]>(`${environment.apiUrl}/api/userpreferences/sections`);
    }

    saveUserSections(sectionIds: number[]): Observable<void> {
        return this.http.post<void>(`${environment.apiUrl}/api/userpreferences/sections`, sectionIds);
    }

    getUserSources(): Observable<number[]> {
        return this.http.get<number[]>(`${environment.apiUrl}/api/userpreferences/sources`);
    }

    saveUserSources(sourceIds: number[]): Observable<any> {
        return this.http.post<any>(`${environment.apiUrl}/api/userpreferences/sources`, sourceIds);
    }
}