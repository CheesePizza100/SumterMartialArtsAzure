import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { LoginComponent } from './login.component';
import { AuthService } from '../../../../../app/core/services/auth.service';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    authServiceSpy = jasmine.createSpyObj('AuthService', ['login']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [LoginComponent],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty fields', () => {
    expect(component.username).toBe('');
    expect(component.password).toBe('');
    expect(component.loading).toBeFalse();
    expect(component.error).toBe('');
  });

  describe('onSubmit', () => {
    it('should call login with username and password', () => {
      authServiceSpy.login.and.returnValue(of({ role: 'Admin', mustChangePassword: false } as any));
      component.username = 'admin';
      component.password = 'pass123';
      component.onSubmit();
      expect(authServiceSpy.login).toHaveBeenCalledWith('admin', 'pass123');
    });

    it('should navigate to /change-password when mustChangePassword is true', () => {
      authServiceSpy.login.and.returnValue(of({ role: 'Admin', mustChangePassword: true } as any));
      component.onSubmit();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/change-password']);
    });

    it('should navigate to /admin for Admin role', () => {
      authServiceSpy.login.and.returnValue(of({ role: 'Admin', mustChangePassword: false } as any));
      component.onSubmit();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/admin']);
    });

    it('should navigate to /student for Student role', () => {
      authServiceSpy.login.and.returnValue(of({ role: 'Student', mustChangePassword: false } as any));
      component.onSubmit();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/student']);
    });

    it('should navigate to /instructors for Instructor role', () => {
      authServiceSpy.login.and.returnValue(of({ role: 'Instructor', mustChangePassword: false } as any));
      component.onSubmit();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/instructors']);
    });

    it('should navigate to /home for unknown role', () => {
      authServiceSpy.login.and.returnValue(of({ role: 'Other', mustChangePassword: false } as any));
      component.onSubmit();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/home']);
    });

    it('should clear loading and set error on failure', () => {
      spyOn(console, 'error');
      authServiceSpy.login.and.returnValue(throwError(() => new Error('Unauthorized')));
      component.onSubmit();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Invalid username or password');
    });
  });
});
