import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { CreateLoginDialogComponent } from './create-login-dialog.component';

describe('CreateLoginDialogComponent', () => {
  let component: CreateLoginDialogComponent;
  let fixture: ComponentFixture<CreateLoginDialogComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<CreateLoginDialogComponent>>;

  const mockDialogData = {
    studentName: 'Bob Student',
    suggestedUsername: 'bob@example.com'
  };

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [CreateLoginDialogComponent, NoopAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockDialogData }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CreateLoginDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize username from dialog data', () => {
    expect(component.username).toBe('bob@example.com');
  });

  describe('onCancel', () => {
    it('should close dialog without a value', () => {
      component.onCancel();
      expect(dialogRefSpy.close).toHaveBeenCalledWith();
    });
  });

  describe('onCreate', () => {
    it('should close dialog with trimmed username when valid', () => {
      component.username = '  bobstudent  ';
      component.onCreate();
      expect(dialogRefSpy.close).toHaveBeenCalledWith('bobstudent');
    });

    it('should not close dialog when username is empty', () => {
      component.username = '';
      component.onCreate();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });

    it('should not close dialog when username is only whitespace', () => {
      component.username = '   ';
      component.onCreate();
      expect(dialogRefSpy.close).not.toHaveBeenCalled();
    });
  });
});
