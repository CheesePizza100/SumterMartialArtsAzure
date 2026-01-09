import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentService, StudentProfile, PrivateLessonRequest } from '../../services/student.service';

@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './student-dashboard.component.html',
  styleUrls: ['./student-dashboard.component.css']
})
export class StudentDashboardComponent implements OnInit {
  profile: StudentProfile | null = null;
  privateLessonRequests: PrivateLessonRequest[] = [];
  loading = false;
  error = '';

  constructor(private studentService: StudentService) { }

  ngOnInit(): void {
    this.loadProfile();
    this.loadPrivateLessonRequests();
  }

  loadProfile(): void {
    this.loading = true;
    this.error = '';

    this.studentService.getMyProfile().subscribe({
      next: (profile) => {
        this.profile = profile;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load profile';
        this.loading = false;
        console.error('Error loading profile:', err);
      }
    });
  }

  loadPrivateLessonRequests(): void {
    this.studentService.getMyPrivateLessonRequests().subscribe({
      next: (requests) => {
        this.privateLessonRequests = requests;
      },
      error: (err) => {
        console.error('Error loading private lesson requests:', err);
        // Don't show error to user - just fail silently for this optional feature
      }
    });
  }
}
