using AutoMapper;
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
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Tests.Services
{
    internal class AuthServiceTests
    {
        private AuthService authService;
        private Mock<IHttpContextAccessor> contextAccessor;
        private Mock<IEmailSendingService> emailSendingService;
        private Mock<IMapper> mapper;
        private Mock<IUserClaimsPrincipalFactory<User>> principalFactory;
        private Mock<IRepositoryWrapper> repoWrapper;
        private Mock<SignInManager<User>> signInManager;
        private Mock<UserManager<User>> userManager;

        [Test]
        public void CheckingForLocking_Valid_Test()
        {
            //Arrange
            userManager
                .Setup(x => x.IsLockedOutAsync(It.IsAny<User>()))
                .ReturnsAsync(true);
            userManager
                .Setup(x => x.SetLockoutEndDateAsync(It.IsAny<User>(), It.IsNotNull<DateTimeOffset>()));

            //Act
            var result = authService.CheckingForLocking(new UserDTO());

            //Assert
            userManager.Verify();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task FindByIdAsync_Valid_TestAsync()
        {
            //Arrange
            userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());

            //Act
            var result = await authService.FindByIdAsync("id");

            //Assert
            userManager.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(UserDTO), result);
        }

        [Test]
        public async Task GetAuthSchemesAsync_Valid_TestAsync()
        {
            //Arrange
            var items = new AuthenticationScheme[] { }.AsQueryable();

            signInManager
                .Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(items);

            //Act
            var result = await authService.GetAuthSchemesAsync();

            //Assert
            userManager.Verify();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetIdForUserAsync_Valid_TestAsync()
        {
            //Arrange
            userManager
                .Setup(u => u.GetUserIdAsync(It.IsAny<User>()))
                .ReturnsAsync("1");

            //Act
            var result = await authService.GetIdForUserAsync(GetTestUserWithAllFields());

            //Assert
            userManager.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(string), result);
        }

        [Test]
        public void GetUser_Valid_Test()
        {
            //Arrange

            //Act
            var result = authService.GetUser(GetTestUserWithAllFields());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(UserDTO), result);
        }

        [Test]
        public async Task RefreshSignInAsync_InValid_Except_Test()
        {
            //Arrange
            signInManager
                .Setup(x => x.RefreshSignInAsync(It.IsAny<User>()))
                .Throws(new Exception());

            //Act
            var result = await authService.RefreshSignInAsync(new UserDTO());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task RefreshSignInAsync_Valid_Test()
        {
            //Arrange
            signInManager
                .Setup(x => x.RefreshSignInAsync(It.IsAny<User>()));

            //Act
            var result = await authService.RefreshSignInAsync(new UserDTO());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            contextAccessor = new Mock<IHttpContextAccessor>();
            principalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            signInManager = new Mock<SignInManager<User>>(userManager.Object,
                           contextAccessor.Object, principalFactory.Object, null, null, null, null);
            emailSendingService = new Mock<IEmailSendingService>();
            mapper = new Mock<IMapper>();
            mapper
                .Setup(s => s.Map<User, UserDTO>(It.IsAny<User>()))
                .Returns(GetTestUserDtoWithAllFields());
            mapper
                .Setup(s => s.Map<UserDTO, User>(It.IsAny<UserDTO>()))
                .Returns(GetTestUserWithEmailsSendedTime());
            repoWrapper = new Mock<IRepositoryWrapper>();

            authService = new AuthService(userManager.Object,
                                          signInManager.Object,
                                          emailSendingService.Object,
                                          mapper.Object,
                                          repoWrapper.Object);
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
