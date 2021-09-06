using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Jwt;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Resources;
using EPlast.BLL.Models;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using BadRequestResult = Microsoft.AspNetCore.Mvc.BadRequestResult;

namespace EPlast.Tests.Controllers
{
    public class LoginControllerTests
    {
        public (
            Mock<IAuthService>,
            Mock<IResources>,
            Mock<IJwtService>,
            Mock<ILoggerService<LoginController>>,
            Mock<IUserDatesService>,
            Mock<IUserManagerService>,
            LoginController
            ) CreateLoginController()
        {
            Mock<IAuthService> mockAuthService = new Mock<IAuthService>();
            Mock<IResources> mockResources = new Mock<IResources>();
            Mock<IJwtService> mockJwtService = new Mock<IJwtService>();
            Mock<ILoggerService<LoginController>> mockLoggerService = new Mock<ILoggerService<LoginController>>();
            Mock<IUserDatesService> mockUserDataServices = new Mock<IUserDatesService>();
            Mock<IUserManagerService> mockUserManagerService = new Mock<IUserManagerService>();
            LoginController loginController = new LoginController(
                mockAuthService.Object,
                mockResources.Object,
                mockJwtService.Object,
                mockLoggerService.Object,
                mockUserDataServices.Object,
                mockUserManagerService.Object);

            return (
                mockAuthService,
                mockResources,
                mockJwtService,
                mockLoggerService,
                mockUserDataServices,
                mockUserManagerService,
                loginController
                );
        }

