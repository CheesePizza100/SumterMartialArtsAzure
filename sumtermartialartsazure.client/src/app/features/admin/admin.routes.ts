import { Routes } from '@angular/router';
import { AdminPrivateLessonsComponent } from './components/admin-private-lessons/admin-private-lessons.component';
import { AdminStudentsComponent } from './components/admin-students/admin-students.component';
import { AdminInstructorsComponent } from './components/admin-instructors/admin-instructors.component';
import { AnalyticsDashboardComponent } from './components/analytics-dashboard/analytics-dashboard.component';
import { AdminEmailTemplatesComponent } from './components/admin-email-templates/admin-email-templates.component';
import { AdminEmailTemplateEditComponent } from './components/admin-email-template-edit/admin-email-template-edit.component';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'students',
    pathMatch: 'full'
  },
  {
    path: 'students',
    component: AdminStudentsComponent
  },
  {
    path: 'private-lessons',
    component: AdminPrivateLessonsComponent
  },
  {
    path: 'instructors',
    component: AdminInstructorsComponent
  },
  {
    path: 'email-templates',
    component: AdminEmailTemplatesComponent
  },
  {
    path: 'email-templates/:id',
    component: AdminEmailTemplateEditComponent
  },
  {
    path: 'analytics',
    component: AnalyticsDashboardComponent
  }
];
