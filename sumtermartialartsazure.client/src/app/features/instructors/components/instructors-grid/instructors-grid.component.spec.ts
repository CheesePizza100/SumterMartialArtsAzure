import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InstructorsGridComponent } from './instructors-grid.component';
import { Instructor } from '../../models/instructor.model';

@Component({ selector: 'app-instructor-card', standalone: true, template: '' })
class MockInstructorCardComponent {
  @Input() instructor!: Instructor;
}

const mockInstructors: Instructor[] = [
  {
    id: 1, name: 'Jane Doe', rank: 'Black', bio: 'Bio',
    photoUrl: 'photo.jpg', programIds: [], achievements: [], specialties: []
  },
  {
    id: 2, name: 'John Smith', rank: 'Brown', bio: 'Bio',
    photoUrl: 'photo.jpg', programIds: [], achievements: [], specialties: []
  }
];

describe('InstructorsGridComponent', () => {
  let component: InstructorsGridComponent;
  let fixture: ComponentFixture<InstructorsGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InstructorsGridComponent]
    })
      .overrideComponent(InstructorsGridComponent, {
        set: { imports: [CommonModule, MockInstructorCardComponent] }
      })
      .compileComponents();

    fixture = TestBed.createComponent(InstructorsGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default values', () => {
    expect(component.instructors).toEqual([]);
    expect(component.showTitle).toBeTrue();
    expect(component.title).toBe('Meet Our Instructors');
  });

  it('should accept instructors input', () => {
    component.instructors = mockInstructors;
    fixture.detectChanges();
    expect(component.instructors.length).toBe(2);
  });

  it('should accept custom title', () => {
    component.title = 'Our Team';
    fixture.detectChanges();
    expect(component.title).toBe('Our Team');
  });

  it('should accept showTitle input', () => {
    component.showTitle = false;
    fixture.detectChanges();
    expect(component.showTitle).toBeFalse();
  });
});
