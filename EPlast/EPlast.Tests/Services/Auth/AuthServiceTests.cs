﻿using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;

namespace EPlast.Tests.Services
{
    internal class AuthServiceTests
    {
        private AuthService _authService;
        private Mock<IHttpContextAccessor> _contextAccessor;
        private Mock<IEmailSendingService> _emailSendingService;
        private Mock<IEmailContentService> _mockEmailContentService;
        private Mock<IMapper> _mapper;
        private Mock<IUserClaimsPrincipalFactory<User>> _principalFactory;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<SignInManager<User>> _signInManager;
        private Mock<UserManager<User>> _userManager;

        [Test]
        public void CheckingForLocking_Valid_Test()
        {
            //Arrange
            _userManager
                .Setup(x => x.IsLockedOutAsync(It.IsAny<User>()))
                .ReturnsAsync(true);
            _userManager
                .Setup(x => x.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsNotNull<DateTimeOffset>()));

            //Act
            var result = _authService.CheckingForLocking(new UserDTO());

            //Assert
            _userManager.Verify();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task FindByIdAsync_Valid_TestAsync()
        {
            //Arrange
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());

            //Act
            var result = await _authService.FindByIdAsync("id");

            //Assert
            _userManager.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(UserDTO), result);
        }

        [Test]
        public async Task GetAuthSchemesAsync_Valid_TestAsync()
        {
            //Arrange
            var items = new AuthenticationScheme[] { }.AsQueryable();

            _signInManager
                .Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(items);

            //Act
            var result = await _authService.GetAuthSchemesAsync();

            //Assert
            _userManager.Verify();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetIdForUserAsync_Valid_TestAsync()
        {
            //Arrange
            _userManager
                .Setup(u => u.GetUserIdAsync(It.IsAny<User>()))
                .ReturnsAsync("1");

            //Act
            var result = await _authService.GetIdForUserAsync(GetTestUserWithAllFields());

            //Assert
            _userManager.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(string), result);
        }

        [Test]
        public void GetUser_Valid_Test()
        {
            //Arrange

            //Act
            var result = _authService.GetUser(GetTestUserWithAllFields());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(UserDTO), result);
        }

        [Test]
        public async Task RefreshSignInAsync_InValid_Except_Test()
        {
            //Arrange
            _signInManager
                .Setup(x => x.RefreshSignInAsync(It.IsAny<User>()))
                .Throws(new Exception());

            //Act
            var result = await _authService.RefreshSignInAsync(new UserDTO());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task RefreshSignInAsync_Valid_Test()
        {
            //Arrange
            _signInManager
                .Setup(x => x.RefreshSignInAsync(It.IsAny<User>()));

            //Act
            var result = await _authService.RefreshSignInAsync(new UserDTO());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void GetGoogleUserAsync_Valid()
        {
            //Arrange
            var memoryConfig = new Dictionary<string, string>();
            memoryConfig["Mode"] = "Test";
            ConfigSettingLayoutRenderer.DefaultConfiguration = new ConfigurationBuilder().AddInMemoryCollection(memoryConfig).Build();
            var layoutRenderer = new ConfigSettingLayoutRenderer { Item = "Mode" };

            //Act
            var result =  _authService.GetGoogleUserAsync("providerToken");

            //Assert
            Assert.IsNotNull(result);
        }

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _contextAccessor = new Mock<IHttpContextAccessor>();
            _principalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _signInManager = new Mock<SignInManager<User>>(_userManager.Object,
                           _contextAccessor.Object, _principalFactory.Object, null, null, null, null);
            _emailSendingService = new Mock<IEmailSendingService>();
            _mockEmailContentService = new Mock<IEmailContentService>();
            _mapper = new Mock<IMapper>();
            _mapper
                .Setup(s => s.Map<User, UserDTO>(It.IsAny<User>()))
                .Returns(GetTestUserDtoWithAllFields());
            _mapper
                .Setup(s => s.Map<UserDTO, User>(It.IsAny<UserDTO>()))
                .Returns(GetTestUserWithEmailsSendedTime());
            _repoWrapper = new Mock<IRepositoryWrapper>();

            _authService = new AuthService(_userManager.Object,
                                          _signInManager.Object,
                                          _emailSendingService.Object,
                                          _mockEmailContentService.Object,
                                          _mapper.Object,
                                          _repoWrapper.Object);
        }

        private int GetTestDifferenceInTime()
        {
            return 360;
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
    }
}
