using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services
{
    public class AccountServiceTests
    {
        public (Mock<SignInManager<User>>, Mock<UserManager<User>>, Mock<IEmailConfirmation>, AccountService) CreateAccountService()
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
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null);

            Mock<IRepositoryWrapper> mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            Mock<ILogger<AccountController>> mockLogger = new Mock<ILogger<AccountController>>();
            Mock<IEmailConfirmation> mockEmailConfirmation = new Mock<IEmailConfirmation>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper
               .Setup(s => s.Map<UserDTO, User>(It.IsAny<UserDTO>()))
               .Returns(GetTestUserWithEmailsSendedTime());

            AccountService accountService = new AccountService(mockUserManager.Object, mockSignInManager.Object,
               mockEmailConfirmation.Object, mockMapper.Object, null);

            return (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService);
        }

        [Fact]
        public async Task TestSignInAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
               .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
               .ReturnsAsync(GetTestUserWithAllFields());

            mockSignInManager
                .Setup(s => s.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            //Act
            var result = await accountService.SignInAsync(GetTestLoginDto());

            //Assert
            var identityResult = Assert.IsType<SignInResult>(result);
            Assert.NotNull(identityResult);
        }

        [Fact]
        public async Task TestCreateAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
              .Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
              .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = await accountService.CreateUserAsync(GetTestRegisterDto());

            //Assert
            var identityResult = Assert.IsType<IdentityResult>(result);
            Assert.NotNull(identityResult);
        }

        [Fact]
        public async Task TestConfirmEmail()
        {
            //Arrange
           var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
              .Setup(s => s.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()))
              .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = await accountService.ConfirmEmailAsync(GetTestCode(), GetTestCode());

            //Assert
            var identityResult = Assert.IsType<IdentityResult>(result);
            Assert.NotNull(identityResult);
        }

        [Fact]
        public async Task TestChangePassword()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
              .Setup(s => s.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = await accountService.ChangePasswordAsync(GetTestCode(), GetTestChangePasswordDto());

            //Assert
            var identityResult = Assert.IsType<IdentityResult>(result);
            Assert.NotNull(identityResult);
        }

        [Fact]
        public void TestGetAuthProperties()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockSignInManager
              .Setup(s => s.ConfigureExternalAuthenticationProperties(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(GetTestAuthenticationProperties());

            //Act
            AuthenticationProperties result = accountService.GetAuthProperties(GetTestProvider(), GetTestProvider());

            //Assert
            var authResult = Assert.IsType<AuthenticationProperties>(result);
            Assert.NotNull(authResult);
        }

        [Fact]
        public async Task TestInfoAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockSignInManager
              .Setup(s => s.GetExternalLoginInfoAsync(It.IsAny<string>()))
              .ReturnsAsync(GetExternalLoginInfoFake());

            //Act
            ExternalLoginInfo result = await accountService.GetInfoAsync();

            //Assert
            var authResult = Assert.IsType<ExternalLoginInfo>(result);
            Assert.NotNull(authResult);
        }

        [Fact]
        public async Task TestGetSignInResultAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockSignInManager
              .Setup(s => s.ExternalLoginSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
              .ReturnsAsync(SignInResult.Success);

            //Act
            SignInResult result = await accountService.GetSignInResultAsync(GetExternalLoginInfoFake());

            //Assert
            var authResult = Assert.IsType<SignInResult>(result);
            Assert.NotNull(authResult);
        }

        [Fact]                                               
        public async Task TestIsEmailConfirmedAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
              .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
              .ReturnsAsync(true);

            //Act
            bool result = await accountService.IsEmailConfirmedAsync(GetTestUserDtoWithAllFields());

            //Assert
            var authResult = Assert.IsType<bool>(result);
            Assert.True(result);
        }

        [Fact]
        public async Task TestIsEmailConfirmedAsyncFalse()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
              .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
              .ReturnsAsync(false);

            //Act
            bool result = await accountService.IsEmailConfirmedAsync(GetTestUserDtoWithAllFields());

            //Assert
            var authResult = Assert.IsType<bool>(result);
            Assert.False(result);
        }

        [Fact]
        public async Task TestAddRoleAndTokenAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestCodeForResetPasswordAndConfirmEmail());

            //Act
            string token = await accountService.AddRoleAndTokenAsync(GetTestRegisterDto());

            //Assert
            var result = Assert.IsType<string>(token);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestGenerateConfToken()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestCode());

            //Act
            string token = await accountService.GenerateConfToken(GetTestUserDtoWithAllFields());

            //Assert
            var result = Assert.IsType<string>(token);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestGenerateResetTokenAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestCode());

            //Act
            string token = await accountService.GenerateResetTokenAsync(GetTestUserDtoWithAllFields());

            //Assert
            var result = Assert.IsType<string>(token);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestResetPasswordAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            //Act
            var identityResult = await accountService.ResetPasswordAsync(GetTestCode(), GetTestResetPasswordDto());

            //Assert
            var result = Assert.IsType<IdentityResult>(identityResult);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindByEmailReturnsNull()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            //Act
            var findResult = await accountService.FindByEmailAsync(GetTestEmail());

            //Assert
            Assert.Null(findResult);
        }

        [Fact]
        public void GoogleAuthentication()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            //Act
            var result = accountService.GoogleAuthentication(GetTestEmail(), GetExternalLoginInfoFake());

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GoogleAuthenticationUserNull()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            //Act
            var result = accountService.GoogleAuthentication(GetTestEmail(), GetExternalLoginInfoFake());
            
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetIdForUserTest()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(GetTestIdForUser());

            //Act
            var result = accountService.GetIdForUser(ClaimsPrincipal.Current);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("aaaa-bbbb-cccc", result);
        }

        [Fact]
        public void GetIdForUserTestReturnNull()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();
            mockUserManager
                .Setup(s => s.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns((string)null);

            //Act
            var result = accountService.GetIdForUser(ClaimsPrincipal.Current);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetTimeAfterRegistrTest()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();

            //Act
            var result = accountService.GetTimeAfterRegistr(GetTestUserDtoWithEmailsSendedTime());

            //Assert
            Assert.Equal(360, result);
        }

        [Fact]
        public void GetTimeAfterResetTest()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountService) = CreateAccountService();

            //Act
            var result = accountService.GetTimeAfterReset(GetTestUserDtoWithEmailsSendedTime());

            //Assert
            Assert.Equal(360, result);
        }

        private string GetTestCode()
        {
            return new string("500");
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

        private LoginDto GetTestLoginDto()
        {
            var loginDto = new LoginDto
            {
                Email = "andriishainoha@gmail.com",
                Password = "andrii123",
                RememberMe = true,
                ReturnUrl = "/google.com/"
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

        private ResetPasswordDto GetTestResetPasswordDto()
        {
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = "andriishainoha@gmail.com",
                Password = "andrii123",
                ConfirmPassword = "andrii123",
                Code = "500"
            };
            return resetPasswordDto;
        }

        private ChangePasswordDto GetTestChangePasswordDto()
        {
            var changePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = "password123",
                NewPassword = "newpassword123",
                ConfirmPassword = "newpassword123"
            };
            return changePasswordDto;
        }

        private AuthenticationProperties GetTestAuthenticationProperties()
        {
           Dictionary<string, string> authenticationDictionary = new Dictionary<string, string>(3);
            authenticationDictionary.Add("First", "Google");
            authenticationDictionary.Add("Second", "Facebook");
            authenticationDictionary.Add("Third", "Amazon");
            var authProperties = new AuthenticationProperties(authenticationDictionary);
            return authProperties;
        }

        private string GetTestEmail()
        {
            return new string("andriishainoha@gmail.com");
        }
        private string GetTestProvider()
        {
            return new string("fakeProvider");
        }

        private ExternalLoginInfo GetExternalLoginInfoFake()
        {
            var claims = new List<ClaimsIdentity>();
            var info = new ExternalLoginInfo(new ClaimsPrincipal(claims), "Google", "GoogleExample", "GoogleForDisplay");
            return info;
        }

        private string GetTestCodeForResetPasswordAndConfirmEmail()
        {
           return new string("500");
        }

        private string GetTestIdForUser()
        {
            return "aaaa-bbbb-cccc";
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

        private UserDTO GetTestUserDtoWithEmailsSendedTime()
        {
            IDateTimeHelper dateTimeResetingPassword = new DateTimeHelper();
            var timeEmailSended = dateTimeResetingPassword
                    .GetCurrentTime()
                    .AddMinutes(-GetTestDifferenceInTime());

            return new UserDTO()
            {
                EmailSendedOnForgotPassword = timeEmailSended,
                EmailSendedOnRegister = timeEmailSended
            };
        }

        private int GetTestDifferenceInTime()
        {
            return 360;
        }
    }
}
