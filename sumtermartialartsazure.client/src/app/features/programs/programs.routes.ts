import { Routes } from '@angular/router';
import { ProgramsListComponent } from './components/programs-list/programs-list.component';
import { ProgramDetailsComponent } from './components/program-details/program-details.component';

export const PROGRAM_ROUTES: Routes = [
  {
    path: '',
    component: ProgramsListComponent
  },
  {
    path: ':id',
    component: ProgramDetailsComponent
  }
];
