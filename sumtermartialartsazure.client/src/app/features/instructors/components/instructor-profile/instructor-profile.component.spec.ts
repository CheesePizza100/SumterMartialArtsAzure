import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';

import { InstructorProfileComponent } from './instructor-profile.component';
import { InstructorsService } from '../../services/instructors.service';
import { InstructorProfile } from '../../models/instructor.model';

const mockProfile: InstructorProfile = {
  id: 1,
  name: 'Jane Doe',
  email: 'jane@example.com',
  rank: 'Black',
  bio: 'Experienced instructor',
  photoUrl: 'photo.jpg',
  programs: [{ id: 1, name: 'Karate' }],
  classSchedule: [{ daysOfWeek: 'Mon/Wed', startTime: '6:00 PM', duration: '1 hour' }]
};

describe('InstructorProfileComponent', () => {
  let component: InstructorProfileComponent;
  let fixture: ComponentFixture<InstructorProfileComponent>;
  let instructorsServiceSpy: jasmine.SpyObj<InstructorsService>;

  beforeEach(async () => {
    instructorsServiceSpy = jasmine.createSpyObj('InstructorsService', ['getMyProfile']);
    instructorsServiceSpy.getMyProfile.and.returnValue(of(mockProfile));

    await TestBed.configureTestingModule({
      imports: [InstructorProfileComponent],
      providers: [
        { provide: InstructorsService, useValue: instructorsServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(InstructorProfileComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  describe('loadProfile', () => {
    it('should set profile and clear loading on success', () => {
      fixture.detectChanges();
      expect(component.profile).toEqual(mockProfile);
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });

    it('should set error and clear loading on failure', () => {
      spyOn(console, 'error');
      instructorsServiceSpy.getMyProfile.and.returnValue(throwError(() => new Error('error')));
      fixture.detectChanges();
      expect(component.profile).toBeNull();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Failed to load profile');
    });
  });

  describe('getInitials', () => {
    it('should return initials from a two-word name', () => {
      expect(component.getInitials('Jane Doe')).toBe('JD');
    });

    it('should return only first two initials for longer names', () => {
      expect(component.getInitials('Mary Jane Watson')).toBe('MJ');
    });

    it('should return single initial for one word name', () => {
      expect(component.getInitials('Madonna')).toBe('M');
    });

    it('should return uppercase initials', () => {
      expect(component.getInitials('john smith')).toBe('JS');
    });
  });
});
