using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Jellyfin.Plugin.Newsletters.Configuration;

namespace Jellyfin.Plugin.Newsletters.Emails
{
    public class EmailService
    {
        private readonly PluginConfiguration _config;
        private readonly ILogger _logger;

        public EmailService(PluginConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> SendEmail(string recipient, string subject, string htmlBody)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_config.SenderName, _config.SmtpUsername));
                message.To.Add(MailboxAddress.Parse(recipient));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                var secureOption = _config.SmtpUseSsl
                    ? SecureSocketOptions.SslOnConnect
                    : SecureSocketOptions.StartTls;

                await client.ConnectAsync(_config.SmtpHost, _config.SmtpPort, secureOption);
                await client.AuthenticateAsync(_config.SmtpUsername, _config.SmtpPassword);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {Recipient}", recipient);
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to send email via ProtonMail SMTP.");
                return false;
            }
        }
    }
}
