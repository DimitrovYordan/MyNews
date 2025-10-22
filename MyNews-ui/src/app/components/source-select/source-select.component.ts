import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { TranslateModule } from "@ngx-translate/core";

import { SourcesService } from "../../services/sources.service";
import { Source } from "../../interfaces/source";
import { UserPreferencesService } from "../../services/user-preferences.service";
import { NamesUtilsService } from "../../shared/names-utils.service";
import { AuthService } from "../../services/auth.service";
import { ModalComponent } from "../../shared/modal/modal.component";

@Component({
    selector: 'app-source-select',
    imports: [ModalComponent, TranslateModule],
    templateUrl: './source-select.component.html',
    styleUrls: ['./source-select.component.scss']
})
export class SourceSelectComponent implements OnInit {
    sources: Source[] = [];
    selectedSources: number[] = [];
    isLoading: boolean = true;
    isAllSelected: boolean = false;

    showModal: boolean = false;
    modalMessage: string = '';
    modalType: 'success' | 'error' = 'success';

    constructor(
        private authService: AuthService,
        private sourcesService: SourcesService,
        private userPreferencesService: UserPreferencesService,
        private utils: NamesUtilsService,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.isLoading = true;

        this.sourcesService.getSources().subscribe(allSources => {
            this.sources = allSources.map(s => ({
                ...s,
                displayName: this.utils.getDomain(s.name)
            }));

            this.userPreferencesService.getUserSources().subscribe(userSources => {
                this.selectedSources = userSources;
                this.isAllSelected = this.selectedSources.length === this.sources.length;
                this.isLoading = false;
            });
        });
    }

    toggleSelection(id: number): void {
        if (this.selectedSources.includes(id)) {
            this.selectedSources = this.selectedSources.filter(s => s !== id);
        } else {
            this.selectedSources.push(id);
        }

        this.isAllSelected = this.selectedSources.length === this.sources.length;
    }

    toggleAll(): void {
        if (this.isAllSelected) {
            this.selectedSources = [];
            this.isAllSelected = false;
        } else {
            this.selectedSources = this.sources.map(s => s.id);
            this.isAllSelected = true;
        }
    }

    saveSelection(): void {
        this.userPreferencesService.saveUserSources(this.selectedSources).subscribe({
            next: () => {
                this.authService.hasSelectedSections$.next(true);
                this.modalMessage = 'SUCCESS_SAVED_SOURCES';
                this.modalType = 'success';
                this.showModal = true;
            },
            error: () => {
                this.modalMessage = 'ERROR_SOURCES';
                this.modalType = 'error';
                this.showModal = true;
            }
        });
    }

    get selectAllButtonText(): string {
        return this.selectedSources.length === this.sources.length ? 'Unselect All' : 'Select All';
    }

    closeModal(): void {
        this.showModal = false;
        if (this.modalType === 'success') {
            this.router.navigate(['/news']);
        }
    }
}