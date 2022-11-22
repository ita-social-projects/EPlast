#nullable enable

using System;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.HostURL;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    public class AuthControllerTestsAuth
    {
        [SetUp]
        public void SetUp()
        {
            _userDatesServiceMock = new Mock<IUserDatesService>(MockBehavior.Strict);
            _emailSendingServiceMock = new Mock<IEmailSendingService>(MockBehavior.Strict);
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _cityParticipantServiceMock = new Mock<ICityParticipantsService>(MockBehavior.Strict);
            _userManagerMock = new Mock<UserManager<User>>(MockBehavior.Strict, Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _userManagerMock
                .SetupSet(m => m.Logger = null);
            _hostURLService = new Mock<IHostUrlService>();
            _loggerServiceMock = new Mock<ILoggerService<AuthController>>();
            _controller = new AuthController(
                _userDatesServiceMock.Object,
                _emailSendingServiceMock.Object,
                _mapperMock.Object,
                _cityParticipantServiceMock.Object,
                _userManagerMock.Object,
                _hostURLService.Object,
                _loggerServiceMock.Object
            );
        }

        [Test]
        public void ConfirmEmail_InvalidModelState_ReturnsRedirectWithError400()
        {
            // Arrange
            _controller.ModelState.AddModelError("", "");
            _hostURLService.Setup(h => h.GetSignInURL(400)).Returns(GetURL(400));

            // Act
            var response = _controller.ConfirmEmail("", "").Result;

            // Assert
            Assert.IsInstanceOf<RedirectResult>(response);
            Assert.True((response as RedirectResult)?.Url.Contains("error=400"));
        }

        [Test]
        public void ConfirmEmail_UserDoesNotExist_ReturnsRedirectWithError404()
        {
            // Arrange
            _userManagerMock
                .Setup(m => m.FindByIdAsync(""))
                .ReturnsAsync(value: null!);
            _hostURLService.Setup(h => h.GetSignInURL(404)).Returns(GetURL(404));

            // Act
            var response = _controller.ConfirmEmail("", "").Result;

            // Assert
            Assert.IsInstanceOf<RedirectResult>(response);
            Assert.True((response as RedirectResult)?.Url.Contains("error=404"));
        }

        [Test]
        public void ConfirmEmail_12HrElapsed_DeletesUserAndRedirectsWithError410()
        {
            // Arrange
            var user = new User()
            {
                RegistredOn = DateTime.Now.AddHours(-12)
            };

            _userManagerMock
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock
                .Setup(m => m.DeleteAsync(It.Is<User>(v => v == user)))
                .ReturnsAsync(value: null!);
            _hostURLService.Setup(h => h.GetSignInURL(410)).Returns(GetURL(410));

            // Act
            var response = _controller.ConfirmEmail("", "").Result;

            // Assert
            Assert.IsInstanceOf<RedirectResult>(response);
            Assert.True((response as RedirectResult)?.Url.Contains("error=410"));
            _userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void ConfirmEmail_UserManagerReturnsError_ReturnsRedirectWithError400()
        {
            // Arrange
            var user = new User()
            {
                RegistredOn = DateTime.Now
            };

            _userManagerMock
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock
                .Setup(m => m.ConfirmEmailAsync(It.Is<User>(v => v == user), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());
            _hostURLService.Setup(h => h.GetSignInURL(400)).Returns(GetURL(400));

            // Act
            var response = _controller.ConfirmEmail("", "").Result;

            // Assert
            Assert.IsInstanceOf<RedirectResult>(response);
            Assert.True((response as RedirectResult)?.Url.Contains("error=400"));
            _userManagerMock.Verify(m => m.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ConfirmEmail_ValidWithCityIdSet_RedirectsWithNoError()
        {
            // Arrange
            var user = new User()
            {
                CityId = 1,
                RegistredOn = DateTime.Now
            };

            _userManagerMock
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock
                .Setup(m => m.ConfirmEmailAsync(It.Is<User>(v => v == user), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userDatesServiceMock
                .Setup(m => m.AddDateEntryAsync(It.Is<string>(v => v == user.Id)))
                .ReturnsAsync(true);
            _cityParticipantServiceMock
                .Setup(m => m.AddFollowerAsync(It.Is<int>(v => v == user.CityId), It.Is<string>(v => v == user.Id)))
                .ReturnsAsync(value: null!);
            _userManagerMock
                .Setup(m => m.AddToRoleAsync(It.Is<User>(v => v == user), It.IsAny<string>()))
                .ReturnsAsync(value: null!);
            _hostURLService.Setup(h => h.GetSignInURL(It.IsAny<int>())).Returns(GetURL(It.IsAny<int>()));
            _hostURLService.Setup(h => h.SignInURL).Returns(GetURL());

            // Act
            var response = _controller.ConfirmEmail("", "").Result;

            // Assert
            Assert.IsInstanceOf<RedirectResult>(response);
            Assert.False((response as RedirectResult)?.Url.Contains("error"));
            _userManagerMock.Verify(m => m.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            _userDatesServiceMock.Verify(m => m.AddDateEntryAsync(It.IsAny<string>()), Times.Once);
            _cityParticipantServiceMock.Verify(m => m.AddFollowerAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ConfirmEmail_ValidWithRegionIdSet_RedirectsWithNoError()
        {
            // Arrange
            var user = new User()
            {
                RegionId = 1,
                RegistredOn = DateTime.Now
            };

            _userManagerMock
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock
                .Setup(m => m.ConfirmEmailAsync(It.Is<User>(v => v == user), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userDatesServiceMock
                .Setup(m => m.AddDateEntryAsync(It.Is<string>(v => v == user.Id)))
                .ReturnsAsync(true);
            _cityParticipantServiceMock
                .Setup(m => m.AddNotificationUserWithoutSelectedCity(It.Is<User>(v => v == user), It.Is<int>(v => v == user.RegionId)))
                .Returns(Task.CompletedTask);
            _userManagerMock
                .Setup(m => m.AddToRoleAsync(It.Is<User>(v => v == user), It.IsAny<string>()))
                .ReturnsAsync(value: null!);
            _hostURLService.Setup(h => h.GetSignInURL(It.IsAny<int>())).Returns(GetURL(It.IsAny<int>()));
            _hostURLService.Setup(h => h.SignInURL).Returns(GetURL());

            // Act
            var response = _controller.ConfirmEmail("", "").Result;

            // Assert
            Assert.IsInstanceOf<RedirectResult>(response);
            Assert.False((response as RedirectResult)?.Url.Contains("error"));
            _userManagerMock.Verify(m => m.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            _userDatesServiceMock.Verify(m => m.AddDateEntryAsync(It.IsAny<string>()), Times.Once);
            _cityParticipantServiceMock.Verify(m => m.AddNotificationUserWithoutSelectedCity(It.IsAny<User>(), It.IsAny<int>()), Times.Once);
        }

        [TestCase(0)]
        [TestCase(3)]
        [TestCase(8)]
        public void SignUp_NonExistingGenderId_ReturnsBadRequest(int genderId)
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                GenderId = genderId
            };

            // Act
            var response = _controller.SignUp(registerDto).Result;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }

        [Test]
        public void SignUp_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("", "");
            var registerDto = new RegisterDto()
            {
                GenderId = 1
            };

            // Act
            var response = _controller.SignUp(registerDto).Result;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }

        [Test]
        public void SignUp_UserAlreadyExistsWithConfirmedEmail_ReturnsConflict()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                GenderId = 1
            };
            var user = new User();

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock
                .Setup(m => m.IsEmailConfirmedAsync(It.Is<User>(v => v == user)))
                .ReturnsAsync(true);

            // Act
            var response = _controller.SignUp(registerDto).Result;

            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(response);

            var errorObject = (AuthController.ConflictErrorObject?)(response as ConflictObjectResult)?.Value;
            Assert.IsTrue(errorObject?.IsEmailConfirmed);
        }

        [Test]
        public void SignUp_UserAlreadyExistsWithNotConfirmedEmail_ReturnsConflict()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                GenderId = 1
            };
            var user = new User { RegistredOn = DateTime.Now.AddHours(6) };

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock
                .Setup(m => m.IsEmailConfirmedAsync(It.Is<User>(v => v == user)))
                .ReturnsAsync(false);

            // Act
            var response = _controller.SignUp(registerDto).Result;

            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(response);

            var errorObject = (AuthController.ConflictErrorObject?)(response as ConflictObjectResult)?.Value;
            Assert.IsFalse(errorObject?.IsEmailConfirmed);
        }

        [Test]
        public void SignUp_UserCreationFailed_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                GenderId = 1
            };
            var user = new User();

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(value: null!);
            _mapperMock
                .Setup(m => m.Map<User>(It.Is<RegisterDto>(v => v == registerDto)))
                .Returns(user);
            _userManagerMock
                .Setup(m => m.CreateAsync(It.Is<User>(v => v == user), It.Is<string>(v => v == registerDto.Password)))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var response = _controller.SignUp(registerDto).Result;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void SignUp_EmailSendingServiceThrowsException_DeletesUserAndRethrows()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                GenderId = 1,
                Email = ""
            };
            var user = new User()
            {
                Email = registerDto.Email
            };
            var message = new MimeMessage();

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(value: null!);
            _mapperMock
                .Setup(m => m.Map<User>(It.Is<RegisterDto>(v => v == registerDto)))
                .Returns(user);
            _userManagerMock
                .Setup(m => m.CreateAsync(It.Is<User>(v => v == user), It.Is<string>(v => v == registerDto.Password)))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock
                .Setup(m => m.GenerateEmailConfirmationTokenAsync(It.Is<User>(v => v == user)))
                .ReturnsAsync("");
            _emailSendingServiceMock
                .Setup(m => m.Compose(It.Is<MailboxAddress>(v => v.Address == user.Email), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(message);
            _emailSendingServiceMock
                .Setup(m => m.SendEmailAsync(It.Is<MimeMessage>(v => v == message)))
                .ThrowsAsync(new SmtpException());
            _userManagerMock
                .Setup(m => m.DeleteAsync(It.Is<User>(v => v == user)))
                .ReturnsAsync(value: null!);

            var urlHelper = new Mock<IUrlHelper>(MockBehavior.Loose);
            _controller.Url = urlHelper.Object;

            // Act
            AsyncTestDelegate action = () => _controller.SignUp(registerDto);

            // Assert
            Assert.ThrowsAsync<SmtpException>(action);
            _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()), Times.Once);
            _emailSendingServiceMock.Verify(m => m.SendEmailAsync(It.IsAny<MimeMessage>()), Times.Once);
            _userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void SignUp_Valid_SendsEmailAndReturnsOkWithUserDTO()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                GenderId = 1,
                Email = "",
                Oblast = UkraineOblasts.Crimea
            };
            var user = new User()
            {
                Id = "",
                Email = registerDto.Email
            };
            var message = new MimeMessage();

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(value: null!);
            _mapperMock
                .Setup(m => m.Map<User>(It.Is<RegisterDto>(v => v == registerDto)))
                .Returns(user);
            _userManagerMock
                .Setup(m => m.CreateAsync(It.Is<User>(v => v == user), It.Is<string>(v => v == registerDto.Password)))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock
                .Setup(m => m.GenerateEmailConfirmationTokenAsync(It.Is<User>(v => v == user)))
                .ReturnsAsync("");
            _emailSendingServiceMock
                .Setup(m => m.Compose(It.Is<MailboxAddress>(v => v.Address == user.Email), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(message);
            _emailSendingServiceMock
                .Setup(m => m.SendEmailAsync(It.Is<MimeMessage>(v => v == message)))
                .Returns(Task.CompletedTask);
            _mapperMock
                .Setup(m => m.Map<UserDto>(It.Is<User>(v => v == user)))
                .Returns(new UserDto());

            var urlHelper = new Mock<IUrlHelper>(MockBehavior.Loose);
            _controller.Url = urlHelper.Object;

            // Act
            var response = _controller.SignUp(registerDto).Result;

            // Assert
            Assert.IsInstanceOf<NoContentResult>(response);
            _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()), Times.Once);
            _emailSendingServiceMock.Verify(m => m.SendEmailAsync(It.IsAny<MimeMessage>()), Times.Once);
        }

        [Test]
        public void ResendConfirmationEmail_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("", "");

            // Act
            var response = _controller.ResendConfirmationEmail("").Result;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }

        [Test]
        public void ResendConfirmationEmail_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _userManagerMock
                .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(value: null!);

            // Act
            var response = _controller.ResendConfirmationEmail("").Result;

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(response);
        }

        [Test]
        public void ResendConfirmationEmail_EmailAlreadyConfirmed_ReturnsBadRequest()
        {
            // Arrange
            var user = new User();

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock
                .Setup(m => m.IsEmailConfirmedAsync(It.Is<User>(v => v == user)))
                .ReturnsAsync(true);

            // Act
            var response = _controller.ResendConfirmationEmail("").Result;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }

        [Test]
        public void ResendConfirmationEmail_Valid_SendsEmailAndReturnsNoContent()
        {
            // Arrange
            var user = new User()
            {
                Email = ""
            };
            var message = new MimeMessage();

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock
                .Setup(m => m.IsEmailConfirmedAsync(It.Is<User>(v => v == user)))
                .ReturnsAsync(false);
            _userManagerMock
                .Setup(m => m.GenerateEmailConfirmationTokenAsync(It.Is<User>(v => v == user)))
                .ReturnsAsync("");
            _emailSendingServiceMock
                .Setup(m => m.Compose(It.IsAny<MailboxAddress>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(message);
            _emailSendingServiceMock
                .Setup(m => m.SendEmailAsync(It.Is<MimeMessage>(v => v == message)))
                .Returns(Task.CompletedTask);

            var urlHelper = new Mock<IUrlHelper>(MockBehavior.Loose);
            _controller.Url = urlHelper.Object;

            // Act
            var response = _controller.ResendConfirmationEmail("").Result;

            // Assert
            Assert.IsInstanceOf<NoContentResult>(response);
            _userManagerMock.Verify(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()), Times.Once);
            _emailSendingServiceMock.Verify(m => m.SendEmailAsync(It.IsAny<MimeMessage>()), Times.Once);
        }

        [Test]
        public void Feedback_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("", "");

            // Act
            var response = _controller.Feedback(new FeedbackDto()).Result;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }

        [Test]
        public void Feedback_Valid_SendsEmailAndReturnsNoContent()
        {
            // Arrange
            var message = new MimeMessage();

            _emailSendingServiceMock
                .Setup(m => m.Compose(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(message);
            _emailSendingServiceMock
                .Setup(m => m.SendEmailAsync(It.Is<MimeMessage>(v => v == message)))
                .Returns(Task.CompletedTask);

            // Act
            var response = _controller.Feedback(new FeedbackDto()).Result;

            // Assert
            Assert.IsInstanceOf<NoContentResult>(response);
            _emailSendingServiceMock.Verify(m => m.SendEmailAsync(It.IsAny<MimeMessage>()), Times.Once);
        }

        private Mock<IUserDatesService> _userDatesServiceMock = null!;
        private Mock<IEmailSendingService> _emailSendingServiceMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private Mock<ICityParticipantsService> _cityParticipantServiceMock = null!;
        private Mock<UserManager<User>> _userManagerMock = null!;
        private Mock<IHostUrlService> _hostURLService = null!;
        private AuthController _controller = null!;
        private Mock<ILoggerService<AuthController>> _loggerServiceMock = null!;

        private string GetURL()
        {
            return $"localhost";
        }        
        
        private string GetURL(int error)
        {
            return $"{GetURL()}?error={error}";
        }
    }
}
