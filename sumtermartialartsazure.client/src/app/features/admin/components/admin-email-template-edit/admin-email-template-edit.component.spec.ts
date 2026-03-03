import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { AdminEmailTemplateEditComponent } from './admin-email-template-edit.component';
import { AdminEmailTemplatesService } from '../../services/admin-email-templates.service';
import { EmailTemplateDetail } from '../../models/email-template.model';

const mockTemplate: EmailTemplateDetail = {
  id: 1,
  name: 'Welcome Email',
  subject: 'Welcome!',
  body: 'Hello there',
  description: 'Sent on signup'
} as EmailTemplateDetail;

describe('AdminEmailTemplateEditComponent', () => {
  let component: AdminEmailTemplateEditComponent;
  let fixture: ComponentFixture<AdminEmailTemplateEditComponent>;
  let emailTemplatesServiceSpy: jasmine.SpyObj<AdminEmailTemplatesService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    emailTemplatesServiceSpy = jasmine.createSpyObj('AdminEmailTemplatesService', [
      'getTemplateById',
      'updateTemplate'
    ]);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [AdminEmailTemplateEditComponent],
      providers: [
        { provide: AdminEmailTemplatesService, useValue: emailTemplatesServiceSpy },
        { provide: Router, useValue: routerSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: (key: string) => '1'  // simulate route param id=1
              }
            }
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AdminEmailTemplateEditComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    emailTemplatesServiceSpy.getTemplateById.and.returnValue(of(mockTemplate));
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should read id from route and call loadTemplate', () => {
      emailTemplatesServiceSpy.getTemplateById.and.returnValue(of(mockTemplate));
      fixture.detectChanges();
      expect(emailTemplatesServiceSpy.getTemplateById).toHaveBeenCalledWith(1);
    });
  });

  describe('loadTemplate', () => {
    it('should set template and clear loading on success', () => {
      emailTemplatesServiceSpy.getTemplateById.and.returnValue(of(mockTemplate));
      fixture.detectChanges();

      expect(component.template).toEqual(mockTemplate);
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });

    it('should set error and clear loading on failure', () => {
      spyOn(console, 'error');
      emailTemplatesServiceSpy.getTemplateById.and.returnValue(
        throwError(() => new Error('Network error'))
      );
      fixture.detectChanges();

      expect(component.template).toBeNull();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Failed to load email template');
    });
  });

  describe('saveTemplate', () => {
    beforeEach(() => {
      emailTemplatesServiceSpy.getTemplateById.and.returnValue(of(mockTemplate));
      fixture.detectChanges(); // load template first
    });

    it('should do nothing if template is null', () => {
      component.template = null;
      component.saveTemplate();
      expect(emailTemplatesServiceSpy.updateTemplate).not.toHaveBeenCalled();
    });

    it('should call updateTemplate with correct payload on save', () => {
      emailTemplatesServiceSpy.updateTemplate.and.returnValue(of(void 0));
      component.saveTemplate();

      expect(emailTemplatesServiceSpy.updateTemplate).toHaveBeenCalledWith(1, {
        name: mockTemplate.name,
        subject: mockTemplate.subject,
        body: mockTemplate.body,
        description: mockTemplate.description
      });
    });

    it('should set successMessage and clear saving on success', () => {
      emailTemplatesServiceSpy.updateTemplate.and.returnValue(of(void 0));
      component.saveTemplate();

      expect(component.successMessage).toBe('Template updated successfully!');
      expect(component.saving).toBeFalse();
    });

    it('should clear successMessage after 3 seconds', fakeAsync(() => {
      emailTemplatesServiceSpy.updateTemplate.and.returnValue(of(void 0));
      component.saveTemplate();

      expect(component.successMessage).toBe('Template updated successfully!');
      tick(3000);
      expect(component.successMessage).toBe('');
    }));

    it('should set error and clear saving on failure', () => {
      spyOn(console, 'error');
      emailTemplatesServiceSpy.updateTemplate.and.returnValue(
        throwError(() => new Error('Save error'))
      );
      component.saveTemplate();

      expect(component.error).toBe('Failed to update template');
      expect(component.saving).toBeFalse();
    });
  });

  describe('cancel', () => {
    it('should navigate to /admin/email-templates', () => {
      emailTemplatesServiceSpy.getTemplateById.and.returnValue(of(mockTemplate));
      fixture.detectChanges();

      component.cancel();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/admin/email-templates']);
    });
  });
});
