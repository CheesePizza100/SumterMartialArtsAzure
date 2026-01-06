import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Student, Attendance } from '../models/student.model';
import { environment } from '../../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AdminStudentsService {
  private baseUrl = `${environment.apiUrl}/api`;

  constructor(private http: HttpClient) { }

  getAllStudents(): Observable<Student[]> {
    return this.http.get<Student[]>(`${this.baseUrl}/students`);
  }

  getStudentById(id: number): Observable<Student> {
    return this.http.get<Student>(`${this.baseUrl}/students/${id}`);
  }

  searchStudents(searchTerm: string): Observable<Student[]> {
    return this.http.get<Student[]>(`${this.baseUrl}/students/search`, {
      params: { q: searchTerm }
    });
  }

  updateStudent(id: number, student: {
    name?: string;
    email?: string;
    phone?: string;
  }): Observable<Student> {
    return this.http.put<Student>(`${this.baseUrl}/students/${id}`, student);
  }

  addTestResult(studentId: number, testData: {
    programId: number;
    programName: string;
    rank: string;
    result: string;
    notes: string;
    testDate: string;
  }): Observable<{ success: boolean; message: string }> {
    return this.http.post<{ success: boolean; message: string }>(
      `${this.baseUrl}/students/${studentId}/test-results`,
      testData
    );
  }

  updateProgramNotes(
    studentId: number,
    programId: number,
    notes: string
  ): Observable<{ success: boolean; message: string }> {
    return this.http.patch<{ success: boolean; message: string }>(
      `${this.baseUrl}/students/${studentId}/programs/${programId}/notes`,
      { notes }
    );
  }

  getAttendanceDetails(studentId: number): Observable<Attendance> {
    return this.http.get<Attendance>(`${this.baseUrl}/students/${studentId}/attendance`);
  }

  createStudent(student: {
    name: string;
    email: string;
    phone: string;
  }): Observable<Student> {
    return this.http.post<Student>(`${this.baseUrl}/students`, student);
  }

  createStudentLogin(studentId: number, request: {
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
    }>(`${this.baseUrl}/students/${studentId}/create-login`, request);
  }

  enrollInProgram(studentId: number, enrollment: {
    programId: number;
    programName: string;
    initialRank: string;
  }): Observable<{ success: boolean; message: string }> {
    return this.http.post<{ success: boolean; message: string }>(
      `${this.baseUrl}/students/${studentId}/enroll`,
      enrollment
    );
  }

  recordAttendance(
    studentId: number,
    programId: number,
    classesAttended: number
  ): Observable<{ success: boolean; message: string }> {
    return this.http.post<{ success: boolean; message: string }>(
      `${this.baseUrl}/students/${studentId}/attendance`,
      { programId, classesAttended }
    );
  }

  deactivateStudent(studentId: number): Observable<{ success: boolean; message: string }> {
    return this.http.delete<{ success: boolean; message: string }>(
      `${this.baseUrl}/students/${studentId}`
    );
  }
}
