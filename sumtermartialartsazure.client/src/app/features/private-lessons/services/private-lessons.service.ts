import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { PrivateLessonRequest, LessonTime } from '../models/private-lesson.model';
import { environment } from '../../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class PrivateLessonsService {
  private baseUrl = `${environment.apiUrl}/api`;

  constructor(private http: HttpClient) { }

  submitLessonRequest(request: PrivateLessonRequest): Observable<PrivateLessonRequest> {
    return this.http.post<PrivateLessonRequest>(
      `${this.baseUrl}/PrivateLessons`,
      request
    );
  }

  getInstructorAvailability(instructorId: number, count: number = 30): Observable<LessonTime[]> {
    return this.http.get<{ availableSlots: LessonTime[] }>(
      `${this.baseUrl}/instructors/${instructorId}/availability?count=${count}`
    ).pipe(
      map(response => response.availableSlots)
    );
  }

  getAllRequests(): Observable<PrivateLessonRequest[]> {
    return this.http.get<PrivateLessonRequest[]>(
      `${this.baseUrl}/PrivateLesson`
    );
  }

  updateRequestStatus(id: number, status: string): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/PrivateLesson/${id}/status`,
      status
    );
  }
}