        [Test]
        public async Task FacebookLogin_Inalid_BadRequest_Test()
        {
            // Arrange
            var (_,
                mockResources,
                _,
                mockLoggerService,
                _,
                _,
                loginController) = CreateLoginController();
            var userInfo = GetTestFacebookUserInfoWithSomeFields();

            mockResources
                .Setup(s => s.ResourceForGender[userInfo.Gender])
                .Returns(new LocalizedString(userInfo.Gender, userInfo.Gender));
            mockLoggerService
                .Setup(s => s.LogError(It.IsAny<string>()));

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await loginController.FacebookLogin(userInfo);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            mockResources.Verify();
            mockLoggerService.Verify();
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task FacebookLogin_Inalid_Exception_Test()
        {
            // Arrange
            var (_,
                mockResources,
                _,
                _,
                _,
                _,
                loginController) = CreateLoginController();
            var userInfo = GetTestFacebookUserInfoWithSomeFields();

            mockResources
                .Setup(s => s.ResourceForGender[userInfo.Gender])
                .Throws(new Exception());

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await loginController.FacebookLogin(userInfo);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            mockResources.Verify();
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task FacebookLogin_Valid_Test()
        {
            // Arrange
            var (mockAuthService,
                mockResources,
                _,
                _,
                mockUserDataServices,
                _,
                loginController) = CreateLoginController();
            var userInfo = GetTestFacebookUserInfoWithSomeFields();

            mockResources
                .Setup(s => s.ResourceForGender[userInfo.Gender])
                .Returns(new LocalizedString(userInfo.Gender, userInfo.Gender));
            mockAuthService
                .Setup(s => s.FacebookLoginAsync(userInfo))
                .ReturnsAsync(new UserDTO());
            mockUserDataServices
                .Setup(s => s.AddDateEntryAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var expected = StatusCodes.Status200OK;
            var result = await loginController.FacebookLogin(userInfo);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            mockResources.Verify();
            mockAuthService.Verify();
            mockUserDataServices.Verify();
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task GoogleLogin_Invalid_BadRequest_Test()
        {
            // Arrange
            var (_,
                _,
                _,
                _,
                _,
                _,
                loginController) = CreateLoginController();
            string googleToken = It.IsAny<string>();

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await loginController.GoogleLogin(googleToken);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task GoogleLogin_Invalid_Exception_Test()
        {
            // Arrange
            var (mockAuthService,
                _,
                _,
                mockLoggerService,
                _,
                _,
                loginController) = CreateLoginController();
            string googleToken = It.IsAny<string>();
            mockAuthService
                .Setup(s => s.GetGoogleUserAsync(It.IsAny<string>()))
                .Throws(new Exception());
            mockLoggerService
                .Setup(s => s.LogError(It.IsAny<string>()));

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await loginController.GoogleLogin(googleToken);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            mockAuthService.Verify();
            mockLoggerService.Verify();
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public void Logout_Valid_Test()
        {
            // Arrange
            var (mockAuthService,
                _,
                _,
                _,
                _,
                _,
                loginController) = CreateLoginController();
            mockAuthService
                .Setup(s => s.SignOutAsync());

            // Act
            var expected = StatusCodes.Status200OK;
            var result = loginController.Logout();
            var actual = (result as OkResult).StatusCode;

            // Assert
            mockAuthService.Verify();
            Assert.IsInstanceOf<OkResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_GoogleLogin_Valid()
        {
            // Arrange
            var (mockAuthService,
                _,
                _,
                _,
                mockUserDataServices,
                _,
                loginController) = CreateLoginController();
            string googleToken = It.IsAny<string>();
            mockAuthService
                .Setup(s => s.GetGoogleUserAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields);
            mockUserDataServices
                .Setup(s => s.UserHasMembership(It.IsAny<string>()))
                .ReturnsAsync(true);
            mockUserDataServices
                .Setup(s => s.AddDateEntryAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            mockAuthService.Verify();
            mockUserDataServices.Verify();
            var expected = StatusCodes.Status200OK;
            var result = await loginController.GoogleLogin(googleToken);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_AccountLocked()
        {
            //Arrange
            var (mockAuthService,
                mockResources,
                _,
                _,
                _,
                _,
                loginController) = CreateLoginController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            mockAuthService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            mockResources
                .Setup(s => s.ResourceForErrors["Account-Locked"])
                .Returns(GetAccountLocked());

            //Act
            var result = await loginController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            mockAuthService.Verify();
            mockResources.Verify();
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetAccountLocked().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_EmailNotConfirmed()
        {
            //Arrange
            var (mockAuthService,
                mockResources,
                _,
                _,
                _,
                _,
                loginController) = CreateLoginController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(false);

            mockResources
                .Setup(s => s.ResourceForErrors["Login-NotConfirmed"])
                .Returns(GetLoginNotConfirmed());

            //Act
            var result = await loginController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            mockAuthService.Verify();
            mockResources.Verify();
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetLoginNotConfirmed().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_LoginInCorrectPassword()
        {
            //Arrange
            var (mockAuthService,
                mockResources,
                _,
                _,
                _,
                _,
                loginController) = CreateLoginController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            mockAuthService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            mockResources
                .Setup(s => s.ResourceForErrors["Login-InCorrectPassword"])
                .Returns(GetLoginInCorrectPassword());

            //Act
            var result = await loginController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            mockAuthService.Verify();
            mockResources.Verify();
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetLoginInCorrectPassword().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_ModelIsNotValid()
        {
            //Arrange
            var (_,
                mockResources,
                _,
                _,
                _,
                _,
                loginController) = CreateLoginController();
            loginController.ModelState.AddModelError("NameError", "Required");

            mockResources
                .Setup(s => s.ResourceForErrors["ModelIsNotValid"])
                .Returns(GetModelIsNotValid());

            //Act
            var result = await loginController.Login(GetTestLoginDto()) as ObjectResult;

            //Assert
            mockResources.Verify();
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(GetModelIsNotValid().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_Succeeded()
        {
            //Arrange
            var (mockAuthService,
                _,
                mockJwtService,
                _,
                _,
                _,
                loginController) = CreateLoginController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            mockAuthService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            mockJwtService
                .Setup(s => s.GenerateJWTTokenAsync(GetTestUserDtoWithAllFields()))
                .ReturnsAsync(It.IsAny<string>);

            //Act
            var expected = StatusCodes.Status200OK;
            var result = await loginController.Login(GetTestLoginDto());
            var actual = (result as ObjectResult).StatusCode;

            //Assert
            mockAuthService.Verify();
            mockJwtService.Verify();
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_UserNull()
        {
            //Arrange
            var (mockAuthService,
                mockResources,
                _,
                _,
                _,
                _,
                loginController) = CreateLoginController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockResources
                .Setup(s => s.ResourceForErrors["Login-NotRegistered"])
                .Returns(GetLoginNotRegistered());

            //Act
            var result = await loginController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            mockAuthService.Verify();
            mockResources.Verify();
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetLoginNotRegistered().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        private FacebookUserInfo GetTestFacebookUserInfoWithSomeFields()
        {
            return new FacebookUserInfo()
            {
                Gender = "Male"
            };
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

        private LocalizedString GetAccountLocked()
        {
            var localizedString = new LocalizedString("Account-Locked",
                "Ваш акаунт заблоковано. Повторіть цю дію пізніше, або скиньте свій пароль.");
            return localizedString;
        }

        private LoginDto GetTestLoginDto()
        {
            var loginDto = new LoginDto
            {
                Email = "andriishainoha@gmail.com",
                Password = "andrii123",
                RememberMe = true
            };
            return loginDto;
        }

        private LocalizedString GetLoginNotConfirmed()
        {
            var localizedString = new LocalizedString("Login-NotConfirmed",
                "Ваш акаунт не підтверджений, будь ласка увійдіть та зробіть підтвердження");
            return localizedString;
        }

        private LocalizedString GetLoginInCorrectPassword()
        {
            var localizedString = new LocalizedString("Login-InCorrectPassword",
                "Ви ввели неправильний пароль, спробуйте ще раз");
            return localizedString;
        }

        private LocalizedString GetModelIsNotValid()
        {
            var localizedString = new LocalizedString("ModelIsNotValid",
                "Введені дані є неправильними");
            return localizedString;
        }

        private LocalizedString GetLoginNotRegistered()
        {
            var localizedString = new LocalizedString("Login-NotRegistered",
                "Ви не зареєстровані в системі, або не підтвердили свою електронну пошту");
            return localizedString;
        }
    }
}
