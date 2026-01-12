import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminEmailTemplatesService } from '../../services/admin-email-templates.service';
import { EmailTemplateDetail } from '../../models/email-template.model';

@Component({
  selector: 'app-admin-email-template-edit',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-email-template-edit.component.html',
  styleUrls: ['./admin-email-template-edit.component.css']
})
export class AdminEmailTemplateEditComponent implements OnInit {
  template: EmailTemplateDetail | null = null;
  loading = false;
  saving = false;
  error = '';
  successMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private emailTemplatesService: AdminEmailTemplatesService
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadTemplate(id);
  }

  loadTemplate(id: number): void {
    this.loading = true;
    this.error = '';

    this.emailTemplatesService.getTemplateById(id).subscribe({
      next: (template) => {
        this.template = template;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load email template';
        this.loading = false;
        console.error('Error loading template:', err);
      }
    });
  }

  saveTemplate(): void {
    if (!this.template) return;

    this.saving = true;
    this.error = '';
    this.successMessage = '';

    const request = {
      name: this.template.name,
      subject: this.template.subject,
      body: this.template.body,
      description: this.template.description
    };

    this.emailTemplatesService.updateTemplate(this.template.id, request).subscribe({
      next: () => {
        this.successMessage = 'Template updated successfully!';
        this.saving = false;
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (err) => {
        this.error = 'Failed to update template';
        this.saving = false;
        console.error('Error updating template:', err);
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/admin/email-templates']);
  }
}
