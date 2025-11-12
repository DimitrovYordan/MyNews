import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  errorMessage: string | null = null;
  loading: boolean = false;
  showPassword: boolean = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit() {
    this.loginForm.valueChanges.subscribe(() => {
      this.errorMessage = null;
    });
  }

  submitLogin() {
    this.loginForm.markAllAsTouched();

    if (this.loginForm.valid) {
      this.loading = true;
      this.errorMessage = null;
      
      this.authService.login(this.loginForm.value).subscribe({
        next: () => {
          this.router.navigate(['/news']);
          this.loading = false;
        },
        error: (err: any) => {
          this.loading = false;
          if (err.status === 401) {
            this.errorMessage = 'ERROR_FORM_INVALID';
          } else {
            this.errorMessage = 'ERROR_FORM_INVALID';
          }
        }
      });
    }
  }

  triggerForgotPassword() {
    this.router.navigate(['/forgot-password']);
  }

  closeLogin() {
    this.router.navigate(['/']);
  }
}
