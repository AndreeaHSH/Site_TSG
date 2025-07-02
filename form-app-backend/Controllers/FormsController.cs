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
        private readonly ILogger<FormsController> _logger;

        public FormsController(
            ApplicationDbContext context, 
            IPdfService pdfService,
            ILogger<FormsController> logger)
        {
            _context = context;
            _pdfService = pdfService;
            _logger = logger;
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
            _logger.LogInformation("Creating a new student form");
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state");
                return BadRequest(ModelState);
            }

            var studentForm = new StudentForm
            {
                // Personal Information
                Name = studentFormDto.Name,
                Surname = studentFormDto.Surname,
                Email = studentFormDto.Email ?? "not-provided@example.com",
                Phone = studentFormDto.Phone ?? "Not provided",
                BirthDate = studentFormDto.BirthDate ?? DateTime.Now.AddYears(-20),
                
                // Academic Information
                Faculty = studentFormDto.Faculty,
                Specialization = studentFormDto.Specialization ?? "Not specified",
                Year = studentFormDto.Year ?? "Not specified",
                StudentId = studentFormDto.StudentId,
                
                // Role Preferences
                PreferredRole = studentFormDto.PreferredRole ?? "Not specified",
                AlternativeRole = studentFormDto.AlternativeRole,
                
                // Technical Skills
                ProgrammingLanguages = studentFormDto.ProgrammingLanguages,
                Frameworks = studentFormDto.Frameworks,
                Tools = studentFormDto.Tools,
                
                // Experience and Motivation
                Experience = studentFormDto.Experience,
                Motivation = studentFormDto.Motivation,
                Contribution = studentFormDto.Contribution ?? "Not specified",
                
                // Availability
                TimeCommitment = studentFormDto.TimeCommitment ?? "Not specified",
                Schedule = studentFormDto.Schedule,
                
                // Documents
                Portfolio = studentFormDto.Portfolio,
                
                // System fields
                SubmissionDate = DateTime.UtcNow
            };

            _context.StudentForms.Add(studentForm);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created student form with ID: {studentForm.Id}");

            // Generate PDF
            var pdfBytes = _pdfService.GeneratePdf(studentForm);

            // Return the PDF file
            return File(pdfBytes, "application/pdf", $"StudentForm_{studentForm.Id}.pdf");
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

            // Update properties (only update if provided)
            studentForm.Name = studentFormDto.Name;
            studentForm.Surname = studentFormDto.Surname;
            studentForm.Faculty = studentFormDto.Faculty;
            studentForm.Motivation = studentFormDto.Motivation;
            
            // Update optional fields only if provided
            if (!string.IsNullOrEmpty(studentFormDto.Email))
                studentForm.Email = studentFormDto.Email;
            if (!string.IsNullOrEmpty(studentFormDto.Phone))
                studentForm.Phone = studentFormDto.Phone;
            if (studentFormDto.BirthDate.HasValue)
                studentForm.BirthDate = studentFormDto.BirthDate.Value;
            if (!string.IsNullOrEmpty(studentFormDto.Specialization))
                studentForm.Specialization = studentFormDto.Specialization;
            if (!string.IsNullOrEmpty(studentFormDto.Year))
                studentForm.Year = studentFormDto.Year;
            if (!string.IsNullOrEmpty(studentFormDto.PreferredRole))
                studentForm.PreferredRole = studentFormDto.PreferredRole;
            if (!string.IsNullOrEmpty(studentFormDto.Contribution))
                studentForm.Contribution = studentFormDto.Contribution;
            if (!string.IsNullOrEmpty(studentFormDto.TimeCommitment))
                studentForm.TimeCommitment = studentFormDto.TimeCommitment;
                
            studentForm.StudentId = studentFormDto.StudentId;
            studentForm.AlternativeRole = studentFormDto.AlternativeRole;
            studentForm.ProgrammingLanguages = studentFormDto.ProgrammingLanguages;
            studentForm.Frameworks = studentFormDto.Frameworks;
            studentForm.Tools = studentFormDto.Tools;
            studentForm.Experience = studentFormDto.Experience;
            studentForm.Schedule = studentFormDto.Schedule;
            studentForm.Portfolio = studentFormDto.Portfolio;

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

            _context.StudentForms.Remove(studentForm);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully deleted student form with ID: {id}");
            return NoContent();
        }

        private bool StudentFormExists(int id)
        {
            return _context.StudentForms.Any(e => e.Id == id);
        }
    }
}