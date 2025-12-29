import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-create-student-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './create-student-dialog.component.html',
  styleUrls: ['./create-student-dialog.component.css']
})
export class CreateStudentDialogComponent {
  name = '';
  email = '';
  phone = '';
  isSubmitting = false;

  constructor(
    public dialogRef: MatDialogRef<CreateStudentDialogComponent>
  ) { }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (!this.canSubmit) return;

    this.dialogRef.close({
      name: this.name.trim(),
      email: this.email.trim(),
      phone: this.phone.trim()
    });
  }

  get canSubmit(): boolean {
    return !!(
      this.name.trim() &&
      this.email.trim() &&
      this.isValidEmail(this.email) &&
      this.phone.trim() &&
      !this.isSubmitting
    );
  }

  isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }
}
