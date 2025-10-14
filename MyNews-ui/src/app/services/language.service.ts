import { BehaviorSubject } from "rxjs";

export class LanguageService {
    private readonly storageKey = 'preferredLanguage';
    private currentLang$ = new BehaviorSubject<string>(this.getSavedLanguage());

    public language$ = this.currentLang$.asObservable();

    setLanguage(lang: string) {
        if (lang === 'DEFAULT') {
            localStorage.removeItem(this.storageKey);
            this.currentLang$.next('DEFAULT');
        } else {
            localStorage.setItem(this.storageKey, lang.toUpperCase());
            this.currentLang$.next(lang.toUpperCase());
        }
    }

    getLanguage(): string {
        return this.currentLang$.getValue();
    }

    private getSavedLanguage() {
        return localStorage.getItem(this.storageKey) || 'DEFAULT';
    }   
}