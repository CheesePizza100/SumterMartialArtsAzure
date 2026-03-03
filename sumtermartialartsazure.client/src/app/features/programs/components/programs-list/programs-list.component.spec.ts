import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter } from '@angular/core';

import { ProgramsListComponent } from './programs-list.component';
import { ProgramsService } from '../../services/programs.service';
import { Program, ProgramCategory } from '../../models/program.model';

@Component({ selector: 'app-program-filters', standalone: true, template: '' })
class MockProgramFiltersComponent {
  @Input() currentFilter: ProgramCategory = 'all';
  @Output() filterChange = new EventEmitter<ProgramCategory>();
}

@Component({ selector: 'app-program-card', standalone: true, template: '' })
class MockProgramCardComponent {
  @Input() program!: Program;
}

const mockPrograms: Program[] = [
  { id: 1, name: 'Kids Karate', description: '', ageGroup: 'Kids 4-12', details: '', duration: '', schedule: '', imageUrl: '', instructors: [] },
  { id: 2, name: 'Adult BJJ', description: '', ageGroup: 'Adult', details: '', duration: '', schedule: '', imageUrl: '', instructors: [] },
  { id: 3, name: 'Competition', description: '', ageGroup: 'Advanced Competition', details: '', duration: '', schedule: '', imageUrl: '', instructors: [] }
];

describe('ProgramsListComponent', () => {
  let component: ProgramsListComponent;
  let fixture: ComponentFixture<ProgramsListComponent>;
  let programsServiceSpy: jasmine.SpyObj<ProgramsService>;

  beforeEach(async () => {
    programsServiceSpy = jasmine.createSpyObj('ProgramsService', ['getPrograms']);
    programsServiceSpy.getPrograms.and.returnValue(of(mockPrograms));

    await TestBed.configureTestingModule({
      imports: [ProgramsListComponent],
      providers: [
        { provide: ProgramsService, useValue: programsServiceSpy }
      ]
    })
      .overrideComponent(ProgramsListComponent, {
        set: {
          imports: [CommonModule, MockProgramFiltersComponent, MockProgramCardComponent]
        }
      })
      .compileComponents();

    fixture = TestBed.createComponent(ProgramsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('loadPrograms', () => {
    it('should set programs and filteredPrograms on success', () => {
      expect(component.programs).toEqual(mockPrograms);
      expect(component.filteredPrograms).toEqual(mockPrograms);
    });

    it('should log error on failure', () => {
      spyOn(console, 'error');
      programsServiceSpy.getPrograms.and.returnValue(
        throwError(() => new Error('error'))
      );
      component.ngOnInit();
      expect(console.error).toHaveBeenCalled();
    });
  });

  describe('onFilterChange', () => {
    it('should set currentFilter', () => {
      component.onFilterChange('kids');
      expect(component.currentFilter).toBe('kids');
    });

    it('should return all programs for all filter', () => {
      component.onFilterChange('all');
      expect(component.filteredPrograms).toEqual(mockPrograms);
    });

    it('should filter to kids programs', () => {
      component.onFilterChange('kids');
      expect(component.filteredPrograms.length).toBe(1);
      expect(component.filteredPrograms[0].name).toBe('Kids Karate');
    });

    it('should filter to adult programs', () => {
      component.onFilterChange('adult');
      expect(component.filteredPrograms.length).toBe(1);
      expect(component.filteredPrograms[0].name).toBe('Adult BJJ');
    });

    it('should filter to competition programs', () => {
      component.onFilterChange('competition');
      expect(component.filteredPrograms.length).toBe(1);
      expect(component.filteredPrograms[0].name).toBe('Competition');
    });
  });
});
