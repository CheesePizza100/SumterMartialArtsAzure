import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

export interface UpdateNotesDialogData {
  studentName: string;
  programName: string;
  currentNotes: string;
}

@Component({
  selector: 'app-update-notes-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './update-notes-dialog.component.html',
  styleUrls: ['./update-notes-dialog.component.css']
})
export class UpdateNotesDialogComponent {
  notes: string;

  constructor(
    public dialogRef: MatDialogRef<UpdateNotesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UpdateNotesDialogData
  ) {
    this.notes = data.currentNotes;
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    this.dialogRef.close(this.notes);
  }
}
