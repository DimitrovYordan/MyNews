import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { BehaviorSubject, Observable, tap } from "rxjs";

import { AuthRequest } from "../interfaces/auth-request";
import { SignupData } from "../interfaces/signup";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'http://localhost:5271/api/auth';
    private tokenKey = 'auth_token';

    public isLoggedIn$ = new BehaviorSubject<boolean>(false);

    constructor(private http: HttpClient) {
        const token = localStorage.getItem(this.tokenKey);
        this.isLoggedIn$.next(!!token);
    }

    login(credentials: AuthRequest): Observable<any> {
        return this.http.post(`${this.apiUrl}/login`, credentials).pipe(
            tap((res: any) => {
                localStorage.setItem(this.tokenKey, res.token);
                this.isLoggedIn$.next(true);
            })
        );
    }

    logout() {
        localStorage.removeItem(this.tokenKey);
        this.isLoggedIn$.next(false);
    }

    signup(credentials: SignupData): Observable<any> {
        return this.http.post(`${this.apiUrl}/register`, credentials);
    }

    forgotPassword(email: string): Observable<any> {
        return this.http.post(`${this.apiUrl}/forgot-password`, { email });
    }
}