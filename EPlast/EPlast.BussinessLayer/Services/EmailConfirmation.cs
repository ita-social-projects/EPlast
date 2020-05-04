using EPlast.BussinessLayer.Interfaces;
using EPlast.BussinessLayer.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public class EmailConfirmation:IEmailConfirmation
    {
        public IOptions<EmailServiceSettings> Settings { get; }

        public EmailConfirmation(IOptions<EmailServiceSettings> settings)
        {
            Settings = settings;
        }

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
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(SMTPServer, Port, true);
                await client.AuthenticateAsync(SMTPServerLogin, SMTPServerPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
