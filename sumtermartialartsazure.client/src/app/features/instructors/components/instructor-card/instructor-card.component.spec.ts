import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';

import { InstructorCardComponent } from './instructor-card.component';
import { Instructor } from '../../models/instructor.model';

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

describe('InstructorCardComponent', () => {
  let component: InstructorCardComponent;
  let fixture: ComponentFixture<InstructorCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InstructorCardComponent, RouterModule.forRoot([])]
    }).compileComponents();

    fixture = TestBed.createComponent(InstructorCardComponent);
    component = fixture.componentInstance;
    component.instructor = mockInstructor;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should accept instructor input', () => {
    expect(component.instructor).toEqual(mockInstructor);
  });
});
