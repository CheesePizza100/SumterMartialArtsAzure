import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { InstructorDashboardComponent } from './instructor-dashboard.component';
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
      attendance: mockAttendance
    },
    {
      programId: 2,
      programName: 'BJJ',
      currentRank: 'White',
      enrolledDate: '2025-03-01',
      attendance: { last30Days: 4, total: 20, attendanceRate: 70 }
    }
  ],
  testHistory: []
};

const mockStudents: InstructorStudent[] = [mockStudent];

describe('InstructorDashboardComponent', () => {
  let component: InstructorDashboardComponent;
  let fixture: ComponentFixture<InstructorDashboardComponent>;
  let instructorsServiceSpy: jasmine.SpyObj<InstructorsService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    instructorsServiceSpy = jasmine.createSpyObj('InstructorsService', ['getMyStudents']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    instructorsServiceSpy.getMyStudents.and.returnValue(of(mockStudents));

    await TestBed.configureTestingModule({
      imports: [InstructorDashboardComponent],
      providers: [
        { provide: InstructorsService, useValue: instructorsServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(InstructorDashboardComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  describe('loadStudents', () => {
    it('should set students and clear loading on success', () => {
      fixture.detectChanges();
      expect(component.students).toEqual(mockStudents);
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });

    it('should set error and clear loading on failure', () => {
      spyOn(console, 'error');
      instructorsServiceSpy.getMyStudents.and.returnValue(throwError(() => new Error('error')));
      fixture.detectChanges();
      expect(component.students).toEqual([]);
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Failed to load students');
    });
  });

  describe('getTotalAttendance', () => {
    beforeEach(() => fixture.detectChanges());

    it('should sum last30Days across all programs', () => {
      const result = component.getTotalAttendance(mockStudent);
      expect(result.last30Days).toBe(10); // 6 + 4
    });

    it('should average attendanceRate across programs', () => {
      const result = component.getTotalAttendance(mockStudent);
      expect(result.rate).toBe(75); // (80 + 70) / 2
    });

    it('should return rate of 0 when student has no programs', () => {
      const result = component.getTotalAttendance({ ...mockStudent, programs: [] });
      expect(result.rate).toBe(0);
      expect(result.last30Days).toBe(0);
    });
  });

  describe('viewStudent', () => {
    beforeEach(() => fixture.detectChanges());

    it('should navigate to instructor student detail route', () => {
      component.viewStudent(1);
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/instructor/students', 1]);
    });
  });
});
