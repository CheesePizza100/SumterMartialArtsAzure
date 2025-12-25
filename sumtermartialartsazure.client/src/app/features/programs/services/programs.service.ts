import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Program } from '../models/program.model';

@Injectable({
  providedIn: 'root'
})
export class ProgramsService {
  // adjust to your .NET backend URL
  //http://localhost:5208/api/Programs
  //https://localhost:5208/api/programs
  //private baseUrl = 'http://localhost:5208/api';
  private baseUrl = '/api';

  constructor(private http: HttpClient) { }

  getPrograms(): Observable<Program[]> {
    return this.http.get<Program[]>(`${this.baseUrl}/Programs`);
  }

  getProgramById(id: number) {
    return this.http.get<Program>(`${this.baseUrl}/programs/${id}`);
  }
}
