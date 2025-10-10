import { bootstrapApplication } from '@angular/platform-browser';

import { TranslateService } from '@ngx-translate/core';

import { App } from './app/app';
import { appConfig } from './app/app.config';
import { inject, runInInjectionContext } from '@angular/core';

bootstrapApplication(App, {
  ...appConfig,
  providers: [
    ...appConfig.providers
  ]
}).then(appRef => {
    runInInjectionContext(appRef.injector, () => {
      const translate = inject(TranslateService);
      translate.setFallbackLang('en');
      translate.use('bg');
    });
}).catch((err) => console.error(err));