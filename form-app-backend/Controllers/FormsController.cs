using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using form_app_backend.Data;
using form_app_backend.DTOs;
using form_app_backend.Models;
using form_app_backend.Services;

namespace form_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;
        private readonly ILogger<FormsController> _logger;
        private readonly IWebHostEnvironment _environment;

        public FormsController(
            ApplicationDbContext context, 
            IPdfService pdfService,
            IEmailService emailService,
            ILogger<FormsController> logger,
            IWebHostEnvironment environment)
        {
            _context = context;
            _pdfService = pdfService;
            _emailService = emailService;
            _logger = logger;
            _environment = environment;
        }

        // GET: api/Forms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentForm>>> GetStudentForms()
        {
            _logger.LogInformation("Getting all student forms");
            return await _context.StudentForms.ToListAsync();
        }

        // GET: api/Forms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentForm>> GetStudentForm(int id)
        {
            _logger.LogInformation($"Getting student form with ID: {id}");
            
            var studentForm = await _context.StudentForms.FindAsync(id);

            if (studentForm == null)
            {
                _logger.LogWarning($"Student form with ID: {id} not found");
                return NotFound();
            }

            return studentForm;
        }

        // POST: api/Forms 
        [HttpPost]
        public async Task<ActionResult> PostStudentForm([FromBody] StudentFormDto studentFormDto)
        {
            _logger.LogInformation("Creating a new student form (JSON)");
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state");
                return BadRequest(ModelState);
            }

            var studentForm = await CreateStudentFormFromDto(studentFormDto, null);
            
            _context.StudentForms.Add(studentForm);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created student form with ID: {studentForm.Id}");

            return await GeneratePdfAndSendEmail(studentForm);
        }

        // POST: api/Forms/upload )
        [HttpPost("upload")]
        public async Task<ActionResult> PostStudentFormWithFile()
        {
            _logger.LogInformation("Creating a new student form with file upload");

            try
            {
                // Extract form data from multipart request
                var studentFormDto = await ExtractFormDataFromRequest();
                
                if (studentFormDto == null)
                {
                    return BadRequest("Invalid form data");
                }

                // Handle file upload
                IFormFile? cvFile = null;
                if (Request.Form.Files.Count > 0)
                {
                    cvFile = Request.Form.Files["cv"];
                    if (cvFile != null)
                    {
                        _logger.LogInformation($"CV file received: {cvFile.FileName}, Size: {cvFile.Length}");
                        
                        // Validate file
                        if (!IsValidFile(cvFile))
                        {
                            return BadRequest("Invalid file type or size. Please upload PDF, DOC, or DOCX files under 5MB.");
                        }
                    }
                }

                var studentForm = await CreateStudentFormFromDto(studentFormDto, cvFile);
                
                _context.StudentForms.Add(studentForm);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Created student form with ID: {studentForm.Id}");

                return await GeneratePdfAndSendEmail(studentForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing form with file upload");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Forms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentForm(int id, StudentFormDto studentFormDto)
        {
            _logger.LogInformation($"Updating student form with ID: {id}");
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state");
                return BadRequest(ModelState);
            }

            var studentForm = await _context.StudentForms.FindAsync(id);
            if (studentForm == null)
            {
                _logger.LogWarning($"Student form with ID: {id} not found");
                return NotFound();
            }

            // Update properties
            UpdateStudentFormFromDto(studentForm, studentFormDto);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully updated student form with ID: {id}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!StudentFormExists(id))
                {
                    _logger.LogWarning($"Student form with ID: {id} not found during update");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"Error updating student form with ID: {id}");
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Forms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentForm(int id)
        {
            _logger.LogInformation($"Deleting student form with ID: {id}");
            
            var studentForm = await _context.StudentForms.FindAsync(id);
            if (studentForm == null)
            {
                _logger.LogWarning($"Student form with ID: {id} not found");
                return NotFound();
            }

            if (!string.IsNullOrEmpty(studentForm.CvFilePath))
            {
                var filePath = Path.Combine(_environment.WebRootPath, studentForm.CvFilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.StudentForms.Remove(studentForm);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully deleted student form with ID: {id}");
            return NoContent();
        }

        // GET: api/Forms/5/cv
        [HttpGet("{id}/cv")]
        public async Task<ActionResult> DownloadCv(int id)
    {
        _logger.LogInformation($"Downloading CV for student form ID: {id}");
    
        var studentForm = await _context.StudentForms.FindAsync(id);
    
        if (studentForm == null)
    {
        _logger.LogWarning($"Student form with ID: {id} not found");
        return NotFound("Application not found");
    }
    
    if (string.IsNullOrEmpty(studentForm.CvFilePath) || string.IsNullOrEmpty(studentForm.CvFileName))
    {
        _logger.LogWarning($"No CV file found for student form ID: {id}");
        return NotFound("CV file not found - no file was uploaded");
    }
    
    // Construct full file path
    var fullFilePath = Path.Combine(_environment.WebRootPath, studentForm.CvFilePath);
    
    _logger.LogInformation($"Looking for CV file at: {fullFilePath}");
    
    // Check if file exists on disk
    if (!System.IO.File.Exists(fullFilePath))
    {
        _logger.LogError($"CV file not found on disk: {fullFilePath}");
        return NotFound($"CV file not found on server. Expected location: {studentForm.CvFilePath}");
    }
    
    try
    {
        // Read file from disk
        var fileBytes = await System.IO.File.ReadAllBytesAsync(fullFilePath);
        
        // Determine content type based on file extension
        var contentType = GetContentType(studentForm.CvFileName);
        
        _logger.LogInformation($"Serving CV file: {studentForm.CvFileName} ({fileBytes.Length} bytes)");
        
        return File(fileBytes, contentType, studentForm.CvFileName);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"Error reading CV file: {fullFilePath}");
        return StatusCode(500, "Error reading CV file from server");
    }
}

// Helper method to determine content type
private string GetContentType(string fileName)
{
    var extension = Path.GetExtension(fileName).ToLowerInvariant();
    return extension switch
    {
        ".pdf" => "application/pdf",
        ".doc" => "application/msword",
        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        _ => "application/octet-stream"
    };
}


        // Helper Methods
        private async Task<StudentForm> CreateStudentFormFromDto(StudentFormDto dto, IFormFile? cvFile)
        {
            var studentForm = new StudentForm
            {
                // Personal Information
                Name = dto.Name ?? "",
                Surname = dto.Surname ?? "",
                Email = dto.Email,
                Phone = dto.Phone,
                BirthDate = dto.BirthDate,

                // Academic Information
                Faculty = dto.Faculty ?? "",
                Specialization = dto.Specialization,
                Year = dto.Year,
                StudentId = dto.StudentId,

                // Role Preferences
                PreferredRole = dto.PreferredRole,
                AlternativeRole = dto.AlternativeRole,

                // Technical Skills
                ProgrammingLanguages = dto.ProgrammingLanguages,
                Frameworks = dto.Frameworks,
                Tools = dto.Tools,

                // Experience and Motivation
                Experience = dto.Experience,
                Motivation = dto.Motivation ?? "",
                Contribution = dto.Contribution,

                // Availability
                TimeCommitment = dto.TimeCommitment,
                Schedule = dto.Schedule,

                // Documents
                Portfolio = dto.Portfolio,

                // System fields
                SubmissionDate = DateTime.UtcNow
            };

            // Handle CV file upload
            if (cvFile != null)
            {
                var uploadResult = await SaveCvFile(cvFile);
                studentForm.CvFileName = uploadResult.FileName;
                studentForm.CvFilePath = uploadResult.FilePath;
            }

            return studentForm;
        }

        private void UpdateStudentFormFromDto(StudentForm studentForm, StudentFormDto dto)
        {
            studentForm.Name = dto.Name ?? studentForm.Name;
            studentForm.Surname = dto.Surname ?? studentForm.Surname;
            studentForm.Email = dto.Email;
            studentForm.Phone = dto.Phone;
            studentForm.BirthDate = dto.BirthDate;
            studentForm.Faculty = dto.Faculty ?? studentForm.Faculty;
            studentForm.Specialization = dto.Specialization;
            studentForm.Year = dto.Year;
            studentForm.StudentId = dto.StudentId;
            studentForm.PreferredRole = dto.PreferredRole;
            studentForm.AlternativeRole = dto.AlternativeRole;
            studentForm.ProgrammingLanguages = dto.ProgrammingLanguages;
            studentForm.Frameworks = dto.Frameworks;
            studentForm.Tools = dto.Tools;
            studentForm.Experience = dto.Experience;
            studentForm.Motivation = dto.Motivation ?? studentForm.Motivation;
            studentForm.Contribution = dto.Contribution;
            studentForm.TimeCommitment = dto.TimeCommitment;
            studentForm.Schedule = dto.Schedule;
            studentForm.Portfolio = dto.Portfolio;
        }

        private async Task<ActionResult> GeneratePdfAndSendEmail(StudentForm studentForm)
        {
            // Generate PDF
            var pdfBytes = _pdfService.GeneratePdf(studentForm);

            // Send admin notification email with PDF attachment
            try
            {
                await _emailService.SendNotificationEmailAsync(studentForm, pdfBytes);
                _logger.LogInformation($"Admin notification email sent for form ID: {studentForm.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send admin notification email for form ID: {studentForm.Id}");
                // Continue execution - don't fail the request if email fails
            }

            // Return the PDF file
            return File(pdfBytes, "application/pdf", $"StudentForm_{studentForm.Id}.pdf");
        }

        private async Task<StudentFormDto?> ExtractFormDataFromRequest()
        {
            try
            {
                var form = Request.Form;
                
                // Parse birth date
                DateTime? birthDate = null;
                if (DateTime.TryParse(form["birthDate"], out var parsedDate))
                {
                    birthDate = parsedDate;
                }

                // Parse boolean values
                bool.TryParse(form["dataProcessingAgreement"], out var dataProcessing);
                bool.TryParse(form["termsAgreement"], out var terms);
                bool.TryParse(form["newsletterSubscription"], out var newsletter);

                return new StudentFormDto
                {
                    Name = form["name"].ToString(),
                    Surname = form["surname"].ToString(),
                    Email = form["email"].ToString(),
                    Phone = form["phone"].ToString(),
                    BirthDate = birthDate,
                    Faculty = form["faculty"].ToString(),
                    Specialization = form["specialization"].ToString(),
                    Year = form["year"].ToString(),
                    StudentId = form["studentId"].ToString(),
                    PreferredRole = form["preferredRole"].ToString(),
                    AlternativeRole = form["alternativeRole"].ToString(),
                    ProgrammingLanguages = form["programmingLanguages"].ToString(),
                    Frameworks = form["frameworks"].ToString(),
                    Tools = form["tools"].ToString(),
                    Experience = form["experience"].ToString(),
                    Motivation = form["motivation"].ToString(),
                    Contribution = form["contribution"].ToString(),
                    TimeCommitment = form["timeCommitment"].ToString(),
                    Schedule = form["schedule"].ToString(),
                    Portfolio = form["portfolio"].ToString(),
                    DataProcessingAgreement = dataProcessing,
                    TermsAgreement = terms,
                    NewsletterSubscription = newsletter
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting form data from request");
                return null;
            }
        }

        private bool IsValidFile(IFormFile file)
        {
            // Check file size (5MB limit)
            if (file.Length > 5 * 1024 * 1024)
                return false;

            // Check file extension
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            return allowedExtensions.Contains(fileExtension);
        }

        private async Task<(string FileName, string FilePath)> SaveCvFile(IFormFile file)
        {
            // Create uploads directory if it doesn't exist
            var uploadsDir = Path.Combine(_environment.WebRootPath, "uploads", "cvs");
            Directory.CreateDirectory(uploadsDir);

            // Generate unique filename
            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsDir, uniqueFileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for database storage
            var relativePath = Path.Combine("uploads", "cvs", uniqueFileName);
            return (file.FileName, relativePath);
        }

        private bool StudentFormExists(int id)
        {
            return _context.StudentForms.Any(e => e.Id == id);
        }
    }
}