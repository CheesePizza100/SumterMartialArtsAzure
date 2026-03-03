import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { ConfirmDialogComponent } from './confirm-dialog.component';

const mockDialogData = {
  title: 'Deactivate Student',
  message: 'Are you sure?',
  confirmText: 'Deactivate',
  cancelText: 'Cancel',
  isDestructive: true
};

describe('ConfirmDialogComponent', () => {
  let component: ConfirmDialogComponent;
  let fixture: ComponentFixture<ConfirmDialogComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<ConfirmDialogComponent>>;

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [ConfirmDialogComponent, NoopAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockDialogData }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ConfirmDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should expose dialog data', () => {
    expect(component.data).toEqual(mockDialogData);
  });

  describe('onCancel', () => {
    it('should close dialog with false', () => {
      component.onCancel();
      expect(dialogRefSpy.close).toHaveBeenCalledWith(false);
    });
  });

  describe('onConfirm', () => {
    it('should close dialog with true', () => {
      component.onConfirm();
      expect(dialogRefSpy.close).toHaveBeenCalledWith(true);
    });
  });
});
