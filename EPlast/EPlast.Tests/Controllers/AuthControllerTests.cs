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

namespace EPlast.Tests.Controllers
{
    public class AuthControllerTestsAuth
    {
        public (Mock<IAuthService>, Mock<IJwtService>, Mock<IUserService>, Mock<IResources>, Mock<UserManager<User>>, AuthController) CreateAuthController()
        {
            Mock<IAuthService> mockAuthService = new Mock<IAuthService>();
            Mock<IUserService> mockUserService = new Mock<IUserService>();
            Mock<IResources> mockResources = new Mock<IResources>();
            Mock<IJwtService> moskJwtService = new Mock<IJwtService>();
            var store = new Mock<IUserStore<User>>();
            Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            AuthController AuthController = new AuthController(mockAuthService.Object, null, moskJwtService.Object, null, null, mockUserManager.Object, mockResources.Object);
            return (mockAuthService, moskJwtService, mockUserService, mockResources, mockUserManager, AuthController);
        }

        //Login
        [Test]
        public async Task Test_LoginPost_UserNull()
        {
            //Arrange
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

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

        //[Test]
        //public void Test_GetGoogleClientId()
        //{
        //    //Arrange
        //    var (mockAuthService, moskJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

        //    //Act
        //    var expected = StatusCodes.Status200OK;
        //    var result = AuthController.GetGoogleClientId();
        //    var actual = (result as StatusCodeResult).StatusCode;

        //    //Assert
        //    Assert.IsInstanceOf<StatusCodeResult>(result);
        //    Assert.AreEqual(actual, expected);
        //    Assert.NotNull(result);
        //}

        [Test]
        public async Task Test_LoginPost_LoginInCorrectPassword()
        {
            //Arrange
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
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

        //Register
        [Test]
        public async Task Test_RegisterPost_ModelIsNotValid()
        {
            //Arrange
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockAuthService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(IdentityResult.Failed(null));

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["Register-InCorrectPassword"])
                .Returns(GetRegisterInCorrectPassword());

            //Act
            var result = await AuthController.Register(GetTestRegisterDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetRegisterInCorrectPassword().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ForgotPost_ModelIsNotValid()
        {
            //Arrange
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["ModelIsNotValid"])
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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields().EmailConfirmed);

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["Forgot-NotRegisteredUser"])
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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

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
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();

            AuthController.Url = mockUrlHelper.Object;
            AuthController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["ForgotPasswordConfirmation"])
                .Returns(GetForgotPasswordConfirmation());

            //Act
            var result = await AuthController.ForgotPassword(GetTestForgotPasswordDto()) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(GetForgotPasswordConfirmation().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        //ResetPassword
        [Test]
        public async Task Test_ResetPost_ResetNotRegisteredUser()
        {
            //Arrange
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["Reset-NotRegisteredUser"])
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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

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
                .Setup(s => s.ResourceForErrors["ResetPasswordConfirmation"])
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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<ResetPasswordDto>()))
                .Returns(Task.FromResult(IdentityResult.Failed(null)));

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["Reset-PasswordProblems"])
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
            var (mockAuthService, mockJwtService, mockUserService, mockStringLocalizer, mockUserManager, AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
                .Setup(s => s.ResourceForErrors["ModelIsNotValid"])
                .Returns(GetModelIsNotValid());

            //Act
            var result = await AuthController.ResetPassword(GetTestResetPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetModelIsNotValid().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        //Fakes
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