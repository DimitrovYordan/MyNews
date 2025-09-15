import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App implements OnInit {
  protected readonly title = signal('My News');

  isLoggedIn: boolean = false;
  isUserMenuOpen: boolean = false;
  userFirstName: string = '';
  userLastName: string = '';

  constructor(public authService: AuthService, private router: Router) {
    this.authService.isLoggedIn$.subscribe(status => {
      this.isLoggedIn = status;

      const currentUser = this.authService.getCurrentUser();
      if (currentUser) {
        this.userFirstName = currentUser.firstName;
        this.userLastName = currentUser.lastName;
      }
    });
  }

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.userFirstName = user.firstName;
      this.userLastName = user.lastName;
      this.isLoggedIn = true;
    }

    this.authService.isLoggedIn$.subscribe(status => this.isLoggedIn = status);
  }

  openLogin() {
    this.router.navigate(['/login']);
  }

  openSignup() {
    this.router.navigate(['signup']);
  }

  openForgotPassword() {
    this.router.navigate(['/forgot-password']);
  }

  logout() {
    this.authService.logout();
    this.isLoggedIn = false;
    this.userFirstName = '';
    this.userLastName = '';
    this.isUserMenuOpen = false;
    this.router.navigate(['/']);
  }

  toggleUserMenu() {
    this.isUserMenuOpen = !this.isUserMenuOpen;
  }

  goToProfile() {
    this.router.navigate(['/settings']);
    this.isUserMenuOpen = false;
  }

  getInitials(): string {
    if (!this.userFirstName || !this.userLastName) {
      return '';
    }

    return this.userFirstName[0].toUpperCase() + this.userLastName[0].toUpperCase();
  }
}
