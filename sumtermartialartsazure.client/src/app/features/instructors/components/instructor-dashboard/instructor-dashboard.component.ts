import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { InstructorStudent } from '../../models/instructor.model';
import { InstructorsService } from '../../services/instructors.service';

@Component({
  selector: 'app-instructor-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './instructor-dashboard.component.html',
  styleUrls: ['./instructor-dashboard.component.css']
})
export class InstructorDashboardComponent implements OnInit {
  students: InstructorStudent[] = [];
  loading = false;
  error = '';

  constructor(
    private instructorsService: InstructorsService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.loading = true;
    this.error = '';

    this.instructorsService.getMyStudents().subscribe({
      next: (students) => {
        this.students = students;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load students';
        this.loading = false;
        console.error('Error loading students:', err);
      }
    });
  }

  getTotalAttendance(student: InstructorStudent): { last30Days: number; rate: number } {
    const last30Days = student.programs.reduce((sum, p) => sum + p.attendance.last30Days, 0);
    const rate = student.programs.length > 0
      ? Math.round(student.programs.reduce((sum, p) => sum + p.attendance.attendanceRate, 0) / student.programs.length)
      : 0;
    return { last30Days, rate };
  }

  viewStudent(studentId: number): void {
    this.router.navigate(['/instructor/students', studentId]);
  }
}
