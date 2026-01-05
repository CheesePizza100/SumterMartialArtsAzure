import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { Student } from '../../models/student.model'
import { AdminStudentsService } from '../../services/admin-students.service';
import { RecordTestDialogComponent } from '../record-test-dialog/record-test-dialog.component';
import { CreateStudentDialogComponent } from '../create-student-dialog/create-student-dialog.component';
import { EnrollProgramDialogComponent } from '../enroll-program-dialog/enroll-program-dialog.component';
import { ProgramsService } from '../../../programs/services/programs.service';
import { RecordAttendanceDialogComponent } from '../record-attendance-dialog/record-attendance-dialog.component';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-admin-students',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatTabsModule,
    MatChipsModule,
    MatSnackBarModule,
    FormsModule
  ],
  templateUrl: './admin-students.component.html',
  styleUrls: ['./admin-students.component.css']
})
export class AdminStudentsComponent implements OnInit {
  students: Student[] = [];
  filteredStudents: Student[] = [];
  selectedStudent: Student | null = null;
  searchTerm = '';
  isLoading = true;
  activeTab = 0;
  error?: string;

  displayedColumns: string[] = [
    'name',
    'contact',
    'programs',
    'attendance',
    'lastTest',
    'actions'
  ];

  constructor(private adminStudentsService: AdminStudentsService,
              private programsService: ProgramsService,
              private dialog: MatDialog,
              private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.isLoading = true;
    this.error = undefined;

    this.adminStudentsService.getAllStudents().subscribe({
      next: (students) => {
        this.students = students;
        this.filteredStudents = students;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading students:', err);
        this.error = 'Failed to load students. Please try again.';
        this.isLoading = false;
      }
    });
  }

  onSearchChange(): void {
    if (!this.searchTerm.trim()) {
      this.filteredStudents = this.students;
      return;
    }

    // Use backend search if available, otherwise filter locally
    this.adminStudentsService.searchStudents(this.searchTerm).subscribe({
      next: (students) => {
        this.filteredStudents = students;
      },
      error: (err) => {
        console.error('Error searching students:', err);
        // Fallback to local filtering
        const term = this.searchTerm.toLowerCase();
        this.filteredStudents = this.students.filter(student =>
          student.name.toLowerCase().includes(term) ||
          student.email.toLowerCase().includes(term) ||
          student.programs.some(p => p.name.toLowerCase().includes(term))
        );
      }
    });
  }

  selectStudent(student: Student): void {
    this.selectedStudent = student;
    this.activeTab = 0;
  }

  backToList(): void {
    this.selectedStudent = null;
  }

  formatDate(dateStr: string): string {
    return new Date(dateStr).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }

  getLastTestDate(student: Student): string {
    if (student.testHistory.length === 0) {
      return 'No tests yet';
    }
    return this.formatDate(student.testHistory[0].date);
  }

  getResultColor(result: string): string {
    return result.toLowerCase() === 'pass' ? 'primary' : 'warn';
  }

