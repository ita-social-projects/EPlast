using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Resources;
using EPlast.BLL.Interfaces.Jwt;
using NLog.Extensions.Logging;
using EPlast.BLL.Interfaces.ActiveMembership;
using System;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Models;
using System.Collections.Generic;

namespace EPlast.Tests.Controllers
{
    public class AuthControllerTestsAuth
    {
        public (
            Mock<IAuthService>, 
            Mock<IJwtService>, 
            Mock<IUserDatesService>, 
            Mock<IHomeService>, 
            Mock<ILoggerService<AuthController>>, 
            Mock<IUserService>, Mock<IResources>, 
            Mock<UserManager<User>>, 
            AuthController
            ) CreateAuthController()
        {
            Mock<IAuthService> mockAuthService = new Mock<IAuthService>();
            Mock<IUserService> mockUserService = new Mock<IUserService>();
            Mock<IResources> mockResources = new Mock<IResources>();
            Mock<IHomeService> mockHomeService = new Mock<IHomeService>();
            Mock<IJwtService> mockJwtService = new Mock<IJwtService>();
            Mock<IUserDatesService> mockUserDataServices = new Mock<IUserDatesService>();
            Mock<ILoggerService<AuthController>> mockLoggerService = new Mock<ILoggerService<AuthController>>();
            var store = new Mock<IUserStore<User>>();
            
            Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            
            AuthController AuthController = new AuthController(mockAuthService.Object, mockLoggerService.Object, mockJwtService.Object, mockHomeService.Object, mockUserDataServices.Object, mockUserManager.Object, mockResources.Object);
            return (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockResources, mockUserManager, AuthController);
        }

        [Test]
        public async Task Test_LoginPost_UserNull()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["Login-NotRegistered"])
                .Returns(GetLoginNotRegistered());

