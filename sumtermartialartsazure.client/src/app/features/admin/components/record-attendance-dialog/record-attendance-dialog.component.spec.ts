import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { RecordAttendanceDialogComponent } from './record-attendance-dialog.component';

const mockDialogData = {
  studentId: 1,
  studentName: 'Bob Student',
  programs: [
    { programId: 1, programName: 'Karate', currentTotal: 20, currentLast30Days: 8 },
    { programId: 2, programName: 'BJJ', currentTotal: 10, currentLast30Days: 3 }
  ]
};

describe('RecordAttendanceDialogComponent', () => {
  let component: RecordAttendanceDialogComponent;
  let fixture: ComponentFixture<RecordAttendanceDialogComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<RecordAttendanceDialogComponent>>;

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [RecordAttendanceDialogComponent, NoopAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockDialogData }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RecordAttendanceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default values', () => {
    expect(component.selectedProgramId).toBeUndefined();
    expect(component.classesAttended).toBe(1);
    expect(component.isSubmitting).toBeFalse();
  });

  describe('selectedProgram', () => {
    it('should return undefined when no program selected', () => {
      expect(component.selectedProgram).toBeUndefined();
    });

    it('should return matching program when selectedProgramId is set', () => {
      component.selectedProgramId = 1;
      expect(component.selectedProgram).toEqual(mockDialogData.programs[0]);
    });
  });

  describe('currentTotal', () => {
    it('should return 0 when no program selected', () => {
      expect(component.currentTotal).toBe(0);
    });

    it('should return currentTotal of selected program', () => {
      component.selectedProgramId = 1;
      expect(component.currentTotal).toBe(20);
    });
  });

  describe('currentLast30Days', () => {
    it('should return 0 when no program selected', () => {
      expect(component.currentLast30Days).toBe(0);
    });

    it('should return currentLast30Days of selected program', () => {
      component.selectedProgramId = 1;
      expect(component.currentLast30Days).toBe(8);
    });
  });

  describe('newTotal', () => {
    it('should return classesAttended when no program selected', () => {
      component.classesAttended = 3;
      expect(component.newTotal).toBe(3);
    });

    it('should return currentTotal plus classesAttended', () => {
      component.selectedProgramId = 1;
      component.classesAttended = 5;
      expect(component.newTotal).toBe(25);
    });
  });

  describe('newLast30Days', () => {
    it('should return sum when under 30', () => {
      component.selectedProgramId = 1; // currentLast30Days = 8
      component.classesAttended = 5;
      expect(component.newLast30Days).toBe(13);
    });

    it('should cap at 30', () => {
      component.selectedProgramId = 1; // currentLast30Days = 8
      component.classesAttended = 25;
      expect(component.newLast30Days).toBe(30);
    });

    it('should return classesAttended when no program selected', () => {
      component.classesAttended = 4;
      expect(component.newLast30Days).toBe(4);
    });
  });

  describe('canSubmit', () => {
    it('should be false when no program selected', () => {
      expect(component.canSubmit).toBeFalse();
    });

    it('should be false when classesAttended is 0', () => {
      component.selectedProgramId = 1;
      component.classesAttended = 0;
      expect(component.canSubmit).toBeFalse();
    });

    it('should be false when classesAttended exceeds 30', () => {
      component.selectedProgramId = 1;
      component.classesAttended = 31;
      expect(component.canSubmit).toBeFalse();
    });

    it('should be false when isSubmitting is true', () => {
      component.selectedProgramId = 1;
      component.classesAttended = 5;
      component.isSubmitting = true;
      expect(component.canSubmit).toBeFalse();
    });

    it('should be true when all conditions met', () => {
      component.selectedProgramId = 1;
      component.classesAttended = 5;
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
    it('should not close when canSubmit is false', () => {
      component.selectedProgramId = undefined;
      component.onSubmit();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });

    it('should close with correct payload when valid', () => {
      component.selectedProgramId = 2;
      component.classesAttended = 4;
      component.onSubmit();
      expect(dialogRefSpy.close).toHaveBeenCalledWith({
        programId: 2,
        classesAttended: 4
      });
    });
  });
});
