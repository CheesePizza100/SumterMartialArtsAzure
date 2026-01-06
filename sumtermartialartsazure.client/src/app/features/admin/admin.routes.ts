import { Routes } from '@angular/router';
import { AdminPrivateLessonsComponent } from './components/admin-private-lessons/admin-private-lessons.component';
import { AdminStudentsComponent } from './components/admin-students/admin-students.component';
import { AdminInstructorsComponent } from './components/admin-instructors/admin-instructors.component';
import { AnalyticsDashboardComponent } from './components/analytics-dashboard/analytics-dashboard.component';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'private-lessons',
    pathMatch: 'full'
  },
  {
    path: 'private-lessons',
    component: AdminPrivateLessonsComponent
  },
  {
    path: 'students',
    component: AdminStudentsComponent
  },
  {
    path: 'instructors',
    component: AdminInstructorsComponent
  },
  {
    path: 'analytics',
    component: AnalyticsDashboardComponent
  }
];
