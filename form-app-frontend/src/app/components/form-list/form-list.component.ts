import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormService } from '../../services/form.service';
import { StudentForm } from '../../models/student-form';

@Component({
  selector: 'app-form-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './form-list.component.html',
  styleUrls: ['./form-list.component.scss']
})
export class FormListComponent implements OnInit {
  forms: StudentForm[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';

  constructor(private formService: FormService) { }

  ngOnInit(): void {
    this.loadForms();
  }

  loadForms(): void {
    this.loading = true;
    this.errorMessage = '';
    
    this.formService.getForms()
      .subscribe({
        next: (data: StudentForm[]) => {
          this.forms = data;
          this.loading = false;
        },
        error: (error) => {
          this.errorMessage = 'Error loading forms. Please try again.';
          this.loading = false;
          console.error('Error loading forms:', error);
        }
      });
  }

  deleteForm(id: number): void {
    if (confirm('Are you sure you want to delete this form?')) {
      this.loading = true;
      this.errorMessage = '';
      this.successMessage = '';
      
      this.formService.deleteForm(id)
        .subscribe({
          next: () => {
            this.successMessage = 'Form deleted successfully!';
            this.forms = this.forms.filter(form => form.id !== id);
            this.loading = false;
          },
          error: (error) => {
            this.errorMessage = 'Error deleting form. Please try again.';
            this.loading = false;
            console.error('Error deleting form:', error);
          }
        });
    }
  }
}