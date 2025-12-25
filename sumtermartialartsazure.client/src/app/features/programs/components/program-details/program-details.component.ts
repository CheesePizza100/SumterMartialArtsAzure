import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ProgramsService } from '../../services/programs.service';
import { Program } from '../../models/program.model';
import { EnrollDialogComponent } from '../../dialogs/enroll-dialog/enroll-dialog.component';

@Component({
  selector: 'app-program-details',
  standalone: true,
  imports: [CommonModule, RouterModule, MatDialogModule],
  templateUrl: './program-details.component.html',
  styleUrls: ['./program-details.component.css']
})
export class ProgramDetailsComponent implements OnInit {
  program?: Program;
  isLoading = true;
  error?: string;

  constructor(
    private route: ActivatedRoute,
    private programsService: ProgramsService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadProgram();
  }

  private loadProgram(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (isNaN(id)) {
      this.error = 'Invalid program ID';
      this.isLoading = false;
      return;
    }

    this.programsService.getProgramById(id).subscribe({
      next: (data) => {
        this.program = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading program:', err);
        this.error = 'Failed to load program details';
        this.isLoading = false;
      }
    });
  }

  openEnrollDialog(): void {
    if (!this.program) return;

    this.dialog.open(EnrollDialogComponent, {
      width: '400px',
      data: this.program
    });
  }
}
