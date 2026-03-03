import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { EnrollProgramDialogComponent } from './enroll-program-dialog.component';

const mockDialogData = {
  studentId: 1,
  studentName: 'Bob Student',
  availablePrograms: [
    { id: 1, name: 'Karate' },
    { id: 2, name: 'BJJ' }
  ]
};

describe('EnrollProgramDialogComponent', () => {
  let component: EnrollProgramDialogComponent;
  let fixture: ComponentFixture<EnrollProgramDialogComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<EnrollProgramDialogComponent>>;

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [EnrollProgramDialogComponent, NoopAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockDialogData }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(EnrollProgramDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default values', () => {
    expect(component.selectedProgramId).toBeUndefined();
    expect(component.initialRank).toBe('White Belt');
    expect(component.isSubmitting).toBeFalse();
  });

  it('should expose dialog data', () => {
    expect(component.data).toEqual(mockDialogData);
  });

  describe('selectedProgram', () => {
    it('should return undefined when no program is selected', () => {
      expect(component.selectedProgram).toBeUndefined();
    });

    it('should return the matching program when selectedProgramId is set', () => {
      component.selectedProgramId = 1;
      expect(component.selectedProgram).toEqual({ id: 1, name: 'Karate' });
    });

    it('should return undefined for an unknown programId', () => {
      component.selectedProgramId = 999;
      expect(component.selectedProgram).toBeUndefined();
    });
  });

  describe('canSubmit', () => {
    it('should be false when no program is selected', () => {
      expect(component.canSubmit).toBeFalse();
    });

    it('should be false when initialRank is empty', () => {
      component.selectedProgramId = 1;
      component.initialRank = '';
      expect(component.canSubmit).toBeFalse();
    });

    it('should be false when isSubmitting is true', () => {
      component.selectedProgramId = 1;
      component.initialRank = 'White Belt';
      component.isSubmitting = true;
      expect(component.canSubmit).toBeFalse();
    });

    it('should be true when program selected, rank set, and not submitting', () => {
      component.selectedProgramId = 1;
      expect(component.canSubmit).toBeTrue();
    });
  });

  describe('onCancel', () => {
    it('should close dialog without a value', () => {
      component.onCancel();
      expect(dialogRefSpy.close).toHaveBeenCalledWith();
    });
  });

  describe('onSubmit', () => {
    it('should not close dialog when canSubmit is false', () => {
      component.selectedProgramId = undefined;
      component.onSubmit();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });

    it('should close dialog with correct payload when valid', () => {
      component.selectedProgramId = 2;
      component.initialRank = 'White Belt';
      component.onSubmit();
      expect(dialogRefSpy.close).toHaveBeenCalledWith({
        programId: 2,
        programName: 'BJJ',
        initialRank: 'White Belt'
      });
    });

    it('should trim initialRank before closing', () => {
      component.selectedProgramId = 1;
      component.initialRank = '  Blue Belt  ';
      component.onSubmit();
      expect(dialogRefSpy.close).toHaveBeenCalledWith({
        programId: 1,
        programName: 'Karate',
        initialRank: 'Blue Belt'
      });
    });
  });
});
