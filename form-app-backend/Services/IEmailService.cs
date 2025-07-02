using form_app_backend.Models;

namespace form_app_backend.Services
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(StudentForm studentForm);
        Task SendNotificationEmailAsync(StudentForm studentForm);
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendConfirmationEmailAsync(StudentForm studentForm)
        {
            // For now, just log the email sending
            _logger.LogInformation($"Sending confirmation email to {studentForm.Email} for form ID {studentForm.Id}");
            
            // TODO: Implement actual email sending logic here
            // You can use services like SendGrid, SMTP, etc.
            
            await Task.CompletedTask;
        }

        public async Task SendNotificationEmailAsync(StudentForm studentForm)
        {
            // For now, just log the notification sending
            _logger.LogInformation($"Sending notification email for new submission from {studentForm.Name} {studentForm.Surname}");
            
          
            await Task.CompletedTask;
        }
    }
}