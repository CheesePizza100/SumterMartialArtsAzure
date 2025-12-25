import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Instructor } from '../../models/instructor.model';
import { InstructorCardComponent } from '../instructor-card/instructor-card.component';

@Component({
  selector: 'app-instructors-grid',
  standalone: true,
  imports: [CommonModule, InstructorCardComponent],
  templateUrl: './instructors-grid.component.html',
  styleUrls: ['./instructors-grid.component.css']
})
export class InstructorsGridComponent {
  @Input() instructors: Instructor[] = [];
  @Input() showTitle = true;
  @Input() title = 'Meet Our Instructors';
}
