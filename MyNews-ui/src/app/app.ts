import { Component, signal } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

import { AuthService } from './services/auth.service';
import { LoginComponent } from './auth/login/login.component';
import { SignupComponent } from './auth/signup/signup.component';
import { SettingsComponent } from './auth/settings/settings.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, LoginComponent, SignupComponent, SettingsComponent],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App {
  protected readonly title = signal('My News');

  showLogin: boolean = false;
  showSignup: boolean = false;
  isLoggedIn: boolean = false;
  isUserMenuOpen: boolean = false;
  showSettings: boolean = false;
  userFirstName: string = '';
  userLastName: string = '';

  constructor(public authService: AuthService, private router: Router) {
    this.authService.showLogin$.subscribe(val => this.showLogin = val);
    this.authService.showSignup$.subscribe(val => this.showSignup = val);
    this.authService.isLoggedIn$.subscribe(status => {
      this.isLoggedIn = status;

      const currentUser = this.authService.getCurrentUser();
      if (currentUser) {
        this.userFirstName = currentUser.firstName;
        this.userLastName = currentUser.lastName;
      }
    });
  }

  openLogin() {
    this.showLogin = true;
    this.showSignup = false;
  }

  openSignup() {
    this.showSignup = true;
    this.showLogin = false;
  }

  closeForms() {
    this.authService.closeForms();
  }

  onLoginSuccess(user: { firstName: string; lastName: string }) {
    console.log('User logged in', user);
    this.authService.isLoggedIn$.next(true);
    this.authService.getCurrentUser();
    this.userFirstName = user.firstName;
    this.userLastName = user.lastName;
    this.showLogin = false;
    this.isUserMenuOpen = false;

    this.router.navigate(['/sections']);
  }

  onSignupSuccess(user: { firstName: string; lastName: string }) {
    console.log('User signed up', user);
    this.authService.isLoggedIn$.next(true);
    this.userFirstName = user.firstName;
    this.userLastName = user.lastName;
    this.showSignup = false;

    this.router.navigate(['/sections']);
  }

  logout() {
    this.authService.logout();
    this.isLoggedIn = false;
    this.userFirstName = '';
    this.userLastName = '';
    this.isUserMenuOpen = false;
  }

  toggleUserMenu() {
    this.isUserMenuOpen = !this.isUserMenuOpen;
  }

  goToProfile() {
    this.router.navigate(['/settings']);
    this.isUserMenuOpen = false;
    this.openSettings();
  }

  openSettings() {
    this.showSettings = true;
    this.isUserMenuOpen = false;
  }

  closeSettings() {
    console.log('Close setting app.');
    this.showSettings = false;
  }

  getInitials(): string {
    if (!this.userFirstName || !this.userLastName) {
      return '';
    }

    return this.userFirstName[0].toUpperCase() + this.userLastName[0].toUpperCase();
  }
}
