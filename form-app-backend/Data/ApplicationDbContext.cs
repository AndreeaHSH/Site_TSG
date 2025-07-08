using Microsoft.EntityFrameworkCore;
using form_app_backend.Models;

namespace form_app_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StudentForm> StudentForms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentForm>()
                .ToTable("StudentForm", "dbo");  // This maps to the SQL table name

            modelBuilder.Entity<StudentForm>()
                .Property(s => s.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<StudentForm>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<StudentForm>()
                .Property(s => s.Name)
                .IsRequired();

            modelBuilder.Entity<StudentForm>()
                .Property(s => s.Surname)
                .IsRequired();

            modelBuilder.Entity<StudentForm>()
                .Property(s => s.Faculty)
                .IsRequired();

            modelBuilder.Entity<StudentForm>()
                .Property(s => s.Motivation)
                .IsRequired();
        }
    }
}