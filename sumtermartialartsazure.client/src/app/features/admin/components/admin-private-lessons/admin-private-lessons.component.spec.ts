import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError, Subject } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { AdminPrivateLessonsComponent } from './admin-private-lessons.component';
import { AdminPrivateLessonsService } from '../../services/admin-private-lessons.service';
import { PrivateLessonRequest } from '../../models/private-lesson-request.model';

const mockRequest: PrivateLessonRequest = {
  id: 1,
  instructorId: 10,
  instructorName: 'Jane Doe',
  studentName: 'Bob Student',
  studentEmail: 'bob@example.com',
  studentPhone: '555-1234',
  requestedStart: '2026-03-01T10:00:00',
  requestedEnd: '2026-03-01T11:00:00',
  status: 'Pending',
  createdAt: '2026-02-15T08:00:00'
};

const mockRequests: PrivateLessonRequest[] = [mockRequest];

describe('AdminPrivateLessonsComponent', () => {
  let component: AdminPrivateLessonsComponent;
  let fixture: ComponentFixture<AdminPrivateLessonsComponent>;
  let adminServiceSpy: jasmine.SpyObj<AdminPrivateLessonsService>;
  let dialogSpy: jasmine.SpyObj<MatDialog>;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;

  beforeEach(async () => {
    adminServiceSpy = jasmine.createSpyObj('AdminPrivateLessonsService', [
      'getAllRequests',
      'updateStatus'
    ]);
    dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
    snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);

    // Default: both calls in ngOnInit return successfully
    adminServiceSpy.getAllRequests.and.returnValue(of(mockRequests));

    await TestBed.configureTestingModule({
      imports: [AdminPrivateLessonsComponent, NoopAnimationsModule],
      providers: [
        { provide: AdminPrivateLessonsService, useValue: adminServiceSpy },
        { provide: MatDialog, useValue: dialogSpy },
        { provide: MatSnackBar, useValue: snackBarSpy }
      ]
    })
      .overrideComponent(AdminPrivateLessonsComponent, {
        set: {
          imports: [
            CommonModule,
            MatTableModule,
            MatButtonModule,
            MatProgressSpinnerModule,
            MatChipsModule,
            MatTooltipModule,
            MatTabsModule
            // MatSnackBarModule intentionally omitted so spy wins
          ]
        }
      })
      .compileComponents();

    fixture = TestBed.createComponent(AdminPrivateLessonsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should call loadRequests with Pending and loadPendingCount on init', () => {
      fixture.detectChanges();
      expect(adminServiceSpy.getAllRequests).toHaveBeenCalledWith('Pending');
      expect(adminServiceSpy.getAllRequests).toHaveBeenCalledTimes(2); // loadRequests + loadPendingCount
    });
  });

  describe('loadRequests', () => {
    it('should set requests and clear isLoading on success', () => {
      fixture.detectChanges();
      expect(component.requests).toEqual(mockRequests);
      expect(component.isLoading).toBeFalse();
      expect(component.error).toBeUndefined();
    });

    it('should handle null response by setting empty array', () => {
      adminServiceSpy.getAllRequests.and.returnValue(of(null as any));
      fixture.detectChanges();
      expect(component.requests).toEqual([]);
    });

    it('should set error and clear isLoading on failure', () => {
      spyOn(console, 'error');
      adminServiceSpy.getAllRequests.and.returnValue(throwError(() => new Error('Network error')));
      fixture.detectChanges();

      expect(component.requests).toEqual([]);
      expect(component.isLoading).toBeFalse();
      expect(component.error).toBe('Failed to load requests');
    });
  });

  describe('loadPendingCount', () => {
    it('should set pendingCount from response length', () => {
      fixture.detectChanges();
      expect(component.pendingCount).toBe(mockRequests.length);
    });

    it('should silently handle error', () => {
      spyOn(console, 'error');
      adminServiceSpy.getAllRequests.and.returnValue(throwError(() => new Error('error')));
      fixture.detectChanges();
      expect(component.pendingCount).toBe(0); // unchanged from default
    });
  });

  describe('approve', () => {
    beforeEach(() => {
      fixture.detectChanges();
      adminServiceSpy.getAllRequests.calls.reset();
    });

    it('should show success snackbar and reload on successful approval', () => {
      adminServiceSpy.updateStatus.and.returnValue(of({ success: true, message: '' }));
      adminServiceSpy.getAllRequests.and.returnValue(of(mockRequests));

      component.approve(mockRequest);

      expect(adminServiceSpy.updateStatus).toHaveBeenCalledWith(1, 'Approved', undefined);
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Request approved successfully', 'Close',
        jasmine.objectContaining({ duration: 3000 })
      );
      expect(adminServiceSpy.getAllRequests).toHaveBeenCalledTimes(2); // loadRequests + loadPendingCount
    });

    it('should show response message when success is false', () => {
      adminServiceSpy.updateStatus.and.returnValue(
        of({ success: false, message: 'Slot unavailable' })
      );

      component.approve(mockRequest);

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Slot unavailable', 'Close', { duration: 5000 }
      );
    });

    it('should show fallback message when success is false and no message', () => {
      adminServiceSpy.updateStatus.and.returnValue(of({ success: false, message: '' }));

      component.approve(mockRequest);

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Failed to approve request', 'Close', { duration: 5000 }
      );
    });

    it('should show error snackbar on failure', () => {
      spyOn(console, 'error');
      adminServiceSpy.updateStatus.and.returnValue(throwError(() => new Error('error')));

      component.approve(mockRequest);

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Error approving request', 'Close', { duration: 3000 }
      );
    });
  });

  describe('reject', () => {
    let afterClosedSubject: Subject<any>;

    beforeEach(() => {
      fixture.detectChanges();
      adminServiceSpy.getAllRequests.calls.reset();
      afterClosedSubject = new Subject<any>();
      dialogSpy.open.and.returnValue({ afterClosed: () => afterClosedSubject.asObservable() } as any);
    });

    it('should open dialog with correct data', () => {
      component.reject(mockRequest);
      expect(dialogSpy.open).toHaveBeenCalledWith(
        jasmine.any(Function),
        { width: '400px', data: { studentName: mockRequest.studentName } }
      );
    });

    it('should do nothing if dialog closed without reason', () => {
      component.reject(mockRequest);
      afterClosedSubject.next(null);
      expect(adminServiceSpy.updateStatus).not.toHaveBeenCalled();
    });

    it('should call updateStatus with Rejected and reason', () => {
      adminServiceSpy.updateStatus.and.returnValue(of({ success: true, message: '' }));
      adminServiceSpy.getAllRequests.and.returnValue(of(mockRequests));

      component.reject(mockRequest);
      afterClosedSubject.next('Not a good fit');

      expect(adminServiceSpy.updateStatus).toHaveBeenCalledWith(1, 'Rejected', 'Not a good fit');
    });

    it('should show success snackbar and reload on successful rejection', () => {
      adminServiceSpy.updateStatus.and.returnValue(of({ success: true, message: '' }));
      adminServiceSpy.getAllRequests.and.returnValue(of(mockRequests));

      component.reject(mockRequest);
      afterClosedSubject.next('Not a good fit');

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Request rejected', 'Close', { duration: 3000 }
      );
      expect(adminServiceSpy.getAllRequests).toHaveBeenCalledTimes(2);
    });

    it('should show error snackbar on rejection failure', () => {
      spyOn(console, 'error');
      adminServiceSpy.updateStatus.and.returnValue(throwError(() => new Error('error')));

      component.reject(mockRequest);
      afterClosedSubject.next('Some reason');

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'Error rejecting request', 'Close', { duration: 3000 }
      );
    });
  });

  describe('onTabChange', () => {
    beforeEach(() => {
      fixture.detectChanges();
      adminServiceSpy.getAllRequests.calls.reset();
      adminServiceSpy.getAllRequests.and.returnValue(of(mockRequests));
    });

    it('should load Pending requests on tab 0', () => {
      component.onTabChange(0);
      expect(adminServiceSpy.getAllRequests).toHaveBeenCalledWith('Pending');
    });

    it('should load Recent requests on tab 1', () => {
      component.onTabChange(1);
      expect(adminServiceSpy.getAllRequests).toHaveBeenCalledWith('Recent');
    });

    it('should load All requests on tab 2', () => {
      component.onTabChange(2);
      expect(adminServiceSpy.getAllRequests).toHaveBeenCalledWith('All');
    });
  });

  describe('formatDateTime', () => {
    it('should format a date string into a readable format', () => {
      const result = component.formatDateTime('2026-03-01T10:00:00');
      expect(result).toContain('Mar');
      expect(result).toContain('2026');
    });
  });

  describe('formatTimeRange', () => {
    it('should return a formatted time range string', () => {
      const result = component.formatTimeRange('2026-03-01T10:00:00', '2026-03-01T11:00:00');
      expect(result).toContain('Mar');
      expect(result).toContain('10');
      expect(result).toContain('11');
    });
  });

  describe('getStatusColor', () => {
    it('should return accent for pending', () => expect(component.getStatusColor('pending')).toBe('accent'));
    it('should return primary for approved', () => expect(component.getStatusColor('approved')).toBe('primary'));
    it('should return warn for rejected', () => expect(component.getStatusColor('rejected')).toBe('warn'));
    it('should return empty string for unknown', () => expect(component.getStatusColor('unknown')).toBe(''));
  });

  describe('isPending', () => {
    it('should return true for pending status', () => {
      expect(component.isPending({ ...mockRequest, status: 'Pending' })).toBeTrue();
    });
    it('should return false for non-pending status', () => {
      expect(component.isPending({ ...mockRequest, status: 'Approved' })).toBeFalse();
    });
  });
});
