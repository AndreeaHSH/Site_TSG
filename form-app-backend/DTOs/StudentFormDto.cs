using System.ComponentModel.DataAnnotations;

namespace form_app_backend.DTOs
{
    public class StudentFormDto
    {
        // Personal Information
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        [Required]
        [StringLength(100)]
        public string? Surname { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }
        
        [Required]
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [Required]
        public DateTime? BirthDate { get; set; }
        
        // Academic Information
        [Required]
        [StringLength(200)]
        public string? Faculty { get; set; }
        
        [Required]
        [StringLength(200)]
        public string? Specialization { get; set; }
        
        [Required]
        [StringLength(20)]
        public string? Year { get; set; }
        
        [StringLength(50)]
        public string? StudentId { get; set; }
        
        // Role Preferences
        [Required]
        [StringLength(100)]
        public string? PreferredRole { get; set; }
        
        [StringLength(100)]
        public string? AlternativeRole { get; set; }
        
        // Technical Skills
        public string? ProgrammingLanguages { get; set; }
        public string? Frameworks { get; set; }
        public string? Tools { get; set; }
        
        // Experience and Motivation
        public string? Experience { get; set; }
        
        [Required]
        [MinLength(100, ErrorMessage = "Motivation must be at least 100 characters long.")]
        public string? Motivation { get; set; }
        
        [Required]
        public string? Contribution { get; set; }
        
        // Availability
        [Required]
        [StringLength(50)]
        public string? TimeCommitment { get; set; }
        
        public string? Schedule { get; set; }
        
        // Documents
        public string? Portfolio { get; set; }
        public IFormFile? CvFile { get; set; }
        
        // Agreements
        [Required]
        public bool DataProcessingAgreement { get; set; }
        
        [Required]
        public bool TermsAgreement { get; set; }
        
        public bool NewsletterSubscription { get; set; }
    }
}