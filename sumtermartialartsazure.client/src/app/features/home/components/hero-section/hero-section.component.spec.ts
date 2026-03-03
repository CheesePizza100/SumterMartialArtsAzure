import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';

import { HeroSectionComponent } from './hero-section.component';

describe('HeroSectionComponent', () => {
  let fixture: ComponentFixture<HeroSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeroSectionComponent, RouterModule.forRoot([])]
    }).compileComponents();

    fixture = TestBed.createComponent(HeroSectionComponent);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
