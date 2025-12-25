import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PrivateLessonsService } from '../../services/private-lessons.service';
import { PrivateLessonDialogData, PrivateLessonRequest, LessonTime } from '../../models/private-lesson.model';
import { DateFilterUtil } from '../../utils/date-filter.util';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-private-lesson-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatSelectModule
  ],
  templateUrl: './private-lesson-dialog.component.html',
  styleUrls: ['./private-lesson-dialog.component.css']
})
export class PrivateLessonDialogComponent implements OnInit {
  lessonForm!: FormGroup;
  availableSlots: LessonTime[] = []; // Changed from dates to slots
  filteredSlots: LessonTime[] = []; // Slots for selected date
  availableDates: Date[] = [];
  isLoading = true;
  error?: string;
  dateFilter!: (d: Date | null) => boolean;
  dateClass!: (d: Date) => string;

  constructor(
    private fb: FormBuilder,
    private privateLessonsService: PrivateLessonsService,
    private dialogRef: MatDialogRef<PrivateLessonDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PrivateLessonDialogData
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadAvailability();
  }

  private initializeForm(): void {
    this.lessonForm = this.fb.group({
      studentName: ['', Validators.required],
      studentEmail: ['', [Validators.required, Validators.email]],
      studentPhone: [''],
      preferredDate: ['', Validators.required],
      selectedSlot: [{ value: '', disabled: true }, Validators.required], // Disabled until date selected
      notes: ['']
    });

    // Watch for date changes to update available time slots
    this.lessonForm.get('preferredDate')?.valueChanges.subscribe(date => {
      this.onDateSelected(date);
    });
  }

  private loadAvailability(): void {
    // Fetch available slots for next 30 days (adjust as needed)
    this.privateLessonsService.getInstructorAvailability(this.data.instructorId, 30).subscribe({
      next: (slots) => {
        this.availableSlots = slots;

        // Extract unique dates from slots
        this.availableDates = [...new Set(
          slots.map(slot => new Date(slot.start).toDateString())
        )].map(dateStr => new Date(dateStr));

        this.dateFilter = DateFilterUtil.createDateFilter(this.availableDates);
        this.dateClass = DateFilterUtil.createDateClass(this.availableDates);
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading availability:', err);
        this.error = 'Failed to load available dates';
        this.isLoading = false;
      }
    });
  }

  private onDateSelected(date: Date | null): void {
    if (!date) {
      this.filteredSlots = [];
      this.lessonForm.get('selectedSlot')?.disable();
      this.lessonForm.get('selectedSlot')?.setValue('');
      return;
    }

    // Filter slots for the selected date
    const selectedDateStr = date.toDateString();
    this.filteredSlots = this.availableSlots.filter(slot => {
      const slotDate = new Date(slot.start);
      return slotDate.toDateString() === selectedDateStr;
    });

    // Enable the time slot selector
    this.lessonForm.get('selectedSlot')?.enable();
    this.lessonForm.get('selectedSlot')?.setValue('');
  }

  formatTimeSlot(slot: LessonTime): string {
    const start = new Date(slot.start);
    const end = new Date(slot.end);
    return `${start.toLocaleTimeString('en-US', {
      hour: 'numeric',
      minute: '2-digit'
    })} - ${end.toLocaleTimeString('en-US', {
      hour: 'numeric',
      minute: '2-digit'
    })}`;
  }

  submit(): void {
    if (this.lessonForm.invalid) return;

    const formValue = this.lessonForm.value;
    const selectedSlot = formValue.selectedSlot as LessonTime;

    const command: PrivateLessonRequest = {
      instructorId: this.data.instructorId,
      studentName: formValue.studentName,
      studentEmail: formValue.studentEmail,
      studentPhone: formValue.studentPhone,
      requestedStart: selectedSlot.start,
      requestedEnd: selectedSlot.end,
      notes: formValue.notes
    };

    this.privateLessonsService.submitLessonRequest(command).subscribe({
      next: (result) => {
        this.dialogRef.close(result);
      },
      error: (err) => {
        console.error('Error submitting request:', err);
        this.error = 'Failed to submit request. Please try again.';
      }
    });
  }

  close(): void {
    this.dialogRef.close();
  }
}
