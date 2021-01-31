using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services
{
    internal class AuthEmailServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _mockEmailConfirmation = new Mock<IEmailConfirmation>();
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
            authEmailService = new AuthEmailService(
                _mockEmailConfirmation.Object,
                _mockAuthService.Object,
                _mockUserManager.Object,
                _mockUrlHelperFactory.Object,
                _mockActionContextAccessor.Object,
                _mockHttpContextAccessor.Object);
        }

        [Test]
        public void SendEmailRegistrAsync_Valid_Test()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockEmailConfirmation
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            var expected = true;

            //Act
            var result = authEmailService.SendEmailRegistrAsync("email");

            //Assert
            _mockEmailConfirmation.Verify();
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.Result);
        }

        [Test]
        public void SendEmailResetingAsync_Valid_Test()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockEmailConfirmation
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            //Act
            var result = authEmailService.SendEmailResetingAsync("confirmationLink", new BLL.DTO.Account.ForgotPasswordDto());

            //Assert
            _mockEmailConfirmation.Verify();
            _mockUserManager.Verify();
            Assert.IsNotNull(result);
        }

        [Test]
        public void SendEmailReminderAsync_Valid_Test()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockEmailConfirmation
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            //Act
            var result = authEmailService.SendEmailReminderAsync("citiesUrl", new BLL.DTO.UserProfiles.UserDTO());

            //Assert
            _mockEmailConfirmation.Verify();
            _mockUserManager.Verify();
            Assert.IsNotNull(result);
        }

        private Mock<IEmailConfirmation> _mockEmailConfirmation;
        private Mock<IAuthService> _mockAuthService;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IUrlHelperFactory> _mockUrlHelperFactory;
        private Mock<IActionContextAccessor> _mockActionContextAccessor;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private AuthEmailService authEmailService;
        private Mock<IUrlHelper> _Url;
    }
}
