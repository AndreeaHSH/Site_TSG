import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormService } from '../../services/form.service';
import { StudentForm } from '../../models/student-form';

@Component({
  selector: 'app-form-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './form-detail.component.html',
  styleUrls: ['./form-detail.component.scss']
})
export class FormDetailComponent implements OnInit {
  form: StudentForm | null = null;
  loading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private formService: FormService
  ) { }

  ngOnInit(): void {
    this.loadForm();
  }

  loadForm(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loading = true;
      this.errorMessage = '';
      
      this.formService.getForm(+id)
        .subscribe({
          next: (data: StudentForm) => {
            this.form = data;
            this.loading = false;
          },
          error: (error) => {
            this.errorMessage = 'Error loading form details. Please try again.';
            this.loading = false;
            console.error('Error loading form details:', error);
          }
        });
    }
  }
}