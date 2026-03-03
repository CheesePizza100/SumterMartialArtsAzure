import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { Component } from '@angular/core';
import { AppComponent } from './app.component';

@Component({ selector: 'app-header', standalone: true, template: '' })
class MockHeaderComponent { }

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [provideRouter([])]
    })
      .overrideComponent(AppComponent, {
        set: {
          imports: [RouterOutlet, MockHeaderComponent]
        }
      })
      .compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    expect(fixture.componentInstance).toBeTruthy();
  });
});
