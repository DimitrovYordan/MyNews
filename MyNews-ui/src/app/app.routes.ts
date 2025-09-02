import { Routes } from '@angular/router';

import { NewsListComponent } from './components/news-list/news-list.component';

export const appRoutes: Routes = [
    { path: '', redirectTo: '/news', pathMatch: 'full' },
    { path: 'news', component: NewsListComponent }
];
