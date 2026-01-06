import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StudentService, StudentProfile } from '../../services/student.service';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './student-profile.component.html',
  styleUrls: ['./student-profile.component.css']
})
export class StudentProfileComponent implements OnInit {
  profile: StudentProfile | null = null;
  loading = false;
  saving = false;
  error = '';
  successMessage = '';

  constructor(private studentService: StudentService) { }

  ngOnInit(): void {
    this.loadProfile();
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

  updateProfile(): void {
    if (!this.profile) return;

    this.saving = true;
    this.error = '';
    this.successMessage = '';

    this.studentService.updateMyContactInfo({
      name: this.profile.name,
      email: this.profile.email,
      phone: this.profile.phone
    }).subscribe({
      next: () => {
        this.saving = false;
        this.successMessage = 'Profile updated successfully!';
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (err) => {
        this.saving = false;
        this.error = 'Failed to update profile';
        console.error('Error updating profile:', err);
      }
    });
  }
}
