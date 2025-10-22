import { Routes } from '@angular/router';

import { NewsListComponent } from './components/news-list/news-list.component';
import { SectionSelectComponent } from './components/section-select/section-select.component';
import { HomeComponent } from './components/home/home.component';
import { AuthGuard } from './guards/auth.guard';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { LoginComponent } from './auth/login/login.component';
import { SignupComponent } from './auth/signup/signup.component';
import { SettingsComponent } from './auth/settings/settings.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';
import { ContactComponent } from './components/contact/contact.component';
import { SourceSelectComponent } from './components/source-select/source-select.component';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [AuthGuard] },
  { path: 'signup', component: SignupComponent, canActivate: [AuthGuard] },
  { path: 'forgot-password', component: ForgotPasswordComponent, canActivate: [AuthGuard] },
  { path: 'reset-password', component: ResetPasswordComponent, canActivate: [AuthGuard] },

  { path: 'sources', component: SourceSelectComponent, canActivate: [AuthGuard] },
  { path: 'sections', component: SectionSelectComponent, canActivate: [AuthGuard] },
  { path: 'news', component: NewsListComponent, canActivate: [AuthGuard] },
  { path: 'settings', component: SettingsComponent, canActivate: [AuthGuard] },
  { path: 'contact', component: ContactComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
