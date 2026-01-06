import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

export interface CreateLoginDialogData {
  studentName: string;
  suggestedUsername: string;
}

@Component({
  selector: 'app-create-login-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './create-login-dialog.component.html',
  styleUrls: ['./create-login-dialog.component.css']
})
export class CreateLoginDialogComponent {
  username: string;

  constructor(
    public dialogRef: MatDialogRef<CreateLoginDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateLoginDialogData
  ) {
    this.username = data.suggestedUsername;
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onCreate(): void {
    if (this.username.trim()) {
      this.dialogRef.close(this.username.trim());
    }
  }
}
