import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { of, throwError } from 'rxjs';
import { provideRouter } from '@angular/router';

import { HeaderComponent } from './header.component';
import { AuthService } from '../../../core/services/auth.service';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;

  beforeEach(async () => {
    authServiceSpy = jasmine.createSpyObj('AuthService', [
      'isAdmin',
      'isLoggedIn',
      'logout',
      'forceLogout'
    ]);
    authServiceSpy.isAdmin.and.returnValue(false);
    authServiceSpy.isLoggedIn.and.returnValue(false);

    await TestBed.configureTestingModule({
      imports: [HeaderComponent, RouterModule.forRoot([])],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        provideRouter([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with menus closed', () => {
    expect(component.mobileMenuOpen).toBeFalse();
    expect(component.adminDropdownOpen).toBeFalse();
  });

  it('should have correct nav links defined', () => {
    expect(component.navLinks.length).toBe(3);
  });

  it('should have correct admin links defined', () => {
    expect(component.adminLinks.length).toBe(5);
  });

  describe('isAdmin getter', () => {
    it('should return false when user is not admin', () => {
      authServiceSpy.isAdmin.and.returnValue(false);
      expect(component.isAdmin).toBeFalse();
    });

    it('should return true when user is admin', () => {
      authServiceSpy.isAdmin.and.returnValue(true);
      expect(component.isAdmin).toBeTrue();
    });
  });

  describe('toggleMobile', () => {
    it('should open mobile menu when closed', () => {
      component.mobileMenuOpen = false;
      component.toggleMobile();
      expect(component.mobileMenuOpen).toBeTrue();
    });

    it('should close mobile menu when open', () => {
      component.mobileMenuOpen = true;
      component.toggleMobile();
      expect(component.mobileMenuOpen).toBeFalse();
    });
  });

  describe('closeMobileMenu', () => {
    it('should close mobile menu', () => {
      component.mobileMenuOpen = true;
      component.closeMobileMenu();
      expect(component.mobileMenuOpen).toBeFalse();
    });
  });

  describe('toggleAdminDropdown', () => {
    it('should open dropdown when closed', () => {
      component.adminDropdownOpen = false;
      component.toggleAdminDropdown();
      expect(component.adminDropdownOpen).toBeTrue();
    });

    it('should close dropdown when open', () => {
      component.adminDropdownOpen = true;
      component.toggleAdminDropdown();
      expect(component.adminDropdownOpen).toBeFalse();
    });
  });

  describe('closeAdminDropdown', () => {
    it('should close admin dropdown', () => {
      component.adminDropdownOpen = true;
      component.closeAdminDropdown();
      expect(component.adminDropdownOpen).toBeFalse();
    });
  });

  describe('onDocumentClick', () => {
    it('should close admin dropdown when clicking outside', () => {
      component.adminDropdownOpen = true;
      const event = new MouseEvent('click');
      Object.defineProperty(event, 'target', { value: document.createElement('div') });
      component.onDocumentClick(event);
      expect(component.adminDropdownOpen).toBeFalse();
    });

    it('should keep dropdown open when clicking inside admin-dropdown-container', () => {
      component.adminDropdownOpen = true;
      const container = document.createElement('div');
      container.classList.add('admin-dropdown-container');
      const inner = document.createElement('button');
      container.appendChild(inner);
      document.body.appendChild(container);

      const event = new MouseEvent('click');
      Object.defineProperty(event, 'target', { value: inner });
      component.onDocumentClick(event);

      expect(component.adminDropdownOpen).toBeTrue();
      document.body.removeChild(container);
    });
  });

  describe('logout', () => {
    it('should call auth.logout', () => {
      authServiceSpy.logout.and.returnValue(of(void 0));
      component.logout();
      expect(authServiceSpy.logout).toHaveBeenCalled();
    });

    it('should call forceLogout on logout error', () => {
      spyOn(console, 'error');
      authServiceSpy.logout.and.returnValue(throwError(() => new Error('error')));
      component.logout();
      expect(authServiceSpy.forceLogout).toHaveBeenCalled();
    });
  });
});
