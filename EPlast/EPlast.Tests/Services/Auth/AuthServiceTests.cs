using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Moq;
using NLog.Extensions.Logging;
using NUnit.Framework;

namespace EPlast.Tests.Services.Auth
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
        private Mock<IGenderRepository> _gender;

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
        public async Task FacebookLoginAsync_IsNotNull()
        {
            //Arrange
            var user = new User();
            var userDto = new UserDTO();
            var userId = Guid.NewGuid().ToString();
            var facebookUser = new FacebookUserInfo()
            {
                Email = "test@test.com",
                UserId = userId,
                Name = "John",
                Birthday = "11.08.2000"
            };
            _userManager.Setup(x => x.FindByEmailAsync(facebookUser.Email))
                .ReturnsAsync(user);
            _mapper.Setup(x => x.Map<User, UserDTO>(user)).Returns(userDto);
            //Act
            var result = await _authService.FacebookLoginAsync(facebookUser);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, userDto);
        }
        [Test]
        public async Task FacebookLoginAsync_IsNull()
        {
            //Arrange
            var user = new User();
            var userDto = new UserDTO();
            var userId = Guid.NewGuid().ToString();
            var facebookUser = new FacebookUserInfo()
            {
                Email = "test@test.com",
                UserId = userId,
                Name = "John Gasiuk",
                Birthday = "11.08.2000",
                Gender = "female"
            };
            _repoWrapper.SetupGet(x => x.Gender).Returns(_gender.Object);
            var genders = new Gender[]
            {
                new Gender()
                {
                    ID = 3
                }
            }.AsQueryable();
            _gender.Setup(x => x.FindByCondition(a => a.Name == facebookUser.Gender))
                .Returns(genders);
            user = new User
            {
                SocialNetworking = true,
                UserName = facebookUser.Email ?? facebookUser.UserId,
                FirstName = facebookUser.Name.Split(' ')[0],
                Email = facebookUser.Email ?? "facebookdefaultmail@gmail.com",
                LastName = facebookUser.Name.Split(' ')[1],
                ImagePath = "default_user_image.png",
                EmailConfirmed = true,
                RegistredOn = DateTime.Now,
                UserProfile = new UserProfile
                {
                    Birthday = DateTime.Parse(facebookUser.Birthday, CultureInfo.InvariantCulture),
                    GenderID = 3
                }
            };
            _userManager.Setup(x => x.FindByEmailAsync(facebookUser.Email))
                .ReturnsAsync((User)null);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "500", Description = "456" }));

            _mapper.Setup(x => x.Map<User, UserDTO>(user)).Returns(userDto);
            //Act
            var result = await _authService.FacebookLoginAsync(facebookUser);
            //Assert
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
            var memoryConfig = new Dictionary<string, string>
            {
                ["Mode"] = "Test"
            };
            ConfigSettingLayoutRenderer.DefaultConfiguration = new ConfigurationBuilder().AddInMemoryCollection(memoryConfig).Build();

            //Act
            var result =  _authService.GetGoogleUserAsync("providerToken");

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteUsers_EmailNotConfirmed()
        {
            //Arrange
            _repoWrapper.Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(FakeListOfUsers());

            //Act
            await _authService.DeleteUserIfEmailNotConfirmedAsync();

            //Assert
            _repoWrapper.Verify();
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

            _authService = new AuthService(
                _userManager.Object,
                _signInManager.Object,
                _emailSendingService.Object,
                _mockEmailContentService.Object,
                _mapper.Object,
                _repoWrapper.Object
            );
            _gender = new Mock<IGenderRepository>();
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
            var timeEmailSended = DateTime.Now
                .AddMinutes(-GetTestDifferenceInTime());

            return new User()
            {
                EmailSendedOnForgotPassword = timeEmailSended,
                EmailSendedOnRegister = timeEmailSended
            };
        }

        private IEnumerable<User> FakeListOfUsers()
        {
            return new List<User>
            {
                new User()
                {
                    FirstName = "Mykola",
                    EmailConfirmed = true,
                    RegistredOn = new DateTime(1999, 09, 05, 09, 09, 09)
                },

                new User()
                {
                    FirstName = "Illia",
                    EmailConfirmed = true,
                    RegistredOn = DateTime.Now
                },

                new User()
                {
                    FirstName = "Liuba",
                    EmailConfirmed = false,
                    RegistredOn = DateTime.Now
                },

                new User()
                {
                    FirstName = "Yurii",
                    EmailConfirmed = false,
                    RegistredOn = new DateTime(1999, 09, 05, 09, 09, 09)
                }
            }.Where(c => c.EmailConfirmed == false);
        }
    }
}
