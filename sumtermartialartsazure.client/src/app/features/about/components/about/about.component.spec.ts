import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { provideRouter } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AboutComponent } from './about.component';
import { Input } from '@angular/core';
import { InstructorsService } from '../../../instructors/services/instructors.service';
import { Instructor } from '../../../instructors/models/instructor.model';

const mockInstructors: Instructor[] = [
  { id: 1, name: 'Jane Doe' } as Instructor,
  { id: 2, name: 'John Smith' } as Instructor,
];

@Component({ selector: 'app-instructors-grid', standalone: true, template: '' })
class MockInstructorsGridComponent {
  @Input() instructors: any[] = [];
}

@Component({ selector: 'app-mission-section', standalone: true, template: '' })
class MockMissionSectionComponent { }

describe('AboutComponent', () => {
  let component: AboutComponent;
  let fixture: ComponentFixture<AboutComponent>;
  let instructorsServiceSpy: jasmine.SpyObj<InstructorsService>;

  beforeEach(async () => {
    // Create a spy object so we never make real HTTP calls
    instructorsServiceSpy = jasmine.createSpyObj('InstructorsService', ['getInstructors']);

    await TestBed.configureTestingModule({
      imports: [
        AboutComponent,
      ],
      providers: [
        { provide: InstructorsService, useValue: instructorsServiceSpy },
        provideRouter([]),
      ]
    })
    .overrideComponent(AboutComponent, {
      set: {
        imports: [CommonModule, MockInstructorsGridComponent, MockMissionSectionComponent]
      }
    }).compileComponents();

    fixture = TestBed.createComponent(AboutComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    instructorsServiceSpy.getInstructors.and.returnValue(of([]));
    fixture.detectChanges(); // triggers ngOnInit
    expect(component).toBeTruthy();
  });

  describe('on init', () => {
    it('should start with isLoading true before data arrives', () => {
      // Don't call detectChanges yet — check the virgin state
      expect(component.isLoading).toBeTrue();
      expect(component.instructors).toEqual([]);
      expect(component.error).toBeUndefined();
    });

    it('should load instructors and set isLoading to false on success', () => {
      instructorsServiceSpy.getInstructors.and.returnValue(of(mockInstructors));
      fixture.detectChanges();

      expect(instructorsServiceSpy.getInstructors).toHaveBeenCalledTimes(1);
      expect(component.instructors).toEqual(mockInstructors);
      expect(component.isLoading).toBeFalse();
      expect(component.error).toBeUndefined();
    });

    it('should set error message and isLoading to false on failure', () => {
      spyOn(console, 'error');
      instructorsServiceSpy.getInstructors.and.returnValue(
        throwError(() => new Error('Network error'))
      );
      fixture.detectChanges();

      expect(component.instructors).toEqual([]);
      expect(component.isLoading).toBeFalse();
      expect(component.error).toBe('Failed to load instructors');
    });
  });
});
