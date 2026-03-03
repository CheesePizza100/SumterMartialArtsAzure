import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramFiltersComponent, ProgramCategory } from './program-filters.component';

describe('ProgramFiltersComponent', () => {
  let component: ProgramFiltersComponent;
  let fixture: ComponentFixture<ProgramFiltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProgramFiltersComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ProgramFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default filter of all', () => {
    expect(component.currentFilter).toBe('all');
  });

  it('should have 4 filters defined', () => {
    expect(component.filters.length).toBe(4);
  });

  it('should have correct filter values', () => {
    const values = component.filters.map(f => f.value);
    expect(values).toEqual(['all', 'kids', 'adult', 'competition']);
  });

  describe('onFilterChange', () => {
    it('should emit the selected filter', () => {
      spyOn(component.filterChange, 'emit');
      component.onFilterChange('kids');
      expect(component.filterChange.emit).toHaveBeenCalledWith('kids');
    });

    it('should emit each filter category correctly', () => {
      spyOn(component.filterChange, 'emit');
      const categories: ProgramCategory[] = ['all', 'kids', 'adult', 'competition'];
      categories.forEach(category => {
        component.onFilterChange(category);
        expect(component.filterChange.emit).toHaveBeenCalledWith(category);
      });
    });
  });

  describe('currentFilter input', () => {
    it('should accept a filter value as input', () => {
      component.currentFilter = 'adult';
      fixture.detectChanges();
      expect(component.currentFilter).toBe('adult');
    });
  });
});
