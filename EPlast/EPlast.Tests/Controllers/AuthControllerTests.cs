using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Resources;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    public class AuthControllerTestsAuth
    {
        [Test]
        public async Task ConfirmingEmail_Inalid_ConfirmEmailAsyncReturnsFailed_Test()
        {
            //Arrange
            var (mockAuthService,
                _,
                _,
                _,
                mockAuthEmailService,
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
        public async Task ConfirmingEmail_Invalid_TotallTimeGreaterThan180_Test()
        {
            // Arrange
            var (mockAuthService,
                _,
                _,
                mockResources,
                _,
                AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";
            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthService
                .Setup(s => s.GetTimeAfterRegistr(It.IsAny<UserDTO>()))
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
                _,
                mockAuthEmailService,
                AuthController) = CreateAuthController();
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
            var result = await AuthController.ConfirmingEmailAsync(userId, token);
            var actual = (result as BadRequestResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        public (
            Mock<IAuthService>,
            Mock<IUserDatesService>,
            Mock<IHomeService>,
            Mock<IResources>,
            Mock<IAuthEmailService>,
            AuthController
            ) CreateAuthController()
        {
            Mock<IAuthService> mockAuthService = new Mock<IAuthService>();
            Mock<IUserDatesService> mockUserDataServices = new Mock<IUserDatesService>();
            Mock<IHomeService> mockHomeService = new Mock<IHomeService>();
            Mock<IResources> mockResources = new Mock<IResources>();
            Mock<IAuthEmailService> mockAuthEmailService = new Mock<IAuthEmailService>();

            AuthController AuthController = new AuthController(
                mockAuthService.Object,
                mockUserDataServices.Object,
                mockHomeService.Object,
                mockResources.Object,
                mockAuthEmailService.Object
                );

            return (
                mockAuthService,
                mockUserDataServices,
                mockHomeService,
                mockResources,
                mockAuthEmailService,
                AuthController);
        }

        [Test]
        public async Task Register_InValid_SMTPEr_Test()
        {
            // Arrange
            var (mockAuthService,
                _,
                _,
                mockResources,
                mockAuthEmailService,
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
            AuthController.Url = mockUrlHelper.Object;
            AuthController.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.Register(registerDto);
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
                _,
                mockResources,
                mockAuthEmailService,
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
                _,
                mockResources,
                mockAuthEmailService,
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
                mockResources,
                _,
                AuthController) = CreateAuthController();
            ContactsDto contactsDto = new ContactsDto();
            AuthController.ModelState.AddModelError("NameError", "Required");

            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.SendContacts(contactsDto);
            var actual = (result as BadRequestObjectResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task SendContacts_Valid_Test()
        {
            //Arrange
            var (_,
                _,
                mockHomeService,
                mockResources,
                _,
                AuthController) = CreateAuthController();
            ContactsDto contactsDto = new ContactsDto();
            mockHomeService
                .Setup(s => s.SendEmailAdmin(contactsDto));
            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);

            //Act
            var expected = StatusCodes.Status200OK;
            var result = await AuthController.SendContacts(contactsDto);
            var actual = (result as OkObjectResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_RegisterPost_ModelIsNotValid()
        {
            //Arrange
            var (_,
                _,
                _,
                mockResources,
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
                _,
                mockResources,
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
        public async Task Test_RegisterPost_RegisterRegisteredUser()
        {
            //Arrange
            var (mockAuthService,
                _,
                _,
                mockResources,
                _,
                AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

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
                "Перегляньте електронну адресу та підтвердьте реєстрацію у системі.");
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
    }
}
