using Microsoft.EntityFrameworkCore;
using form_app_backend.Data;
using form_app_backend.Services;
using form_app_backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure EmailSettings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Register services
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", 
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

if (string.IsNullOrEmpty(app.Environment.WebRootPath))
{
    app.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
}

if (!Directory.Exists(app.Environment.WebRootPath))
{
    Directory.CreateDirectory(app.Environment.WebRootPath);
}

// Configure static files
app.UseStaticFiles();

// Apply Entity Framework migrations on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        app.Logger.LogInformation("Applying Entity Framework migrations...");
        context.Database.Migrate();
        app.Logger.LogInformation("Entity Framework migrations applied successfully.");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Failed to apply Entity Framework migrations.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();