import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent {
  resetForm: FormGroup;
  token: string = '';
  errorMessage: string = '';
  showPassword: boolean = false;
  showRepeatPassword: boolean = false;

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {
    this.resetForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6), Validators.pattern(/^(?=.*[0-9])(?=.*[!@#$%^&*])/)]],
      repeatPassword: ['', Validators.required]
    }, { validators: this.passwordsMatchValidator });

    this.activatedRoute.queryParams.subscribe(params => {
      this.token = params['token'] || '';
    });
  }

  passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const repeat = group.get('repeatPassword')?.value;
    return password === repeat ? null : { passwordsMismatch: true };
  }

  submit(): void {
    if (this.resetForm.valid) {
      this.authService.resetPassword({ token: this.token, newPassword: this.resetForm.value.password }).subscribe({
        next: () => {
          alert('Password reset successfully!');
          this.router.navigate(['/login']);
        },
        error: () => {
          alert('Invalid or expired token.')
        }
      });
    } else {
      this.resetForm.markAllAsTouched();
    }
  }
}
