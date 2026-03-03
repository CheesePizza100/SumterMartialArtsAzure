import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { ChangePasswordComponent } from './change-password.component';
import { AuthService } from '../../../../core/services/auth.service';

describe('ChangePasswordComponent', () => {
  let component: ChangePasswordComponent;
  let fixture: ComponentFixture<ChangePasswordComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    authServiceSpy = jasmine.createSpyObj('AuthService', ['changePassword', 'getCurrentUser']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [ChangePasswordComponent],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ChangePasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('isValid', () => {
    it('should return false when currentPassword is empty', () => {
      component.currentPassword = '';
      component.newPassword = 'newpass123';
      component.confirmPassword = 'newpass123';
      expect(component.isValid()).toBeFalse();
    });

    it('should return false when newPassword is less than 8 characters', () => {
      component.currentPassword = 'oldpass';
      component.newPassword = 'short';
      component.confirmPassword = 'short';
      expect(component.isValid()).toBeFalse();
    });

    it('should return false when passwords do not match', () => {
      component.currentPassword = 'oldpass';
      component.newPassword = 'newpass123';
      component.confirmPassword = 'different';
      expect(component.isValid()).toBeFalse();
    });

    it('should return true when all fields are valid', () => {
      component.currentPassword = 'oldpass';
      component.newPassword = 'newpass123';
      component.confirmPassword = 'newpass123';
      expect(component.isValid()).toBeTrue();
    });
  });

  describe('onSubmit', () => {
    it('should set error and not call service when invalid', () => {
      component.currentPassword = '';
      component.onSubmit();
      expect(component.error).toBe('Please fill in all fields correctly');
      expect(authServiceSpy.changePassword).not.toHaveBeenCalled();
    });

    it('should call changePassword with correct args when valid', () => {
      authServiceSpy.changePassword.and.returnValue(of(void 0));
      authServiceSpy.getCurrentUser.and.returnValue(null);

      component.currentPassword = 'oldpass';
      component.newPassword = 'newpass123';
      component.confirmPassword = 'newpass123';
      component.onSubmit();

      expect(authServiceSpy.changePassword).toHaveBeenCalledWith('oldpass', 'newpass123');
    });

    it('should navigate to /admin for Admin role on success', () => {
      authServiceSpy.changePassword.and.returnValue(of(void 0));
      authServiceSpy.getCurrentUser.and.returnValue({ role: 'Admin' } as any);

      component.currentPassword = 'oldpass';
      component.newPassword = 'newpass123';
      component.confirmPassword = 'newpass123';
      component.onSubmit();

      expect(routerSpy.navigate).toHaveBeenCalledWith(['/admin']);
    });

    it('should navigate to /student for Student role on success', () => {
      authServiceSpy.changePassword.and.returnValue(of(void 0));
      authServiceSpy.getCurrentUser.and.returnValue({ role: 'Student' } as any);

      component.currentPassword = 'oldpass';
      component.newPassword = 'newpass123';
      component.confirmPassword = 'newpass123';
      component.onSubmit();

      expect(routerSpy.navigate).toHaveBeenCalledWith(['/student']);
    });

    it('should navigate to /home for unknown role on success', () => {
      authServiceSpy.changePassword.and.returnValue(of(void 0));
      authServiceSpy.getCurrentUser.and.returnValue({ role: 'Other' } as any);

      component.currentPassword = 'oldpass';
      component.newPassword = 'newpass123';
      component.confirmPassword = 'newpass123';
      component.onSubmit();

      expect(routerSpy.navigate).toHaveBeenCalledWith(['/home']);
    });

    it('should navigate to /home when getCurrentUser returns null', () => {
      authServiceSpy.changePassword.and.returnValue(of(void 0));
      authServiceSpy.getCurrentUser.and.returnValue(null);

      component.currentPassword = 'oldpass';
      component.newPassword = 'newpass123';
      component.confirmPassword = 'newpass123';
      component.onSubmit();

      expect(routerSpy.navigate).toHaveBeenCalledWith(['/home']);
    });

    it('should clear loading and set error on failure', () => {
      spyOn(console, 'error');
      authServiceSpy.changePassword.and.returnValue(
        throwError(() => ({ error: { message: 'Wrong password' } }))
      );

      component.currentPassword = 'oldpass';
      component.newPassword = 'newpass123';
      component.confirmPassword = 'newpass123';
      component.onSubmit();

      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Wrong password');
    });

    it('should use fallback error message when error has no message', () => {
      spyOn(console, 'error');
      authServiceSpy.changePassword.and.returnValue(
        throwError(() => ({ error: {} }))
      );

      component.currentPassword = 'oldpass';
      component.newPassword = 'newpass123';
      component.confirmPassword = 'newpass123';
      component.onSubmit();

      expect(component.error).toBe('Failed to change password. Please check your current password.');
    });
  });
});
