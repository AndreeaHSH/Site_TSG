using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace form_app_backend.Models
{
    public class StudentForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        // Personal Information
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        public DateTime? BirthDate { get; set; }
        
        // Academic Information
        [Required]
        [StringLength(200)]
        public string Faculty { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Specialization { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Year { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? StudentId { get; set; }
        
        // Role Preferences
        [Required]
        [StringLength(100)]
        public string PreferredRole { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? AlternativeRole { get; set; }
        
        // Technical Skills 
        public string? ProgrammingLanguages { get; set; } // JSON array
        
        public string? Frameworks { get; set; }
        
        public string? Tools { get; set; }
        
        // Experience and Motivation
        public string? Experience { get; set; }
        
        [Required]
        [MinLength(100, ErrorMessage = "Motivation must be at least 100 characters long.")]
        public string Motivation { get; set; } = string.Empty;
        
        [Required]
        public string Contribution { get; set; } = string.Empty;
        
        // Availability
        [Required]
        [StringLength(50)]
        public string TimeCommitment { get; set; } = string.Empty;
        
        public string? Schedule { get; set; } // JSON array
        
        // Documents
        public string? CvFileName { get; set; }
        
        public string? CvFilePath { get; set; }
        
        public string? Portfolio { get; set; }
        
        // System fields
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
    }
}