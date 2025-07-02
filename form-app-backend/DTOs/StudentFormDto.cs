using System.ComponentModel.DataAnnotations;

namespace form_app_backend.DTOs
{
    public class StudentFormDto
    {
        // Personal Information (keep original 4 fields required for Angular compatibility)
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;
        
        // Make Email and Phone optional to maintain Angular compatibility
        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        // Academic Information (keep Faculty required for Angular compatibility)
        [Required]
        [StringLength(200)]
        public string Faculty { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Specialization { get; set; }
        
        [StringLength(20)]
        public string? Year { get; set; }
        
        [StringLength(50)]
        public string? StudentId { get; set; }
        
        // Role Preferences (optional for Angular compatibility)
        [StringLength(100)]
        public string? PreferredRole { get; set; }
        
        [StringLength(100)]
        public string? AlternativeRole { get; set; }
        
        // Technical Skills (all optional)
        public string? ProgrammingLanguages { get; set; }
        
        public string? Frameworks { get; set; }
        
        public string? Tools { get; set; }
        
        // Experience and Motivation (keep Motivation required for Angular compatibility)
        public string? Experience { get; set; }
        
        [Required]
        [MinLength(100, ErrorMessage = "Motivation must be at least 100 characters long.")]
        public string Motivation { get; set; } = string.Empty;
        
        public string? Contribution { get; set; }
        
        // Availability (all optional)
        public string? TimeCommitment { get; set; }
        
        public string? Schedule { get; set; }
        
        // Documents (optional)
        public string? Portfolio { get; set; }
    }
}