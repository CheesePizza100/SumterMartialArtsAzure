import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, map } from 'rxjs';
import { Instructor, InstructorWithPrograms, InstructorProfile, InstructorStudent } from '../models/instructor.model';
import { environment } from '../../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})

export class InstructorsService {
  private baseUrl = `${environment.apiUrl}/api`;
  constructor(private http: HttpClient) { }

  getInstructors(): Observable<Instructor[]> {
    return this.http.get<Instructor[]>(`${this.baseUrl}/instructors`);  }

  getInstructorById(id: number): Observable<Instructor> {
    return this.http.get<Instructor>(`${this.baseUrl}/instructors/${id}`);
  }


  getInstructorWithPrograms(id: number): Observable<InstructorWithPrograms> {
    return forkJoin({
      instructor: this.getInstructorById(id),
      allPrograms: this.http.get<any[]>(`${this.baseUrl}/programs`)
    }).pipe(
      map(({ instructor, allPrograms }) => {
        const programs = allPrograms
          .filter(p => instructor.programIds?.includes(p.id))
          .map(p => ({ id: p.id, name: p.name }));

        return {
          ...instructor,
          programs
        };
      })
    );
  }

  getInstructorAvailability(instructorId: number): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/instructors/${instructorId}/availability`);
  }

  getMyProfile(): Observable<InstructorProfile> {
    return this.http.get<InstructorProfile>(`${this.baseUrl}/instructors/me`);
  }

  getMyStudents(): Observable<InstructorStudent[]> {
    return this.http.get<InstructorStudent[]>(`${this.baseUrl}/instructors/me/students`);
  }

  getStudentDetail(studentId: number): Observable<InstructorStudent> {
    return this.http.get<InstructorStudent>(`${this.baseUrl}/instructors/me/students/${studentId}`);
  }

  recordTestResult(studentId: number, request: {
    programId: number;
    programName: string;
    rank: string;
    result: string;
    notes: string;
    testDate: Date;
  }): Observable<any> {
    return this.http.post(`${this.baseUrl}/instructors/me/students/${studentId}/test-results`, request);
  }

  recordAttendance(studentId: number, request: {
    programId: number;
    classesAttended: number;
  }): Observable<any> {
    return this.http.post(`${this.baseUrl}/instructors/me/students/${studentId}/attendance`, request);
  }

  updateProgramNotes(studentId: number, programId: number, notes: string): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/instructors/me/students/${studentId}/programs/${programId}/notes`,
      { notes }
    );
  }
}
