import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { Component } from '@angular/core';

import { HomeComponent } from './home.component';

@Component({ selector: 'app-hero-section', standalone: true, template: '' })
class MockHeroSectionComponent { }

@Component({ selector: 'app-features-section', standalone: true, template: '' })
class MockFeaturesSectionComponent { }

describe('HomeComponent', () => {
  let fixture: ComponentFixture<HomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeComponent, RouterModule.forRoot([])]
    })
      .overrideComponent(HomeComponent, {
        set: {
          imports: [MockHeroSectionComponent, MockFeaturesSectionComponent]
        }
      })
      .compileComponents();

    fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
