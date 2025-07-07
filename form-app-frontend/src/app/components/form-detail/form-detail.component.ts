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

  // Download CV functionality
  downloadCv(formId: number, fileName: string): void {
    console.log(`Attempting to download CV for form ID: ${formId}, filename: ${fileName}`);

    this.formService.downloadCv(formId).subscribe({
      next: (blob: Blob) => {
        if (blob.size === 0) {
          console.error('Received empty blob');
          alert('CV file is empty or could not be downloaded.');
          return;
        }

        // Create blob URL and trigger download
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName || `CV_${formId}.pdf`;

        // Append to document, click, and remove
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);

        // Clean up
        window.URL.revokeObjectURL(url);

        console.log(`CV downloaded successfully: ${fileName}`);
      },
      error: (error) => {
        console.error('Error downloading CV:', error);

        let errorMessage = 'Error downloading CV. Please try again.';

        if (error.status === 404) {
          errorMessage = 'CV file not found. The file may have been deleted or moved.';
        } else if (error.status === 500) {
          errorMessage = 'Server error while downloading CV. Please contact support.';
        }

        alert(errorMessage);
      }
    });
  }

  // Helper method to check if CV exists
  hasCv(form: any): boolean {
    return form && form.cvFileName && form.cvFileName.trim().length > 0;
  }

  // Helper method to get file icon based on extension (Font Awesome icons)
  getCvIcon(fileName: string): string {
    if (!fileName) return 'fas fa-file';

    const extension = fileName.split('.').pop()?.toLowerCase();
    switch (extension) {
      case 'pdf': return 'fas fa-file-pdf';
      case 'doc':
      case 'docx': return 'fas fa-file-word';
      default: return 'fas fa-file';
    }
  }
}
