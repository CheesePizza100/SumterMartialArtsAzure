import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { Instructor } from '../../instructors/models/instructor.model';

@Injectable({
  providedIn: 'root'
})
export class AdminInstructorsService {
  private baseUrl = `${environment.apiUrl}/api`;

  constructor(private http: HttpClient) { }

  getAllInstructors(): Observable<Instructor[]> {
    return this.http.get<Instructor[]>(`${this.baseUrl}/instructors`);
  }

  createInstructorLogin(instructorId: number, request: {
    username: string;
    password?: string | null;
  }): Observable<{
    success: boolean;
    message: string;
    username: string;
    temporaryPassword: string;
    userId: string;
  }> {
    return this.http.post<{
      success: boolean;
      message: string;
      username: string;
      temporaryPassword: string;
      userId: string;
    }>(`${this.baseUrl}/instructors/${instructorId}/create-login`, request);
  }
}
