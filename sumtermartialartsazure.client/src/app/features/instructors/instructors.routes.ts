import { Routes } from '@angular/router';
import { InstructorDetailsComponent } from './components/instructor-details/instructor-details.component';
import { InstructorDashboardComponent } from './components/instructor-dashboard/instructor-dashboard.component';
import { InstructorProfileComponent } from './components/instructor-profile/instructor-profile.component';
import { InstructorStudentDetailComponent } from './components/instructor-student-detail/instructor-student-detail.component';

 
export const INSTRUCTOR_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: ':id',
    component: InstructorDetailsComponent
  },
  {
    path: 'dashboard',
    component: InstructorDashboardComponent
  },
  {
    path: 'profile',
    component: InstructorProfileComponent
  },
  {
    path: 'students/:id',
    component: InstructorStudentDetailComponent
  }
];
