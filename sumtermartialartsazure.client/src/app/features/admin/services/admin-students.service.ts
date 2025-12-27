import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Student } from '../models/student.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminStudentsService {
  private apiUrl = `${environment.apiUrl}/admin/students`;

  constructor(private http: HttpClient) { }

  /**
   * Get all students
   */
  getAllStudents(): Observable<Student[]> {
    return this.http.get<Student[]>(this.apiUrl);
  }

  /**
   * Get a single student by ID
   */
  getStudentById(id: number): Observable<Student> {
    return this.http.get<Student>(`${this.apiUrl}/${id}`);
  }

  /**
   * Search students by name, email, or program
   */
  searchStudents(searchTerm: string): Observable<Student[]> {
    return this.http.get<Student[]>(`${this.apiUrl}/search`, {
      params: { q: searchTerm }
    });
  }

  /**
   * Update student information
   */
  updateStudent(id: number, student: Partial<Student>): Observable<Student> {
    return this.http.put<Student>(`${this.apiUrl}/${id}`, student);
  }

  /**
   * Add test result for a student
   */
  addTestResult(studentId: number, testData: {
    programName: string;
    rank: string;
    result: 'Pass' | 'Fail';
    notes: string;
    testDate: string;
  }): Observable<any> {
    return this.http.post(`${this.apiUrl}/${studentId}/test-results`, testData);
  }

  /**
   * Update program notes for a student
   */
  updateProgramNotes(studentId: number, programName: string, notes: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${studentId}/programs/${programName}/notes`, { notes });
  }

  /**
   * Get attendance details for a student
   */
  getAttendanceDetails(studentId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${studentId}/attendance`);
  }
}
