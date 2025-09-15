import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

import { BehaviorSubject, Observable, tap } from "rxjs";

import { AuthRequest } from "../interfaces/auth-request";
import { SignupData } from "../interfaces/signup";
import { AuthResponse } from "../interfaces/auth-response";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'http://localhost:5271/api';
    private token: string | null = null;
    private currentUser: AuthResponse | null = null;

    public showLogin$ = new BehaviorSubject<boolean>(false);
    public showSignup$ = new BehaviorSubject<boolean>(false);
    public isLoggedIn$ = new BehaviorSubject<boolean>(false);

    constructor(private http: HttpClient, private router: Router) { }

    login(credentials: AuthRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, credentials).pipe(
            tap(res => {
                this.token = res.token;
                this.currentUser = res;
                this.isLoggedIn$.next(true);
            })
        );
    }

    logout() {
        this.token = null;
        this.currentUser = null;
        this.isLoggedIn$.next(false);
        this.router.navigate(['/'], { replaceUrl: true });
    }

    signup(credentials: SignupData): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, credentials).pipe(
            tap(res => {
                this.token = res.token;
                this.isLoggedIn$.next(true);
            })
        );
    }

    forgotPassword(email: string): Observable<any> {
        return this.http.post(`${this.apiUrl}/auth/forgot-password`, { email });
    }

    getCurrentUser(): AuthResponse | null {
        return this.currentUser;
    }

    updateProfile(data: any): Observable<any> {
        if (!this.token) {
            throw new Error('User not logged in');
        }

        return this.http.put(`${this.apiUrl}/users/update-profile`, data, {
            headers: { Authorization: `Bearer ${this.token}` }
        });
    }

    openLogin() {
        this.showLogin$.next(true);
        this.showSignup$.next(false);
    }

    openSignup() {
        this.showSignup$.next(true);
        this.showLogin$.next(false);
    }

    closeForms() {
        this.showLogin$.next(false);
        this.showSignup$.next(false);
    }

    isLoggedIn(): boolean {
        return !!this.token;
    }
}