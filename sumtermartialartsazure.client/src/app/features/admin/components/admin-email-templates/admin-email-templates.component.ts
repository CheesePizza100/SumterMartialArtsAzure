import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AdminEmailTemplatesService } from '../../services/admin-email-templates.service';
import { EmailTemplate } from '../../models/email-template.model';

@Component({
  selector: 'app-admin-email-templates',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-email-templates.component.html',
  styleUrls: ['./admin-email-templates.component.css']
})
export class AdminEmailTemplatesComponent implements OnInit {
  templates: EmailTemplate[] = [];
  loading = false;
  error = '';

  constructor(
    private emailTemplatesService: AdminEmailTemplatesService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadTemplates();
  }

  loadTemplates(): void {
    this.loading = true;
    this.error = '';

    this.emailTemplatesService.getAllTemplates().subscribe({
      next: (templates) => {
        this.templates = templates;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load email templates';
        this.loading = false;
        console.error('Error loading templates:', err);
      }
    });
  }

  editTemplate(id: number): void {
    this.router.navigate(['/admin/email-templates', id]);
  }
}
