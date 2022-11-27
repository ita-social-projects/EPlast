using EPlast.BLL.DTO;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Services
{
    internal class EmailSendingServiceTests
    {
        private Mock<ILoggerService<EmailSendingService>> _mockLoggerService;
        private Mock<IOptions<EmailServiceSettings>> _mockOptions;
        private EmailSendingService emailSendingService;

        private static EmailServiceSettings emailServiceSettings = new EmailServiceSettings() 
        {
            SMTPServer = "SMTPServer",
            Port = 0,
            SMTPServerLogin = "SMTPServerLogin",
            SMTPServerPassword = "SMTPServerPassword"
        };

        const string name = "name";
        const string subject = "subject";
        const string message = "message";
        const string title = "title";
        const string address = "address";
        const string email = "email";

        public bool ExceptionHandlerMethod(Action action)
        {
            bool exceptionOccured = false;
            try
            {
                action();
            }
            catch
            {
                exceptionOccured = true;
            }
            return exceptionOccured;
        }

        [Test]
        public async Task SendEmailAsync_Valid_Test()
        {
            _mockOptions
                .Setup(x => x.Value)
                .Returns(emailServiceSettings);

            var result = await emailSendingService.SendEmailAsync(email, subject, message, title);
            _mockOptions.Verify();
            _mockLoggerService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(false, result);
        }

        [Test]
        public Task SendEmailAsync_DoesNotThrowException()
        {
            _mockOptions
                .Setup(x => x.Value)
                .Returns(emailServiceSettings);

            MimeMessage message = new MimeMessage();
            Action action = async () => await emailSendingService.SendEmailAsync(message);
            bool exceptionOccured = ExceptionHandlerMethod(action);

            Assert.False(exceptionOccured);
            return Task.CompletedTask;
        }

        [Test]
        public Task Compose_Test_IsAssignable()
        {
            _mockOptions
                .Setup(x => x.Value)
                .Returns(emailServiceSettings);

            var reciever = new MailboxAddress(name, address);
            IEnumerable<MailboxAddress> recievers = new MailboxAddress[] { reciever };

            var result = emailSendingService.Compose(recievers, subject, message);
            _mockOptions.Verify();
            _mockLoggerService.Verify();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<MimeMessage>(result);
            return Task.CompletedTask;
        }

        [Test]
        public Task Compose_Test_IsInstanceOf()
        {
            _mockOptions
                .Setup(x => x.Value)
                .Returns(emailServiceSettings);

            var reciever = new MailboxAddress(name, address);
            IEnumerable<MailboxAddress> recievers = new MailboxAddress[] { reciever };

            var result = emailSendingService.Compose(recievers, subject, message);
            _mockOptions.Verify();
            _mockLoggerService.Verify();

            Assert.NotNull(result);
            Assert.IsInstanceOf<MimeMessage>(result);
            return Task.CompletedTask;
        }

        [Test]
        public Task Compose_ThreeStringInput_Test_IsAssignable()
        {
            _mockOptions
                .Setup(x => x.Value)
                .Returns(emailServiceSettings);

            var result = emailSendingService.Compose(address, subject, message);
            _mockOptions.Verify();
            _mockLoggerService.Verify();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<MimeMessage>(result);
            return Task.CompletedTask;
        }

        [Test]
        public Task Compose_MailboxAddressTwoStringInput_Test_IsAssignable()
        {
            _mockOptions
                .Setup(x => x.Value)
                .Returns(emailServiceSettings);

            var reciever = new MailboxAddress(name, address);

            var result = emailSendingService.Compose(reciever, subject, message);
            _mockOptions.Verify();
            _mockLoggerService.Verify();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<MimeMessage>(result);
            return Task.CompletedTask;
        }

        [SetUp]
        public void SetUp()
        {
            _mockOptions = new Mock<IOptions<EmailServiceSettings>>();
            _mockLoggerService = new Mock<ILoggerService<EmailSendingService>>();
            emailSendingService = new EmailSendingService(
                _mockOptions.Object,
                _mockLoggerService.Object);
        }
    }
}
