// File: Services/EmailSender.cs
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace PhanVuBaoMinh.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _smtpServer = configuration["EmailSettings:SmtpServer"]
                ?? throw new InvalidOperationException("EmailSettings:SmtpServer không được cấu hình trong appsettings.json.");

            var smtpPortValue = configuration["EmailSettings:SmtpPort"];
            if (string.IsNullOrEmpty(smtpPortValue) || !int.TryParse(smtpPortValue, out _smtpPort))
            {
                throw new InvalidOperationException("EmailSettings:SmtpPort không được cấu hình hoặc không hợp lệ trong appsettings.json.");
            }

            _smtpUser = configuration["EmailSettings:SmtpUser"]
                ?? throw new InvalidOperationException("EmailSettings:SmtpUser không được cấu hình trong appsettings.json.");

            _smtpPass = configuration["EmailSettings:SmtpPass"]
                ?? throw new InvalidOperationException("EmailSettings:SmtpPass không được cấu hình trong appsettings.json.");

            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUser),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                using var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent successfully to {email}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {email}. Error: {ex.Message}");
                throw;
            }
        }
    }
}