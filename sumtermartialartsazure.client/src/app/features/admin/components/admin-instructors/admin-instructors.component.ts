import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { AdminInstructorsService } from '../../services/admin-instructors.service';
import { CreateLoginDialogComponent } from '../create-login-dialog/create-login-dialog.component';
import { Instructor } from '../../../instructors/models/instructor.model';

@Component({
  selector: 'app-admin-instructors',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './admin-instructors.component.html',
  styleUrls: ['./admin-instructors.component.css']
})
export class AdminInstructorsComponent implements OnInit {
  instructors: Instructor[] = [];
  isLoading = true;

  displayedColumns: string[] = ['name', 'email', 'rank', 'actions'];

  constructor(
    private adminInstructorsService: AdminInstructorsService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadInstructors();
  }

  loadInstructors(): void {
    this.isLoading = true;

    this.adminInstructorsService.getAllInstructors().subscribe({
      next: (instructors) => {
        this.instructors = instructors;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading instructors:', err);
        this.isLoading = false;
        this.snackBar.open('Failed to load instructors', 'Close', {
          duration: 3000
        });
      }
    });
  }

  createLoginForInstructor(instructor: Instructor, event: Event): void {
    event.stopPropagation();

    const dialogRef = this.dialog.open(CreateLoginDialogComponent, {
      width: '500px',
      data: {
        studentName: instructor.name,
        suggestedUsername: instructor.email
      }
    });

    dialogRef.afterClosed().subscribe(username => {
      if (username) {
        this.adminInstructorsService.createInstructorLogin(instructor.id, {
          username,
          password: null
        }).subscribe({
          next: (result) => {
            this.snackBar.open(
              `Login created for ${instructor.name}!\nUsername: ${result.username}\nTemporary Password: ${result.temporaryPassword}\n\nAn email has been sent to the instructor.`,
              'Close',
              {
                duration: 10000,
                panelClass: ['success-snackbar']
              }
            );
            this.loadInstructors();
          },
          error: (err) => {
            const errorMessage = err.error?.message || 'Failed to create login';
            this.snackBar.open(errorMessage, 'Close', {
              duration: 5000,
              panelClass: ['error-snackbar']
            });
            console.error('Error creating login:', err);
          }
        });
      }
    });
  }
}
