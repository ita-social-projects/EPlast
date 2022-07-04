using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services;
using EPlast.BLL.Settings;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services
{
    internal class EmailSendingServiceTests
    {
        private Mock<ILoggerService> _mockLoggerService;
        private Mock<IOptions<EmailServiceSettings>> _mockOptions;
        private EmailSendingService emailSendingService;

        [Test]
        public async Task SendEmailAsync_InValid_MailBoxExc_Test()
        {
            _mockOptions
                .Setup(x => x.Value)
                .Returns(new EmailServiceSettings() { SMTPServerLogin = "SMTPServerLogin" });

            var result = await emailSendingService.SendEmailAsync("email", "subject", "message", "title");

            _mockOptions.Verify();
            _mockLoggerService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(false, result);
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

            var result = await emailSendingService.SendEmailAsync("email", "subject", "message", "title");
            _mockOptions.Verify();
            _mockLoggerService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(false, result);
        }

        [SetUp]
        public void SetUp()
        {
            _mockOptions = new Mock<IOptions<EmailServiceSettings>>();
            _mockLoggerService = new Mock<ILoggerService>();
            emailSendingService = new EmailSendingService(
                _mockOptions.Object,
                _mockLoggerService.Object);
        }
    }
}
