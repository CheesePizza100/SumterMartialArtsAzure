export interface EmailTemplate {
  id: number;
  templateKey: string;
  name: string;
  subject: string;
  description?: string;
  isActive: boolean;
  lastModified: string;
}

export interface EmailTemplateDetail {
  id: number;
  templateKey: string;
  name: string;
  subject: string;
  body: string;
  description?: string;
  isActive: boolean;
}

export interface UpdateEmailTemplateRequest {
  name: string;
  subject: string;
  body: string;
  description?: string;
}
