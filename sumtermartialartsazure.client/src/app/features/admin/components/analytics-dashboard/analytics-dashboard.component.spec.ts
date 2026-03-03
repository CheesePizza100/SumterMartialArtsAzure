import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';

import { AnalyticsDashboardComponent } from './analytics-dashboard.component';
import { EventSourcingService } from '../../services/event-sourcing.service';
import { ProgressionAnalytics } from '../../models/event-sourcing.model';

const mockAnalytics: ProgressionAnalytics = {
  totalEnrollments: 50,
  totalTests: 40,
  passedTests: 35,
  failedTests: 5,
  passRate: 87.5,
  totalPromotions: 30,
  averageDaysToBlue: 180,
  mostActiveTestingMonths: [
    { year: 2025, month: 3, testCount: 10 },
    { year: 2025, month: 6, testCount: 15 },
    { year: 2025, month: 9, testCount: 8 }
  ],
  currentRankDistribution: [
    { rank: 'White Belt', count: 20 },
    { rank: 'Blue Belt', count: 15 },
    { rank: 'Black Belt', count: 5 }
  ]
};

describe('AnalyticsDashboardComponent', () => {
  let component: AnalyticsDashboardComponent;
  let fixture: ComponentFixture<AnalyticsDashboardComponent>;
  let eventSourcingServiceSpy: jasmine.SpyObj<EventSourcingService>;

  beforeEach(async () => {
    eventSourcingServiceSpy = jasmine.createSpyObj('EventSourcingService', ['getProgressionAnalytics']);
    eventSourcingServiceSpy.getProgressionAnalytics.and.returnValue(of(mockAnalytics));

    await TestBed.configureTestingModule({
      imports: [AnalyticsDashboardComponent, NoopAnimationsModule],
      providers: [
        { provide: EventSourcingService, useValue: eventSourcingServiceSpy }
      ]
    })
      .overrideComponent(AnalyticsDashboardComponent, {
        set: {
          imports: [
            CommonModule,
            FormsModule,
            MatCardModule,
            MatProgressSpinnerModule,
            MatSelectModule,
            MatFormFieldModule
          ]
        }
      })
      .compileComponents();

    fixture = TestBed.createComponent(AnalyticsDashboardComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  describe('loadAnalytics', () => {
    it('should set analytics and clear isLoading on success', () => {
      fixture.detectChanges();
      expect(component.analytics).toEqual(mockAnalytics);
      expect(component.isLoading).toBeFalse();
      expect(component.error).toBeUndefined();
    });

    it('should call service with undefined programId by default', () => {
      fixture.detectChanges();
      expect(eventSourcingServiceSpy.getProgressionAnalytics).toHaveBeenCalledWith(undefined);
    });

    it('should call service with selectedProgramId when set', () => {
      component.selectedProgramId = 3;
      component.loadAnalytics();
      expect(eventSourcingServiceSpy.getProgressionAnalytics).toHaveBeenCalledWith(3);
    });

    it('should set error and clear isLoading on failure', () => {
      spyOn(console, 'error');
      eventSourcingServiceSpy.getProgressionAnalytics.and.returnValue(
        throwError(() => new Error('Network error'))
      );
      fixture.detectChanges();
      expect(component.analytics).toBeNull();
      expect(component.isLoading).toBeFalse();
      expect(component.error).toBe('Failed to load analytics. Please try again.');
    });
  });

  describe('onProgramFilterChange', () => {
    it('should call loadAnalytics when filter changes', () => {
      fixture.detectChanges();
      eventSourcingServiceSpy.getProgressionAnalytics.calls.reset();
      eventSourcingServiceSpy.getProgressionAnalytics.and.returnValue(of(mockAnalytics));

      component.selectedProgramId = 2;
      component.onProgramFilterChange();

      expect(eventSourcingServiceSpy.getProgressionAnalytics).toHaveBeenCalledWith(2);
    });
  });

  describe('getMonthName', () => {
    it('should return correct month names', () => {
      expect(component.getMonthName(1)).toBe('Jan');
      expect(component.getMonthName(6)).toBe('Jun');
      expect(component.getMonthName(12)).toBe('Dec');
    });
  });

  describe('getBarWidth', () => {
    it('should return correct percentage', () => {
      expect(component.getBarWidth(50, 100)).toBe(50);
      expect(component.getBarWidth(25, 100)).toBe(25);
    });

    it('should return 0 when maxCount is 0', () => {
      expect(component.getBarWidth(10, 0)).toBe(0);
    });

    it('should return 100 when count equals maxCount', () => {
      expect(component.getBarWidth(15, 15)).toBe(100);
    });
  });

  describe('maxTestCount', () => {
    it('should return the highest testCount from mostActiveTestingMonths', () => {
      fixture.detectChanges();
      expect(component.maxTestCount).toBe(15);
    });

    it('should return 0 when analytics is null', () => {
      component.analytics = null;
      expect(component.maxTestCount).toBe(0);
    });

    it('should return 0 when mostActiveTestingMonths is empty', () => {
      component.analytics = { ...mockAnalytics, mostActiveTestingMonths: [] };
      expect(component.maxTestCount).toBe(0);
    });
  });

  describe('maxRankCount', () => {
    it('should return the highest count from currentRankDistribution', () => {
      fixture.detectChanges();
      expect(component.maxRankCount).toBe(20);
    });

    it('should return 0 when analytics is null', () => {
      component.analytics = null;
      expect(component.maxRankCount).toBe(0);
    });

    it('should return 0 when currentRankDistribution is empty', () => {
      component.analytics = { ...mockAnalytics, currentRankDistribution: [] };
      expect(component.maxRankCount).toBe(0);
    });
  });
});
