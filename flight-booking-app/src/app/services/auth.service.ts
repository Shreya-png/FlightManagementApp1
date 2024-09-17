import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { isPlatformBrowser } from '@angular/common';
//import { jwtDecode } from 'jwt-decode';
//jwtDecode

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5001/api/Users';
  private jwtHelper = new JwtHelperService();
  private platformId: Object;
  private tokenKey = 'token';

  constructor(private http: HttpClient, @Inject(PLATFORM_ID) platformId: Object) {
    this.platformId = platformId;
  }

  register(registrationData: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, registrationData);
  }

  login(loginData: { username: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, loginData);
  }

  storeToken(token: string): void {
      localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    //if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(this.tokenKey);
    //}
    //console.log('@14');
    //return null;
    //return localStorage.getItem('token');
  }

  getUserIdFromToken(): string | null {
    const token = this.getToken();
    if (token) {
      try {
        const decodedToken = this.jwtHelper.decodeToken(token) as { nameid?: string };
        //const decodedToken: any = jwtDecode('token');
        console.log('Decoded Token:', decodedToken);
       // return decodedToken.UserId || null;
        return decodedToken.nameid || null;
      } catch (error) {
        console.error('Error decoding token:', error);
       // return null;
      }
    }
    return null;
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return token != null && !this.jwtHelper.isTokenExpired(token);
  }

  logout() {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this.tokenKey);
    }
  }
  
}