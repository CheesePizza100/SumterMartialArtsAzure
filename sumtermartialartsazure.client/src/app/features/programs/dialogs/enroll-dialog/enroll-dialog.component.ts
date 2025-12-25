import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Program } from '../../models/program.model';

@Component({
  selector: 'app-enroll-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './enroll-dialog.component.html',
  styleUrls: ['./enroll-dialog.component.css']
})
export class EnrollDialogComponent implements OnInit {
  enrollForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<EnrollDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Program
  ) { }

  ngOnInit(): void {
    this.enrollForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.enrollForm.invalid) return;

    const enrollmentData = {
      programId: this.data.id,
      programName: this.data.name,
      ...this.enrollForm.value
    };

    // TODO: Call enrollment API
    console.log('Enrollment submitted:', enrollmentData);

    this.dialogRef.close(enrollmentData);
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
