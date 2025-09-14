import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent {
  @Output() close = new EventEmitter<void>();

  settingsForm!: FormGroup;
  showPassword: boolean = false;
  showRepeatPassword: boolean = false;

  constructor(private fb: FormBuilder, private authService: AuthService) {
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
          this.close.emit();
        },
        error: (err) => {
          console.error('Error updating profile', err);
        }
      });
    }
  }

  closeSettings(): void {
    this.close.emit();
    console.log('Close setting settings component.');
  }
}
