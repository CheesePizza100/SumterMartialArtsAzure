import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Instructor } from '../../models/instructor.model';

@Component({
  selector: 'app-instructor-card',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './instructor-card.component.html',
  styleUrls: ['./instructor-card.component.css']
})
export class InstructorCardComponent {
  @Input() instructor!: Instructor;
}
