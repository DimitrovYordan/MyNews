import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

import { TranslateModule } from '@ngx-translate/core';

import { AuthService } from '../../services/auth.service';
import { ModalComponent } from "../../shared/modal/modal.component";
import { LocationSelectorComponent } from "../../components/location-selector/location-selector";

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ModalComponent, TranslateModule, LocationSelectorComponent],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {
  signupForm: FormGroup;
  errorMessage: string | null = null;
  showPassword: boolean = false;
  showRepeatPassword: boolean = false;
  loading: boolean = false;
  showSignupSuccessModal: boolean = false;
  showWelcomeModal: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private http: HttpClient
  ) {
    this.signupForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      country: ['', Validators.required],
      city: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.pattern(/^(?=.*[0-9])(?=.*[^a-zA-Z0-9]).*$/)]],
      repeatPassword: ['', Validators.required]
    }, { validators: this.passwordsMatchValidator });

    const hasSeenWelcome = sessionStorage.getItem('welcomeShown');
    this.showWelcomeModal = !hasSeenWelcome;
  }

  passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const repeatPassword = group.get('repeatPassword')?.value;

    return password === repeatPassword ? null : { passwordsMismatch: true };
  }

  submitSignup() {
    this.signupForm.markAllAsTouched();

    this.loading = true;
    this.errorMessage = null;

    this.authService.signup(this.signupForm.value).subscribe({
      next: () => {
        this.loading = false;
        this.authService.hasSelectedSections$.next(false);
        this.showSignupSuccessModal = true;
      },
      error: (err) => {
        this.loading = false;

        if (err.status === 400 && err.error?.message?.includes('already exists')) {
          this.signupForm.get('email')?.setErrors({ emailTaken: true });
        } else {
          this.errorMessage = 'ERROR_SIGNUP';
        }
      }
    });
  }

  closeSignup() {
    this.router.navigate(['/']);
  }

  goToSections() {
    this.showSignupSuccessModal = false;
    this.router.navigate(['/sections'], { queryParams: { onboarding: 'true' } });
  }

  get emailErrors() {
    const control = this.signupForm.get('email');

    if (control?.hasError('required') && control.touched) return 'ERROR_EMAIL_REQUIRED';
    if (control?.hasError('email') && control.touched) return 'ERROR_VALID_EMAIL';
    if (control?.hasError('emailTaken')) return 'ERROR_TAKEN_EMAIL';

    return null;
  }
}