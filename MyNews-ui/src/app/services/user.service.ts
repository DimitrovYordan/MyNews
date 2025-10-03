import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { AuthResponse } from "../interfaces/auth-response";

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private apiUrl = 'http://localhost:5271/api/users';

    constructor(private http: HttpClient) { }

    getProfile(): Observable<AuthResponse> {
        return this.http.get<AuthResponse>(`${this.apiUrl}/me`);
    }

    updateProfile(data: any): Observable<AuthResponse> {
        return this.http.put<AuthResponse>(`${this.apiUrl}/update-profile`, data);
    }

    deleteAccount(): Observable<{ message: string }> {
        return this.http.delete<{ message: string }>(`${this.apiUrl}/delete-profile`);
    }
}