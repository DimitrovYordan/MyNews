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
    private tokenKey = 'auth_token';
    private currentUser: AuthResponse | null = null;

    public showLogin$ = new BehaviorSubject<boolean>(false);
    public showSignup$ = new BehaviorSubject<boolean>(false);
    public isLoggedIn$ = new BehaviorSubject<boolean>(false);

    constructor(private http: HttpClient, private router: Router) {
        const storedUser = localStorage.getItem('current_user');
        if (storedUser) {
            this.currentUser = JSON.parse(storedUser);
        }
    }

    login(credentials: AuthRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, credentials).pipe(
            tap(res => {
                localStorage.setItem(this.tokenKey, res.token);
                this.isLoggedIn$.next(true);
                this.currentUser = res;
            })
        );
    }

    logout() {
        localStorage.removeItem(this.tokenKey);
        this.isLoggedIn$.next(false);
        this.router.navigate(['/'], { replaceUrl: true });
    }

    signup(credentials: SignupData): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, credentials).pipe(
            tap(res => {
                localStorage.setItem(this.tokenKey, res.token);
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
        const token = localStorage.getItem(this.tokenKey);
        if (!token) {
            throw new Error('User not logged in');
        }

        return this.http.put(`${this.apiUrl}/users/update-profile`, data, {
            headers: { Authorization: `Bearer ${token}` }
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
}