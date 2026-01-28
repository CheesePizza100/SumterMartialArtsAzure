import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';

export interface StudentProfile {
  id: number;
  name: string;
  email: string;
  phone: string;
  programs: ProgramEnrollment[];
  testHistory: TestHistory[];
}

export interface ProgramEnrollment {
  programId: number;
  name: string;
  rank: string;
  enrolledDate: string;
  lastTest?: string;
  testNotes?: string;
  attendance: Attendance;
}

export interface Attendance {
  last30Days: number;
  total: number;
  attendanceRate: number;
}

export interface TestHistory {
  date: string;
  program: string;
  rank: string;
  result: string;
  notes?: string;
}
export interface UpdateContactInfoRequest {
  name?: string;
  email?: string;
  phone?: string;
}

export interface PrivateLessonRequest {
  id: number;
  instructorId: number;
  instructorName: string;
  requestedStart: string;
  requestedEnd: string;
  status: string;
  notes?: string;
  rejectionReason?: string;
  createdAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private baseUrl = `${environment.apiUrl}/api`;

  constructor(private http: HttpClient) { }

  getMyProfile(): Observable<StudentProfile> {
    return this.http.get<StudentProfile>(`${this.baseUrl}/students/me`);
  }

  updateMyContactInfo(request: UpdateContactInfoRequest): Observable<any> {
    return this.http.put(`${this.baseUrl}/students/me`, request);
  }

  getMyPrivateLessonRequests(): Observable<PrivateLessonRequest[]> {
    return this.http.get<PrivateLessonRequest[]>(`${this.baseUrl}/students/me/private-lessons`);
  }
}
