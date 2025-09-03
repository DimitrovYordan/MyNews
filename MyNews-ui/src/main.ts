import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

import { appRoutes } from './app/app.routes';
import { SectionSelectComponent } from './app/components/section-select/section-select.component';

bootstrapApplication(SectionSelectComponent, {
  providers: [
    provideRouter(appRoutes),
    provideHttpClient()
  ]
}).catch((err) => console.error(err));
