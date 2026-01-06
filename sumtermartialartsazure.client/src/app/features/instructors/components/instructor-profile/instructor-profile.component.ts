import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InstructorProfile } from '../../models/instructor.model';
import { InstructorsService } from '../../services/instructors.service';

@Component({
  selector: 'app-instructor-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './instructor-profile.component.html',
  styles: [`
  `]
})
export class InstructorProfileComponent implements OnInit {
  profile: InstructorProfile | null = null;
  loading = false;
  error = '';

  constructor(private instructorsService: InstructorsService) { }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.loading = true;
    this.error = '';

    this.instructorsService.getMyProfile().subscribe({
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

  getInitials(name: string): string {
    return name
      .split(' ')
      .map(n => n[0])
      .join('')
      .toUpperCase()
      .substring(0, 2);
  }
}
