import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { AuthService } from '../../services/auth.service';
import { ModalComponent } from '../../shared/modal/modal.component';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ModalComponent, TranslateModule],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;

  errorMessage: string | null = null;
  loading: boolean = false;
  showModal: boolean = false;
  modalMessage: string = '';
  modalType: 'success' | 'error' = 'success';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  ngOnInit() {
    this.forgotPasswordForm.valueChanges.subscribe(() => {
      this.errorMessage = null;
    });
  }

  onSubmit(): void {
    this.forgotPasswordForm.markAllAsTouched();

    if (this.forgotPasswordForm.valid) {
      this.loading = true;
      this.errorMessage = null;

      this.authService.forgotPassword(this.forgotPasswordForm.value.email).subscribe({
        next: () => {
          this.showModal = true;
          this.modalType = 'success';
          this.modalMessage = 'MODAL_PASSWORD_RESET';
          this.forgotPasswordForm.reset();
          this.forgotPasswordForm.markAsPristine();
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.showModal = true;
          this.modalType = 'error';
          this.modalMessage = 'MODAL_PASSWORD_FAILED';
        },
      });
    }
  }

  closeModal() {
    this.showModal = false;
  }

  goBack() {
    this.router.navigate(['/login']);
  }

  get formErrors() {
    const controlEmail = this.forgotPasswordForm.get('email');

    if (controlEmail?.hasError('required') && controlEmail.touched) return 'EMAIL_REQUIRED';
    if (controlEmail?.hasError('email') && controlEmail.touched) return 'VALID_EMAIL';

    return null;
  }
}
