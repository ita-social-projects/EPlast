using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.HostURL;
using EPlast.BLL.Models;
using EPlast.BLL.Services.Auth;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Moq;
using NLog;
using NLog.Extensions.Logging;
using NUnit.Framework;

namespace EPlast.Tests.Services
{
    internal class AuthEmailServiceTests
    {
        private Mock<IAuthService> _mockAuthService;
        private Mock<IEmailSendingService> _mockEmailSendingService;
        private Mock<IEmailContentService> _mockEmailContentService;
        private Mock<IHostUrlService> _mockHostUrlService;
        private Mock<UserManager<User>> _mockUserManager;
        private AuthEmailService _authEmailService;

        [TestCase("email")]
        public void SendEmailGreetingAsync_Valid(string email)
        {
            //Arrange
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockEmailContentService.Setup(x => x.GetAuthGreetingEmail(It.IsAny<string>()))
                .Returns(new EmailModel());
            _mockEmailSendingService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(true);

            var memoryConfig = new Dictionary<string, string>
            {
                ["Mode"] = "Test"
            };
            ConfigSettingLayoutRenderer.DefaultConfiguration = new ConfigurationBuilder().AddInMemoryCollection(memoryConfig).Build();
            var layoutRenderer = new ConfigSettingLayoutRenderer { Item = "Mode" };

            //Act
            var result = _authEmailService.SendEmailGreetingAsync(email);
            var configResult = layoutRenderer.Render(LogEventInfo.CreateNullEvent());

            //Assert
            Assert.AreEqual("Test", configResult);
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<Task<bool>>(result);
        }

        [Test]
        public void SendEmailRegistrAsync_Valid_Test()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockEmailSendingService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockEmailContentService
                .Setup(x => x.GetAuthRegisterEmail(It.IsAny<string>()))
                .Returns(new EmailModel());
            var expected = true;

            //Act
            var result = _authEmailService.SendEmailRegistrAsync("email");

            //Assert
            _mockEmailSendingService.Verify();
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.Result);
        }

        [TestCase("email", "userId")]
        public void SendEmailJoinToCityReminderAsync_Valid(string email, string userId)
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockEmailSendingService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockEmailContentService
                .Setup(x => x.GetAuthJoinToCityReminderEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new EmailModel());

            //Act
            var result = _authEmailService.SendEmailJoinToCityReminderAsync(email, userId);

            //Assert
            _mockEmailSendingService.Verify();
            _mockUserManager.Verify();
            Assert.IsNotNull(result);
        }

        [Test]
        public void SendEmailResetingAsync_Valid_Test()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockEmailContentService
                .Setup(x => x.GetAuthResetPasswordEmail(It.IsAny<string>()))
                .Returns(new EmailModel());
            _mockEmailSendingService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            //Act
            var result = _authEmailService.SendEmailResetingAsync("confirmationLink", new BLL.DTO.Account.ForgotPasswordDto());

            //Assert
            _mockEmailSendingService.Verify();
            _mockUserManager.Verify();
            Assert.IsNotNull(result);
        }

        [SetUp]
        public void SetUp()
        {
            _mockEmailSendingService = new Mock<IEmailSendingService>();
            _mockEmailContentService = new Mock<IEmailContentService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockHostUrlService = new Mock<IHostUrlService>();
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _authEmailService = new AuthEmailService(
                _mockEmailSendingService.Object,
                _mockEmailContentService.Object,
                _mockAuthService.Object,
                _mockUserManager.Object,
                _mockHostUrlService.Object);
        }
    }
}
