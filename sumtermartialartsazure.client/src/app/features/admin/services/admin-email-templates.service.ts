import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { EmailTemplate, EmailTemplateDetail, UpdateEmailTemplateRequest } from '../models/email-template.model';

@Injectable({
  providedIn: 'root'
})
export class AdminEmailTemplatesService {
  private baseUrl = `${environment.apiUrl}/api/admin/email-templates`;

  constructor(private http: HttpClient) { }

  getAllTemplates(): Observable<EmailTemplate[]> {
    return this.http.get<EmailTemplate[]>(`${this.baseUrl}`);
  }

  getTemplateById(id: number): Observable<EmailTemplateDetail> {
    return this.http.get<EmailTemplateDetail>(`${this.baseUrl}/${id}`);
  }

  updateTemplate(id: number, request: UpdateEmailTemplateRequest): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, request);
  }
}
