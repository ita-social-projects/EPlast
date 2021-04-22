using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.UserManager
{
    class UserManagerServiceTests
    {
        private UserManagerService _userManagerService;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IMapper> _mockMapper;

        private readonly List<string> _roles = new List<string>();
        private readonly UserDTO _userDTO = new UserDTO();

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
                .Setup(x => x.Map<User, UserDTO>(It.IsAny<User>()))
                .Returns(_userDTO);

            //Act
            var result = await _userManagerService.FindByIdAsync(userId);

            //Assert
            Assert.IsNotNull(result);
        }

        [SetUp]
        public void SetUp()
        {
            _mockMapper = new Mock<IMapper>();
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _userManagerService = new UserManagerService(_mockUserManager.Object, _mockMapper.Object);
        }
    }
}
