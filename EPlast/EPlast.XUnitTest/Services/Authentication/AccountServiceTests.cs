﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.HostURL;
using EPlast.BLL.Services;
using EPlast.BLL.Services.Auth;
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
using Xunit;

namespace EPlast.XUnitTest.Services
{
    public class AuthServiceTests
    {
        public (Mock<SignInManager<User>>, Mock<UserManager<User>>, AuthService) CreateAuthService()
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
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper
               .Setup(s => s.Map<UserDto, User>(It.IsAny<UserDto>()))
               .Returns(GetTestUserWithEmailsSendedTime());

            AuthService AuthService = new AuthService(
                mockUserManager.Object,
                mockSignInManager.Object,
                mockMapper.Object,
                mockRepositoryWrapper.Object);

            return (mockSignInManager, mockUserManager, AuthService);
        }

        public (
            Mock<IEmailSendingService>,
            Mock<IEmailContentService>,
            Mock<IAuthService>,
            Mock<IHostUrlService>,
            Mock<UserManager<User>>,
            AuthEmailService
            ) CreateAuthEmailService()
        {
            Mock<IEmailSendingService> mockEmailConfirmatioService = new Mock<IEmailSendingService>();
            Mock<IEmailContentService> mockEmailContentService = new Mock<IEmailContentService>();
            Mock<IAuthService> mockAuthSerive = new Mock<IAuthService>();
            var store = new Mock<IUserStore<User>>();
            Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            Mock<IHostUrlService> mockHostUrlService = new Mock<IHostUrlService>();
            AuthEmailService authEmailService = new AuthEmailService(
                    mockEmailConfirmatioService.Object,
                    mockEmailContentService.Object,
                    mockAuthSerive.Object,
                    mockUserManager.Object,
                    mockHostUrlService.Object);
            return (
                mockEmailConfirmatioService,
                mockEmailContentService,
                mockAuthSerive,
                mockHostUrlService,
                mockUserManager,
                authEmailService
                );
        }

        [Fact]
        public async Task TestSignInAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockUserManager
               .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
               .ReturnsAsync(GetTestUserWithAllFields());

            mockSignInManager
                .Setup(s => s.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            //Act
            var result = await AuthService.SignInAsync(GetTestLoginDto());

            //Assert
            var identityResult = Assert.IsType<SignInResult>(result);
            Assert.NotNull(identityResult);
        }

        [Fact]
        public async Task TestCreateAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockUserManager
              .Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
              .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = await AuthService.CreateUserAsync(GetTestRegisterDto());

