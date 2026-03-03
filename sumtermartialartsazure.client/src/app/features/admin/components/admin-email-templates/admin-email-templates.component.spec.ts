import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { AdminEmailTemplatesComponent } from './admin-email-templates.component';
import { AdminEmailTemplatesService } from '../../services/admin-email-templates.service';
import { EmailTemplate } from '../../models/email-template.model';

const mockTemplates: EmailTemplate[] = [
  { id: 1, name: 'Welcome Email', subject: 'Welcome!' } as EmailTemplate,
  { id: 2, name: 'Reset Password', subject: 'Reset your password' } as EmailTemplate,
];

describe('AdminEmailTemplatesComponent', () => {
  let component: AdminEmailTemplatesComponent;
  let fixture: ComponentFixture<AdminEmailTemplatesComponent>;
  let emailTemplatesServiceSpy: jasmine.SpyObj<AdminEmailTemplatesService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    emailTemplatesServiceSpy = jasmine.createSpyObj('AdminEmailTemplatesService', ['getAllTemplates']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [AdminEmailTemplatesComponent],
      providers: [
        { provide: AdminEmailTemplatesService, useValue: emailTemplatesServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AdminEmailTemplatesComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    emailTemplatesServiceSpy.getAllTemplates.and.returnValue(of([]));
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  describe('loadTemplates', () => {
    it('should set templates and clear loading on success', () => {
      emailTemplatesServiceSpy.getAllTemplates.and.returnValue(of(mockTemplates));
      fixture.detectChanges();

      expect(component.templates).toEqual(mockTemplates);
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });

    it('should set error and clear loading on failure', () => {
      spyOn(console, 'error');
      emailTemplatesServiceSpy.getAllTemplates.and.returnValue(
        throwError(() => new Error('Network error'))
      );
      fixture.detectChanges();

      expect(component.templates).toEqual([]);
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Failed to load email templates');
    });
  });
});
