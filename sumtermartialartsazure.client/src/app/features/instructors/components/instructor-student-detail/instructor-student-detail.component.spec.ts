import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError, Subject } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';

import { InstructorStudentDetailComponent } from './instructor-student-detail.component';
import { InstructorsService } from '../../services/instructors.service';
import { InstructorStudent } from '../../models/instructor.model';

const mockAttendance = { last30Days: 6, total: 40, attendanceRate: 80 };

const mockStudent: InstructorStudent = {
  id: 1,
  name: 'Bob Student',
  email: 'bob@example.com',
  phone: '555-1234',
  programs: [
    {
      programId: 1,
      programName: 'Karate',
      currentRank: 'Blue',
      enrolledDate: '2025-01-01',
      attendance: mockAttendance,
      instructorNotes: 'Good progress'
    },
    {
      programId: 2,
      programName: 'BJJ',
      currentRank: 'White',
      enrolledDate: '2025-03-01',
      attendance: { last30Days: 4, total: 20, attendanceRate: 70 }
    }
  ],
  testHistory: [
    { date: '2025-06-15', program: 'Karate', rank: 'Blue', result: 'Pass', notes: '' }
  ]
};

describe('InstructorStudentDetailComponent', () => {
  let component: InstructorStudentDetailComponent;
  let fixture: ComponentFixture<InstructorStudentDetailComponent>;
  let instructorsServiceSpy: jasmine.SpyObj<InstructorsService>;
  let dialogSpy: jasmine.SpyObj<MatDialog>;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;
  let routerSpy: jasmine.SpyObj<Router>;

  const createComponent = async (paramId: string) => {
    await TestBed.configureTestingModule({
      imports: [InstructorStudentDetailComponent, NoopAnimationsModule],
      providers: [
        { provide: InstructorsService, useValue: instructorsServiceSpy },
        { provide: MatDialog, useValue: dialogSpy },
        { provide: MatSnackBar, useValue: snackBarSpy },
        { provide: Router, useValue: routerSpy },
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: { get: () => paramId } } }
        }
      ]
    })
      .overrideComponent(InstructorStudentDetailComponent, {
        set: {
          imports: [
            CommonModule,
            MatTabsModule,
            MatButtonModule,
            MatIconModule
            // MatSnackBarModule omitted so snackBarSpy wins
          ]
        }
      })
      .compileComponents();

    fixture = TestBed.createComponent(InstructorStudentDetailComponent);
    component = fixture.componentInstance;
  };

  beforeEach(() => {
    instructorsServiceSpy = jasmine.createSpyObj('InstructorsService', [
      'getStudentDetail',
      'recordTestResult',
      'recordAttendance',
      'updateProgramNotes'
    ]);
    dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
    snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    instructorsServiceSpy.getStudentDetail.and.returnValue(of(mockStudent));
  });

  describe('ngOnInit / loadStudent', () => {
    it('should create and load student on valid id', async () => {
      await createComponent('1');
      fixture.detectChanges();

      expect(component).toBeTruthy();
      expect(instructorsServiceSpy.getStudentDetail).toHaveBeenCalledWith(1);
      expect(component.student).toEqual(mockStudent);
      expect(component.loading).toBeFalse();
    });

    it('should not call loadStudent when id is falsy', async () => {
      await createComponent('0');
      fixture.detectChanges();
      expect(instructorsServiceSpy.getStudentDetail).not.toHaveBeenCalled();
    });

    it('should set error on service failure', async () => {
      spyOn(console, 'error');
      instructorsServiceSpy.getStudentDetail.and.returnValue(throwError(() => new Error('error')));
      await createComponent('1');
      fixture.detectChanges();
      expect(component.error).toBe('Failed to load student details');
      expect(component.loading).toBeFalse();
    });
  });

  describe('backToStudents', () => {
    it('should navigate to instructor dashboard', async () => {
      await createComponent('1');
      fixture.detectChanges();
      component.backToStudents();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/instructor/dashboard']);
    });
  });

  describe('formatDate', () => {
    it('should format a date string', async () => {
      await createComponent('1');
      fixture.detectChanges();
      const result = component.formatDate('2026-03-15');
      expect(result).toContain('Mar');
      expect(result).toContain('2026');
    });
  });

  describe('getResultColor', () => {
    it('should return primary for pass', async () => {
      await createComponent('1');
      expect(component.getResultColor('pass')).toBe('primary');
      expect(component.getResultColor('Pass')).toBe('primary');
    });

    it('should return warn for fail', async () => {
      await createComponent('1');
      expect(component.getResultColor('fail')).toBe('warn');
    });
  });

  describe('getTotalAttendance', () => {
    it('should return zeros when student is null', async () => {
      await createComponent('1');
      component.student = null;
      expect(component.getTotalAttendance()).toEqual({ total: 0, last30Days: 0, rate: 0 });
    });

    it('should aggregate attendance across programs', async () => {
      await createComponent('1');
      fixture.detectChanges();
      const result = component.getTotalAttendance();
      expect(result.total).toBe(60);      // 40 + 20
      expect(result.last30Days).toBe(10); // 6 + 4
      expect(result.rate).toBe(75);       // (80 + 70) / 2
    });
  });

  describe('recordTestResult', () => {
    beforeEach(async () => {
      await createComponent('1');
      fixture.detectChanges();
      instructorsServiceSpy.getStudentDetail.calls.reset();
      instructorsServiceSpy.getStudentDetail.and.returnValue(of(mockStudent));
    });

    it('should do nothing if student is null', () => {
      component.student = null;
      component.recordTestResult({});
      expect(instructorsServiceSpy.recordTestResult).not.toHaveBeenCalled();
    });

    it('should show success snackbar and reload on success', () => {
      instructorsServiceSpy.recordTestResult.and.returnValue(of({ success: true, message: '' }));
      component.recordTestResult({ programId: 1, rank: 'Yellow', result: 'Pass' });
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Test result recorded successfully!', 'Close',
        jasmine.objectContaining({ duration: 3000 })
      );
      expect(instructorsServiceSpy.getStudentDetail).toHaveBeenCalledTimes(1);
    });

    it('should show response message when success is false', () => {
      instructorsServiceSpy.recordTestResult.and.returnValue(
        of({ success: false, message: 'Invalid data' })
      );
      component.recordTestResult({});
      expect(snackBarSpy.open).toHaveBeenCalledWith('Invalid data', 'Close', { duration: 5000 });
    });

    it('should show error snackbar on failure', () => {
      spyOn(console, 'error');
      instructorsServiceSpy.recordTestResult.and.returnValue(throwError(() => new Error('error')));
      component.recordTestResult({});
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Error recording test result', 'Close', { duration: 3000 }
      );
    });
  });

  describe('recordAttendance', () => {
    beforeEach(async () => {
      await createComponent('1');
      fixture.detectChanges();
      instructorsServiceSpy.getStudentDetail.calls.reset();
      instructorsServiceSpy.getStudentDetail.and.returnValue(of(mockStudent));
    });

    it('should do nothing if student is null', () => {
      component.student = null;
      component.recordAttendance(1, 3);
      expect(instructorsServiceSpy.recordAttendance).not.toHaveBeenCalled();
    });

    it('should show success snackbar and reload on success', () => {
      instructorsServiceSpy.recordAttendance.and.returnValue(
        of({ success: true, message: 'Attendance recorded' })
      );
      component.recordAttendance(1, 3);
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Attendance recorded', 'Close', jasmine.objectContaining({ duration: 3000 })
      );
      expect(instructorsServiceSpy.getStudentDetail).toHaveBeenCalledTimes(1);
    });

    it('should show error snackbar on failure', () => {
      spyOn(console, 'error');
      instructorsServiceSpy.recordAttendance.and.returnValue(throwError(() => new Error('error')));
      component.recordAttendance(1, 3);
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Error recording attendance', 'Close', { duration: 3000 }
      );
    });
  });

  describe('updateProgramNotes', () => {
    beforeEach(async () => {
      await createComponent('1');
      fixture.detectChanges();
      instructorsServiceSpy.getStudentDetail.calls.reset();
      instructorsServiceSpy.getStudentDetail.and.returnValue(of(mockStudent));
    });

    it('should do nothing if student is null', () => {
      component.student = null;
      component.updateProgramNotes(1, 'some notes');
      expect(instructorsServiceSpy.updateProgramNotes).not.toHaveBeenCalled();
    });

    it('should show success snackbar and reload on success', () => {
      instructorsServiceSpy.updateProgramNotes.and.returnValue(
        of({ success: true, message: '' })
      );
      component.updateProgramNotes(1, 'Great progress');
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Notes updated successfully!', 'Close', jasmine.objectContaining({ duration: 3000 })
      );
      expect(instructorsServiceSpy.getStudentDetail).toHaveBeenCalledTimes(1);
    });

    it('should show error snackbar on failure', () => {
      spyOn(console, 'error');
      instructorsServiceSpy.updateProgramNotes.and.returnValue(throwError(() => new Error('error')));
      component.updateProgramNotes(1, 'notes');
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Error updating notes', 'Close', { duration: 3000 }
      );
    });
  });

  describe('dialog methods', () => {
    let afterClosedSubject: Subject<any>;

    beforeEach(async () => {
      await createComponent('1');
      fixture.detectChanges();
      afterClosedSubject = new Subject<any>();
      dialogSpy.open.and.returnValue({ afterClosed: () => afterClosedSubject.asObservable() } as any);
    });

    it('openRecordTestDialog should do nothing if student is null', () => {
      component.student = null;
      component.openRecordTestDialog();
      expect(dialogSpy.open).not.toHaveBeenCalled();
    });

    it('openRecordTestDialog should open dialog with correct data', () => {
      component.openRecordTestDialog();
      expect(dialogSpy.open).toHaveBeenCalledWith(
        jasmine.any(Function),
        jasmine.objectContaining({
          width: '600px',
          data: jasmine.objectContaining({ studentId: 1, studentName: 'Bob Student' })
        })
      );
    });

    it('openRecordTestDialog should call recordTestResult when dialog returns result', () => {
      instructorsServiceSpy.recordTestResult.and.returnValue(of({ success: true, message: '' }));
      instructorsServiceSpy.getStudentDetail.and.returnValue(of(mockStudent));
      component.openRecordTestDialog();
      afterClosedSubject.next({ programId: 1, rank: 'Yellow', result: 'Pass' });
      expect(instructorsServiceSpy.recordTestResult).toHaveBeenCalled();
    });

    it('openRecordAttendanceDialog should do nothing if student is null', () => {
      component.student = null;
      component.openRecordAttendanceDialog();
      expect(dialogSpy.open).not.toHaveBeenCalled();
    });

    it('openRecordAttendanceDialog should call recordAttendance when dialog returns result', () => {
      instructorsServiceSpy.recordAttendance.and.returnValue(
        of({ success: true, message: 'Attendance recorded' })
      );
      instructorsServiceSpy.getStudentDetail.and.returnValue(of(mockStudent));
      component.openRecordAttendanceDialog();
      afterClosedSubject.next({ programId: 1, classesAttended: 3 });
      expect(instructorsServiceSpy.recordAttendance).toHaveBeenCalled();
    });

    it('openUpdateNotesDialog should do nothing if student is null', () => {
      component.student = null;
      component.openUpdateNotesDialog(mockStudent.programs[0]);
      expect(dialogSpy.open).not.toHaveBeenCalled();
    });

    it('openUpdateNotesDialog should call updateProgramNotes when dialog returns notes', () => {
      instructorsServiceSpy.updateProgramNotes.and.returnValue(of({ success: true, message: '' }));
      instructorsServiceSpy.getStudentDetail.and.returnValue(of(mockStudent));
      component.openUpdateNotesDialog(mockStudent.programs[0]);
      afterClosedSubject.next('Updated notes');
      expect(instructorsServiceSpy.updateProgramNotes).toHaveBeenCalledWith(1, 1, 'Updated notes');
    });

    it('openUpdateNotesDialog should not call updateProgramNotes when dialog returns undefined', () => {
      component.openUpdateNotesDialog(mockStudent.programs[0]);
      afterClosedSubject.next(undefined);
      expect(instructorsServiceSpy.updateProgramNotes).not.toHaveBeenCalled();
    });
  });
});
