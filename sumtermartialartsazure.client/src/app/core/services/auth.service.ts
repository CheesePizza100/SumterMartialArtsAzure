import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, map } from 'rxjs';
import { Router } from '@angular/router';

export interface User {
  userId: string;
  username: string;
  role: 'Admin' | 'Student' | 'Instructor';
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  username: string;
  userId: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly USER_KEY = 'current_user';

  private currentUserSubject = new BehaviorSubject<User | null>(this.getUserFromStorage());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  private getUserFromStorage(): User | null {
    const userJson = localStorage.getItem(this.USER_KEY);
    return userJson ? JSON.parse(userJson) : null;
  }

  isLoggedIn(): boolean {
    return this.currentUserSubject.value !== null && this.getToken() !== null;
  }

  isAdmin(): boolean {
    return this.currentUserSubject.value?.role === 'Admin';
  }

  isStudent(): boolean {
    return this.currentUserSubject.value?.role === 'Student';
  }

  isInstructor(): boolean {
    return this.currentUserSubject.value?.role === 'Instructor';
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  login(username: string, password: string): Observable<User> {
    const request: LoginRequest = { username, password };

    return this.http.post<LoginResponse>('/api/auth/login', request)
      .pipe(
        tap(response => {
          // Store token
          localStorage.setItem(this.TOKEN_KEY, response.token);

          // Store user info
          const user: User = {
            userId: response.userId,
            username: response.username,
            role: response.role as 'Admin' | 'Student' | 'Instructor'
          };
          localStorage.setItem(this.USER_KEY, JSON.stringify(user));

          // Update subject
          this.currentUserSubject.next(user);
        }),
        map(response => ({
          userId: response.userId,
          username: response.username,
          role: response.role as 'Admin' | 'Student' | 'Instructor'
        }))
      );
  }

  logout(): Observable<void> {
    return this.http.post<void>('/api/auth/logout', {})
      .pipe(
        tap(() => {
          this.clearAuthData();
        })
      );
  }

  private clearAuthData(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  // Force logout (no API call - for when token is invalid)
  forceLogout(): void {
    this.clearAuthData();
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const expiry = payload.exp * 1000; // Convert to milliseconds
      return Date.now() > expiry;
    } catch {
      return true;
    }
  }
}