            //Assert
            var identityResult = Assert.IsType<IdentityResult>(result);
            Assert.NotNull(identityResult);
        }

        [Fact]
        public async Task TestConfirmEmail()
        {
            //Arrange
            var (
                _,
                _,
                _,
                _,
                mockUserManager,
                authEmailService) = CreateAuthEmailService();
            mockUserManager
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
              .Setup(s => s.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()))
              .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = await authEmailService.ConfirmEmailAsync(GetTestCode(), GetTestCode());

            //Assert
            var identityResult = Assert.IsType<IdentityResult>(result);
            Assert.NotNull(identityResult);
        }

        [Fact]
        public async Task TestChangePassword()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockUserManager
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
              .Setup(s => s.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = await AuthService.ChangePasswordAsync(GetTestCode(), GetTestChangePasswordDto());

            //Assert
            var identityResult = Assert.IsType<IdentityResult>(result);
            Assert.NotNull(identityResult);
        }

        [Fact]
        public void TestGetAuthProperties()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockSignInManager
              .Setup(s => s.ConfigureExternalAuthenticationProperties(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(GetTestAuthenticationProperties());

            //Act
            AuthenticationProperties result = AuthService.GetAuthProperties(GetTestProvider(), GetTestProvider());

            //Assert
            var authResult = Assert.IsType<AuthenticationProperties>(result);
            Assert.NotNull(authResult);
        }

        [Fact]
        public async Task TestInfoAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockSignInManager
              .Setup(s => s.GetExternalLoginInfoAsync(It.IsAny<string>()))
              .ReturnsAsync(GetExternalLoginInfoFake());

            //Act
            ExternalLoginInfo result = await AuthService.GetInfoAsync();

            //Assert
            var authResult = Assert.IsType<ExternalLoginInfo>(result);
            Assert.NotNull(authResult);
        }

        [Fact]
        public async Task TestGetSignInResultAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockSignInManager
              .Setup(s => s.ExternalLoginSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
              .ReturnsAsync(SignInResult.Success);

            //Act
            SignInResult result = await AuthService.GetSignInResultAsync(GetExternalLoginInfoFake());

            //Assert
            var authResult = Assert.IsType<SignInResult>(result);
            Assert.NotNull(authResult);
        }

        [Fact]
        public async Task TestIsEmailConfirmedAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockUserManager
              .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
              .ReturnsAsync(true);

            //Act
            bool result = await AuthService.IsEmailConfirmedAsync(GetTestUserDtoWithAllFields());

            //Assert
            Assert.IsType<bool>(result);
            Assert.True(result);
        }

        [Fact]
        public async Task TestIsEmailConfirmedAsyncFalse()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockUserManager
              .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
              .ReturnsAsync(false);

            //Act
            bool result = await AuthService.IsEmailConfirmedAsync(GetTestUserDtoWithAllFields());

            //Assert
            Assert.IsType<bool>(result);
            Assert.False(result);
        }

        [Fact]
        public async Task TestAddRoleAndTokenAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestCodeForResetPasswordAndConfirmEmail());
            var registerDTO = GetTestRegisterDto();
            //Act
            string token = await AuthService.AddRoleAndTokenAsync(registerDTO.Email);

            //Assert
            Assert.NotNull(token);
            Assert.IsType<string>(token);
        }

        [Fact]
        public async Task TestGenerateConfToken()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockUserManager
                .Setup(s => s.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestCode());

            //Act
            string token = await AuthService.GenerateConfToken(GetTestUserDtoWithAllFields());

            //Assert
            var result = Assert.IsType<string>(token);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestGenerateResetTokenAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockUserManager
                .Setup(s => s.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestCode());

            //Act
            string token = await AuthService.GenerateResetTokenAsync(GetTestUserDtoWithAllFields());

            //Assert
            var result = Assert.IsType<string>(token);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestResetPasswordAsync()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockUserManager
                .Setup(s => s.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            //Act
            var identityResult = await AuthService.ResetPasswordAsync(GetTestCode(), GetTestResetPasswordDto());

            //Assert
            var result = Assert.IsType<IdentityResult>(identityResult);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindByEmailReturnsNull()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            //Act
            var findResult = await AuthService.FindByEmailAsync(GetTestEmail());

            //Assert
            Assert.Null(findResult);
        }

        [Fact]
        public void GetTimeAfterRegistrTest()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();

            //Act
            var result = AuthService.GetTimeAfterRegister(GetTestUserDtoWithEmailsSendedTime());

            //Assert
            Assert.Equal(360, result);
        }

        [Fact]
        public void GetTimeAfterResetTest()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, AuthService) = CreateAuthService();

            //Act
            var result = AuthService.GetTimeAfterReset(GetTestUserDtoWithEmailsSendedTime());

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

        private UserDto GetTestUserDtoWithAllFields()
        {
            return new UserDto()
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
                RememberMe = true
            };
            return loginDto;
        }

        private RegisterDto GetTestRegisterDto()
        {
            var registerDto = new RegisterDto
            {
                FirstName = "Andrii",
                LastName = "Shainoha",
                GenderId = 1,
                Birthday = DateTime.Now.AddYears(-18),
                Address = "вулиця Героїв України",
                PhoneNumber = "+380123456789",
                Email = "andriishainoha@gmail.com",
                Password = "andrii123"
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
            Dictionary<string, string> authenticationDictionary = new Dictionary<string, string>(3)
            {
                { "First", "Google" },
                { "Second", "Facebook" },
                { "Third", "Amazon" }
            };
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

        private User GetTestUserWithEmailsSendedTime()
        {
            var timeEmailSended = DateTime.Now.AddMinutes(-GetTestDifferenceInTime());

            return new User()
            {
                EmailSendedOnForgotPassword = timeEmailSended,
                EmailSendedOnRegister = timeEmailSended
            };
        }

        private UserDto GetTestUserDtoWithEmailsSendedTime()
        {
            var timeEmailSended = DateTime.Now.AddMinutes(-GetTestDifferenceInTime());

            return new UserDto()
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
