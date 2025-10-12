using System.Net;
using System.Net.Mail;

using MyNews.Api.Interfaces;

namespace MyNews.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string firstName, string token)
        {
            var resetLink = $"{_configuration["FrontendUrl"]}reset-password?token={token}";

            var message = new MailMessage
            {
                From = new MailAddress(_configuration["Email:FromAddress"], "My News"),
                Subject = "Reset your password",
                Body = $@"
                    <p>Hello {firstName},</p>
                    <p>Click the link below to reset your password:</p>
                    <p><a href='{resetLink}'>{resetLink}</a></p>
                    <p>This link expires in 15 minutes.</p>",
                IsBodyHtml = true
            };
            message.To.Add(toEmail);

            try
            {
                using (var smtpClient = new SmtpClient(
                    _configuration["Email:SmtpServer"],
                    int.Parse(_configuration["Email:Port"])))
                {
                    smtpClient.Credentials = new NetworkCredential(
                        _configuration["Email:Username"],
                        _configuration["Email:Password"]);
                    smtpClient.EnableSsl = bool.Parse(_configuration["Email:EnableSsl"]);

                    await smtpClient.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                var logPath = Path.Combine(AppContext.BaseDirectory, "Logs", "errors.txt");
                Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);

                await File.AppendAllTextAsync(logPath,
                    $"{DateTime.UtcNow}: Failed to send email to {toEmail}. Error: {ex}\n");

                throw;
            }
        }

        public async Task SendContactMessageAsync(string title, string messageBody, string fromEmail)
        {
            var body = $@"
                        Contact form message:
                        ---------------------
                        From: {fromEmail}
                        
                        {messageBody}
                        ";

            var message = new MailMessage
            {
                From = new MailAddress(_configuration["Email:FromAddress"], "My News Contact Form"),
                Subject = $"📩 Contact Form: {title}",
                Body = body,
                IsBodyHtml = false
            };

            message.To.Add(_configuration["Email:FromAddress"]);

            try
            {
                using var smtpClient = new SmtpClient(
                    _configuration["Email:SmtpServer"],
                    int.Parse(_configuration["Email:Port"])
                )
                {
                    Credentials = new NetworkCredential(
                        _configuration["Email:Username"],
                        _configuration["Email:Password"]
                    ),
                    EnableSsl = bool.Parse(_configuration["Email:EnableSsl"])
                };

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                var logPath = Path.Combine(AppContext.BaseDirectory, "Logs", "errors.txt");
                Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
                await File.AppendAllTextAsync(logPath,
                    $"{DateTime.UtcNow}: Failed to send contact message. Error: {ex}\n");
                throw;
            }
        }
    }
}