            //Act
            var result = await AuthController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetLoginNotRegistered().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_EmailNotConfirmed()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(false);

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["Login-NotConfirmed"])
                .Returns(GetLoginNotConfirmed());

            //Act
            var result = await AuthController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetLoginNotConfirmed().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_AccountLocked()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            mockAuthService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["Account-Locked"])
                .Returns(GetAccountLocked());

            //Act
            var result = await AuthController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetAccountLocked().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_Succeeded()
        {
            //Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

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
            var result = await AuthController.Login(GetTestLoginDto());
            var actual = (result as ObjectResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(actual, expected);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_LoginInCorrectPassword()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            mockAuthService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["Login-InCorrectPassword"])
                .Returns(GetLoginInCorrectPassword());

            //Act
            var result = await AuthController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetLoginInCorrectPassword().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_ModelIsNotValid()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["ModelIsNotValid"])
                .Returns(GetModelIsNotValid());

            //Act
            var result = await AuthController.Login(GetTestLoginDto()) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(GetModelIsNotValid().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_GoogleLogin_Valid()
        {
            // Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
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
            var expected = StatusCodes.Status200OK;
            var result = await AuthController.GoogleLogin(googleToken);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task GoogleLogin_Invalid_BadRequest_Test()
        {
            // Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            string googleToken = It.IsAny<string>();

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.GoogleLogin(googleToken);
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
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            string googleToken = It.IsAny<string>();
            mockAuthService
                .Setup(s => s.GetGoogleUserAsync(It.IsAny<string>()))
                .Throws(new Exception());
            mockLoggerService
                .Setup(s => s.LogError(It.IsAny<string>()));

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.GoogleLogin(googleToken);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task FacebookLogin_Valid_Test()
        {
            // Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockResources, mockUserManager, AuthController) = CreateAuthController();
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
            var result = await AuthController.FacebookLogin(userInfo);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task FacebookLogin_Inalid_BadRequest_Test()
        {
            // Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockResources, mockUserManager, AuthController) = CreateAuthController();
            var userInfo = GetTestFacebookUserInfoWithSomeFields();

            mockResources
                .Setup(s => s.ResourceForGender[userInfo.Gender])
                .Returns(new LocalizedString(userInfo.Gender, userInfo.Gender));
            mockLoggerService
                .Setup(s => s.LogError(It.IsAny<string>()));

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.FacebookLogin(userInfo);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task FacebookLogin_Inalid_Exception_Test()
        {
            // Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockResources, mockUserManager, AuthController) = CreateAuthController();
            var userInfo = GetTestFacebookUserInfoWithSomeFields();

            mockResources
                .Setup(s => s.ResourceForGender[userInfo.Gender])
                .Throws(new Exception());

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.FacebookLogin(userInfo);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Register_Valid_Test()
        {
            // Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            RegisterDto registerDto = new RegisterDto();
            mockAuthService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(IdentityResult.Success);
            mockAuthService
                .Setup(s => s.AddRoleAndTokenAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync("token");
            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["Confirm-Registration"])
                .Returns(GetConfirmRegistration());

            var queueStuff = new Queue<UserDTO>();
            queueStuff.Enqueue(null); 
            queueStuff.Enqueue(new UserDTO());
            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(queueStuff.Dequeue);

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
        public async Task Test_RegisterPost_ModelIsNotValid()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
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
        public async Task Test_RegisterPost_RegisterRegisteredUser()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["Register-RegisteredUser"])
                .Returns(GetRegisterRegisteredUser());

            //Act
            var result = await AuthController.Register(GetTestRegisterDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetRegisterRegisteredUser().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_RegisterPost_RegisterInCorrectPassword()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockAuthService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(IdentityResult.Failed(null));

            mockStringLocalizer
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
        public async Task ConfirmingEmail_Invalid_FindByIdAsyncReturnsNull_Test()
        {
            // Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ConfirmingEmail(userId, token);
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
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";
            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthService
                .Setup(s => s.GetTimeAfterRegistr(It.IsAny<UserDTO>()))
                .Returns(180);
            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetConfirmedEmailNotAllowedMessage());

            // Act
            var expectedCode = StatusCodes.Status200OK;
            var expectedMessage = GetConfirmedEmailNotAllowedMessage();
            var result = await AuthController.ConfirmingEmail(userId, token);
            var actualCode = (result as OkObjectResult).StatusCode;
            var actualMessage = (result as OkObjectResult).Value;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedCode, actualCode);
            Assert.AreEqual(expectedMessage.ToString(), actualMessage.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task ConfirmingEmail_Invalid_TokenIsNull_Test()
        {
            // Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "";

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ConfirmingEmail(userId, token);
            var actual = (result as BadRequestResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ConfirmingEmail_Inalid_ConfirmEmailAsyncReturnsFailed_Test()
        {
            //Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthService
                .Setup(s => s.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(null));

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ConfirmingEmail(userId, token);
            var actual = (result as BadRequestResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(actual, expected);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ResendEmailForRegistering_Valid_Test()
        {
            // Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockResources, mockUserManager, AuthController) = CreateAuthController();
            string userId = "userId";
            RegisterDto registerDto = new RegisterDto();
            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthService
                .Setup(s => s.GenerateConfToken(It.IsAny<UserDTO>()))
                .ReturnsAsync("token");
            mockAuthService
                .Setup(s => s.SendEmailRegistr(It.IsAny<string>(), It.IsAny<RegisterDto>()));
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("callbackUrl")
                .Verifiable();
            AuthController.Url = mockUrlHelper.Object;
            AuthController.ControllerContext.HttpContext = new DefaultHttpContext();

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
        public async Task ResendEmailForRegistering_Invalid_FindByIdAsyncReturnsNull_Test()
        {
            // Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockResources, mockUserManager, AuthController) = CreateAuthController();
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
        public void Logout_Valid_Test()
        {
            // Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockResources, mockUserManager, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.SignOutAsync());

            // Act
            var expected = StatusCodes.Status200OK;
            var result = AuthController.Logout();
            var actual = (result as OkResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ForgotPost_ModelIsNotValid()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetModelIsNotValid());

            //Act
            var result = await AuthController.ForgotPassword(GetTestForgotPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetModelIsNotValid().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ForgotPost_ForgotNotRegisteredUser()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields().EmailConfirmed);

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetForgotNotRegisteredUser());

            //Act
            var result = await AuthController.ForgotPassword(GetTestForgotPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetForgotNotRegisteredUser().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ForgotPost_ForgotPasswordConfirmation()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetTestUserWithEmailConfirmed().EmailConfirmed);

            mockAuthService
                .Setup(i => i.GenerateResetTokenAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetTestCodeForResetPasswordAndConfirmEmail());

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("callbackUrl")
                .Verifiable();

            AuthController.Url = mockUrlHelper.Object;
            AuthController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetForgotPasswordConfirmation());

            //Act
            var result = await AuthController.ForgotPassword(GetTestForgotPasswordDto()) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(GetForgotPasswordConfirmation().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task ResetPasswordGet_Valid_Test()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            //Act
            var expected = StatusCodes.Status200OK;
            var result = await AuthController.ResetPassword(userId, token);
            var actual = (result as OkObjectResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ResetPasswordGet_Invalid_TokenIsNull_Test()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            string userId = "userId";
            string token = null;

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ResetPassword(userId, token);
            var actual = (result as BadRequestResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ResetPasswordGet_Invalid_TotallTimeGreaterThan180_Test()
        {
            // Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            string userId = "userId";
            string token = "token";
            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthService
                .Setup(s => s.GetTimeAfterReset(It.IsAny<UserDTO>()))
                .Returns(180);

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);

            // Act
            var expectedCode = StatusCodes.Status200OK;
            var result = await AuthController.ResetPassword(userId, token);
            var actual = (result as OkObjectResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedCode, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ResetNotRegisteredUser()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetResetNotRegisteredUser());

            //Act
            var result = await AuthController.ResetPassword(GetTestResetPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetResetNotRegisteredUser().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ResetPasswordConfirmation()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
               .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
               .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<ResetPasswordDto>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockAuthService
                .Setup(s => s.CheckingForLocking(It.IsAny<UserDTO>()))
                .Verifiable();

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetResetPasswordConfirmation());

            //Act
            var result = await AuthController.ResetPassword(GetTestResetPasswordDto()) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(GetResetPasswordConfirmation().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ResetPasswordProblems()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<ResetPasswordDto>()))
                .Returns(Task.FromResult(IdentityResult.Failed(null)));

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetResetPasswordProblems());

            //Act
            var result = await AuthController.ResetPassword(GetTestResetPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetResetPasswordProblems().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ModelIsNotValid()
        {
            //Arrange
            var (mockAuthService, mockJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetModelIsNotValid());

            //Act
            var result = await AuthController.ResetPassword(GetTestResetPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetModelIsNotValid().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task ChangePassword_Valid_Test()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            ChangePasswordDto changePasswordDto = new ChangePasswordDto();
            mockAuthService
                .Setup(s => s.GetUser(It.IsAny<User>()))
                .Returns(new UserDTO());
            mockAuthService
                .Setup(s => s.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<ChangePasswordDto>()))
                .ReturnsAsync(IdentityResult.Success);
            mockAuthService
                .Setup(s => s.RefreshSignInAsync(It.IsAny<UserDTO>()));
            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);

            //Act
            var expected = StatusCodes.Status200OK;
            var result = await AuthController.ChangePassword(changePasswordDto);
            var actual = (result as OkObjectResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ChangePassword_InValid_ChangePasswordAsyncFailed_Test()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            ChangePasswordDto changePasswordDto = new ChangePasswordDto();
            mockAuthService
                .Setup(s => s.GetUser(It.IsAny<User>()))
                .Returns(new UserDTO());
            mockAuthService
                .Setup(s => s.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<ChangePasswordDto>()))
                .ReturnsAsync(IdentityResult.Failed(null));
            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ChangePassword(changePasswordDto);
            var actual = (result as BadRequestObjectResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ChangePassword_InValid_GetUserFailed_Test()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            ChangePasswordDto changePasswordDto = new ChangePasswordDto();
            
            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ChangePassword(changePasswordDto);
            var actual = (result as BadRequestResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ChangePassword_InValid_ModelStateIsNotValid_Test()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            ChangePasswordDto changePasswordDto = new ChangePasswordDto();
            AuthController.ModelState.AddModelError("NameError", "Required");
            mockStringLocalizer
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await AuthController.ChangePassword(changePasswordDto);
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
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            ContactsDto contactsDto = new ContactsDto();
            mockHomeService
                .Setup(s => s.SendEmailAdmin(contactsDto));
            mockStringLocalizer
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
        public async Task SendContacts_Invalid_ModelStateIsNotValid_Test()
        {
            //Arrange
            var (mockAuthService, moskJwtService, mockUserDataServices, mockHomeService, mockLoggerService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            ContactsDto contactsDto = new ContactsDto();
            AuthController.ModelState.AddModelError("NameError", "Required");
            
            mockStringLocalizer
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

        private LocalizedString GetLoginNotRegistered()
        {
            var localizedString = new LocalizedString("Login-NotRegistered",
                "Ви не зареєстровані в системі, або не підтвердили свою електронну пошту");
            return localizedString;
        }

        private LocalizedString GetLoginInCorrectPassword()
        {
            var localizedString = new LocalizedString("Login-InCorrectPassword",
                "Ви ввели неправильний пароль, спробуйте ще раз");
            return localizedString;
        }

        private LocalizedString GetLoginNotConfirmed()
        {
            var localizedString = new LocalizedString("Login-NotConfirmed",
                "Ваш акаунт не підтверджений, будь ласка увійдіть та зробіть підтвердження");
            return localizedString;
        }

        private LocalizedString GetAccountLocked()
        {
            var localizedString = new LocalizedString("Account-Locked",
                "Ваш акаунт заблоковано. Повторіть цю дію пізніше, або скиньте свій пароль.");
            return localizedString;
        }

        private LocalizedString GetModelIsNotValid()
        {
            var localizedString = new LocalizedString("ModelIsNotValid",
                "Введені дані є неправильними");
            return localizedString;
        }

        private LocalizedString GetRegisterInCorrectData()
        {
            var localizedString = new LocalizedString("Register-InCorrectData",
                "Дані введені неправильно");
            return localizedString;
        }

        private LocalizedString GetRegisterRegisteredUser()
        {
            var localizedString = new LocalizedString("Register-RegisteredUser",
                "Користувач з введеною електронною поштою вже зареєстрований в системі, " +
                "можливо він не підтвердив свою реєстрацію");
            return localizedString;
        }

        private LocalizedString GetRegisterInCorrectPassword()
        {
            var localizedString = new LocalizedString("Register-InCorrectPassword",
                "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8");
            return localizedString;
        }

        private LocalizedString GetConfirmRegistration()
        {
            var localizedString = new LocalizedString("Confirm-Registration",
                "Перегляньте електронну адресу та підтвердьте реєстрацію у системі.");
            return localizedString;
        }

        private LocalizedString GetForgotNotRegisteredUser()
        {
            var localizedString = new LocalizedString("Forgot-NotRegisteredUser",
                "Користувача із заданою електронною поштою немає в системі або він не підтвердив свою реєстрацію.");
            return localizedString;
        }

        private LocalizedString GetForgotPasswordConfirmation()
        {
            var localizedString = new LocalizedString("ForgotPasswordConfirmation",
                "Для скидування пароля, перейдіть за посиланням в листі, яке відправлене на вашу електронну пошту.");
            return localizedString;
        }

        private LocalizedString GetResetNotRegisteredUser()
        {
            var localizedString = new LocalizedString("Reset-NotRegisteredUser",
                "Користувача із заданою електронною поштою немає в системі або він не підтвердив свою реєстрацію");
            return localizedString;
        }

        private LocalizedString GetResetPasswordConfirmation()
        {
            var localizedString = new LocalizedString("ResetPasswordConfirmation",
                "Ваш пароль успішно скинутий.");
            return localizedString;
        }

        private LocalizedString GetResetPasswordProblems()
        {
            var localizedString = new LocalizedString("Reset-PasswordProblems",
                "Проблеми зі скидуванням пароля або введений новий пароль повинен вміщати 8символів, " +
                "включаючи літери та цифри");
            return localizedString;
        }

        private LocalizedString GetConfirmedEmailNotAllowedMessage()
        {
            var localizedString = new LocalizedString("ConfirmedEmailNotAllowed",
                "Час очікування підтвердження електронної пошти вийшов");
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

        private ForgotPasswordDto GetTestForgotPasswordDto()
        {
            var forgotpasswordDto = new ForgotPasswordDto
            {
                Email = "andriishainoha@gmail.com"
            };
            return forgotpasswordDto;
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

        private FacebookUserInfo GetTestFacebookUserInfoWithSomeFields()
        {
            return new FacebookUserInfo()
            {
                Gender = "Male"
            };
        }

        private string GetTestCodeForResetPasswordAndConfirmEmail()
        {
            return new string("500");
        }

        private UserDTO GetTestUserWithEmailConfirmed()
        {
            return new UserDTO()
            {
                EmailConfirmed = true
            };
        }

        private ResetPasswordDto GetTestResetPasswordDto()
        {
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = "andriishainoha@gmail.com",
                Password = "andrii123",
                ConfirmPassword = "andrii123"
            };
            return resetPasswordDto;
        }
    }
}