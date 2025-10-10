import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { MissingTranslationHandler, TranslateCompiler, TranslateLoader, TranslateParser, TranslateService, TranslateStore } from '@ngx-translate/core';

import { appRoutes } from './app.routes';
import { authInterceptorFn } from './interceptors/auth.interceptor';
import { loadingInterceptorFn } from './interceptors/loading.interceptor';
import { LanguageService } from './services/language.service';
import { MyTranslateLoader } from './app';
import { CustomTranslateCompiler } from './services/custom-translate-compiler';
import { CustomTranslateParser } from './services/custom-translate-parser';
import { CustomMissingTranslationHandler } from './services/custom-missing-translation-handler';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(appRoutes),
    provideHttpClient(withInterceptors([authInterceptorFn, loadingInterceptorFn])),
    LanguageService,
    {
      provide: TranslateLoader,
      useClass: MyTranslateLoader
    },
    {
      provide: TranslateCompiler,
      useClass: CustomTranslateCompiler
    },
    {
      provide: TranslateParser,
      useClass: CustomTranslateParser
    },
    {
      provide: MissingTranslationHandler,
      useClass: CustomMissingTranslationHandler
    },
    TranslateService,
    TranslateStore
  ]
};