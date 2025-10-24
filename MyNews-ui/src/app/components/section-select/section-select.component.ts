import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ActivatedRoute, Router } from "@angular/router";

import { TranslateModule } from "@ngx-translate/core";

import { SectionService } from "../../services/section.service";
import { UserPreferencesService } from "../../services/user-preferences.service";
import { Section } from "../../interfaces/section";
import { NamesUtilsService } from "../../shared/names-utils.service";
import { ModalComponent } from "../../shared/modal/modal.component";
import { AuthService } from "../../services/auth.service";

@Component({
    selector: 'app-section-select',
    standalone: true,
    imports: [CommonModule, ModalComponent, TranslateModule],
    templateUrl: './section-select.component.html',
    styleUrls: ['section-select.component.scss'],
})
export class SectionSelectComponent implements OnInit {
    sections: Section[] = [];
    selectedSections: number[] = [];
    showSections: boolean = true;
    isLoading: boolean = false;
    isAllSelected: boolean = false;
    isOnboarding: boolean = false;

    showModal: boolean = false;
    modalMessage: string = '';
    modalType: 'success' | 'error' = 'success';

    constructor(
        private authService: AuthService,
        private sectionService: SectionService,
        private userPreferencesService: UserPreferencesService,
        private router: Router,
        private namesUtilsService: NamesUtilsService,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.route.queryParams.subscribe(params => {
            this.isOnboarding = params['onboarding'] === 'true';
        })

        this.sectionService.getSections().subscribe(data => {
            this.sections = data.map(s => ({
                ...s, displayName: this.namesUtilsService.formatSectionName(s.name)
            }));

            this.userPreferencesService.getUserSections().subscribe(userSections => {
                this.selectedSections = userSections;
                this.isAllSelected = this.selectedSections.length === this.sections.length;
            });
        });
    }

    toggleSelection(id: number): void {
        if (this.selectedSections.includes(id)) {
            this.selectedSections = this.selectedSections.filter(s => s !== id);
        }
        else {
            this.selectedSections.push(id);
        }
    }

    toggleAll(): void {
        if (this.isAllSelected) {
            this.selectedSections = [];
            this.isAllSelected = false;
        } else {
            this.selectedSections = this.sections.map(s => s.id);
            this.isAllSelected = true;
        }
    }

    saveSelection(): void {
        this.userPreferencesService.saveUserSections(this.selectedSections).subscribe({
            next: () => {
                this.authService.hasSelectedSections$.next(true);
                this.modalMessage = 'SUCCESS_SAVED_SECTIONS';
                this.modalType = 'success';
                this.showModal = true;
            },
            error: (err) => {
                this.modalMessage = 'ERROR_SECTIONS';
                this.modalType = 'error';
                this.showModal = true;
            }
        });
    }

    onCheckboxChange(section: Section, event: Event) {
        const input = event.target as HTMLInputElement;
        if (input.checked) {
            if (!this.selectedSections.includes(section.id)) {
                this.selectedSections.push(section.id)
            }
        } else {
            this.selectedSections = this.selectedSections.filter(s => s !== section.id)
        }
    }

    selectAll(): void {
        this.selectedSections = this.sections.map(s => s.id);
    }

    get selectAllButtonText(): string {
        return this.selectedSections.length === this.sections.length ? 'Unselect All' : 'Select All';
    }

    closeModal(confirmed: boolean) {
        this.showModal = false;
        if (this.modalType === 'success') {
            if (this.isOnboarding) {
                this.router.navigate(['/sources'], { queryParams: { onboarding: 'true' } });
            } else {
                this.router.navigate(['/news']);
            }
        }
    }
}