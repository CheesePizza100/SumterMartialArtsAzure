import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError, Subject } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

import { InstructorDetailsComponent } from './instructor-details.component';
import { InstructorsService } from '../../services/instructors.service';
import { Instructor } from '../../models/instructor.model';

@Component({ selector: 'app-belt-timeline', standalone: true, template: '' })
class MockBeltTimelineComponent {
  @Input() currentRank = '';
}

const mockInstructor: Instructor = {
  id: 1,
  name: 'Jane Doe',
  email: 'jane@example.com',
  rank: 'Black',
  bio: 'Experienced instructor',
  photoUrl: 'photo.jpg',
  programIds: [],
  achievements: [],
  specialties: [],
  yearsOfExperience: 10
};

describe('InstructorDetailsComponent', () => {
  let component: InstructorDetailsComponent;
  let fixture: ComponentFixture<InstructorDetailsComponent>;
  let instructorsServiceSpy: jasmine.SpyObj<InstructorsService>;
  let dialogSpy: jasmine.SpyObj<MatDialog>;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;

  const createComponent = async (paramId: string) => {
    await TestBed.configureTestingModule({
      imports: [InstructorDetailsComponent, NoopAnimationsModule],
      providers: [
        { provide: InstructorsService, useValue: instructorsServiceSpy },
        { provide: MatDialog, useValue: dialogSpy },
        { provide: MatSnackBar, useValue: snackBarSpy },
        provideHttpClient(withInterceptorsFromDi()),
        provideHttpClientTesting(),
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: { get: () => paramId } } }
        }
      ]
    })
      .overrideComponent(InstructorDetailsComponent, {
        set: {
          imports: [
            CommonModule,
            RouterModule,
            MatButtonModule,
            MockBeltTimelineComponent
          ]
        }
      })
      .compileComponents();

    fixture = TestBed.createComponent(InstructorDetailsComponent);
    component = fixture.componentInstance;
  };

  beforeEach(() => {
    instructorsServiceSpy = jasmine.createSpyObj('InstructorsService', ['getInstructorById']);
    dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
    snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);
  });

  describe('loadInstructor', () => {
    it('should create and load instructor on valid id', async () => {
      instructorsServiceSpy.getInstructorById.and.returnValue(of(mockInstructor));
      await createComponent('1');
      fixture.detectChanges();

      expect(component).toBeTruthy();
      expect(instructorsServiceSpy.getInstructorById).toHaveBeenCalledWith(1);
      expect(component.instructor).toEqual(mockInstructor);
      expect(component.isLoading).toBeFalse();
    });

    it('should set error when id is not a number', async () => {
      await createComponent('abc');
      fixture.detectChanges();

      expect(component.error).toBe('Invalid instructor ID');
      expect(component.isLoading).toBeFalse();
      expect(instructorsServiceSpy.getInstructorById).not.toHaveBeenCalled();
    });

    it('should set error on service failure', async () => {
      spyOn(console, 'error');
      instructorsServiceSpy.getInstructorById.and.returnValue(
        throwError(() => new Error('Network error'))
      );
      await createComponent('1');
      fixture.detectChanges();

      expect(component.error).toBe('Failed to load instructor details');
      expect(component.isLoading).toBeFalse();
    });
  });

  describe('openPrivateLessonDialog', () => {
    let afterClosedSubject: Subject<any>;

    beforeEach(async () => {
      instructorsServiceSpy.getInstructorById.and.returnValue(of(mockInstructor));
      await createComponent('1');
      fixture.detectChanges();

      afterClosedSubject = new Subject<any>();
      dialogSpy.open.and.returnValue({ afterClosed: () => afterClosedSubject.asObservable() } as any);
    });

    it('should do nothing if instructor is not loaded', () => {
      component.instructor = undefined;
      component.openPrivateLessonDialog();
      expect(dialogSpy.open).not.toHaveBeenCalled();
    });

    it('should open dialog with correct data', () => {
      component.openPrivateLessonDialog();
      expect(dialogSpy.open).toHaveBeenCalledWith(
        jasmine.any(Function),
        {
          width: '500px',
          data: { instructorId: 1, instructorName: 'Jane Doe' }
        }
      );
    });

    it('should show snackbar when dialog returns a result', () => {
      component.openPrivateLessonDialog();
      afterClosedSubject.next(true);

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        jasmine.stringContaining('private lesson request has been submitted'),
        'Close',
        jasmine.objectContaining({ duration: 5000 })
      );
    });

    it('should not show snackbar when dialog is cancelled', () => {
      component.openPrivateLessonDialog();
      afterClosedSubject.next(null);

      expect(snackBarSpy.open).not.toHaveBeenCalled();
    });
  });
});
