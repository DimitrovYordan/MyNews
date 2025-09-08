import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { AuthRequest } from "../interfaces/auth-request";
import { SignupData } from "../interfaces/signup";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'http://localhost:5271/api/auth';

    constructor(private http: HttpClient) { }

    login(credentials: AuthRequest): Observable<any> {
        return this.http.post(`${this.apiUrl}/login`, credentials);
    }

    signup(credentials: SignupData): Observable<any> {
        return this.http.post(`${this.apiUrl}/signup`, credentials);
    }

    forgotPassword(email: string): Observable<any> {
        return this.http.post(`${this.apiUrl}/forgot-password`, { email });
    }
}