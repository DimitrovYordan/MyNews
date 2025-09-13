import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  @Output() forgotPassword = new EventEmitter<void>();
  @Output() close = new EventEmitter<void>();
  @Output() loginSuccess = new EventEmitter<{ firstName: string; lastName: string }>();

  loginForm: FormGroup;

  errorMessage: string | null = null;
  loading = false;
  showPassword: boolean = false;

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  submitLogin() {
    if (this.loginForm.valid) {
      this.loading = true;
      this.authService.login(this.loginForm.value).subscribe({
        next: (res) => {
          localStorage.setItem('token', res.token);
          this.loginSuccess.emit({
            firstName: res.firstName,
            lastName: res.lastName
          });
          this.loading = false;
          this.close.emit();
        },
        error: (err: any) => {
          this.errorMessage = err.error?.message || 'Invalid email or password.';

          this.loading = false;
        }
      });
    }
  }
}
