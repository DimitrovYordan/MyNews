import { Routes } from '@angular/router';

import { NewsListComponent } from './components/news-list/news-list.component';
import { SectionSelectComponent } from './components/section-select/section-select.component';

export const appRoutes: Routes = [
  { path: '', component: SectionSelectComponent },
  { path: 'news', component: NewsListComponent },
];