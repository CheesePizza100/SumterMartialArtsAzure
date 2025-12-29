import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

export interface RecordAttendanceDialogData {
  studentId: number;
  studentName: string;
  programs: Array<{
    programId: number;
    programName: string;
    currentTotal: number;
    currentLast30Days: number;
  }>;
}

@Component({
  selector: 'app-record-attendance-dialog',
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
  templateUrl: './record-attendance-dialog.component.html',
  styleUrls: ['./record-attendance-dialog.component.css']
})
export class RecordAttendanceDialogComponent {
  selectedProgramId?: number;
  classesAttended: number = 1;
  isSubmitting = false;

  constructor(
    public dialogRef: MatDialogRef<RecordAttendanceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RecordAttendanceDialogData
  ) { }

  get selectedProgram() {
    return this.data.programs.find(p => p.programId === this.selectedProgramId);
  }

  get currentTotal(): number {
    return this.selectedProgram?.currentTotal || 0;
  }

  get currentLast30Days(): number {
    return this.selectedProgram?.currentLast30Days || 0;
  }

  get newTotal(): number {
    return this.currentTotal + (this.classesAttended || 0);
  }

  get newLast30Days(): number {
    return Math.min(this.currentLast30Days + (this.classesAttended || 0), 30);
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (!this.canSubmit) return;

    this.dialogRef.close({
      programId: this.selectedProgramId,
      classesAttended: this.classesAttended
    });
  }

  get canSubmit(): boolean {
    return !!(
      this.selectedProgramId &&
      this.classesAttended &&
      this.classesAttended > 0 &&
      this.classesAttended <= 30 &&
      !this.isSubmitting
    );
  }
}
