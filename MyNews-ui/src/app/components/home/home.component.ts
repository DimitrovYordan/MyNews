import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  showLogin: boolean = false;
  showSignup: boolean = false;

  onLoginClose() {
    this.showLogin = false;
  }

  onSignupClose() {
    this.showSignup = false;
  }
}
