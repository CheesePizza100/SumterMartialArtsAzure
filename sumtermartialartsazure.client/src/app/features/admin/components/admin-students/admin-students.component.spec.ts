import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError, Subject } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { AdminStudentsComponent } from './admin-students.component';
import { AdminStudentsService } from '../../services/admin-students.service';
import { ProgramsService } from '../../../programs/services/programs.service';
import { Student } from '../../models/student.model';

const mockAttendance = { last30Days: 5, total: 30, attendanceRate: 85 };

const mockProgram = {
  programId: 1,
  name: 'Karate',
  rank: 'White Belt',
  enrolledDate: '2025-01-01',
  lastTest: null,
  testNotes: null,
  attendance: mockAttendance
};

const mockStudent: Student = {
  id: 1,
  name: 'Bob Student',
  email: 'bob@example.com',
  phone: '555-1234',
  hasLogin: false,
  programs: [mockProgram],
  testHistory: [
    { date: '2025-06-01', program: 'Karate', rank: 'White Belt', result: 'Pass', notes: '' }
  ]
};

const mockStudents: Student[] = [mockStudent];

describe('AdminStudentsComponent', () => {
  let component: AdminStudentsComponent;
  let fixture: ComponentFixture<AdminStudentsComponent>;
  let adminStudentsServiceSpy: jasmine.SpyObj<AdminStudentsService>;
  let programsServiceSpy: jasmine.SpyObj<ProgramsService>;
  let dialogSpy: jasmine.SpyObj<MatDialog>;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;

  beforeEach(async () => {
    adminStudentsServiceSpy = jasmine.createSpyObj('AdminStudentsService', [
      'getAllStudents',
      'searchStudents',
      'getStudentById',
      'addTestResult',
      'createStudent',
      'createStudentLogin',
      'enrollInProgram',
      'recordAttendance',
      'deactivateStudent'
    ]);
    programsServiceSpy = jasmine.createSpyObj('ProgramsService', ['getPrograms']);
    dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
    snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);

    adminStudentsServiceSpy.getAllStudents.and.returnValue(of(mockStudents));

    await TestBed.configureTestingModule({
      imports: [AdminStudentsComponent, NoopAnimationsModule],
      providers: [
        { provide: AdminStudentsService, useValue: adminStudentsServiceSpy },
        { provide: ProgramsService, useValue: programsServiceSpy },
        { provide: MatDialog, useValue: dialogSpy },
        { provide: MatSnackBar, useValue: snackBarSpy }
      ]
    })
      .overrideComponent(AdminStudentsComponent, {
        set: {
          imports: [
            CommonModule,
            FormsModule,
            MatTableModule,
            MatButtonModule,
            MatIconModule,
            MatInputModule,
            MatFormFieldModule,
            MatProgressSpinnerModule,
            MatTabsModule,
            MatChipsModule
          ]
        }
      })
      .compileComponents();

    fixture = TestBed.createComponent(AdminStudentsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  describe('loadStudents', () => {
    it('should set students and filteredStudents on success', () => {
      fixture.detectChanges();
      expect(component.students).toEqual(mockStudents);
      expect(component.filteredStudents).toEqual(mockStudents);
      expect(component.isLoading).toBeFalse();
      expect(component.error).toBeUndefined();
    });

    it('should set error on failure', () => {
      spyOn(console, 'error');
      adminStudentsServiceSpy.getAllStudents.and.returnValue(throwError(() => new Error('error')));
      fixture.detectChanges();
      expect(component.error).toBe('Failed to load students. Please try again.');
      expect(component.isLoading).toBeFalse();
    });
  });

  describe('onSearchChange', () => {
    beforeEach(() => fixture.detectChanges());

    it('should reset filteredStudents when search term is empty', () => {
      component.searchTerm = '';
      component.onSearchChange();
      expect(component.filteredStudents).toEqual(mockStudents);
    });

    it('should call searchStudents and update filteredStudents on success', () => {
      adminStudentsServiceSpy.searchStudents.and.returnValue(of([mockStudent]));
      component.searchTerm = 'Bob';
      component.onSearchChange();
      expect(adminStudentsServiceSpy.searchStudents).toHaveBeenCalledWith('Bob');
      expect(component.filteredStudents).toEqual([mockStudent]);
    });

    it('should fall back to local filtering on search error', () => {
      spyOn(console, 'error');
      adminStudentsServiceSpy.searchStudents.and.returnValue(throwError(() => new Error('error')));
      component.searchTerm = 'bob';
      component.onSearchChange();
      expect(component.filteredStudents).toEqual([mockStudent]); // matches by email
    });

    it('should filter out non-matching students on local fallback', () => {
      spyOn(console, 'error');
      adminStudentsServiceSpy.searchStudents.and.returnValue(throwError(() => new Error('error')));
      component.searchTerm = 'zzznomatch';
      component.onSearchChange();
      expect(component.filteredStudents).toEqual([]);
    });
  });

  describe('selectStudent / backToList', () => {
    beforeEach(() => fixture.detectChanges());

    it('should set selectedStudent and reset activeTab', () => {
      component.activeTab = 2;
      component.selectStudent(mockStudent);
      expect(component.selectedStudent).toEqual(mockStudent);
      expect(component.activeTab).toBe(0);
    });

    it('should clear selectedStudent on backToList', () => {
      component.selectedStudent = mockStudent;
      component.backToList();
      expect(component.selectedStudent).toBeNull();
    });
  });

  it('should format a date string', () => {
    const result = component.formatDate('2026-03-15');
    expect(result).toContain('Mar');
    expect(result).toContain('2026');
  });

  describe('getLastTestDate', () => {
    beforeEach(() => fixture.detectChanges());

    it('should return formatted date of first test', () => {
      const result = component.getLastTestDate(mockStudent);
      expect(result).toContain('2025');
    });

    it('should return No tests yet when testHistory is empty', () => {
      const result = component.getLastTestDate({ ...mockStudent, testHistory: [] });
      expect(result).toBe('No tests yet');
    });
  });

  describe('getResultColor', () => {
    it('should return primary for pass', () => expect(component.getResultColor('pass')).toBe('primary'));
    it('should return warn for fail', () => expect(component.getResultColor('fail')).toBe('warn'));
  });

  describe('getTotalAttendance', () => {
    beforeEach(() => fixture.detectChanges());

    it('should aggregate attendance across programs', () => {
      const result = component.getTotalAttendance(mockStudent);
      expect(result.total).toBe(30);
      expect(result.last30Days).toBe(5);
      expect(result.rate).toBe(85);
    });

    it('should return zero rate when no programs', () => {
      const result = component.getTotalAttendance({ ...mockStudent, programs: [] });
      expect(result.rate).toBe(0);
    });
  });

  describe('recordTestResult', () => {
    beforeEach(() => {
      fixture.detectChanges();
      component.selectedStudent = mockStudent;
      adminStudentsServiceSpy.getAllStudents.calls.reset();
    });

    it('should show success snackbar and reload on success', () => {
      adminStudentsServiceSpy.addTestResult.and.returnValue(of({ success: true, message: '' }));
      adminStudentsServiceSpy.getAllStudents.and.returnValue(of(mockStudents));
      adminStudentsServiceSpy.getStudentById.and.returnValue(of(mockStudent));

      component.recordTestResult({ programId: 1, rank: 'Yellow Belt', result: 'Pass' });

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Test result recorded successfully!', 'Close',
        jasmine.objectContaining({ duration: 3000 })
      );
    });

    it('should show response message when success is false', () => {
      adminStudentsServiceSpy.addTestResult.and.returnValue(
        of({ success: false, message: 'Invalid data' })
      );
      component.recordTestResult({});
      expect(snackBarSpy.open).toHaveBeenCalledWith('Invalid data', 'Close', { duration: 5000 });
    });

    it('should show error snackbar on failure', () => {
      spyOn(console, 'error');
      adminStudentsServiceSpy.addTestResult.and.returnValue(throwError(() => new Error('error')));
      component.recordTestResult({});
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Error recording test result', 'Close', { duration: 3000 }
      );
    });

    it('should do nothing if selectedStudent is null', () => {
      component.selectedStudent = null;
      component.recordTestResult({});
      expect(adminStudentsServiceSpy.addTestResult).not.toHaveBeenCalled();
    });
  });

  describe('createStudent', () => {
    beforeEach(() => {
      fixture.detectChanges();
      adminStudentsServiceSpy.getAllStudents.calls.reset();
      adminStudentsServiceSpy.getAllStudents.and.returnValue(of(mockStudents));
    });

    it('should show success snackbar and reload on success', () => {
      adminStudentsServiceSpy.createStudent.and.returnValue(of(mockStudent));
      component.createStudent({ name: 'New', email: 'new@example.com', phone: '555-0000' });
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Student created successfully!', 'Close',
        jasmine.objectContaining({ duration: 3000 })
      );
      expect(adminStudentsServiceSpy.getAllStudents).toHaveBeenCalledTimes(1);
    });

    it('should show error snackbar on failure', () => {
      spyOn(console, 'error');
      adminStudentsServiceSpy.createStudent.and.returnValue(throwError(() => new Error('error')));
      component.createStudent({ name: 'New', email: 'new@example.com', phone: '555-0000' });
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Error creating student', 'Close', { duration: 3000 }
      );
    });
  });

  describe('createLoginForStudent', () => {
    let afterClosedSubject: Subject<any>;
    const mockEvent = { stopPropagation: jasmine.createSpy('stopPropagation') } as any;

    beforeEach(() => {
      fixture.detectChanges();
      afterClosedSubject = new Subject<any>();
      dialogSpy.open.and.returnValue({ afterClosed: () => afterClosedSubject.asObservable() } as any);
    });

    it('should stop propagation and open dialog', () => {
      component.createLoginForStudent(mockStudent, mockEvent);
      expect(mockEvent.stopPropagation).toHaveBeenCalled();
      expect(dialogSpy.open).toHaveBeenCalledWith(
        jasmine.any(Function),
        jasmine.objectContaining({
          data: { studentName: mockStudent.name, suggestedUsername: mockStudent.email }
        })
      );
    });

    it('should do nothing if dialog closed without username', () => {
      component.createLoginForStudent(mockStudent, mockEvent);
      afterClosedSubject.next(null);
      expect(adminStudentsServiceSpy.createStudentLogin).not.toHaveBeenCalled();
    });

    it('should show success snackbar and reload on success', () => {
      adminStudentsServiceSpy.createStudentLogin.and.returnValue(
        of({ username: 'bobstudent', temporaryPassword: 'TempPass123' } as any)
      );
      adminStudentsServiceSpy.getAllStudents.and.returnValue(of(mockStudents));

      component.createLoginForStudent(mockStudent, mockEvent);
      afterClosedSubject.next('bobstudent');
      afterClosedSubject.complete();

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        jasmine.stringContaining('Login created for Bob Student'),
        'Close',
        jasmine.objectContaining({ duration: 10000 })
      );
    });

    it('should show error snackbar on failure', () => {
      spyOn(console, 'error');
      adminStudentsServiceSpy.createStudentLogin.and.returnValue(
        throwError(() => ({ error: { message: 'Username taken' } }))
      );
      component.createLoginForStudent(mockStudent, mockEvent);
      afterClosedSubject.next('bobstudent');
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Username taken', 'Close', jasmine.objectContaining({ duration: 5000 })
      );
    });
  });

  describe('enrollInProgram', () => {
    beforeEach(() => {
      fixture.detectChanges();
      component.selectedStudent = mockStudent;
      adminStudentsServiceSpy.getStudentById.and.returnValue(of(mockStudent));
    });

    it('should do nothing if selectedStudent is null', () => {
      component.selectedStudent = null;
      component.enrollInProgram({ programId: 2, programName: 'BJJ', initialRank: 'White' });
      expect(adminStudentsServiceSpy.enrollInProgram).not.toHaveBeenCalled();
    });

    it('should show success snackbar and reload student on success', () => {
      adminStudentsServiceSpy.enrollInProgram.and.returnValue(
        of({ success: true, message: 'Enrolled!' })
      );
      component.enrollInProgram({ programId: 2, programName: 'BJJ', initialRank: 'White' });
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Enrolled!', 'Close', jasmine.objectContaining({ duration: 3000 })
      );
    });

    it('should show error snackbar on failure', () => {
      spyOn(console, 'error');
      adminStudentsServiceSpy.enrollInProgram.and.returnValue(throwError(() => new Error('error')));
      component.enrollInProgram({ programId: 2, programName: 'BJJ', initialRank: 'White' });
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Error enrolling student in program', 'Close', { duration: 3000 }
      );
    });
  });

  describe('recordAttendance', () => {
    beforeEach(() => {
      fixture.detectChanges();
      component.selectedStudent = mockStudent;
      adminStudentsServiceSpy.getStudentById.and.returnValue(of(mockStudent));
    });

    it('should do nothing if selectedStudent is null', () => {
      component.selectedStudent = null;
      component.recordAttendance(1, 3);
      expect(adminStudentsServiceSpy.recordAttendance).not.toHaveBeenCalled();
    });

    it('should show success snackbar on success', () => {
      adminStudentsServiceSpy.recordAttendance.and.returnValue(
        of({ success: true, message: 'Attendance recorded' })
      );
      component.recordAttendance(1, 3);
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Attendance recorded', 'Close', jasmine.objectContaining({ duration: 3000 })
      );
    });

    it('should show error snackbar on failure', () => {
      spyOn(console, 'error');
      adminStudentsServiceSpy.recordAttendance.and.returnValue(throwError(() => new Error('error')));
      component.recordAttendance(1, 3);
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Error recording attendance', 'Close', { duration: 3000 }
      );
    });
  });

  describe('deactivateStudent', () => {
    beforeEach(() => {
      fixture.detectChanges();
      component.selectedStudent = mockStudent;
      adminStudentsServiceSpy.getAllStudents.calls.reset();
      adminStudentsServiceSpy.getAllStudents.and.returnValue(of(mockStudents));
    });

    it('should do nothing if selectedStudent is null', () => {
      component.selectedStudent = null;
      component.deactivateStudent();
      expect(adminStudentsServiceSpy.deactivateStudent).not.toHaveBeenCalled();
    });

    it('should show success snackbar, go back to list and reload on success', () => {
      adminStudentsServiceSpy.deactivateStudent.and.returnValue(
        of({ success: true, message: 'Student deactivated' })
      );
      component.deactivateStudent();
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Student deactivated', 'Close', jasmine.objectContaining({ duration: 3000 })
      );
      expect(component.selectedStudent).toBeNull();
      expect(adminStudentsServiceSpy.getAllStudents).toHaveBeenCalledTimes(1);
    });

    it('should show error snackbar on failure', () => {
      spyOn(console, 'error');
      adminStudentsServiceSpy.deactivateStudent.and.returnValue(throwError(() => new Error('error')));
      component.deactivateStudent();
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Error deactivating student', 'Close', { duration: 3000 }
      );
    });
  });
});
