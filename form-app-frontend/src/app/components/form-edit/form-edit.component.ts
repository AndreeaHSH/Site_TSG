import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormService } from '../../services/form.service';
import { StudentForm } from '../../models/student-form';

@Component({
  selector: 'app-form-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './form-edit.component.html',
  styleUrls: ['./form-edit.component.scss']
})
export class FormEditComponent implements OnInit {
  studentForm: FormGroup;
  formId: number = 0;
  submitted = false;
  loading = false;
  submitting = false;
  successMessage = '';
  errorMessage = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private formService: FormService
  ) { 
    // Initialize the form in the constructor
    this.studentForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      surname: ['', [Validators.required]],
      faculty: ['', [Validators.required]],
      motivation: ['', [Validators.required, Validators.minLength(100)]]
    });
  }

  ngOnInit(): void {
    this.loadForm();
  }

  // Getter for easy access to form fields with proper typing
  get f(): { [key: string]: FormControl } {
    return this.studentForm.controls as { [key: string]: FormControl };
  }

  loadForm(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.formId = +id;
      this.loading = true;
      this.errorMessage = '';
      
      this.formService.getForm(this.formId)
        .subscribe({
          next: (data: StudentForm) => {
            this.studentForm.patchValue({
              name: data.name,
              surname: data.surname,
              faculty: data.faculty,
              motivation: data.motivation
            });
            this.loading = false;
          },
          error: (error) => {
            this.errorMessage = 'Error loading form data. Please try again.';
            this.loading = false;
            console.error('Error loading form data:', error);
          }
        });
    }
  }

  onSubmit(): void {
    this.submitted = true;
    this.successMessage = '';
    this.errorMessage = '';

    // Stop if form is invalid
    if (this.studentForm.invalid) {
      return;
    }

    this.submitting = true;

    const formData: StudentForm = {
      name: this.f['name'].value,
      surname: this.f['surname'].value,
      faculty: this.f['faculty'].value,
      motivation: this.f['motivation'].value
    };

    this.formService.updateForm(this.formId, formData)
      .subscribe({
        next: () => {
          this.submitting = false;
          this.successMessage = 'Form updated successfully!';
          
          // Navigate back to the list after a brief delay
          setTimeout(() => {
            this.router.navigate(['/forms']);
          }, 1500);
        },
        error: (error) => {
          this.submitting = false;
          this.errorMessage = 'Error updating the form. Please try again.';
          console.error('Form update error:', error);
        }
      });
  }
}