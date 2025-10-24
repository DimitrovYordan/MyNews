import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { AuthResponse } from "../interfaces/auth-response";
import { environment } from "../../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class UserService {
    constructor(private http: HttpClient) { }

    getProfile(): Observable<AuthResponse> {
        return this.http.get<AuthResponse>(`${environment.apiUrl}/api/users/me`);
    }

    updateProfile(data: any): Observable<AuthResponse> {
        return this.http.put<AuthResponse>(`${environment.apiUrl}/api/users/update-profile`, data);
    }

    deleteAccount(): Observable<{ message: string }> {
        return this.http.delete<{ message: string }>(`${environment.apiUrl}/api/users/delete-profile`);
    }

    getOnboardingStatus(): Observable<{ isOnboardingComplete: boolean }> {
        return this.http.get<{ isOnboardingComplete: boolean }>(`${environment.apiUrl}/api/users/onboarding-status`);
    }

    completeOnboarding(): Observable<{ message: string }> {
        return this.http.post<{ message: string }>(`${environment.apiUrl}/api/users/complete-onboarding`, {});
    }
}