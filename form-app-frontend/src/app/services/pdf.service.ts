import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PdfService {
  constructor() { }

  // Helper method to save a PDF blob
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