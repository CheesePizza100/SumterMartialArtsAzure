import { Routes } from '@angular/router';
import { InstructorDetailsComponent } from './components/instructor-details/instructor-details.component';

export const INSTRUCTOR_ROUTES: Routes = [
  {
    path: ':id',
    component: InstructorDetailsComponent
  }
];
