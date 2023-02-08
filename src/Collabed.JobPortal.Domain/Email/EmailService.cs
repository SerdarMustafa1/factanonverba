using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace Collabed.JobPortal.Email
{
    public class EmailService : ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IEmailSender emailSender, ILogger<EmailService> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task SendEmailAsync(string recipient, string subject, string body, bool isBodyHtml = true)
        {
            try
            {
                await _emailSender.SendAsync(EmailTemplates.WelcomeSender, recipient, subject, body, isBodyHtml);
                _logger.LogInformation($"Email with {subject} successfully sent to {recipient}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Unexpected error occured while sending email with {subject} to {recipient}", ex);
            }
        }
    }
}
