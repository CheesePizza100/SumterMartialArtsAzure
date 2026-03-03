import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';

import { FeaturesSectionComponent } from './features-section.component';

describe('FeaturesSectionComponent', () => {
  let component: FeaturesSectionComponent;
  let fixture: ComponentFixture<FeaturesSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeaturesSectionComponent, RouterModule.forRoot([])]
    }).compileComponents();

    fixture = TestBed.createComponent(FeaturesSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have 6 features defined', () => {
    expect(component.features.length).toBe(6);
  });
});
