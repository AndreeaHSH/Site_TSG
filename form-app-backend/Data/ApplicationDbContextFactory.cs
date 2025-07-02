using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace form_app_backend.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Use the connection string directly for design-time
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=StudentFormDb;User Id=sa;Password=jJAhwd21!;TrustServerCertificate=true");
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}