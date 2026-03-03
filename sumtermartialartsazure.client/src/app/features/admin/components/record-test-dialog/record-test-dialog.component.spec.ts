import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { RecordTestDialogComponent } from './record-test-dialog.component';

const mockDialogData = {
  studentId: 1,
  studentName: 'Bob Student',
  programs: [
    { id: 1, name: 'Karate', currentRank: 'White Belt' },
    { id: 2, name: 'BJJ', currentRank: 'Blue Belt' }
  ]
};

describe('RecordTestDialogComponent', () => {
  let component: RecordTestDialogComponent;
  let fixture: ComponentFixture<RecordTestDialogComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<RecordTestDialogComponent>>;

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [RecordTestDialogComponent, NoopAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockDialogData }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RecordTestDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default values', () => {
    expect(component.selectedProgramId).toBeUndefined();
    expect(component.rankTested).toBe('');
    expect(component.result).toBe('Pass');
    expect(component.notes).toBe('');
    expect(component.isSubmitting).toBeFalse();
    expect(component.testDate).toBeInstanceOf(Date);
  });

  describe('selectedProgram', () => {
    it('should return undefined when no program selected', () => {
      expect(component.selectedProgram).toBeUndefined();
    });

    it('should return matching program', () => {
      component.selectedProgramId = 2;
      expect(component.selectedProgram).toEqual(mockDialogData.programs[1]);
    });
  });

  describe('canSubmit', () => {
    it('should be false when no program selected', () => {
      expect(component.canSubmit).toBeFalse();
    });

    it('should be false when rankTested is empty', () => {
      component.selectedProgramId = 1;
      component.notes = 'Good effort';
      expect(component.canSubmit).toBeFalse();
    });

    it('should be false when notes is empty', () => {
      component.selectedProgramId = 1;
      component.rankTested = 'Yellow Belt';
      expect(component.canSubmit).toBeFalse();
    });

    it('should be false when isSubmitting is true', () => {
      component.selectedProgramId = 1;
      component.rankTested = 'Yellow Belt';
      component.notes = 'Good effort';
      component.isSubmitting = true;
      expect(component.canSubmit).toBeFalse();
    });

    it('should be true when all fields are valid', () => {
      component.selectedProgramId = 1;
      component.rankTested = 'Yellow Belt';
      component.notes = 'Good effort';
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
    it('should not close when required fields are missing', () => {
      component.selectedProgramId = undefined;
      component.onSubmit();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });

    it('should not close when rankTested is empty', () => {
      component.selectedProgramId = 1;
      component.rankTested = '';
      component.notes = 'Good effort';
      component.onSubmit();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });

    it('should not close when notes is empty', () => {
      component.selectedProgramId = 1;
      component.rankTested = 'Yellow Belt';
      component.notes = '';
      component.onSubmit();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });

    it('should close with correct payload when valid', () => {
      const fixedDate = new Date('2026-03-15T10:00:00');
      component.selectedProgramId = 1;
      component.rankTested = '  Yellow Belt  ';
      component.result = 'Pass';
      component.notes = '  Good effort  ';
      component.testDate = fixedDate;

      component.onSubmit();

      expect(dialogRefSpy.close).toHaveBeenCalledWith({
        programId: 1,
        programName: 'Karate',
        rank: 'Yellow Belt',
        result: 'Pass',
        notes: 'Good effort',
        testDate: fixedDate.toISOString()
      });
    });

    it('should close with Fail result when set', () => {
      const fixedDate = new Date('2026-03-15T10:00:00');
      component.selectedProgramId = 2;
      component.rankTested = 'Purple Belt';
      component.result = 'Fail';
      component.notes = 'Needs more practice';
      component.testDate = fixedDate;

      component.onSubmit();

      expect(dialogRefSpy.close).toHaveBeenCalledWith(
        jasmine.objectContaining({ result: 'Fail', programName: 'BJJ' })
      );
    });
  });
});
