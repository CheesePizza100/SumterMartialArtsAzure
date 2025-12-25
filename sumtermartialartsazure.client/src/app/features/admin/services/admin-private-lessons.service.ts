import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PrivateLessonRequest, UpdateStatus } from '../models/private-lesson-request.model';

@Injectable({
  providedIn: 'root'
})
export class AdminPrivateLessonsService {
  private baseUrl = '/api';
  constructor(private http: HttpClient) { }

  getAllRequests(filter: string = 'Pending'): Observable<PrivateLessonRequest[]> {
    return this.http.get<PrivateLessonRequest[]>(
      `${this.baseUrl}/private-lessons?filter=${filter}`
    );
  }

  updateStatus(requestId: number, status: string, rejectionReason?: string): Observable<UpdateStatus> {
    return this.http.patch<UpdateStatus>(
      `${this.baseUrl}/private-lessons/${requestId}/status`,
      { status, rejectionReason }
    );
  }
}
