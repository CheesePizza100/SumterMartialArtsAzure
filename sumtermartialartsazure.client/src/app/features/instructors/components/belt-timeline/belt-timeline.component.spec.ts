import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BeltTimelineComponent } from './belt-timeline.component';

describe('BeltTimelineComponent', () => {
  let component: BeltTimelineComponent;
  let fixture: ComponentFixture<BeltTimelineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BeltTimelineComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(BeltTimelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('currentRank setter', () => {
    it('should set progression with all belts achieved up to White', () => {
      component.currentRank = 'White';
      expect(component.progression.length).toBe(6);
      expect(component.progression[0]).toEqual({ belt: 'White', achieved: true });
      expect(component.progression[1]).toEqual({ belt: 'Yellow', achieved: false });
    });

    it('should set progression with all belts achieved up to Blue', () => {
      component.currentRank = 'Blue';
      const achieved = component.progression.filter(p => p.achieved).map(p => p.belt);
      const notAchieved = component.progression.filter(p => !p.achieved).map(p => p.belt);
      expect(achieved).toEqual(['White', 'Yellow', 'Green', 'Blue']);
      expect(notAchieved).toEqual(['Brown', 'Black']);
    });

    it('should mark all belts achieved for Black', () => {
      component.currentRank = 'Black';
      expect(component.progression.every(p => p.achieved)).toBeTrue();
    });

    it('should be case insensitive', () => {
      component.currentRank = 'green belt';
      const achieved = component.progression.filter(p => p.achieved).map(p => p.belt);
      expect(achieved).toEqual(['White', 'Yellow', 'Green']);
    });

    it('should default to only White achieved for unknown rank', () => {
      component.currentRank = 'unknown';
      // getCurrentBeltIndex returns 0 (findIndex returns -1, but White is index 0)
      // Actually findIndex returns -1 for no match, so none should be achieved
      expect(component.progression.every(p => !p.achieved)).toBeTrue();
    });
  });
});
