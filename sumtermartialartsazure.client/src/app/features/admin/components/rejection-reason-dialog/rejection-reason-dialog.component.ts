import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-rejection-reason-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './rejection-reason-dialog.component.html',
  styleUrls: ['./rejection-reason-dialog.component.css']
})
export class RejectionReasonDialogComponent {
  reason = '';

  constructor(
    public dialogRef: MatDialogRef<RejectionReasonDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { studentName: string }
  ) { }

  cancel(): void {
    this.dialogRef.close();
  }

  submit(): void {
    if (this.reason && this.reason.trim().length > 0) {
      this.dialogRef.close(this.reason.trim());
    }
  }
}
