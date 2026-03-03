import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError, Subject } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { AdminInstructorsComponent } from './admin-instructors.component';
import { AdminInstructorsService } from '../../services/admin-instructors.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Instructor } from '../../../instructors/models/instructor.model';

const mockInstructors: Instructor[] = [
  { id: 1, name: 'Jane Doe', email: 'jane@example.com', rank: 'Black Belt' } as Instructor,
  { id: 2, name: 'John Smith', email: 'john@example.com', rank: 'Brown Belt' } as Instructor,
];

describe('AdminInstructorsComponent', () => {
  let component: AdminInstructorsComponent;
  let fixture: ComponentFixture<AdminInstructorsComponent>;
  let adminInstructorsServiceSpy: jasmine.SpyObj<AdminInstructorsService>;
  let dialogSpy: jasmine.SpyObj<MatDialog>;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;

  beforeEach(async () => {
    adminInstructorsServiceSpy = jasmine.createSpyObj('AdminInstructorsService', [
      'getAllInstructors',
      'createInstructorLogin'
    ]);
    dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
    snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);

    await TestBed.configureTestingModule({
      imports: [AdminInstructorsComponent, NoopAnimationsModule],
      providers: [
        { provide: AdminInstructorsService, useValue: adminInstructorsServiceSpy },
        { provide: MatDialog, useValue: dialogSpy },
        { provide: MatSnackBar, useValue: snackBarSpy }
      ]
    })
    .overrideComponent(AdminInstructorsComponent, {
      set: {
        imports: [
          CommonModule,
          MatTableModule,
          MatButtonModule,
          MatIconModule,
          MatProgressSpinnerModule,
        ]
      }
    })
    .compileComponents();
    fixture = TestBed.createComponent(AdminInstructorsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    adminInstructorsServiceSpy.getAllInstructors.and.returnValue(of([]));
    fixture.detectChanges();
    expect(component).toBeTruthy();

    // temporary debug line
    const snackBar = TestBed.inject(MatSnackBar);
    console.log('snackBar is spy?', snackBar === snackBarSpy);
  });

  describe('loadInstructors', () => {
    it('should set instructors and clear isLoading on success', () => {
      adminInstructorsServiceSpy.getAllInstructors.and.returnValue(of(mockInstructors));
      fixture.detectChanges();

      expect(component.instructors).toEqual(mockInstructors);
      expect(component.isLoading).toBeFalse();
    });

    it('should show snackbar and clear isLoading on failure', () => {
      spyOn(console, 'error');
      adminInstructorsServiceSpy.getAllInstructors.and.returnValue(
        throwError(() => new Error('Network error'))
      );
      fixture.detectChanges();

      expect(component.isLoading).toBeFalse();
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Failed to load instructors', 'Close', { duration: 3000 }
      );
    });
  });

  describe('createLoginForInstructor', () => {
    let afterClosedSubject: Subject<any>;
    const mockEvent = { stopPropagation: jasmine.createSpy('stopPropagation') } as any;

    beforeEach(() => {
      adminInstructorsServiceSpy.getAllInstructors.and.returnValue(of(mockInstructors));
      fixture.detectChanges();

      afterClosedSubject = new Subject<any>();
      dialogSpy.open.and.returnValue({ afterClosed: () => afterClosedSubject.asObservable() } as any);
    });

    it('should stop event propagation and open dialog', () => {
      component.createLoginForInstructor(mockInstructors[0], mockEvent);

      expect(mockEvent.stopPropagation).toHaveBeenCalled();
      expect(dialogSpy.open).toHaveBeenCalledWith(
        jasmine.any(Function),
        {
          width: '500px',
          data: {
            studentName: mockInstructors[0].name,
            suggestedUsername: mockInstructors[0].email
          }
        }
      );
    });

    it('should do nothing if dialog is closed without a username', () => {
      component.createLoginForInstructor(mockInstructors[0], mockEvent);
      afterClosedSubject.next(null); // user cancelled

      expect(adminInstructorsServiceSpy.createInstructorLogin).not.toHaveBeenCalled();
    });

    it('should call createInstructorLogin with correct args when username returned', () => {
      adminInstructorsServiceSpy.createInstructorLogin.and.returnValue(of({
        username: 'janedoe',
        temporaryPassword: 'TempPass123'
      } as any));

      component.createLoginForInstructor(mockInstructors[0], mockEvent);
      afterClosedSubject.next('janedoe');

      expect(adminInstructorsServiceSpy.createInstructorLogin).toHaveBeenCalledWith(1, {
        username: 'janedoe',
        password: null
      });
    });

    it('should show success snackbar and reload instructors on login creation success', () => {
      const mockResult = { username: 'janedoe', temporaryPassword: 'TempPass123' };
      adminInstructorsServiceSpy.createInstructorLogin.and.returnValue(of(mockResult as any));
      // Make sure getAllInstructors always returns something for any call
      adminInstructorsServiceSpy.getAllInstructors.and.returnValue(of(mockInstructors));

      component.createLoginForInstructor(mockInstructors[0], mockEvent);
      afterClosedSubject.next('janedoe');
      afterClosedSubject.complete();

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        jasmine.stringContaining('Login created for Jane Doe'),
        'Close',
        jasmine.objectContaining({ duration: 10000 })
      );
      expect(adminInstructorsServiceSpy.getAllInstructors).toHaveBeenCalledTimes(2);
    });

    it('should show error snackbar on login creation failure', () => {
      spyOn(console, 'error');
      adminInstructorsServiceSpy.createInstructorLogin.and.returnValue(
        throwError(() => ({ error: { message: 'Username already taken' } }))
      );

      component.createLoginForInstructor(mockInstructors[0], mockEvent);
      afterClosedSubject.next('janedoe');

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Username already taken', 'Close',
        jasmine.objectContaining({ duration: 5000 })
      );
    });

    it('should show fallback error message if error has no message', () => {
      spyOn(console, 'error');
      adminInstructorsServiceSpy.createInstructorLogin.and.returnValue(
        throwError(() => ({ error: {} }))
      );

      component.createLoginForInstructor(mockInstructors[0], mockEvent);
      afterClosedSubject.next('janedoe');

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Failed to create login', 'Close',
        jasmine.objectContaining({ duration: 5000 })
      );
    });
  });
});
