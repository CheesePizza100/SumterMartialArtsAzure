import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Program } from '../../models/program.model';

@Component({
  selector: 'app-program-card',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './program-card.component.html',
  styleUrls: ['./program-card.component.css']
})
export class ProgramCardComponent {
  @Input() program!: Program;
}
