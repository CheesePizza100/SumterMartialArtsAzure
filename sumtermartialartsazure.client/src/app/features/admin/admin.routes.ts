//import { Routes } from '@angular/router';
//import { AdminPrivateLessonsComponent } from './components/admin-private-lessons/admin-private-lessons.component';

//export const ADMIN_ROUTES: Routes = [
//  {
//    path: '',
//    component: AdminPrivateLessonsComponent
//  }
//];
import { Routes } from '@angular/router';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'private-lessons',
    pathMatch: 'full'
  },
  {
    path: 'private-lessons',
    loadComponent: () => import('./components/admin-private-lessons/admin-private-lessons.component')
      .then(m => m.AdminPrivateLessonsComponent)
  },
  {
    path: 'students',
    loadComponent: () => import('./components/admin-students/admin-students.component')
      .then(m => m.AdminStudentsComponent)
  }
];
