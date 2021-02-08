using EPlast.BLL;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Settings;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EPlast.Tests.Services
{
    internal class EmailConfiramtionServicesTests
    {
        [SetUp]
        public void SetUp()
        {
            _mockOptions = new Mock<IOptions<EmailServiceSettings>>();
            _mockLoggerService = new Mock<ILoggerService<EmailSendingService>>();
            emailConfirmationService = new EmailSendingService(
                _mockOptions.Object,
                _mockLoggerService.Object);
        }

        [Test]
        public async Task SendEmailAsync_Valid_Test()
        {
            _mockOptions
                .Setup(x => x.Value)
                .Returns(new EmailServiceSettings()
                {
                    SMTPServer = "SMTPServer",
                    Port = 0,
                    SMTPServerLogin = "SMTPServerLogin",
                    SMTPServerPassword = "SMTPServerPassword"
                });

            var result = await emailConfirmationService.SendEmailAsync("email", "subject", "message", "title");
            _mockOptions.Verify();
            _mockLoggerService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task SendEmailAsync_InValid_MailBoxExc_Test()
        {
            _mockOptions
                .Setup(x => x.Value)
                .Returns(new EmailServiceSettings() { SMTPServerLogin = "SMTPServerLogin" });

            var result = await emailConfirmationService.SendEmailAsync("email", "subject", "message", "title");

            _mockOptions.Verify();
            _mockLoggerService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(false, result);
        }

        private Mock<IOptions<EmailServiceSettings>> _mockOptions;
        private Mock<ILoggerService<EmailSendingService>> _mockLoggerService;
        private EmailSendingService emailConfirmationService;
    }
}
