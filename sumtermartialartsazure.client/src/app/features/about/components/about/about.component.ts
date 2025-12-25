import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Instructor } from '../../../instructors/models/instructor.model';
import { InstructorsService } from '../../../instructors/services/instructors.service';
import { InstructorsGridComponent } from '../../../instructors/components/instructors-grid/instructors-grid.component';
import { MissionSectionComponent } from '../mission-section/mission-section.component';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [
    CommonModule,
    InstructorsGridComponent,
    MissionSectionComponent
  ],
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  instructors: Instructor[] = [];
  isLoading = true;
  error?: string;

  constructor(private instructorsService: InstructorsService) { }

  ngOnInit(): void {
    this.loadInstructors();
  }

  private loadInstructors(): void {
    this.instructorsService.getInstructors().subscribe({
      next: (data) => {
        this.instructors = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading instructors:', err);
        this.error = 'Failed to load instructors';
        this.isLoading = false;
      }
    });
  }
}
