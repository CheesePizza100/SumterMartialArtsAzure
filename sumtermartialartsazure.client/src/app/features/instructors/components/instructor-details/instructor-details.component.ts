import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { InstructorsService } from '../../services/instructors.service';
import { Instructor } from '../../models/instructor.model';
import { BeltTimelineComponent } from '../belt-timeline/belt-timeline.component';
import { PrivateLessonDialogComponent } from '../../../private-lessons/dialogs/private-lesson-dialog/private-lesson-dialog.component';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-instructor-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatDialogModule,
    MatButtonModule,
    BeltTimelineComponent,
    MatSnackBarModule
  ],
  templateUrl: './instructor-details.component.html',
  styleUrls: ['./instructor-details.component.css']
})
export class InstructorDetailsComponent implements OnInit {
  instructor?: Instructor;
  isLoading = true;
  error?: string;

  constructor(
    private route: ActivatedRoute,
    private instructorsService: InstructorsService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadInstructor();
  }

  private loadInstructor(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (isNaN(id)) {
      this.error = 'Invalid instructor ID';
      this.isLoading = false;
      return;
    }

    this.instructorsService.getInstructorById(id).subscribe({
      next: (data) => {
        this.instructor = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading instructor:', err);
        this.error = 'Failed to load instructor details';
        this.isLoading = false;
      }
    });
  }
  openPrivateLessonDialog(): void {
    if (!this.instructor) return;

    const dialogRef = this.dialog.open(PrivateLessonDialogComponent, {
      width: '500px',
      data: {
        instructorId: this.instructor.id,
        instructorName: this.instructor.name
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.snackBar.open(
          'Your private lesson request has been submitted! We\'ll contact you soon.',
          'Close',
          {
            duration: 5000, // 5 seconds
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
            panelClass: ['success-snackbar'] // optional custom styling
          }
        );
      }
    });
  }
}
