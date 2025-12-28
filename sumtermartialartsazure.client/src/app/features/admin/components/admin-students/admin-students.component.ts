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
import { FormsModule } from '@angular/forms';
import { Program, TestHistory, Attendance, Student } from '../../models/student.model'
import { AdminStudentsService } from '../../services/admin-students.service';

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

  constructor(private adminStudentsService: AdminStudentsService) { }

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
}
