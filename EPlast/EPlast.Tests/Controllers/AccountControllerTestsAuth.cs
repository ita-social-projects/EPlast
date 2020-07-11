using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
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
                null, null, null, null, null, null, mockMapper.Object, null, mockAccountService.Object, mockStringLocalizer.Object, null);
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
    }
}
