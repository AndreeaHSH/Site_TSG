using System;
using System.IO;
using form_app_backend.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace form_app_backend.Services
{
    public interface IPdfService
    {
        byte[] GeneratePdf(StudentForm studentForm);
    }

    public class PdfService : IPdfService
    {
        public byte[] GeneratePdf(StudentForm studentForm)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Add title
                document.Add(new Paragraph("TSG Volunteer Application Form")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetMarginBottom(20));

                // Personal Information Section
                document.Add(new Paragraph("Personal Information")
                    .SetFontSize(16)
                    .SetMarginTop(20)
                    .SetMarginBottom(10));

                document.Add(new Paragraph($"Name: {studentForm.Name} {studentForm.Surname}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Email: {studentForm.Email}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Phone: {studentForm.Phone}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Birth Date: {studentForm.BirthDate.ToString("yyyy-MM-dd")}")
                    .SetFontSize(12));

                // Academic Information Section
                document.Add(new Paragraph("Academic Information")
                    .SetFontSize(16)
                    .SetMarginTop(20)
                    .SetMarginBottom(10));

                document.Add(new Paragraph($"Faculty: {studentForm.Faculty}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Specialization: {studentForm.Specialization}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Year: {studentForm.Year}")
                    .SetFontSize(12));
                
                if (!string.IsNullOrEmpty(studentForm.StudentId))
                {
                    document.Add(new Paragraph($"Student ID: {studentForm.StudentId}")
                        .SetFontSize(12));
                }

                // Role Preferences Section
                document.Add(new Paragraph("Role Preferences")
                    .SetFontSize(16)
                    .SetMarginTop(20)
                    .SetMarginBottom(10));

                document.Add(new Paragraph($"Preferred Role: {studentForm.PreferredRole}")
                    .SetFontSize(12));
                
                if (!string.IsNullOrEmpty(studentForm.AlternativeRole))
                {
                    document.Add(new Paragraph($"Alternative Role: {studentForm.AlternativeRole}")
                        .SetFontSize(12));
                }

                // Technical Skills Section
                if (!string.IsNullOrEmpty(studentForm.ProgrammingLanguages) || 
                    !string.IsNullOrEmpty(studentForm.Frameworks) || 
                    !string.IsNullOrEmpty(studentForm.Tools))
                {
                    document.Add(new Paragraph("Technical Skills")
                        .SetFontSize(16)
                        .SetMarginTop(20)
                        .SetMarginBottom(10));

                    if (!string.IsNullOrEmpty(studentForm.ProgrammingLanguages))
                    {
                        document.Add(new Paragraph($"Programming Languages: {studentForm.ProgrammingLanguages}")
                            .SetFontSize(12));
                    }

                    if (!string.IsNullOrEmpty(studentForm.Frameworks))
                    {
                        document.Add(new Paragraph($"Frameworks: {studentForm.Frameworks}")
                            .SetFontSize(12));
                    }

                    if (!string.IsNullOrEmpty(studentForm.Tools))
                    {
                        document.Add(new Paragraph($"Tools: {studentForm.Tools}")
                            .SetFontSize(12));
                    }
                }

                // Experience and Motivation Section
                document.Add(new Paragraph("Experience and Motivation")
                    .SetFontSize(16)
                    .SetMarginTop(20)
                    .SetMarginBottom(10));

                if (!string.IsNullOrEmpty(studentForm.Experience))
                {
                    document.Add(new Paragraph("Previous Experience:")
                        .SetFontSize(14)
                        .SetMarginTop(10));
                    document.Add(new Paragraph(studentForm.Experience)
                        .SetFontSize(12)
                        .SetMarginBottom(10));
                }

                document.Add(new Paragraph("Motivation:")
                    .SetFontSize(14)
                    .SetMarginTop(10));
                document.Add(new Paragraph(studentForm.Motivation)
                    .SetFontSize(12)
                    .SetMarginBottom(10));

                document.Add(new Paragraph("How you can contribute:")
                    .SetFontSize(14)
                    .SetMarginTop(10));
                document.Add(new Paragraph(studentForm.Contribution)
                    .SetFontSize(12)
                    .SetMarginBottom(10));

                // Availability Section
                document.Add(new Paragraph("Availability")
                    .SetFontSize(16)
                    .SetMarginTop(20)
                    .SetMarginBottom(10));

                document.Add(new Paragraph($"Time Commitment: {studentForm.TimeCommitment}")
                    .SetFontSize(12));

                if (!string.IsNullOrEmpty(studentForm.Schedule))
                {
                    document.Add(new Paragraph($"Schedule: {studentForm.Schedule}")
                        .SetFontSize(12));
                }

                // Portfolio Section
                if (!string.IsNullOrEmpty(studentForm.Portfolio))
                {
                    document.Add(new Paragraph("Portfolio")
                        .SetFontSize(16)
                        .SetMarginTop(20)
                        .SetMarginBottom(10));

                    document.Add(new Paragraph($"Portfolio Links: {studentForm.Portfolio}")
                        .SetFontSize(12));
                }

                // Submission Information
                document.Add(new Paragraph($"Submission Date: {studentForm.SubmissionDate.ToString("yyyy-MM-dd HH:mm:ss")}")
                    .SetFontSize(12)
                    .SetMarginTop(30));

                // Electronic signature
                document.Add(new Paragraph("Electronically Signed")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(12)
                    .SetMarginTop(40));

                document.Close();
                return memoryStream.ToArray();
            }
        }
    }
}