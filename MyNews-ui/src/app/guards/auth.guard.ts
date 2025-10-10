import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";

import { AuthService } from "../services/auth.service";

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    constructor(private router: Router, private authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        const publicUrls = ['/login', '/signup', '/forgot-password', '/reset-password'];

        const path = state.url.split('?')[0];

        if (publicUrls.includes(path) && this.authService.isLoggedIn()) {
            this.authService.logout();
            this.router.navigate(['/']);
            return false;
        }

        return true;
    }
}