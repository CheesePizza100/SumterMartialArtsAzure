import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Program } from '../models/program.model';
import { environment } from '../../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ProgramsService {
  private baseUrl = `${environment.apiUrl}/api`;

  constructor(private http: HttpClient) { }

  getPrograms(): Observable<Program[]> {
    return this.http.get<Program[]>(`${this.baseUrl}/programs`);
  }

  getProgramById(id: number) {
    return this.http.get<Program>(`${this.baseUrl}/programs/${id}`);
  }
}
