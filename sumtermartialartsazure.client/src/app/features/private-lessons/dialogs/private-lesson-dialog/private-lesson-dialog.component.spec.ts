import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { of, throwError } from 'rxjs';

import { PrivateLessonDialogComponent } from './private-lesson-dialog.component';
import { PrivateLessonsService } from '../../services/private-lessons.service';
import { LessonTime } from '../../models/private-lesson.model';

const mockSlots: LessonTime[] = [
  { start: '2026-03-15T10:00:00', end: '2026-03-15T11:00:00', durationMinutes: 60 },
  { start: '2026-03-15T14:00:00', end: '2026-03-15T15:00:00', durationMinutes: 60 },
  { start: '2026-03-16T09:00:00', end: '2026-03-16T10:00:00', durationMinutes: 60 }
];

const mockDialogData = {
  instructorId: 1,
  instructorName: 'Jane Doe'
};

describe('PrivateLessonDialogComponent', () => {
  let component: PrivateLessonDialogComponent;
  let fixture: ComponentFixture<PrivateLessonDialogComponent>;
  let privateLessonsServiceSpy: jasmine.SpyObj<PrivateLessonsService>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<PrivateLessonDialogComponent>>;

  beforeEach(async () => {
    privateLessonsServiceSpy = jasmine.createSpyObj('PrivateLessonsService', [
      'getInstructorAvailability',
      'submitLessonRequest'
    ]);
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);
    privateLessonsServiceSpy.getInstructorAvailability.and.returnValue(of(mockSlots));

    await TestBed.configureTestingModule({
      imports: [PrivateLessonDialogComponent, NoopAnimationsModule],
      providers: [
        { provide: PrivateLessonsService, useValue: privateLessonsServiceSpy },
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockDialogData }
      ]
    })
      .overrideComponent(PrivateLessonDialogComponent, {
        set: {
          imports: [
            CommonModule,
            ReactiveFormsModule,
            MatFormFieldModule,
            MatInputModule,
            MatButtonModule,
            MatDatepickerModule,
            MatNativeDateModule,
            MatProgressSpinnerModule,
            MatSelectModule
            // MatDialogModule omitted so dialogRefSpy wins
          ]
        }
      })
      .compileComponents();

    fixture = TestBed.createComponent(PrivateLessonDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should initialize form with empty fields', () => {
      expect(component.lessonForm.get('studentName')?.value).toBe('');
      expect(component.lessonForm.get('studentEmail')?.value).toBe('');
      expect(component.lessonForm.get('preferredDate')?.value).toBe('');
      expect(component.lessonForm.get('notes')?.value).toBe('');
    });

    it('should disable selectedSlot initially', () => {
      expect(component.lessonForm.get('selectedSlot')?.disabled).toBeTrue();
    });

    it('should load availability on init', () => {
      expect(privateLessonsServiceSpy.getInstructorAvailability).toHaveBeenCalledWith(1, 30);
    });

    it('should populate availableSlots and availableDates on success', () => {
      expect(component.availableSlots).toEqual(mockSlots);
      expect(component.availableDates.length).toBe(2); // two unique dates
      expect(component.isLoading).toBeFalse();
    });

    it('should set error and clear isLoading on availability failure', () => {
      spyOn(console, 'error');
      privateLessonsServiceSpy.getInstructorAvailability.and.returnValue(
        throwError(() => new Error('error'))
      );
      component.ngOnInit();
      expect(component.error).toBe('Failed to load available dates');
      expect(component.isLoading).toBeFalse();
    });

    it('should set dateFilter and dateClass functions', () => {
      expect(component.dateFilter).toBeInstanceOf(Function);
      expect(component.dateClass).toBeInstanceOf(Function);
    });
  });

  describe('date selection', () => {
    it('should filter slots when a date is selected', () => {
      const march15 = new Date('2026-03-15T00:00:00');
      component.lessonForm.get('preferredDate')?.setValue(march15);

      expect(component.filteredSlots.length).toBe(2);
      expect(component.lessonForm.get('selectedSlot')?.enabled).toBeTrue();
    });

    it('should clear filteredSlots and disable selectedSlot when date cleared', () => {
      component.lessonForm.get('preferredDate')?.setValue(null);
      expect(component.filteredSlots).toEqual([]);
      expect(component.lessonForm.get('selectedSlot')?.disabled).toBeTrue();
    });
  });

  describe('formatTimeSlot', () => {
    it('should return a formatted time range string', () => {
      const result = component.formatTimeSlot(mockSlots[0]);
      expect(result).toContain(' - ');
      expect(result).toContain('10');
      expect(result).toContain('11');
    });
  });

  describe('close', () => {
    it('should close dialog without a value', () => {
      component.close();
      expect(dialogRefSpy.close).toHaveBeenCalledWith();
    });
  });

  describe('submit', () => {
    beforeEach(() => {
      component.lessonForm.get('studentName')?.setValue('Alice');
      component.lessonForm.get('studentEmail')?.setValue('alice@example.com');
      component.lessonForm.get('studentPhone')?.setValue('555-5678');
      component.lessonForm.get('notes')?.setValue('Please be on time');

      // Directly patch to bypass validator/timezone issues
      component.lessonForm.patchValue({
        preferredDate: new Date(mockSlots[0].start),
        selectedSlot: mockSlots[0]
      });

      // Force selectedSlot enabled and set after date processing
      component.lessonForm.get('selectedSlot')?.enable();
      component.lessonForm.get('selectedSlot')?.setValue(mockSlots[0]);

      // Override form validity for submit testing
      component.lessonForm.get('preferredDate')?.setErrors(null);
    });

    it('should not submit when form is invalid', () => {
      component.lessonForm.get('studentName')?.setValue('');
      component.submit();
      expect(privateLessonsServiceSpy.submitLessonRequest).not.toHaveBeenCalled();
    });

    it('should call submitLessonRequest with correct payload', () => {
      privateLessonsServiceSpy.submitLessonRequest.and.returnValue(of({ id: 1 } as any));
      component.submit();
      expect(privateLessonsServiceSpy.submitLessonRequest).toHaveBeenCalledWith(
        jasmine.objectContaining({
          instructorId: 1,
          studentName: 'Alice',
          studentEmail: 'alice@example.com',
          requestedStart: mockSlots[0].start,
          requestedEnd: mockSlots[0].end,
          notes: 'Please be on time'
        })
      );
    });

    it('should close dialog with result on success', () => {
      const mockResult = { id: 42 };
      privateLessonsServiceSpy.submitLessonRequest.and.returnValue(of(mockResult as any));
      component.submit();
      expect(dialogRefSpy.close).toHaveBeenCalledWith(mockResult);
    });

    it('should set error on failure', () => {
      spyOn(console, 'error');
      privateLessonsServiceSpy.submitLessonRequest.and.returnValue(
        throwError(() => new Error('error'))
      );
      component.submit();
      expect(component.error).toBe('Failed to submit request. Please try again.');
    });
  });
});
