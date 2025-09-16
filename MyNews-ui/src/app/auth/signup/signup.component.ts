import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {
  signupForm: FormGroup;
  errorMessage: string | null = null;
  showPassword: boolean = false;
  showRepeatPassword: boolean = false;
  loading: boolean = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.signupForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      country: ['', Validators.required],
      city: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.pattern(/^(?=.*[0-9])(?=.*[!@#$%^&*])/)]],
      repeatPassword: ['', Validators.required]
    }, { validators: this.passwordsMatchValidator });
  }

  passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const repeatPassword = group.get('repeatPassword')?.value;

    return password === repeatPassword ? null : { passwordsMismatch: true };
  }

  submitSignup() {
    this.signupForm.markAllAsTouched();

    if (this.signupForm.valid) {
      this.loading = true;
      this.errorMessage = null;

      this.authService.signup(this.signupForm.value).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/sections']);
        },
        error: (err) => {
          this.loading = false;

          if (err.status === 400 && err.error?.message?.includes('already exists')) {
            this.signupForm.get('email')?.setErrors({ emailTaken: true });
            this.errorMessage = 'This email is already registered.';
          } else {
            this.errorMessage = 'Signup failed. Plase try again.';
          }
        }
      });
    }
  }

  closeSignup() {
    this.router.navigate(['/']);
  }

  get emailErrors() {
    const control = this.signupForm.get('email');
    if (!control) return null;

    if (control.hasError('required') && control.touched) return 'Email is required.';
    if (control.hasError('email') && control.touched) return 'Please enter a valid email.';
    if (control.hasError('emailTaken')) return 'This email is already registered.';

    return null;
  }
}
