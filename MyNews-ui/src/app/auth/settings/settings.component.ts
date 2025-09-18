import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { AuthResponse } from '../../interfaces/auth-response';

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
        next: () => {
          const updateUserNames = this.authService.updateUserNames(
            this.settingsForm.value.firstName,
            this.settingsForm.value.lastName
          );

          const currentUser = updateUserNames || this.authService.getCurrentUser();
          if (!currentUser) {
            return;
          }

          const updatedUser: AuthResponse = {
            ...currentUser,
            country: this.settingsForm.value.country,
            city: this.settingsForm.value.city,
            token: currentUser.token
          };

          this.authService.setCurrentUser(updatedUser);
          this.router.navigate(['/sections']);
        },
        error: (err) => {
          console.error('Error updating profile', err);
        }
      });
    }
  }

  closeSettings(): void {
    this.router.navigate(['/sections']);
  }
}
