import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { UpdateNotesDialogComponent } from './update-notes-dialog.component';

const mockDialogData = {
  studentName: 'Bob Student',
  programName: 'Karate',
  currentNotes: 'Good progress so far'
};

describe('UpdateNotesDialogComponent', () => {
  let component: UpdateNotesDialogComponent;
  let fixture: ComponentFixture<UpdateNotesDialogComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<UpdateNotesDialogComponent>>;

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [UpdateNotesDialogComponent, NoopAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockDialogData }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(UpdateNotesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize notes from dialog data', () => {
    expect(component.notes).toBe('Good progress so far');
  });

  it('should expose dialog data', () => {
    expect(component.data.studentName).toBe('Bob Student');
    expect(component.data.programName).toBe('Karate');
  });

  describe('onCancel', () => {
    it('should close dialog without a value', () => {
      component.onCancel();
      expect(dialogRefSpy.close).toHaveBeenCalledWith();
    });
  });

  describe('onSave', () => {
    it('should close dialog with current notes', () => {
      component.notes = 'Updated notes';
      component.onSave();
      expect(dialogRefSpy.close).toHaveBeenCalledWith('Updated notes');
    });

    it('should close with empty string when notes cleared', () => {
      component.notes = '';
      component.onSave();
      expect(dialogRefSpy.close).toHaveBeenCalledWith('');
    });

    it('should close with original notes if unchanged', () => {
      component.onSave();
      expect(dialogRefSpy.close).toHaveBeenCalledWith('Good progress so far');
    });
  });
});
