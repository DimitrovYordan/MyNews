import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { TranslateModule } from '@ngx-translate/core';

import { ContactService } from '../../services/contact.service';
import { ModalComponent } from '../../shared/modal/modal.component';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-contact',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule, ModalComponent, TranslateModule],
    templateUrl: './contact.component.html',
    styleUrls: ['./contact.component.scss']
})
export class ContactComponent {
    contactForm: FormGroup;

    showModal: boolean = false;
    modalMessage: string = '';
    modalType: 'success' | 'error' = 'success';

    constructor(
        private fb: FormBuilder,
        private contactService: ContactService,
        private authService: AuthService
    ) {
        this.contactForm = this.fb.group({
            title: ['', [Validators.required, Validators.maxLength(50)]],
            message: ['', [Validators.required, Validators.maxLength(1000)]]
        });
    }

    get title() {
        return this.contactForm.get('title');
    }

    get message() {
        return this.contactForm.get('message');
    }

    sendMessage() {
        if (this.contactForm.invalid) {
            this.contactForm.markAllAsTouched();
            return;
        }

        const currentUser = this.authService.getCurrentUser();
        const userEmail = currentUser?.email || '';

        const payload = {
            title: this.contactForm.value.title,
            message: this.contactForm.value.message,
            fromEmail: userEmail
        };

        this.contactService.sendMessage(payload).subscribe({
            next: () => {
                this.modalMessage = 'SUCCESS_SEND_MESSAGE';
                this.modalType = 'success';
                this.showModal = true;
                this.contactForm.reset();
            },
            error: () => {
                this.modalMessage = 'ERROR_CONTACT';
                this.modalType = 'error';
                this.showModal = true
            }
        });
    }

    closeModal(confirmed: boolean) {
        this.showModal = false;
    }
}
