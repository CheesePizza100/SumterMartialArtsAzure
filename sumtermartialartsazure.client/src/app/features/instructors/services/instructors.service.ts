import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, map } from 'rxjs';
import { Instructor, InstructorWithPrograms, ProgramSummary } from '../models/instructor.model';

@Injectable({
  providedIn: 'root'
})

export class InstructorsService {
  private baseUrl = '/api';

  constructor(private http: HttpClient) { }

  //getInstructors(): Observable<Instructor[]> {
  //  return this.http.get<{ instructors: Instructor[] }>(`${this.baseUrl}/instructors`).pipe(
  //    map(response => response.instructors) // Extract the array from the wrapper
  //  );
  //}
  //getInstructors(): Observable<Instructor[]> {
  //  return this.http.get<Instructor[]>(`${this.baseUrl}/instructors`);
  //}
  getInstructors(): Observable<Instructor[]> {
    return this.http.get<{ instructor: Instructor }[]>(`${this.baseUrl}/instructors`).pipe(
      map(response => response.map(item => item.instructor)) // Unwrap each 'instructor' object
    );
  }

  //getInstructorById(id: number): Observable<Instructor> {
  //  return this.http.get<Instructor>(`${this.baseUrl}/Instructors/${id}`);
  //}
  getInstructorById(id: number): Observable<Instructor> {
    return this.http.get<{ instructor: Instructor }>(`${this.baseUrl}/instructors/${id}`).pipe(
      map(response => response.instructor) // Extract from wrapper
    );
  }


  getInstructorWithPrograms(id: number): Observable<InstructorWithPrograms> {
    return forkJoin({
      instructor: this.getInstructorById(id),
      allPrograms: this.http.get<any[]>(`${this.baseUrl}/Programs`)
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
}
