using form_app_backend.Data;
using form_app_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace form_app_backend.Services
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Make sure the database is created
            context.Database.EnsureCreated();

            // Add any seed data if needed
            if (!context.StudentForms.Any())
            {
                // Add a sample form for testing
                context.StudentForms.Add(new StudentForm
                {
                    Name = "Test",
                    Surname = "User",
                    Faculty = "Computer Science",
                    Motivation = "This is a test motivation text that meets the minimum length requirement of 100 characters. It's just here to ensure the database is working correctly.",
                    SubmissionDate = DateTime.UtcNow
                });

                context.SaveChanges();
            }
        }
    }
}