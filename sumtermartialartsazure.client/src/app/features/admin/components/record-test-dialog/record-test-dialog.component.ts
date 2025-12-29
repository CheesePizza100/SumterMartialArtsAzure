import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule } from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';

export interface RecordTestDialogData {
  studentId: number;
  studentName: string;
  programs: Array<{
    id: number;
    name: string;
    currentRank: string;
  }>;
}

@Component({
  selector: 'app-record-test-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    FormsModule,
    MatProgressSpinnerModule,
    MatIconModule
  ],
  templateUrl: './record-test-dialog.component.html',
  styleUrls: ['./record-test-dialog.component.css']
})
export class RecordTestDialogComponent {
  selectedProgramId?: number;
  rankTested = '';
  result: 'Pass' | 'Fail' = 'Pass';
  notes = '';
  testDate = new Date();
  isSubmitting = false;

  constructor(
    public dialogRef: MatDialogRef<RecordTestDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RecordTestDialogData
  ) { }

  get selectedProgram() {
    return this.data.programs.find(p => p.id === this.selectedProgramId);
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (!this.selectedProgramId || !this.rankTested.trim() || !this.notes.trim()) {
      return;
    }

    const program = this.selectedProgram;
    if (!program) return;

    this.dialogRef.close({
      programId: this.selectedProgramId,
      programName: program.name,
      rank: this.rankTested.trim(),
      result: this.result,
      notes: this.notes.trim(),
      testDate: this.testDate.toISOString()
    });
  }

  get canSubmit(): boolean {
    return !!(
      this.selectedProgramId &&
      this.rankTested.trim() &&
      this.notes.trim() &&
      !this.isSubmitting
    );
  }
}
