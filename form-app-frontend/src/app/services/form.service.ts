import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StudentForm } from '../models/student-form';

@Injectable({
  providedIn: 'root'
})
export class FormService {
  private apiUrl = 'http://localhost:5000/api/forms'; // For local development
  // In production with Docker, this would use relative URL '/api/forms'

  constructor(private http: HttpClient) { }

  // Get all forms
  getForms(): Observable<StudentForm[]> {
    return this.http.get<StudentForm[]>(this.apiUrl);
  }

  // Get a specific form by ID
  getForm(id: number): Observable<StudentForm> {
    return this.http.get<StudentForm>(`${this.apiUrl}/${id}`);
  }

  // Submit a new form
  submitForm(form: StudentForm): Observable<Blob> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/pdf'
    });
    
    return this.http.post(this.apiUrl, form, {
      headers: headers,
      responseType: 'blob'
    });
  }

  // Update an existing form
  updateForm(id: number, form: StudentForm): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, form);
  }

  // Delete a form
  deleteForm(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}