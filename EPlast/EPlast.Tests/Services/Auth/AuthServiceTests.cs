using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Models;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Services
{
    internal class AuthServiceTests
    {
        public (Mock<SignInManager<User>>, Mock<UserManager<User>>, Mock<IEmailConfirmation>, AuthService) CreateAuthService()
        {
            Mock<IUserPasswordStore<User>> userPasswordStore = new Mock<IUserPasswordStore<User>>();
            userPasswordStore.Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(IdentityResult.Success));

            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();

            idOptions.SignIn.RequireConfirmedEmail = true;
            idOptions.Password.RequireDigit = true;
            idOptions.Password.RequiredLength = 8;
            idOptions.Password.RequireUppercase = false;
            idOptions.User.RequireUniqueEmail = true;
            idOptions.Password.RequireNonAlphanumeric = false;
            idOptions.Lockout.MaxFailedAccessAttempts = 5;
            idOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<User>>();
            UserValidator<User> validator = new UserValidator<User>();
            userValidators.Add(validator);

            var passValidator = new PasswordValidator<User>();
            var pwdValidators = new List<IPasswordValidator<User>>();
            pwdValidators.Add(passValidator);

            var userStore = new Mock<IUserStore<User>>();
            userStore.Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            var mockUserManager = new Mock<UserManager<User>>(userStore.Object,
                options.Object, new PasswordHasher<User>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<User>>>().Object);

            mockUserManager
                .Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null, null);
            Mock<IRepositoryWrapper> mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            //mockRepositoryWrapper.Object.Gender = new Gender();
            Mock<IEmailConfirmation> mockEmailConfirmation = new Mock<IEmailConfirmation>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper
               .Setup(s => s.Map<UserDTO, User>(It.IsAny<UserDTO>()))
               .Returns(GetTestUserWithEmailsSendedTime());
            mockMapper
                .Setup(s => s.Map<User, UserDTO>(It.IsAny<User>()))
                .Returns(GetTestUserDtoWithAllFields());
            Mock<IUrlHelperFactory> mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            Mock<IActionContextAccessor> mockActionContextAccessor = new Mock<IActionContextAccessor>();
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            AuthService AuthService = new AuthService(
                mockUserManager.Object,
                mockSignInManager.Object,
                mockEmailConfirmation.Object,
                mockMapper.Object,
                mockRepositoryWrapper.Object);

            return (mockSignInManager, mockUserManager, mockEmailConfirmation, AuthService);
        }

        [Test]
        public void CheckingForLocking_Valid_Test()
        {
            //Arrange
            var (mockSignInManager,
                mockUserManager,
                mockEmailConfirmation,
                AuthService) = CreateAuthService();

            mockUserManager
                .Setup(x => x.IsLockedOutAsync(It.IsAny<User>()))
                .ReturnsAsync(true);
            mockUserManager
                .Setup(x => x.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsNotNull<DateTimeOffset>()));

            //Act
            var result = AuthService.CheckingForLocking(new BLL.DTO.UserProfiles.UserDTO());

            //Assert
            mockUserManager.Verify();
            Assert.IsNotNull(result);
        }

        [Test]
        public void FacebookLoginAsync_Valid_Test()
        {
            //Arrange
            var (mockSignInManager,
                mockUserManager,
                mockEmailConfirmation,
                AuthService) = CreateAuthService();

            mockUserManager
               .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
               .ReturnsAsync(GetTestUserWithAllFields());

            //Act
            var result = AuthService.FacebookLoginAsync(new BLL.Models.FacebookUserInfo());

            //Assert
            mockUserManager.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(result.Result);
        }

        //[Test]
        //public void FacebookLoginAsync_Valid_UserIsNull_Test()
        //{
        //    //Arrange
        //    var (mockSignInManager,
        //        mockUserManager,
        //        mockEmailConfirmation,
        //        AuthService) = CreateAuthService();

        // mockUserManager .Setup(s => s.FindByEmailAsync(It.IsAny<string>()));

        // mockUserManager .Setup(s => s.CreateAsync(It.IsAny<User>(),
        // It.IsAny<string>())) .Returns(Task.FromResult(IdentityResult.Success));

        // mockEmailConfirmation .Setup(x =>
        // x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(),
        // It.IsAny<string>(), It.IsAny<string>())); //.ReturnsAsync(true);

        // mockUserManager .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), "Прихильник"));

        // mockSignInManager .Setup(s =>
        // s.SignInAsync(GetTestUserWithAllFields(), false, null));

        // Mock<IMapper> mockMapper = new Mock<IMapper>(); mockMapper .Setup(s
        // => s.Map<User, UserDTO>(It.IsAny<User>()))
        // .Returns(GetTestUserDtoWithAllFields()); Mock<IRepositoryWrapper>
        // mockRepoWrapper = new Mock<IRepositoryWrapper>(); //Type a = ; var
        // items = new Gender[] { }.AsQueryable();

        // mockRepoWrapper.Setup(x => x.Gender).Returns(); mockRepoWrapper
        // .Setup(s => s.Gender
        // .FindByCondition(It.IsAny<Expression<Func<Gender, bool>>>()))
        // .Returns(items); Mock<Gender> mockGender = new Mock<Gender>();

        // //.Returns(

        // //Act var result = AuthService.FacebookLoginAsync(new
        // BLL.Models.FacebookUserInfo() { Name = "Name Surname", Email =
        // "email@com.ua", Birthday = "2008-11-01T19:35:00.0000000Z" });

        //    //Assert
        //    mockUserManager.Verify();
        //    Assert.IsNotNull(result);
        //    Assert.IsNull(result.Result);
        //}

        [Test]
        public async Task FindByIdAsync_Valid_TestAsync()
        {
            //Arrange
            var (mockSignInManager,
                mockUserManager,
                mockEmailConfirmation,
                AuthService) = CreateAuthService();

            mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            //Act
            var result = await AuthService.FindByIdAsync("id");

            //Assert
            mockUserManager.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(UserDTO), result);
        }

        [Test]
        public async Task GetAuthSchemesAsync_Valid_TestAsync()
        {
            //Arrange
            var (mockSignInManager,
                mockUserManager,
                mockEmailConfirmation,
                AuthService) = CreateAuthService();
            var items = new AuthenticationScheme[] { }.AsQueryable();

            mockSignInManager
                .Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(items);

            //Act
            var result = await AuthService.GetAuthSchemesAsync();

            //Assert
            mockUserManager.Verify();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetIdForUserAsync_Valid_TestAsync()
        {
            //Arrange
            var (mockSignInManager,
                mockUserManager,
                mockEmailConfirmation,
                AuthService) = CreateAuthService();

            mockUserManager
                .Setup(u => u.GetUserIdAsync(It.IsAny<User>()))
                .ReturnsAsync("1");

            //Act
            var result = await AuthService.GetIdForUserAsync(GetTestUserWithAllFields());

            //Assert
            mockUserManager.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(string), result);
        }

        [Test]
        public void GetUser_Valid_Test()
        {
            //Arrange
            var (mockSignInManager,
                mockUserManager,
                mockEmailConfirmation,
                AuthService) = CreateAuthService();

            //Act
            var result = AuthService.GetUser(GetTestUserWithAllFields());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(UserDTO), result);
        }

        //[Test]
        //public async Task GetGoogleUserAsync_Valid_TestAsync()
        //{
        //    //Arrange
        //    var (mockSignInManager,
        //        mockUserManager,
        //        mockEmailConfirmation,
        //        AuthService) = CreateAuthService();
        //    AuthService

        //    var result = AuthService.GetGoogleUserAsync("token");
        //}

        //[Test]
        //public void SendEmailResetingAsync_Valid_Test()
        //{
        //    //Arrange
        //    _mockUserManager
        //        .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
        //        .ReturnsAsync(new User());
        //    _mockEmailConfirmation
        //        .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //        .ReturnsAsync(true);

        // //Act var result =
        // authEmailService.SendEmailResetingAsync("confirmationLink", new BLL.DTO.Account.ForgotPasswordDto());

        //    //Assert
        //    _mockEmailConfirmation.Verify();
        //    _mockUserManager.Verify();
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void SendEmailReminderAsync_Valid_Test()
        //{
        //    //Arrange
        //    _mockUserManager
        //        .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
        //        .ReturnsAsync(new User());
        //    _

        // //Act var result =
        // authEmailService.SendEmailReminderAsync("citiesUrl", new BLL.DTO.UserProfiles.UserDTO());

        //    //Assert
        //    _mockEmailConfirmation.Verify();
        //    _mockUserManager.Verify();
        //    Assert.IsNotNull(result);
        //}

        //private Mock<IEmailConfirmation> _mockEmailConfirmation;
        //private Mock<IMapper> _mockMapper;
        //private Mock<IRepositoryWrapper> _mockRepoWrapper;
        //private Mock<SignInManager<User>> _mockSignInManager;
        //private Mock<UserManager<User>> _mockUserManager;
        //private AuthService authService;

        private User GetTestUserWithEmailsSendedTime()
        {
            IDateTimeHelper dateTimeResetingPassword = new DateTimeHelper();
            var timeEmailSended = dateTimeResetingPassword
                    .GetCurrentTime()
                    .AddMinutes(-GetTestDifferenceInTime());

            return new User()
            {
                EmailSendedOnForgotPassword = timeEmailSended,
                EmailSendedOnRegister = timeEmailSended
            };
        }

        private int GetTestDifferenceInTime()
        {
            return 360;
        }

        private User GetTestUserWithAllFields()
        {
            return new User()
            {
                UserName = "andriishainoha@gmail.com",
                FirstName = "Andrii",
                LastName = "Shainoha",
                EmailConfirmed = true,
                SocialNetworking = true
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

        private FacebookUserInfo GetTestFacebookUserInfoWithSomeFields()
        {
            return new FacebookUserInfo()
            {
                Gender = "Male"
            };
        }
    }
}
