import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';

import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent {
  settingsForm!: FormGroup;
  showPassword: boolean = false;
  showRepeatPassword: boolean = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.settingsForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      country: [''],
      city: [''],
      email: ['', [Validators.email]],
      password: ['', [Validators.minLength(6), Validators.pattern(/^(?=.*[0-9])(?=.*[!@#$%^&*])/)]],
      repeatPassword: ['']
    }, { validators: this.passwordsMatchValidator });
  }

  passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const repeatPassword = group.get('repeatPassword')?.value;

    if (!password && !repeatPassword) return null;

    return password === repeatPassword ? null : { passwordsMismatch: true };
  }

  submitSettings(): void {
    if (this.settingsForm.valid) {
      this.authService.updateProfile(this.settingsForm.value).subscribe({
        next: (res) => {
          console.log('Profile update', res);
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Error updating profile', err);
        }
      });
    }
  }

  closeSettings(): void {
    this.router.navigate(['/']);
    console.log('Close setting settings component.');
  }
}
