import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StudentForm } from '../models/student-form';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FormService {
  private apiUrl = 'http://localhost:5000/api/forms';

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

  // Download CV for a specific form
  downloadCv(formId: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${formId}/cv`, {
      responseType: 'blob',
      observe: 'response'
    }).pipe(
      map(response => {
        console.log('CV download response:', response.status);
        return response.body || new Blob();
      }),
      catchError(error => {
        console.error('CV download error:', error);
        throw error;
      })
    );
  }
}
