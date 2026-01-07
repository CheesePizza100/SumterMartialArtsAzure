import { Routes } from '@angular/router';
import { authGuard } from '../../src/app/core/guards/auth.guard';
import { adminGuard } from '../../src/app/core/guards/admin.guard';
import { passwordChangeGuard } from '../../src/app/core/guards/password-change.guard';
import { AUTH_ROUTES } from './features/auth/auth.routes';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    loadChildren: () => import('./features/home/home.routes').then(m => m.HOME_ROUTES)
  },
  {
    path: 'about',
    loadChildren: () => import('./features/about/about.routes').then(m => m.ABOUT_ROUTES)
  },
  {
    path: 'programs',
    loadChildren: () => import('./features/programs/programs.routes').then(m => m.PROGRAM_ROUTES)
  },
  {
    path: 'student',
    loadChildren: () => import('./features/students/student.routes').then(m => m.STUDENT_ROUTES),
    canActivate: [authGuard, passwordChangeGuard]
  },
  {
    path: 'instructors',
    loadChildren: () => import('./features/instructors/instructors.routes').then(m => m.INSTRUCTOR_ROUTES),
  },
  {
    path: 'login',
    loadChildren: () => import('./features/login/login.routes').then(m => m.LOGIN_ROUTES)
  },
  {
    path: 'change-password',
    loadChildren: () => import('./features/auth/auth.routes').then(m => AUTH_ROUTES),
    canActivate: [authGuard]
  },
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.routes').then(m => m.ADMIN_ROUTES),
    canActivate: [authGuard, adminGuard, passwordChangeGuard]
  },
];
