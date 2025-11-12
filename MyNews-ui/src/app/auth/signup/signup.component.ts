import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

import { TranslateModule } from '@ngx-translate/core';

import { AuthService } from '../../services/auth.service';
import { ModalComponent } from "../../shared/modal/modal.component";

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ModalComponent, TranslateModule],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {
  signupForm: FormGroup;
  errorMessage: string | null = null;
  showPassword: boolean = false;
  showRepeatPassword: boolean = false;
  loading: boolean = false;
  showSignupSuccessModal: boolean = false;
  showWelcomeModal: boolean = false;

  countries: any[] = [];
  cities: string[] = [];

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

  ngOnInit(): void {
    this.http.get<any>('https://countriesnow.space/api/v0.1/countries')
      .subscribe(res => {
        this.countries = res.data;
      });
  }

  passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const repeatPassword = group.get('repeatPassword')?.value;

    return password === repeatPassword ? null : { passwordsMismatch: true };
  }

  submitSignup() {
    this.signupForm.markAllAsTouched();
    
    Object.keys(this.signupForm.controls).forEach(key => {
      const control = this.signupForm.get(key);
      control?.markAsTouched();
      control?.updateValueAndValidity();
    });

    console.log('Form valid?', this.signupForm.valid);

    if (this.signupForm.invalid) {
      this.errorMessage = 'ERROR_SIGNUP_INVALID';
      return;
    }

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

  onCountryChange() {
    const selectedCountry = this.signupForm.get('country')?.value;
    const match = this.countries.find(c => c.country === selectedCountry);
    this.cities = match ? match.cities : [];
    this.signupForm.get('city')?.setValue('');
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