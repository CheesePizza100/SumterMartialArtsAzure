import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogRef } from '@angular/material/dialog';

import { CreateStudentDialogComponent } from './create-student-dialog.component';

describe('CreateStudentDialogComponent', () => {
  let component: CreateStudentDialogComponent;
  let fixture: ComponentFixture<CreateStudentDialogComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<CreateStudentDialogComponent>>;

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [CreateStudentDialogComponent, NoopAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CreateStudentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty fields', () => {
    expect(component.name).toBe('');
    expect(component.email).toBe('');
    expect(component.phone).toBe('');
    expect(component.isSubmitting).toBeFalse();
  });

  describe('onCancel', () => {
    it('should close dialog without a value', () => {
      component.onCancel();
      expect(dialogRefSpy.close).toHaveBeenCalledWith();
    });
  });

  describe('canSubmit', () => {
    it('should be false when fields are empty', () => {
      expect(component.canSubmit).toBeFalse();
    });

    it('should be false when email is invalid', () => {
      component.name = 'Bob';
      component.email = 'not-an-email';
      component.phone = '555-1234';
      expect(component.canSubmit).toBeFalse();
    });

    it('should be false when isSubmitting is true', () => {
      component.name = 'Bob';
      component.email = 'bob@example.com';
      component.phone = '555-1234';
      component.isSubmitting = true;
      expect(component.canSubmit).toBeFalse();
    });

    it('should be true when all fields are valid', () => {
      component.name = 'Bob';
      component.email = 'bob@example.com';
      component.phone = '555-1234';
      expect(component.canSubmit).toBeTrue();
    });
  });

  describe('isValidEmail', () => {
    it('should return true for valid emails', () => {
      expect(component.isValidEmail('bob@example.com')).toBeTrue();
      expect(component.isValidEmail('test.user+tag@domain.co')).toBeTrue();
    });

    it('should return false for invalid emails', () => {
      expect(component.isValidEmail('notanemail')).toBeFalse();
      expect(component.isValidEmail('missing@domain')).toBeFalse();
      expect(component.isValidEmail('@nodomain.com')).toBeFalse();
    });
  });

  describe('onSubmit', () => {
    it('should close dialog with trimmed values when valid', () => {
      component.name = 'Bob Student';
      component.email = 'bob@example.com';
      component.phone = '555-1234';
      component.onSubmit();
      expect(dialogRefSpy.close).toHaveBeenCalledWith({
        name: 'Bob Student',
        email: 'bob@example.com',
        phone: '555-1234'
      });
    });

    it('should not close dialog when canSubmit is false', () => {
      component.name = '';
      component.onSubmit();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });
  });
});
