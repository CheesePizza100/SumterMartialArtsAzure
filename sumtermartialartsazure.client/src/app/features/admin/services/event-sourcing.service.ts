import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { MonthlyTestActivity, RankDistribution, ProgressionAnalytics, StudentEvent, StudentRankAtDate } from '../models/event-sourcing.model';


@Injectable({
  providedIn: 'root'
})
export class EventSourcingService {
  private baseUrl = `${environment.apiUrl}/api`;

  constructor(private http: HttpClient) { }

  /**
   * Get analytics across all programs or filtered by program
   */
  getProgressionAnalytics(programId?: number): Observable<ProgressionAnalytics> {
    const url = programId
      ? `${this.baseUrl}/students/analytics/progression?programId=${programId}`
      : `${this.baseUrl}/students/analytics/progression`;
    return this.http.get<ProgressionAnalytics>(url);
  }

  /**
   * Get complete event stream for a student in a program
   */
  getStudentEventStream(studentId: number, programId: number): Observable<StudentEvent[]> {
    return this.http.get<StudentEvent[]>(
      `${this.baseUrl}/students/${studentId}/programs/${programId}/events`
    );
  }

  /**
   * Time-travel query - get student's rank at a specific date
   */
  getStudentRankAtDate(
    studentId: number,
    programId: number,
    asOfDate: string
  ): Observable<StudentRankAtDate> {
    return this.http.get<StudentRankAtDate>(
      `${this.baseUrl}/students/${studentId}/programs/${programId}/rank-at-date`,
      { params: { asOfDate } }
    );
  }
}
