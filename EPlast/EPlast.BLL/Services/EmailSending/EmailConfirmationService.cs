using EPlast.BLL.Interfaces;
using EPlast.BLL.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public class EmailConfirmationService:IEmailConfirmation
    {
        public IOptions<EmailServiceSettings> Settings { get; }

        public EmailConfirmationService(IOptions<EmailServiceSettings> settings)
        {
            Settings = settings;
        }

        ///<inheritdoc/>
        public async Task SendEmailAsync(string email, string subject, string message, string title)
        {
            var SMTPServer = Settings.Value.SMTPServer;
            var Port = Settings.Value.Port;
            var SMTPServerLogin = Settings.Value.SMTPServerLogin;
            var SMTPServerPassword = Settings.Value.SMTPServerPassword;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(title, SMTPServerLogin));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                using (var client = new SmtpClient())
                {
                    throw new MailKit.Security.AuthenticationException();
                    await client.ConnectAsync(SMTPServer, Port, true);
                    await client.AuthenticateAsync(SMTPServerLogin, SMTPServerPassword);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception exс)
            {
                _loggerService.LogError(exс.Message);
                return false;
            }
            return true;
        }
    }
}
