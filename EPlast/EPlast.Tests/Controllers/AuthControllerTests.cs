using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Resources;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using NLog.Extensions.Logging;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    public class AuthControllerTestsAuth
    {
        [Test]
        public async Task ConfirmingEmail_Valid_ReturnRedirect()
        {
            //Arrange
            var (mockAuthService,
                _,
                _,
                mockAuthEmailService,
                _,
                AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";
            var expectedUrl = "Test url";
            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthEmailService
                .Setup(s => s.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mockAuthEmailService
                .Setup(x => x.SendEmailGreetingAsync(It.IsAny<string>())).ReturnsAsync(true);

            var memoryConfig = new Dictionary<string, string>
            {
                {"URLs:SignIn", expectedUrl}
            };
            ConfigSettingLayoutRenderer.DefaultConfiguration = new ConfigurationBuilder().AddInMemoryCollection(memoryConfig).Build();
            //Act
            var result = await AuthController.ConfirmingEmailAsync(userId, token);
            var actual = (result as RedirectResult)?.Url;
            //Assert
            Assert.IsInstanceOf<RedirectResult>(result);
            Assert.AreEqual(expectedUrl, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ConfirmingEmail_GreetingsEmailError_ReturnBadRequest()
        {
            //Arrange
            var (mockAuthService,
                _,
                _,
                mockAuthEmailService,
                _,
                AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";
            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthEmailService
                .Setup(s => s.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mockAuthEmailService
                .Setup(x => x.SendEmailGreetingAsync(It.IsAny<string>())).ReturnsAsync(false);
            var memoryConfig = new Dictionary<string, string>
            {
                {"URLs:SignIn", "some url"}
            };
            ConfigSettingLayoutRenderer.DefaultConfiguration = new ConfigurationBuilder().AddInMemoryCollection(memoryConfig).Build();

            //Act
            var result = await AuthController.ConfirmingEmailAsync(userId, token);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ConfirmingEmail_Invalid_ConfirmEmailAsyncReturnsFailed_Test()
        {
            //Arrange
            var (mockAuthService,
                _,
                _,
                mockAuthEmailService,
                _,
                AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthEmailService
                .Setup(s => s.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(null));

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ConfirmingEmailAsync(userId, token);
            var actual = (result as BadRequestResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ConfirmingEmail_Invalid_FindByIdAsyncReturnsNull_Test()
        {
            // Arrange
            var (_,
                _,
                _,
                _,
                _,
                AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ConfirmingEmailAsync(userId, token);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ConfirmingEmail_Invalid_TokenIsNull_Test()
        {
            // Arrange
            var (_,
                _,
                _,
                _,
                _,
                AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "";

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ConfirmingEmailAsync(userId, token);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ConfirmingEmail_Invalid_TotalTimeGreaterThan180_Test()
        {
            // Arrange
            var (mockAuthService,
                _,
                mockResources,
                _,
                _,
                AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";
            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthService
                .Setup(s => s.GetTimeAfterRegister(It.IsAny<UserDTO>()))
                .Returns(1441);
            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetConfirmedEmailNotAllowedMessage());

            // Act
            var expectedCode = StatusCodes.Status200OK;
            var expectedMessage = GetConfirmedEmailNotAllowedMessage();
            var result = await AuthController.ConfirmingEmailAsync(userId, token);
            var actualCode = (result as OkObjectResult).StatusCode;
            var actualMessage = (result as OkObjectResult).Value;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedCode, actualCode);
            Assert.AreEqual(expectedMessage.ToString(), actualMessage.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task ConfirmingEmail_Invalid_userIdIsNull_Test()
        {
            //Arrange
            var (mockAuthService,
                _,
                _,
                mockAuthEmailService,
                _,
                authController) = CreateAuthController();
            string userId = null;
            string token = null;

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthEmailService
                .Setup(s => s.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(null));

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await authController.ConfirmingEmailAsync(userId, token);
            var actual = (result as BadRequestResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        public (
            Mock<IAuthService>,
            Mock<IUserDatesService>,
            Mock<IResources>,
            Mock<IAuthEmailService>,
            Mock<IEmailSendingService>,
            AuthController
            ) CreateAuthController()
        {
            Mock<IAuthService> mockAuthService = new Mock<IAuthService>();
            Mock<IUserDatesService> mockUserDataServices = new Mock<IUserDatesService>();
            Mock<IResources> mockResources = new Mock<IResources>();
            Mock<IAuthEmailService> mockAuthEmailService = new Mock<IAuthEmailService>();
            Mock<ILoggerService<AuthController>> mockLoggerService = new Mock<ILoggerService<AuthController>>();
            Mock<IEmailSendingService> emailSendingServiceMock = new Mock<IEmailSendingService>();

            AuthController AuthController = new AuthController(
                mockAuthService.Object,
                mockUserDataServices.Object,
                mockResources.Object,
                mockAuthEmailService.Object,
                mockLoggerService.Object,
                emailSendingServiceMock.Object
                );

            return (
                mockAuthService,
                mockUserDataServices,
                mockResources,
                mockAuthEmailService,
                emailSendingServiceMock,
                AuthController
            );
        }

        [Test]
        public async Task Register_InValid_SMTPEr_Test()
        {
            // Arrange
            var (mockAuthService,
                _,
                mockResources,
                mockAuthEmailService,
                _,
                authController) = CreateAuthController();

            RegisterDto registerDto = new RegisterDto();
            mockAuthService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(IdentityResult.Success);
            mockAuthService
                .Setup(s => s.AddRoleAndTokenAsync(It.IsAny<string>()))
                .ReturnsAsync("token");
            mockResources
                .Setup(s => s.ResourceForErrors["Confirm-Registration"])
                .Returns(GetConfirmRegistration());
            mockResources
                .Setup(s => s.ResourceForErrors["Register-SMTPServerError"])
                .Returns(GetRegisterSMTPError());

            var queueStuff = new Queue<UserDTO>();
            queueStuff.Enqueue(null);
            queueStuff.Enqueue(new UserDTO());
            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(queueStuff.Dequeue);
            mockAuthEmailService
                .Setup(s => s.SendEmailRegistrAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("callbackUrl")
                .Verifiable();
            authController.Url = mockUrlHelper.Object;
            authController.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await authController.Register(registerDto);
            var actual = (result as BadRequestObjectResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Register_Valid_Test()
        {
            // Arrange
            var (mockAuthService,
                _,
                mockResources,
                mockAuthEmailService,
                _,
                AuthController) = CreateAuthController();

            RegisterDto registerDto = new RegisterDto();
            mockAuthService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(IdentityResult.Success);
            mockAuthService
                .Setup(s => s.AddRoleAndTokenAsync(It.IsAny<string>()))
                .ReturnsAsync("token");
            mockResources
                .Setup(s => s.ResourceForErrors["Confirm-Registration"])
                .Returns(GetConfirmRegistration());

            var queueStuff = new Queue<UserDTO>();
            queueStuff.Enqueue(null);
            queueStuff.Enqueue(new UserDTO());
            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(queueStuff.Dequeue);
            mockAuthEmailService
                .Setup(s => s.SendEmailRegistrAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("callbackUrl")
                .Verifiable();
            AuthController.Url = mockUrlHelper.Object;
            AuthController.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var expected = StatusCodes.Status200OK;
            var result = await AuthController.Register(registerDto);
            var actual = (result as OkObjectResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ResendEmailForRegistering_Invalid_FindByIdAsyncReturnsNull_Test()
        {
            // Arrange
            var (_,
                _,
                _,
                _,
                _,
                AuthController) = CreateAuthController();
            string userId = "userId";

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ResendEmailForRegistering(userId);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ResendEmailForRegistering_Valid_Test()
        {
            // Arrange
            var (mockAuthService,
                _,
                mockResources,
                mockAuthEmailService,
                _,
                AuthController) = CreateAuthController();
            string userId = "userId";
            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthEmailService
                .Setup(s => s.SendEmailRegistrAsync(It.IsAny<string>()));
            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);

            // Act
            var expected = StatusCodes.Status200OK;
            var result = await AuthController.ResendEmailForRegistering(userId);
            var actual = (result as OkObjectResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task SendContacts_Invalid_ModelStateIsNotValid_Test()
        {
            //Arrange
            var (_,
                _,
                _,
                _,
                _,
                AuthController) = CreateAuthController();
            FeedbackDto contactsDto = new FeedbackDto();
            AuthController.ModelState.AddModelError("Test", "failed");

            //Act
            var result = await AuthController.Feedback(contactsDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task SendContacts_Valid_Test()
        {
            //Arrange
            var (_,
                _,
                _,
                _,
                emailSendingService,
                AuthController) = CreateAuthController();

            FeedbackDto contactsDto = new FeedbackDto();
            emailSendingService.Setup(e => e.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ));

            //Act
            var result = await AuthController.Feedback(contactsDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Test_RegisterPost_ModelIsNotValid()
        {
            //Arrange
            var (_,
                _,
                mockResources,
                _,
                _,
                AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("NameError", "Required");

            mockResources
                .Setup(s => s.ResourceForErrors["Register-InCorrectData"])
                .Returns(GetRegisterInCorrectData());

            //Act
            var result = await AuthController.Register(GetTestRegisterDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetRegisterInCorrectData().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_RegisterPost_RegisterInCorrectPassword()
        {
            //Arrange
            var (mockAuthService,
                _,
                mockResources,
                _,
                _,
                AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockAuthService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(IdentityResult.Failed(null));

            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetRegisterInCorrectPassword());

            //Act
            var result = await AuthController.Register(GetTestRegisterDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetRegisterInCorrectPassword().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_RegisterPost_RegisterRegisteredUserExists()
        {
            //Arrange
            var (mockAuthService,
                _,
                mockResources,
                _,
                _,
                AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFieldsEmailConfirmed());

            mockResources
                .Setup(s => s.ResourceForErrors["Register-RegisteredUserExists"])
                .Returns(GetRegisterRegisteredUser());

            //Act
            var result = await AuthController.Register(GetTestRegisterDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetRegisterRegisteredUser().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_RegisterPost_RegisterRegisteredUser()
        {
            //Arrange
            var (mockAuthService,
                _,
                mockResources,
                _,
                _,
                AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFieldsEmailNotConfirmed());

            mockResources
                .Setup(s => s.ResourceForErrors["Register-RegisteredUser"])
                .Returns(GetRegisterRegisteredUser());

            //Act
            var result = await AuthController.Register(GetTestRegisterDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetRegisterRegisteredUser().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        private LocalizedString GetConfirmedEmailNotAllowedMessage()
        {
            var localizedString = new LocalizedString("ConfirmedEmailNotAllowed",
                "Час очікування підтвердження електронної пошти вийшов");
            return localizedString;
        }

        private LocalizedString GetConfirmRegistration()
        {
            var localizedString = new LocalizedString("Confirm-Registration",
                "Перегляньте електронну пошту та підтвердьте реєстрацію у системі.");
            return localizedString;
        }

        private LocalizedString GetRegisterInCorrectData()
        {
            var localizedString = new LocalizedString("Register-InCorrectData",
                "Дані введені неправильно");
            return localizedString;
        }

        private LocalizedString GetRegisterInCorrectPassword()
        {
            var localizedString = new LocalizedString("Register-InCorrectPassword",
                "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8");
            return localizedString;
        }

        private LocalizedString GetRegisterRegisteredUser()
        {
            var localizedString = new LocalizedString("Register-RegisteredUser",
                "Користувач з введеною електронною поштою вже зареєстрований в системі, " +
                "можливо він не підтвердив свою реєстрацію");
            return localizedString;
        }

        private LocalizedString GetRegisterSMTPError()
        {
            var localizedString = new LocalizedString("Register-SMTPServerError",
                "Помилка поштового сервера");
            return localizedString;
        }

        private RegisterDto GetTestRegisterDto()
        {
            var registerDto = new RegisterDto
            {
                Email = "andriishainoha@gmail.com",
                Name = "Andrii",
                SurName = "Shainoha",
                Password = "andrii123",
                ConfirmPassword = "andrii123"
            };
            return registerDto;
        }

        private UserDTO GetTestUserDtoWithAllFields()
        {
            return new UserDTO()
            {
                UserName = "andriishainoha@gmail.com",
                FirstName = "Andrii",
                LastName = "Shainoha",
                EmailConfirmed = true,
                SocialNetworking = true
            };
        }

        private UserDTO GetTestUserDtoWithAllFieldsEmailConfirmed()
        {
            return new UserDTO()
            {
                UserName = "yurii@gmail.com",
                FirstName = "Yurii",
                LastName = "Ivanov",
                EmailConfirmed = true,
                SocialNetworking = true
            };
        }

        private UserDTO GetTestUserDtoWithAllFieldsEmailNotConfirmed()
        {
            return new UserDTO()
            {
                UserName = "yurii@gmail.com",
                FirstName = "Yurii",
                LastName = "Ivanov",
                EmailConfirmed = false,
                SocialNetworking = true
            };
        }
    }
}
