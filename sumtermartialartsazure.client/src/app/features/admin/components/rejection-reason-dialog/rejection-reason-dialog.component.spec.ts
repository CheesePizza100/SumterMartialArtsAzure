import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { RejectionReasonDialogComponent } from './rejection-reason-dialog.component';

describe('RejectionReasonDialogComponent', () => {
  let component: RejectionReasonDialogComponent;
  let fixture: ComponentFixture<RejectionReasonDialogComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<RejectionReasonDialogComponent>>;

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [RejectionReasonDialogComponent, NoopAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: { studentName: 'Bob Student' } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RejectionReasonDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty reason', () => {
    expect(component.reason).toBe('');
  });

  it('should expose dialog data', () => {
    expect(component.data.studentName).toBe('Bob Student');
  });

  describe('cancel', () => {
    it('should close dialog without a value', () => {
      component.cancel();
      expect(dialogRefSpy.close).toHaveBeenCalledWith();
    });
  });

  describe('submit', () => {
    it('should not close when reason is empty', () => {
      component.reason = '';
      component.submit();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });

    it('should not close when reason is only whitespace', () => {
      component.reason = '   ';
      component.submit();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });

    it('should close with trimmed reason when valid', () => {
      component.reason = '  Not a good fit  ';
      component.submit();
      expect(dialogRefSpy.close).toHaveBeenCalledWith('Not a good fit');
    });
  });
});
