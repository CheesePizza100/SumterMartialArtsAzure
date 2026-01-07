import { Routes } from '@angular/router';
import { InstructorDetailsComponent } from './components/instructor-details/instructor-details.component';
import { InstructorDashboardComponent } from './components/instructor-dashboard/instructor-dashboard.component';
import { InstructorProfileComponent } from './components/instructor-profile/instructor-profile.component';
import { InstructorStudentDetailComponent } from './components/instructor-student-detail/instructor-student-detail.component';
import { authGuard } from '../../core/guards/auth.guard';
import { passwordChangeGuard } from '../../core/guards/password-change.guard';

export const INSTRUCTOR_ROUTES: Routes = [
  // Public route - no guards
  {
    path: ':id',
    component: InstructorDetailsComponent
  },

  // Protected routes - with guards
  {
    path: 'dashboard',
    component: InstructorDashboardComponent,
    canActivate: [authGuard, passwordChangeGuard]
  },
  {
    path: 'profile',
    component: InstructorProfileComponent,
    canActivate: [authGuard, passwordChangeGuard]
  },
  {
    path: 'students/:id',
    component: InstructorStudentDetailComponent,
    canActivate: [authGuard, passwordChangeGuard]
  }
];
