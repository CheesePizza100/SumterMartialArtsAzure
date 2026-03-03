import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { EnrollDialogComponent } from './enroll-dialog.component';
import { Program } from '../../models/program.model';

const mockProgram: Program = {
  id: 1,
  name: 'Karate',
  description: 'Traditional karate',
  ageGroup: 'All Ages',
  details: 'Details here',
  duration: '1 hour',
  schedule: 'Mon/Wed 6pm',
  imageUrl: 'karate.jpg',
  instructors: []
};

describe('EnrollDialogComponent', () => {
  let component: EnrollDialogComponent;
  let fixture: ComponentFixture<EnrollDialogComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<EnrollDialogComponent>>;

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [EnrollDialogComponent, NoopAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockProgram }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(EnrollDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with empty fields', () => {
    expect(component.enrollForm.get('name')?.value).toBe('');
    expect(component.enrollForm.get('email')?.value).toBe('');
    expect(component.enrollForm.get('phone')?.value).toBe('');
  });

  it('should expose program data', () => {
    expect(component.data).toEqual(mockProgram);
  });

  describe('onCancel', () => {
    it('should close dialog without a value', () => {
      component.onCancel();
      expect(dialogRefSpy.close).toHaveBeenCalledWith();
    });
  });

  describe('onSubmit', () => {
    it('should not close when form is invalid', () => {
      component.onSubmit();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });

    it('should not close when email is invalid', () => {
      component.enrollForm.setValue({ name: 'Alice', email: 'notanemail', phone: '555-1234' });
      component.onSubmit();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });

    it('should close with enrollment data when form is valid', () => {
      spyOn(console, 'log');
      component.enrollForm.setValue({
        name: 'Alice',
        email: 'alice@example.com',
        phone: '555-1234'
      });
      component.onSubmit();
      expect(dialogRefSpy.close).toHaveBeenCalledWith({
        programId: 1,
        programName: 'Karate',
        name: 'Alice',
        email: 'alice@example.com',
        phone: '555-1234'
      });
    });
  });
});
