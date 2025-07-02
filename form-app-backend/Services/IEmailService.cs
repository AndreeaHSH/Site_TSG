using form_app_backend.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;

namespace form_app_backend.Services
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(StudentForm studentForm);
        Task SendNotificationEmailAsync(StudentForm studentForm, byte[] pdfAttachment);
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendConfirmationEmailAsync(StudentForm studentForm)
        {
            // This method is not used per requirements (only admin notifications)
            _logger.LogInformation($"Confirmation email not sent per configuration - form ID {studentForm.Id}");
            await Task.CompletedTask;
        }

        public async Task SendNotificationEmailAsync(StudentForm studentForm, byte[] pdfAttachment)
        {
            try
            {
                _logger.LogInformation($"Sending admin notification email for new submission from {studentForm.Name} {studentForm.Surname}");

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("TSG Application System", _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress("TSG Admin", _emailSettings.AdminEmail));
                message.Subject = $"New TSG Application Submitted - {studentForm.Name} {studentForm.Surname}";

                var bodyBuilder = new BodyBuilder();
                
                // Create HTML email body
                bodyBuilder.HtmlBody = CreateAdminNotificationHtml(studentForm);
                
                // Create plain text version
                bodyBuilder.TextBody = CreateAdminNotificationText(studentForm);

                // Attach PDF
                if (pdfAttachment != null && pdfAttachment.Length > 0)
                {
                    bodyBuilder.Attachments.Add($"TSG_Application_{studentForm.Id}.pdf", pdfAttachment, ContentType.Parse("application/pdf"));
                }

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                _logger.LogInformation($"Admin notification email sent successfully for form ID {studentForm.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send admin notification email for form ID {studentForm.Id}");
                throw;
            }
        }

        private string CreateAdminNotificationHtml(StudentForm studentForm)
        {
            return $@"
                <html>
                <body>
                    <h2>🎓 New TSG Application Received</h2>
                    
                    <h3>Personal Information</h3>
                    <ul>
                        <li><strong>Name:</strong> {studentForm.Name} {studentForm.Surname}</li>
                        <li><strong>Email:</strong> {studentForm.Email}</li>
                        <li><strong>Phone:</strong> {studentForm.Phone}</li>
                        <li><strong>Birth Date:</strong> {studentForm.BirthDate?.ToString("yyyy-MM-dd") ?? "Not provided"}</li>
                    </ul>

                    <h3>Academic Information</h3>
                    <ul>
                        <li><strong>Faculty:</strong> {studentForm.Faculty}</li>
                        <li><strong>Specialization:</strong> {studentForm.Specialization}</li>
                        <li><strong>Year:</strong> {studentForm.Year}</li>
                        <li><strong>Student ID:</strong> {studentForm.StudentId ?? "Not provided"}</li>
                    </ul>

                    <h3>Role Preferences</h3>
                    <ul>
                        <li><strong>Preferred Role:</strong> {studentForm.PreferredRole}</li>
                        <li><strong>Alternative Role:</strong> {studentForm.AlternativeRole ?? "Not specified"}</li>
                    </ul>

                    <h3>Technical Skills</h3>
                    <ul>
                        <li><strong>Programming Languages:</strong> {studentForm.ProgrammingLanguages ?? "Not specified"}</li>
                        <li><strong>Frameworks:</strong> {studentForm.Frameworks ?? "Not specified"}</li>
                        <li><strong>Tools:</strong> {studentForm.Tools ?? "Not specified"}</li>
                    </ul>

                    <h3>Motivation</h3>
                    <p>{studentForm.Motivation}</p>

                    <h3>How they can contribute</h3>
                    <p>{studentForm.Contribution}</p>

                    <h3>Availability</h3>
                    <ul>
                        <li><strong>Time Commitment:</strong> {studentForm.TimeCommitment}</li>
                        <li><strong>Schedule:</strong> {studentForm.Schedule ?? "Not specified"}</li>
                    </ul>

                    <hr>
                    <p><small><strong>Submitted:</strong> {studentForm.SubmissionDate:yyyy-MM-dd HH:mm:ss}</small></p>
                    <p><small><strong>Application ID:</strong> {studentForm.Id}</small></p>
                    
                    <p>📎 Complete application details are attached as PDF.</p>
                </body>
                </html>";
        }

        private string CreateAdminNotificationText(StudentForm studentForm)
        {
            return $@"
NEW TSG APPLICATION RECEIVED

Personal Information:
- Name: {studentForm.Name} {studentForm.Surname}
- Email: {studentForm.Email}
- Phone: {studentForm.Phone}
- Birth Date: {studentForm.BirthDate?.ToString("yyyy-MM-dd") ?? "Not provided"}

Academic Information:
- Faculty: {studentForm.Faculty}
- Specialization: {studentForm.Specialization}
- Year: {studentForm.Year}
- Student ID: {studentForm.StudentId ?? "Not provided"}

Role Preferences:
- Preferred Role: {studentForm.PreferredRole}
- Alternative Role: {studentForm.AlternativeRole ?? "Not specified"}

Technical Skills:
- Programming Languages: {studentForm.ProgrammingLanguages ?? "Not specified"}
- Frameworks: {studentForm.Frameworks ?? "Not specified"}
- Tools: {studentForm.Tools ?? "Not specified"}

Motivation:
{studentForm.Motivation}

How they can contribute:
{studentForm.Contribution}

Availability:
- Time Commitment: {studentForm.TimeCommitment}
- Schedule: {studentForm.Schedule ?? "Not specified"}

--
Submitted: {studentForm.SubmissionDate:yyyy-MM-dd HH:mm:ss}
Application ID: {studentForm.Id}

Complete application details are attached as PDF.
            ";
        }
    }
}