  openRecordTestDialog(): void {
    if (!this.selectedStudent) return;

    const dialogRef = this.dialog.open(RecordTestDialogComponent, {
      width: '600px',
      data: {
        studentId: this.selectedStudent.id,
        studentName: this.selectedStudent.name,
        programs: this.selectedStudent.programs.map(p => ({
          id: p.programId,
          name: p.name,
          currentRank: p.rank
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
    if (!this.selectedStudent) return;

    this.adminStudentsService.addTestResult(this.selectedStudent.id, testData).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open('Test result recorded successfully!', 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          // Reload the student data
          this.loadStudents();
          if (this.selectedStudent) {
            this.adminStudentsService.getStudentById(this.selectedStudent.id).subscribe({
              next: (student) => {
                this.selectedStudent = student;
              }
            });
          }
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

  openCreateStudentDialog(): void {
    const dialogRef = this.dialog.open(CreateStudentDialogComponent, {
      width: '600px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.createStudent(result);
      }
    });
  }

  createStudent(studentData: { name: string; email: string; phone: string }): void {
    this.adminStudentsService.createStudent(studentData).subscribe({
      next: (student) => {
        this.snackBar.open('Student created successfully!', 'Close', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        // Reload students list
        this.loadStudents();
      },
      error: (err) => {
        console.error('Error creating student:', err);
        this.snackBar.open('Error creating student', 'Close', {
          duration: 3000
        });
      }
    });
  }

  createLoginForStudent(student: Student, event: Event): void {
    event.stopPropagation(); // Prevent row click from firing

    const username = prompt(`Enter username for ${student.name}:`, student.email);
    if (!username) return;

    this.adminStudentsService.createStudentLogin(student.id, {
      username,
      password: null
    }).subscribe({
      next: (result) => {
        this.snackBar.open(
          `Login created for ${student.name}!\nUsername: ${result.username}\nTemporary Password: ${result.temporaryPassword}\n\nAn email has been sent to the student.`,
          'Close',
          {
            duration: 10000,
            panelClass: ['success-snackbar']
          }
        );
        // Optionally reload the student list to update any UI
        this.loadStudents();
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

  openEnrollProgramDialog(): void {
    if (!this.selectedStudent) return;

    // Get programs the student is NOT already enrolled in
    const enrolledProgramIds = this.selectedStudent.programs.map(p => p.programId);

    this.programsService.getPrograms().subscribe(allPrograms => {
      const availablePrograms = allPrograms.filter(
        p => !enrolledProgramIds.includes(p.id)
      );

      const dialogRef = this.dialog.open(EnrollProgramDialogComponent, {
        width: '600px',
        data: {
          studentId: this.selectedStudent!.id,
          studentName: this.selectedStudent!.name,
          availablePrograms: availablePrograms.map(p => ({
            id: p.id,
            name: p.name
          }))
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.enrollInProgram(result);
        }
      });
    });
  }

  enrollInProgram(enrollmentData: {
    programId: number;
    programName: string;
    initialRank: string;
  }): void {
    if (!this.selectedStudent) return;

    this.adminStudentsService.enrollInProgram(
      this.selectedStudent.id,
      enrollmentData
    ).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open(response.message, 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          // Reload the student data
          this.adminStudentsService.getStudentById(this.selectedStudent!.id).subscribe({
            next: (student) => {
              this.selectedStudent = student;
            }
          });
        } else {
          this.snackBar.open(response.message, 'Close', {
            duration: 5000
          });
        }
      },
      error: (err) => {
        console.error('Error enrolling student:', err);
        this.snackBar.open('Error enrolling student in program', 'Close', {
          duration: 3000
        });
      }
    });
  }

  openRecordAttendanceDialog(): void {
    if (!this.selectedStudent) return;

    const dialogRef = this.dialog.open(RecordAttendanceDialogComponent, {
      width: '600px',
      data: {
        studentId: this.selectedStudent.id,
        studentName: this.selectedStudent.name,
        programs: this.selectedStudent.programs.map(p => ({
          programId: p.programId,
          programName: p.name,
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
    if (!this.selectedStudent) return;

    this.adminStudentsService.recordAttendance(
      this.selectedStudent.id,
      programId,
      classesAttended
    ).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open(response.message, 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          this.adminStudentsService.getStudentById(this.selectedStudent!.id).subscribe({
            next: (student) => {
              this.selectedStudent = student;
            }
          });
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
  getTotalAttendance(student: Student): { total: number; last30Days: number; rate: number } {
    const total = student.programs.reduce((sum, p) => sum + p.attendance.total, 0);
    const last30Days = student.programs.reduce((sum, p) => sum + p.attendance.last30Days, 0);
    const rate = student.programs.length > 0
      ? Math.round(student.programs.reduce((sum, p) => sum + p.attendance.attendanceRate, 0) / student.programs.length)
      : 0;
    return { total, last30Days, rate };
  }

  openDeactivateStudentDialog(): void {
    if (!this.selectedStudent) return;

    // Use Material Dialog for confirmation
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Deactivate Student',
        message: `Are you sure you want to deactivate ${this.selectedStudent.name}? This will deactivate all their program enrollments. You can reactivate them later if needed.`,
        confirmText: 'Deactivate',
        cancelText: 'Cancel',
        isDestructive: true
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.deactivateStudent();
      }
    });
  }

  deactivateStudent(): void {
    if (!this.selectedStudent) return;

    this.adminStudentsService.deactivateStudent(this.selectedStudent.id).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open(response.message, 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          // Go back to list and reload
          this.backToList();
          this.loadStudents();
        } else {
          this.snackBar.open(response.message, 'Close', {
            duration: 5000
          });
        }
      },
      error: (err) => {
        console.error('Error deactivating student:', err);
        this.snackBar.open('Error deactivating student', 'Close', {
          duration: 3000
        });
      }
    });
  }
}
