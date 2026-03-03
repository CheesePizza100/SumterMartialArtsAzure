import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';

import { ProgramCardComponent } from './program-card.component';
import { Program } from '../../models/program.model';

const mockProgram: Program = {
  id: 1,
  name: 'Karate',
  description: 'Traditional karate for all ages',
  ageGroup: 'All Ages',
  details: 'Detailed description here',
  duration: '1 hour',
  schedule: 'Mon/Wed 6pm',
  imageUrl: 'karate.jpg',
  instructors: []
};

describe('ProgramCardComponent', () => {
  let component: ProgramCardComponent;
  let fixture: ComponentFixture<ProgramCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProgramCardComponent, RouterModule.forRoot([])]
    }).compileComponents();

    fixture = TestBed.createComponent(ProgramCardComponent);
    component = fixture.componentInstance;
    component.program = mockProgram;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should accept program input', () => {
    expect(component.program).toEqual(mockProgram);
  });
});
