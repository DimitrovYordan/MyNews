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
    private tokenKey: string = 'auth_token';
    private userKey: string = 'user';
    private currentUser: AuthResponse | null = null;
    private selectedSections: number[] = [];
    private selectedSectionsKey = 'selected_sections';

    public showLogin$ = new BehaviorSubject<boolean>(false);
    public showSignup$ = new BehaviorSubject<boolean>(false);
    public isLoggedIn$ = new BehaviorSubject<boolean>(false);
    public isMenuOpen$ = new BehaviorSubject<boolean>(false);
    public hasSelectedSections$ = new BehaviorSubject<boolean>(false);
    public currentUser$ = new BehaviorSubject<AuthResponse | null>(null);

    constructor(private http: HttpClient, private router: Router) {
        const userData = sessionStorage.getItem(this.userKey);
        if (userData) {
            this.currentUser = JSON.parse(userData);
            this.currentUser$.next(this.currentUser);
            this.isLoggedIn$.next(true);
        }
    }

    login(credentials: AuthRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, credentials).pipe(
            tap(res => {
                sessionStorage.setItem(this.tokenKey, res.token);
                sessionStorage.setItem(this.userKey, JSON.stringify(res));
                this.currentUser = res;
                this.isLoggedIn$.next(true);
                this.currentUser$.next(res);
            })
        );
    }

    logout() {
        sessionStorage.removeItem(this.tokenKey);
        sessionStorage.removeItem(this.userKey);
        this.currentUser = null;
        this.currentUser$.next(null);
        this.isLoggedIn$.next(false);
        this.router.navigate(['/'], { replaceUrl: true });
    }

    signup(credentials: SignupData): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, credentials).pipe(
            tap(res => {
                sessionStorage.setItem(this.tokenKey, res.token);
                sessionStorage.setItem(this.userKey, JSON.stringify(res));
                this.currentUser = res;
                this.isLoggedIn$.next(true);
                this.currentUser$.next(res);
            })
        );
    }

    forgotPassword(email: string): Observable<any> {
        return this.http.post(`${this.apiUrl}/auth/forgot-password`, { email });
    }

    resetPassword(data: { token: string; newPassword: string; }) {
        return this.http.post(`${this.apiUrl}/auth/reset-password`, data);
    }

    setCurrentUser(user: AuthResponse | null) {
        if (!user) {
            this.currentUser = null;
            this.currentUser$.next(null);
            sessionStorage.removeItem(this.tokenKey);
            return;
        }

        const token = user.token || sessionStorage.getItem(this.tokenKey) || '';
        this.currentUser = { ...user, token };
        this.currentUser$.next(this.currentUser);
        sessionStorage.setItem(this.tokenKey, token);
    }

    setSelectedSections(selectedSections: number[]) {
        sessionStorage.setItem(this.selectedSectionsKey, JSON.stringify(selectedSections));
        this.hasSelectedSections$.next(true);
    }

    getSelectedSections(): number[] {
        const stored = sessionStorage.getItem(this.selectedSectionsKey);
        if (stored) {
            try {
                return JSON.parse(stored);
            } catch {
                return [];
            }
        }
        return [];
    }

    getCurrentUser(): AuthResponse | null {
        if (!this.currentUser) {
            const userData = sessionStorage.getItem(this.userKey);
            if (userData) {
                this.currentUser = JSON.parse(userData);
                this.currentUser$.next(this.currentUser);
            }
        }

        return this.currentUser;
    }

    getUserInitials(): string | null {
        const user = this.getCurrentUser();
        if (!user) {
            return null;
        }

        const firstName = user.firstName?.charAt(0).toUpperCase() || '';
        const lastName = user.lastName?.charAt(0).toUpperCase() || '';

        return `${firstName}${lastName}` || null;
    }

    getToken(): string | null {
        return sessionStorage.getItem(this.tokenKey);
    }

    updateProfile(data: any): Observable<any> {
        return this.http.put(`${this.apiUrl}/users/update-profile`, data);
    }

    toggleMenu() {
        this.isMenuOpen$.next(!this.isMenuOpen$.value);
    }

    closeMenu() {
        this.isMenuOpen$.next(false);
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
        return !!this.getToken();
    }
}