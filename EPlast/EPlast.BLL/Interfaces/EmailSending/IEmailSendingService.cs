using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MimeKit;

namespace EPlast.BLL.Interfaces
{
    public interface IEmailSendingService
    {
        /// <summary>
        /// Composes and sends email 
        /// </summary>
        /// <param name="reciever">Reciever of the message</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="body">Text of the message</param>
        /// <param name="senderName">Our organisation name to be shown in letter</param>
        /// <returns>Result of sending email</returns>
        Task<bool> SendEmailAsync(string reciever, string subject, string body, string senderName);

        /// <summary>
        /// Sends email message
        /// </summary>
        /// <param name="message">Email message to be sent</param>
        Task SendEmailAsync(MimeMessage message);

        /// <summary>
        /// Helper method to create MimeMessage to only one reciever by his email
        /// </summary>
        /// <remarks>
        /// NOTE: This method does not validate incoming arguments
        /// </remarks>
        /// <param name="recieverAddress">Email address of single reciever of the message</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="htmlBody">HTML body of the message</param>
        MimeMessage Compose(string recieverAddress, string subject, string htmlBody);

        /// <summary>
        /// Helper method to create MimeMessage to only one reciever
        /// </summary>
        /// <remarks>
        /// NOTE: This method does not validate incoming arguments
        /// </remarks>
        /// <param name="reciever">Single reciever of the message</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="htmlBody">HTML body of the message</param>
        MimeMessage Compose(MailboxAddress reciever, string subject, string htmlBody);

        /// <summary>
        /// Helper method to create MimeMessage to many recievers
        /// </summary>
        /// <remarks>
        /// NOTE: This method does not validate incoming arguments
        /// </remarks>
        /// <param name="reciever">Multiple recievers of the message</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="htmlBody">HTML body of the message</param>
        MimeMessage Compose(IEnumerable<MailboxAddress> recievers, string subject, string htmlBody);
    }
}
