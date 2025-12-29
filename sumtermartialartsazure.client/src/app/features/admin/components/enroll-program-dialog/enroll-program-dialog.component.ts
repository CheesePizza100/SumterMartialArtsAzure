import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

export interface EnrollProgramDialogData {
  studentId: number;
  studentName: string;
  availablePrograms: Array<{
    id: number;
    name: string;
  }>;
}

@Component({
  selector: 'app-enroll-program-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    FormsModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './enroll-program-dialog.component.html',
  styleUrls: ['./enroll-program-dialog.component.css']
})
export class EnrollProgramDialogComponent {
  selectedProgramId?: number;
  initialRank = 'White Belt';
  isSubmitting = false;

  constructor(
    public dialogRef: MatDialogRef<EnrollProgramDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EnrollProgramDialogData
  ) { }

  get selectedProgram() {
    return this.data.availablePrograms.find(p => p.id === this.selectedProgramId);
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (!this.canSubmit) return;

    const program = this.selectedProgram;
    if (!program) return;

    this.dialogRef.close({
      programId: this.selectedProgramId,
      programName: program.name,
      initialRank: this.initialRank.trim()
    });
  }

  get canSubmit(): boolean {
    return !!(
      this.selectedProgramId &&
      this.initialRank.trim() &&
      !this.isSubmitting
    );
  }
}
