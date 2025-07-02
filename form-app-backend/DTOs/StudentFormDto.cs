using System.ComponentModel.DataAnnotations;

namespace form_app_backend.DTOs
{
    public class StudentFormDto
    {
        // Keep existing fields (required for Angular app)
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Faculty { get; set; } = string.Empty;
        
        [Required]
        [MinLength(100, ErrorMessage = "Motivation must be at least 100 characters long.")]
        public string Motivation { get; set; } = string.Empty;
        
        // Add new TSG fields (optional for backward compatibility)
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Specialization { get; set; }
        public string? Year { get; set; }
        public string? StudentId { get; set; }
        public string? PreferredRole { get; set; }
        public string? AlternativeRole { get; set; }
        public string? ProgrammingLanguages { get; set; }
        public string? Frameworks { get; set; }
        public string? Tools { get; set; }
        public string? Experience { get; set; }
        public string? Contribution { get; set; }
        public string? TimeCommitment { get; set; }
        public string? Schedule { get; set; }
        public string? Portfolio { get; set; }
        public bool DataProcessingAgreement { get; set; }
        public bool TermsAgreement { get; set; }
        public bool NewsletterSubscription { get; set; }
    }
}