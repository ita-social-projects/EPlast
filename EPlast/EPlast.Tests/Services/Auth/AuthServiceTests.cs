using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            Mock<IEmailConfirmation> mockEmailConfirmation = new Mock<IEmailConfirmation>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper
               .Setup(s => s.Map<UserDTO, User>(It.IsAny<UserDTO>()))
               .Returns(GetTestUserWithEmailsSendedTime());
            mockMapper
                .Setup(s => s.Map<User, UserDTO>(It.IsAny<User>()))
                .Returns(GetTestUserDtoWithAllFields());

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
        public async Task FacebookLoginAsync_Valid_FindByEmailAsync_Returns_User_TestAsync()
        {
            // Arrange
            var (mockSignInManager,
                mockUserManager,
                mockEmailConfirmation,
                AuthService) = CreateAuthService();
            mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            mockSignInManager
                .Setup(x => x.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<string>()));

            // Act
            var result = await AuthService.FacebookLoginAsync(new BLL.Models.FacebookUserInfo());

            // Assert
            Assert.IsNotNull(result);
        }

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
        public async Task RefreshSignInAsync_Valid_TestAsync()
        {
            //Arrange
            var (mockSignInManager,
                mockUserManager,
                mockEmailConfirmation,
                AuthService) = CreateAuthService();
            mockSignInManager
                .Setup(x => x.RefreshSignInAsync(It.IsAny<User>()));

            //Act
            AuthService.RefreshSignInAsync(new UserDTO());

            //Assert
            mockSignInManager.Verify();
        }

        [Test]
        public async Task SignOutAsync_Valid_TestAsync()
        {
            //Arrange
            var (mockSignInManager,
                mockUserManager,
                mockEmailConfirmation,
                AuthService) = CreateAuthService();
            mockSignInManager
                .Setup(x => x.SignOutAsync());

            //Act
            AuthService.SignOutAsync();

            //Assert
            mockSignInManager.Verify();
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
            var (_,
                _,
                _,
                AuthService) = CreateAuthService();

            //Act
            var result = AuthService.GetUser(GetTestUserWithAllFields());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(UserDTO), result);
        }

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
    }
}
