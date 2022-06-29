using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EPlast.BLL.Services
{
    public class EmailSendingService : IEmailSendingService
    {
        private readonly ILoggerService<EmailSendingService> _loggerService;

        public EmailSendingService(
            IOptions<EmailServiceSettings> settings,
            ILoggerService<EmailSendingService> loggerService
        )
        {
            Settings = settings;
            _loggerService = loggerService;
        }

        public IOptions<EmailServiceSettings> Settings { get; }

        public MimeMessage Compose(string recieverAddress, string subject, string htmlBody)
        {
            string recieverName = recieverAddress.Split("@")[0];
            var reciever = new MailboxAddress(recieverName, recieverAddress);
            return Compose(reciever, subject, htmlBody);
        }

        public MimeMessage Compose(MailboxAddress reciever, string subject, string htmlBody)
        {
            IEnumerable<MailboxAddress> recievers = new MailboxAddress[] { reciever };
            return Compose(recievers, subject, htmlBody);
        }

        public MimeMessage Compose(IEnumerable<MailboxAddress> recievers, string subject, string htmlBody)
        {
            return new MimeMessage(
                new MailboxAddress[] { new MailboxAddress("Eplast", Settings.Value.SMTPServerLogin) },
                recievers,
                subject,
                new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = htmlBody
                }
            );
        }

        [Obsolete("Use SendEmailAsync(MimeMessage message) method instead")]
        public async Task<bool> SendEmailAsync(string reciever, string subject, string body, string senderName)
        {
            var SMTPServer = Settings.Value.SMTPServer;
            var Port = Settings.Value.Port;
            var SMTPServerLogin = Settings.Value.SMTPServerLogin;
            var SMTPServerPassword = Settings.Value.SMTPServerPassword;

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(senderName, SMTPServerLogin));
            emailMessage.To.Add(new MailboxAddress("", reciever));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(SMTPServer, Port, true);
                await client.AuthenticateAsync(SMTPServerLogin, SMTPServerPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception exс)
            {
                _loggerService.LogError(exс.Message);
                return false;
            }
            return true;
        }

        public async Task SendEmailAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(Settings.Value.SMTPServer, Settings.Value.Port, true);
            await client.AuthenticateAsync(Settings.Value.SMTPServerLogin, Settings.Value.SMTPServerPassword);
            await client.SendAsync(message);
        }
    }
}
