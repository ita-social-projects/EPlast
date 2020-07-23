using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.UserProfiles;
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

namespace EPlast.Tests.Controllers
{
    public class AccountControllerTestsAuth
    {
        public (Mock<IAccountService>, Mock<IUserService>, Mock<IMapper>, Mock<IStringLocalizer<AuthenticationErrors>>, AccountController) CreateAccountController()
        {
            Mock<IAccountService> mockAccountService = new Mock<IAccountService>();
            Mock<IUserService> mockUserService = new Mock<IUserService>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            Mock<IStringLocalizer<AuthenticationErrors>> mockStringLocalizer = new Mock<IStringLocalizer<AuthenticationErrors>>();

            AccountController accountController = new AccountController(mockUserService.Object, null, null,
                null, null, null, null, null, null, mockMapper.Object, null, mockAccountService.Object, mockStringLocalizer.Object, null, null);
            return (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController);
        }

        //Login
        [Test]
        public async Task Test_LoginPost_UserNull()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockStringLocalizer
                .Setup(s => s["Login-NotRegistered"])
                .Returns(GetLoginNotRegistered());

            //Act
            var result = await accountController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetLoginNotRegistered().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_EmailNotConfirmed()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAccountService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(false);

            mockStringLocalizer
                .Setup(s => s["Login-NotConfirmed"])
                .Returns(GetLoginNotConfirmed());

            //Act
            var result = await accountController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetLoginNotConfirmed().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_AccountLocked()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAccountService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            mockAccountService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            mockStringLocalizer
                .Setup(s => s["Account-Locked"])
                .Returns(GetAccountLocked());

            //Act
            var result = await accountController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetAccountLocked().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_LoginInCorrectPassword()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAccountService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            mockAccountService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            mockStringLocalizer
                .Setup(s => s["Login-InCorrectPassword"])
                .Returns(GetLoginInCorrectPassword());

            //Act
            var result = await accountController.Login(GetTestLoginDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetLoginInCorrectPassword().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_LoginPost_ModelIsNotValid()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();
            accountController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
                .Setup(s => s["ModelIsNotValid"])
                .Returns(GetModelIsNotValid());

            //Act
            var result = await accountController.Login(GetTestLoginDto()) as ObjectResult;

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
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();
            accountController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
                .Setup(s => s["Register-InCorrectData"])
                .Returns(GetRegisterInCorrectData());

            //Act
            var result = await accountController.Register(GetTestRegisterDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetRegisterInCorrectData().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_RegisterPost_RegisterRegisteredUser()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockStringLocalizer
                .Setup(s => s["Register-RegisteredUser"])
                .Returns(GetRegisterRegisteredUser());

            //Act
            var result = await accountController.Register(GetTestRegisterDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetRegisterRegisteredUser().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_RegisterPost_RegisterInCorrectPassword()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockAccountService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(IdentityResult.Failed(null));

            mockStringLocalizer
                .Setup(s => s["Register-InCorrectPassword"])
                .Returns(GetRegisterInCorrectPassword());

            //Act
            var result = await accountController.Register(GetTestRegisterDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetRegisterInCorrectPassword().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        /*[Test]
        public async Task Test_RegisterPost_ConfirmRegistration()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockAccountService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(IdentityResult.Success);

            mockAccountService
                .Setup(s => s.AddRoleAndTokenAsync(It.IsAny<RegisterDto>()))
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

            accountController.Url = mockUrlHelper.Object;
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockAccountService
                .Setup(s => s.SendEmailRegistr(It.IsAny<string>(), It.IsAny<RegisterDto>()))
                .Verifiable();

            mockStringLocalizer
                .Setup(s => s["Confirm-Registration"])
                .Returns(GetConfirmRegistration());

            //Act
            var result = await accountController.Register(GetTestRegisterDto()) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(GetConfirmRegistration().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }*/

        //ForgotPassword
        [Test]
        public async Task Test_ForgotPost_ModelIsNotValid()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();
            accountController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
                .Setup(s => s["ModelIsNotValid"])
                .Returns(GetModelIsNotValid());

            //Act
            var result = await accountController.ForgotPassword(GetTestForgotPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetModelIsNotValid().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ForgotPost_ForgotNotRegisteredUser()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockAccountService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields().EmailConfirmed);

            mockStringLocalizer
                .Setup(s => s["Forgot-NotRegisteredUser"])
                .Returns(GetForgotNotRegisteredUser());

            //Act
            var result = await accountController.ForgotPassword(GetTestForgotPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetForgotNotRegisteredUser().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ForgotPost_ForgotPasswordConfirmation()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAccountService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetTestUserWithEmailConfirmed().EmailConfirmed);

            mockAccountService
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

            accountController.Url = mockUrlHelper.Object;
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockStringLocalizer
                .Setup(s => s["ForgotPasswordConfirmation"])
                .Returns(GetForgotPasswordConfirmation());

            //Act
            var result = await accountController.ForgotPassword(GetTestForgotPasswordDto()) as ObjectResult;

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
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockStringLocalizer
                .Setup(s => s["Reset-NotRegisteredUser"])
                .Returns(GetResetNotRegisteredUser());

            //Act
            var result = await accountController.ResetPassword(GetTestResetPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetResetNotRegisteredUser().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ResetPasswordConfirmation()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
               .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
               .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAccountService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<ResetPasswordDto>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockAccountService
                .Setup(s => s.CheckingForLocking(It.IsAny<UserDTO>()))
                .Verifiable();

            mockStringLocalizer
                .Setup(s => s["ResetPasswordConfirmation"])
                .Returns(GetResetPasswordConfirmation());

            //Act
            var result = await accountController.ResetPassword(GetTestResetPasswordDto()) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(GetResetPasswordConfirmation().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ResetPasswordProblems()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAccountService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<ResetPasswordDto>()))
                .Returns(Task.FromResult(IdentityResult.Failed(null)));

            mockStringLocalizer
                .Setup(s => s["Reset-PasswordProblems"])
                .Returns(GetResetPasswordProblems());

            //Act
            var result = await accountController.ResetPassword(GetTestResetPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetResetPasswordProblems().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ModelIsNotValid()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, mockStringLocalizer, accountController) = CreateAccountController();
            accountController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
                .Setup(s => s["ModelIsNotValid"])
                .Returns(GetModelIsNotValid());

            //Act
            var result = await accountController.ResetPassword(GetTestResetPasswordDto()) as BadRequestObjectResult;

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
                "Ваш пароль скинутий. Для входу в систему натисніть Увійти/Зареєструватись.");
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
