import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';

import { NewsListComponent } from './components/news-list/news-list.component';
import { SectionSelectComponent } from './components/section-select/section-select.component';
import { LoginComponent } from './auth/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { SignupComponent } from './auth/signup/signup.component';
import { SettingsComponent } from './auth/settings/settings.component';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'sections', component: SectionSelectComponent, canActivate: [AuthGuard] },
  { path: 'news', component: NewsListComponent, canActivate: [AuthGuard] },
  { path: 'settings', component: SettingsComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' }
];
