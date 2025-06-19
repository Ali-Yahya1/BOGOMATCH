using BOGOGMATCH_DOMAIN.INTERFACE;
using BOGOGMATCH_DOMAIN.MODELS.UserManagement;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace BOGOMATCH_INFRASTRUCTURE.SERVICES
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<EmailResult> SendEmailAsync(Email emailModel)
        {
            try
            {
                var apiKey = _configuration["SendGridSettings:ApiKey"];
                var fromEmail = _configuration["SendGridSettings:FromEmail"];
                var fromName = _configuration["SendGridSettings:FromName"];

                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmail, fromName);
                var to = new EmailAddress(emailModel.To);
                var subject = emailModel.Subject;
                var htmlContent = emailModel.Content;

                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    return new EmailResult(true, "Email sent successfully.");
                }

                var responseBody = await response.Body.ReadAsStringAsync();
                return new EmailResult(false, $"Failed to send email. Status: {response.StatusCode}, Body: {responseBody}");
            }
            catch (Exception ex)
            {
                return new EmailResult(false, $"Exception occurred while sending email: {ex.Message}");
            }
        }

        


    }

}
