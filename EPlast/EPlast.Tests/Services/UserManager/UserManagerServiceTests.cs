using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.UserManager
{
    [TestFixture]
    public class UserManagerServiceTests
    {
        private UserManagerService _userManagerService;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private List<string> _roles;
        private UserDto _userDTO;
        private Mock<ILoggerService<UserManagerService>> _mockLoggerService;

        [SetUp]
        public void SetUp()
        {
            _roles = new List<string>();
            _userDTO = new UserDto();
            _mockMapper = new Mock<IMapper>();
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _mockLoggerService = new Mock<ILoggerService<UserManagerService>>();
            _userManagerService = new UserManagerService(_mockUserManager.Object, _mockMapper.Object, _mockLoggerService.Object);
        }

        [Test]
        public async Task IsInRoleAsync_Valid_TestAsync()
        {
            //Arange
            string[] roles = new string[1];
            _mockUserManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            
            //Act
            var result = await _userManagerService.IsInRoleAsync(_userDTO, roles);
            
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsInRoleAsync_InValid_TestAsync()
        {
            //Arange
            string[] roles = new string[1];
            _mockUserManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            
            //Act
            var result = await _userManagerService.IsInRoleAsync(_userDTO, roles);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetRolesAsync_Valid_GetListOfRolesAsync()
        {
            //Arrange
            _mockUserManager
                 .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                 .ReturnsAsync(_roles);

            //Act
            var result = await _userManagerService.GetRolesAsync(_userDTO);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task FindByIdAsync_Valid_GetUserTDOAsync()
        {
            //Arrange
            string userId = "id";
            _mockMapper
                .Setup(x => x.Map<User, UserDto>(It.IsAny<User>()))
                .Returns(_userDTO);

            //Act
            var result = await _userManagerService.FindByIdAsync(userId);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetCurrentUserAsync_ReturnsExistingUser()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            //Act
            var result = await _userManagerService.GetCurrentUserAsync(It.IsAny<ClaimsPrincipal>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<User>(result);
        }

        [Test]
        public async Task GetCurrentUserAsync_ThrowsExceptionReturnsNull()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new ArgumentNullException());
            _mockLoggerService
                .Setup(x => x.LogError(It.IsAny<string>()));

            //Act
            var result = await _userManagerService.GetCurrentUserAsync(It.IsAny<ClaimsPrincipal>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetCurrentUserId_ReturnsUserId()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("ValidIdOfUser");

            //Act
            var result = _userManagerService.GetCurrentUserId(It.IsAny<ClaimsPrincipal>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
        }

        [Test]
        public void GetCurrentUserId_ThrowsExceptionReturnsNull()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Throws(new ArgumentNullException());
            _mockLoggerService
                .Setup(x => x.LogError(It.IsAny<string>()));

            //Act
            var result = _userManagerService.GetCurrentUserId(It.IsAny<ClaimsPrincipal>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserRolesAsync_ReturnsUserRoles()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(GetRoles());

            //Act
            var result = await _userManagerService.GetUserRolesAsync(It.IsAny<User>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<string>>(result);
        }

        [Test]
        public async Task GetUserRolesAsync_ThrowsExceptionReturnsNull()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ThrowsAsync(new ArgumentNullException());
            _mockLoggerService
                .Setup(x => x.LogError(It.IsAny<string>()));

            //Act
            var result = await _userManagerService.GetUserRolesAsync(It.IsAny<User>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task FindUserByIdAsync_ReturnsValidUser()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());

            //Act
            var result = await _userManagerService.FindUserByIdAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<User>(result);
        }

        [Test]
        public async Task FindUserByIdAsync_ThrowsExceptionReturnsNull()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());
            _mockLoggerService
                .Setup(x => x.LogError(It.IsAny<string>()));

            //Act
            var result = await _userManagerService.FindUserByIdAsync(It.IsAny<string>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task FindUserByEmailAsync_ReturnsValidUser()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());

            //Act
            var result = await _userManagerService.FindUserByEmailAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<User>(result);
        }

        [Test]
        public async Task FindUserByEmailAsync_ThrowsExceptionReturnsNull()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentNullException());
            _mockLoggerService
                .Setup(x => x.LogError(It.IsAny<string>()));

            //Act
            var result = await _userManagerService.FindUserByEmailAsync(It.IsAny<string>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task IsUserInRoleAsync_ReturnsBool()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(new bool());

            //Act
            var result = await _userManagerService.IsUserInRoleAsync(It.IsAny<User>(), It.IsAny<string>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task IsUserInRoleAsync_ThrowsExceptionReturnsFalse()
        {
            //Arrange
            _mockUserManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ThrowsAsync(new ArgumentNullException());
            _mockLoggerService
                .Setup(x => x.LogError(It.IsAny<string>()));

            //Act
            var result = await _userManagerService.IsUserInRoleAsync(It.IsAny<User>(), It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        private static List<string> GetRoles()
        {
            return new List<string>
            {
                "Role1",
                "Role2"
            };
        }
    }
}
