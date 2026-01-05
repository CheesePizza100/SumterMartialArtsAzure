import { Routes } from '@angular/router';
import { authGuard, adminGuard } from '../../src/app/core/services/auth.guard';

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
    canActivate: [authGuard]
  },
  {
    path: 'instructors',
    loadChildren: () => import('./features/instructors/instructors.routes').then(m => m.INSTRUCTOR_ROUTES)
  },
  {
    path: 'login',
    loadChildren: () => import('./features/login/login.routes').then(m => m.LOGIN_ROUTES)
  },
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.routes').then(m => m.ADMIN_ROUTES),
    canActivate: [authGuard, adminGuard]
  },
];
