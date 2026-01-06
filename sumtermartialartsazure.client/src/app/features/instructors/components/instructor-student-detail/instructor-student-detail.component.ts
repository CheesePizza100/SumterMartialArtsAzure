import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { InstructorStudent } from '../../models/instructor.model';
import { InstructorsService } from '../../services/instructors.service';
import { RecordTestDialogComponent } from '../../../admin/components/record-test-dialog/record-test-dialog.component';
import { RecordAttendanceDialogComponent } from '../../../admin/components/record-attendance-dialog/record-attendance-dialog.component';
import { UpdateNotesDialogComponent } from '../update-notes-dialog/update-notes-dialog.component';

@Component({
  selector: 'app-instructor-student-detail',
  standalone: true,
  imports: [
    CommonModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule
  ],
  templateUrl: './instructor-student-detail.component.html',
  styleUrls: ['./instructor-student-detail.component.css']
})
export class InstructorStudentDetailComponent implements OnInit {
  student: InstructorStudent | null = null;
  loading = false;
  error = '';
  activeTab = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private instructorsService: InstructorsService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    const studentId = Number(this.route.snapshot.paramMap.get('id'));
    if (studentId) {
      this.loadStudent(studentId);
    }
  }

  loadStudent(studentId: number): void {
    this.loading = true;
    this.error = '';

    this.instructorsService.getStudentDetail(studentId).subscribe({
      next: (student) => {
        this.student = student;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load student details';
        this.loading = false;
        console.error('Error loading student:', err);
      }
    });
  }

  backToStudents(): void {
    this.router.navigate(['/instructor/dashboard']);
  }

  formatDate(dateStr: string): string {
    return new Date(dateStr).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }

  getResultColor(result: string): string {
    return result.toLowerCase() === 'pass' ? 'primary' : 'warn';
  }

  openRecordTestDialog(): void {
    if (!this.student) return;

    const dialogRef = this.dialog.open(RecordTestDialogComponent, {
      width: '600px',
      data: {
        studentId: this.student.id,
        studentName: this.student.name,
        programs: this.student.programs.map(p => ({
          id: p.programId,
          name: p.programName,
          currentRank: p.currentRank
        }))
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.recordTestResult(result);
      }
    });
  }

  recordTestResult(testData: any): void {
    if (!this.student) return;

    this.instructorsService.recordTestResult(this.student.id, testData).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open('Test result recorded successfully!', 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          this.loadStudent(this.student!.id);
        } else {
          this.snackBar.open(response.message || 'Failed to record test result', 'Close', {
            duration: 5000
          });
        }
      },
      error: (err) => {
        console.error('Error recording test:', err);
        this.snackBar.open('Error recording test result', 'Close', {
          duration: 3000
        });
      }
    });
  }

  openRecordAttendanceDialog(): void {
    if (!this.student) return;

    const dialogRef = this.dialog.open(RecordAttendanceDialogComponent, {
      width: '600px',
      data: {
        studentId: this.student.id,
        studentName: this.student.name,
        programs: this.student.programs.map(p => ({
          programId: p.programId,
          programName: p.programName,
          currentTotal: p.attendance?.total || 0,
          currentLast30Days: p.attendance?.last30Days || 0
        }))
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.recordAttendance(result.programId, result.classesAttended);
      }
    });
  }

  recordAttendance(programId: number, classesAttended: number): void {
    if (!this.student) return;

    this.instructorsService.recordAttendance(
      this.student.id,
      { programId, classesAttended }
    ).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open(response.message, 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          this.loadStudent(this.student!.id);
        } else {
          this.snackBar.open(response.message, 'Close', {
            duration: 5000
          });
        }
      },
      error: (err) => {
        console.error('Error recording attendance:', err);
        this.snackBar.open('Error recording attendance', 'Close', {
          duration: 3000
        });
      }
    });
  }

  openUpdateNotesDialog(program: any): void {
    if (!this.student) return;

    const dialogRef = this.dialog.open(UpdateNotesDialogComponent, {
      width: '600px',
      data: {
        studentName: this.student.name,
        programName: program.programName,
        currentNotes: program.instructorNotes || ''
      }
    });

    dialogRef.afterClosed().subscribe(notes => {
      if (notes !== undefined) {
        this.updateProgramNotes(program.programId, notes);
      }
    });
  }

  updateProgramNotes(programId: number, notes: string): void {
    if (!this.student) return;

    this.instructorsService.updateProgramNotes(this.student.id, programId, notes).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open('Notes updated successfully!', 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          this.loadStudent(this.student!.id);
        } else {
          this.snackBar.open(response.message, 'Close', {
            duration: 5000
          });
        }
      },
      error: (err) => {
        console.error('Error updating notes:', err);
        this.snackBar.open('Error updating notes', 'Close', {
          duration: 3000
        });
      }
    });
  }

  getTotalAttendance(): { total: number; last30Days: number; rate: number } {
    if (!this.student) return { total: 0, last30Days: 0, rate: 0 };

    const total = this.student.programs.reduce((sum, p) => sum + p.attendance.total, 0);
    const last30Days = this.student.programs.reduce((sum, p) => sum + p.attendance.last30Days, 0);
    const rate = this.student.programs.length > 0
      ? Math.round(this.student.programs.reduce((sum, p) => sum + p.attendance.attendanceRate, 0) / this.student.programs.length)
      : 0;
    return { total, last30Days, rate };
  }
}
