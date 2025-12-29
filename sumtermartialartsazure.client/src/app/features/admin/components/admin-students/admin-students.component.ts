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
import { Program, TestHistory, Attendance, Student } from '../../models/student.model'
import { AdminStudentsService } from '../../services/admin-students.service';
import { RecordTestDialogComponent } from '../record-test-dialog/record-test-dialog.component';

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
    'lastTest'
  ];

  constructor(private adminStudentsService: AdminStudentsService,
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
}
