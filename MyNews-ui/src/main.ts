import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import { appRoutes } from './app/app.routes';
import { NewsListComponent } from './app/components/news-list/news-list.component';

bootstrapApplication(NewsListComponent, {
  providers: [
    provideRouter(appRoutes),
    provideHttpClient(withInterceptorsFromDi())
  ]
}).catch((err) => console.error(err));
