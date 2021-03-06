﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces;
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
        private Mock<IActionContextAccessor> _mockActionContextAccessor;
        private Mock<IAuthService> _mockAuthService;
        private Mock<IEmailSendingService> _mockEmailSendingService;
        private Mock<IEmailContentService> _mockEmailContentService;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private Mock<IUrlHelperFactory> _mockUrlHelperFactory;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IUrlHelper> _Url;
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

            var memoryConfig = new Dictionary<string, string>();
            memoryConfig["Mode"] = "Test";
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
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            _mockActionContextAccessor = new Mock<IActionContextAccessor>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _Url = new Mock<IUrlHelper>();

            _Url.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("callbackUrl")
                .Verifiable();
            _mockUrlHelperFactory
                .Setup(s => s.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(_Url.Object);
            _mockHttpContextAccessor
                .Setup(x => x.HttpContext.Request.Scheme)
                .Returns("http");

            _authEmailService = new AuthEmailService(
                _mockEmailSendingService.Object,
                _mockEmailContentService.Object,
                _mockAuthService.Object,
                _mockUserManager.Object,
                _mockUrlHelperFactory.Object,
                _mockActionContextAccessor.Object,
                _mockHttpContextAccessor.Object);
        }
    }
}
