import { Routes } from '@angular/router';

export const STUDENT_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./components/student-dashboard/student-dashboard.component')
      .then(m => m.StudentDashboardComponent)
  },
  {
    path: 'profile',
    loadComponent: () => import('./components/student-profile/student-profile.component')
      .then(m => m.StudentProfileComponent)
  }
];
