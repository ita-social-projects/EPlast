using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Resources;
using EPlast.DataAccess.Entities;
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
    internal class PasswordControllerTests
    {
        [Test]
        public async Task ChangePassword_InValid_ChangePasswordAsyncFailed_Test()
        {
            //Arrange
            var (_,
                mockAuthService,
                mockResources,
                _,
                passwordController) = CreatePasswordController();
            ChangePasswordDto changePasswordDto = new ChangePasswordDto();
            mockAuthService
                .Setup(s => s.GetUser(It.IsAny<User>()))
                .Returns(new UserDTO());
            mockAuthService
                .Setup(s => s.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<ChangePasswordDto>()))
                .ReturnsAsync(IdentityResult.Failed(null));
            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await passwordController.ChangePassword(changePasswordDto);
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
            var (_,
                _,
                _,
                _,
                passwordController) = CreatePasswordController();
            ChangePasswordDto changePasswordDto = new ChangePasswordDto();

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await passwordController.ChangePassword(changePasswordDto);
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
            var (_,
                _,
                mockResources,
                _,
                passwordController) = CreatePasswordController();
            ChangePasswordDto changePasswordDto = new ChangePasswordDto();
            passwordController.ModelState.AddModelError("NameError", "Required");
            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await passwordController.ChangePassword(changePasswordDto);
            var actual = (result as BadRequestObjectResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ChangePassword_Valid_Test()
        {
            //Arrange
            var (_,
                mockAuthService,
                mockResources,
                _,
                passwordController) = CreatePasswordController();
            ChangePasswordDto changePasswordDto = new ChangePasswordDto();
            mockAuthService
                .Setup(s => s.GetUser(It.IsAny<User>()))
                .Returns(new UserDTO());
            mockAuthService
                .Setup(s => s.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<ChangePasswordDto>()))
                .ReturnsAsync(IdentityResult.Success);
            mockAuthService
                .Setup(s => s.RefreshSignInAsync(It.IsAny<UserDTO>()));
            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);
            mockAuthService
                .Setup(x => x.RefreshSignInAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            //Act
            var expected = StatusCodes.Status200OK;
            var result = await passwordController.ChangePassword(changePasswordDto);
            var actual = (result as OkObjectResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }


        [Test]
        public async Task ChangePassword_BadRequest_refreshResult()
        {
            //Arrange
            var (_,
                mockAuthService,
                mockResources,
                _,
                passwordController) = CreatePasswordController();
            ChangePasswordDto changePasswordDto = new ChangePasswordDto();
            mockAuthService
                .Setup(s => s.GetUser(It.IsAny<User>()))
                .Returns(new UserDTO());
            mockAuthService
                .Setup(s => s.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<ChangePasswordDto>()))
                .ReturnsAsync(IdentityResult.Success);
            mockAuthService
                .Setup(s => s.RefreshSignInAsync(It.IsAny<UserDTO>()));
            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);
            mockAuthService
                .Setup(x => x.RefreshSignInAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(false);

            //Act
            var result = await passwordController.ChangePassword(changePasswordDto);
          

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
            Assert.AreEqual(400,((BadRequestResult)result).StatusCode);
        }

        public (
            Mock<IAuthEmailService>,
            Mock<IAuthService>,
            Mock<IResources>,
            Mock<UserManager<User>>,
            PasswordController
            ) CreatePasswordController()
        {
            Mock<IAuthEmailService> mockAuthEmailService = new Mock<IAuthEmailService>();
            Mock<IAuthService> mockAuthService = new Mock<IAuthService>();
            Mock<IResources> mockResources = new Mock<IResources>();
            var store = new Mock<IUserStore<User>>();
            Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            PasswordController passwordController = new PasswordController(
                mockAuthService.Object,
                mockResources.Object,
                mockAuthEmailService.Object,
                mockUserManager.Object);
            return (
                mockAuthEmailService,
                mockAuthService,
                mockResources,
                mockUserManager,
                passwordController);
        }

        [Test]
        public async Task ResetPasswordGet_Invalid_TokenIsNull_Test()
        {
            //Arrange
            var (_,
                mockAuthService,
                _,
                _,
                passwordController) = CreatePasswordController();
            string userId = "userId";
            string token = null;

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            //Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await passwordController.ResetPassword(userId, token);
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
            var (_,
                mockAuthService,
                mockResources,
                _,
                passwordController) = CreatePasswordController();
            string userId = "userId";
            string token = "token";
            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());
            mockAuthService
                .Setup(s => s.GetTimeAfterReset(It.IsAny<UserDTO>()))
                .Returns(180);

            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()]);

            // Act
            var expectedCode = StatusCodes.Status200OK;
            var result = await passwordController.ResetPassword(userId, token);
            var actual = (result as OkObjectResult).StatusCode;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedCode, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task ResetPasswordGet_Valid_Test()
        {
            //Arrange
            var (_,
                mockAuthService,
                _,
                _,
                passwordController) = CreatePasswordController();
            string userId = "userId";
            string token = "token";

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            //Act
            var expected = StatusCodes.Status200OK;
            var result = await passwordController.ResetPassword(userId, token);
            var actual = (result as OkObjectResult).StatusCode;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ForgotPost_ForgotNotRegisteredUser()
        {
            //Arrange
            var (_,
                mockAuthService,
                mockResources,
                _,
                passwordController) = CreatePasswordController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields().EmailConfirmed);

            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetForgotNotRegisteredUser());

            //Act
            var result = await passwordController.ForgotPassword(GetTestForgotPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetForgotNotRegisteredUser().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ForgotPost_ForgotPasswordConfirmation()
        {
            //Arrange
            var (_,
                mockAuthService,
                mockResources,
                _,
                passwordController) = CreatePasswordController();

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

            passwordController.Url = mockUrlHelper.Object;
            passwordController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetForgotPasswordConfirmation());

            //Act
            var result = await passwordController.ForgotPassword(GetTestForgotPasswordDto()) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(GetForgotPasswordConfirmation().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ForgotPost_ModelIsNotValid()
        {
            //Arrange
            var (_,
                _,
                mockResources,
                _,
                passwordController) = CreatePasswordController();
            passwordController.ModelState.AddModelError("NameError", "Required");

            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetModelIsNotValid());

            //Act
            var result = await passwordController.ForgotPassword(GetTestForgotPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetModelIsNotValid().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ModelIsNotValid()
        {
            //Arrange
            var (_,
                _,
                mockResources,
                _,
                passwordController) = CreatePasswordController();
            passwordController.ModelState.AddModelError("NameError", "Required");

            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetModelIsNotValid());

            //Act
            var result = await passwordController.ResetPassword(GetTestResetPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetModelIsNotValid().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ResetNotRegisteredUser()
        {
            //Arrange
            var (_,
                mockAuthService,
                mockResources,
                _,
                passwordController) = CreatePasswordController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetResetNotRegisteredUser());

            //Act
            var result = await passwordController.ResetPassword(GetTestResetPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetResetNotRegisteredUser().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ResetPasswordConfirmation()
        {
            //Arrange
            var (_,
                mockAuthService,
                mockResources,
                mockUserManager,
                passwordController) = CreatePasswordController();

            mockAuthService
               .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
               .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<ResetPasswordDto>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockAuthService
                .Setup(s => s.CheckingForLocking(It.IsAny<UserDTO>()))
                .Verifiable();

            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetResetPasswordConfirmation());

            //Act
            var result = await passwordController.ResetPassword(GetTestResetPasswordDto()) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(GetResetPasswordConfirmation().ToString(), result.Value.ToString());
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_ResetPost_ResetPasswordProblems()
        {
            //Arrange
            var (_,
                mockAuthService,
                mockResources,
                _,
                passwordController) = CreatePasswordController();

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<ResetPasswordDto>()))
                .Returns(Task.FromResult(IdentityResult.Failed(null)));

            mockResources
                .Setup(s => s.ResourceForErrors[It.IsAny<string>()])
                .Returns(GetResetPasswordProblems());

            //Act
            var result = await passwordController.ResetPassword(GetTestResetPasswordDto()) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(GetResetPasswordProblems().ToString(), result.Value.ToString());
            Assert.NotNull(result);
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
                "Для скидання пароля, перейдіть за посиланням в листі, яке відправлене на вашу електронну пошту.");
            return localizedString;
        }

        private LocalizedString GetModelIsNotValid()
        {
            var localizedString = new LocalizedString("ModelIsNotValid",
                "Введені дані є неправильними");
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
                "Проблеми зі скиданням пароля або введений новий пароль повинен вміщати 8 символів, " +
                "включаючи літери та цифри");
            return localizedString;
        }

        private string GetTestCodeForResetPasswordAndConfirmEmail()
        {
            return new string("500");
        }

        private ForgotPasswordDto GetTestForgotPasswordDto()
        {
            var forgotpasswordDto = new ForgotPasswordDto
            {
                Email = "andriishainoha@gmail.com"
            };
            return forgotpasswordDto;
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

        private UserDTO GetTestUserWithEmailConfirmed()
        {
            return new UserDTO()
            {
                EmailConfirmed = true
            };
        }
    }
}
