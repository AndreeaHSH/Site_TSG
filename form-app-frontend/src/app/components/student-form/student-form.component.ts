import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormService } from '../../services/form.service';
import { StudentForm } from '../../models/student-form';

@Component({
  selector: 'app-student-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './student-form.component.html',
  styleUrls: ['./student-form.component.scss']
})
export class StudentFormComponent implements OnInit {
  studentForm: FormGroup;
  submitted = false;
  loading = false;
  successMessage = '';
  errorMessage = '';

  constructor(
    private formBuilder: FormBuilder,
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
    // Form is already initialized in constructor
  }

  // Getter for easy access to form fields with proper typing
  get f(): { [key: string]: FormControl } {
    return this.studentForm.controls as { [key: string]: FormControl };
  }

  onSubmit(): void {
    this.submitted = true;
    this.successMessage = '';
    this.errorMessage = '';

    // Stop if form is invalid
    if (this.studentForm.invalid) {
      return;
    }

    this.loading = true;

    const formData: StudentForm = {
      name: this.f['name'].value,
      surname: this.f['surname'].value,
      faculty: this.f['faculty'].value,
      motivation: this.f['motivation'].value
    };

    this.formService.submitForm(formData)
      .subscribe({
        next: (response: Blob) => {
          this.loading = false;
          this.successMessage = 'Form submitted successfully!';
          
          // Save the PDF
          this.savePdf(response, `StudentForm_${formData.name}_${formData.surname}.pdf`);
          
          // Reset the form
          this.submitted = false;
          this.studentForm.reset();
        },
        error: (error) => {
          this.loading = false;
          this.errorMessage = 'Error submitting the form. Please try again.';
          console.error('Form submission error:', error);
        }
      });
  }

  savePdf(blob: Blob, fileName: string): void {
    // Create a URL for the blob
    const url = window.URL.createObjectURL(blob);
    
    // Create an anchor element and set properties
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    
    // Append to the document, click and remove
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    
    // Release the URL object
    window.URL.revokeObjectURL(url);
  }
}