using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.UserProfiles
{
    [TestFixture]
    public class UserProfileAccessServiceTests
    {
        private IUserProfileAccessService _userProfileAccessService;
        private Mock<IUserService> _mockUserService;
        private Mock<UserManager<User>> _mockUserManager;

        [SetUp]
        public void SetUp()
        {
            Mock<IUserStore<User>> _mockStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(_mockStore.Object, null, null, null, null, null, null, null, null);
            _mockUserService = new Mock<IUserService>();
            _userProfileAccessService = new UserProfileAccessService(_mockUserService.Object, _mockUserManager.Object);
        }

        [Test]
        public async Task ApproveAsCityHead_AsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanApproveAsHead(_fakeUser, It.IsAny<string>(), Roles.CityHead);

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ApproveAsCityHead_AsRegistered_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });
            var expected = false;

            //Act
            var result = await _userProfileAccessService.CanApproveAsHead(_fakeUser, It.IsAny<string>(), Roles.CityHead);

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ApproveAsCityHead_AsCityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });
            _mockUserService.Setup(u => u.IsUserSameCity(It.IsAny<UserDTO>(), It.IsAny<UserDTO>())).Returns(true);
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanApproveAsHead(_fakeUser, It.IsAny<string>(), Roles.CityHead);

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ApproveAsCityHead_AsRegionHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });
            _mockUserService.Setup(u => u.IsUserSameRegion(It.IsAny<UserDTO>(), It.IsAny<UserDTO>())).Returns(true);
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanApproveAsHead(_fakeUser, It.IsAny<string>(), Roles.CityHead);

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ViewFullProfile_AsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanViewFullProfile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ViewFullProfile_SameCity_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });
            _mockUserService.Setup(u => u.IsUserSameCity(It.IsAny<UserDTO>(), It.IsAny<UserDTO>())).Returns(true);
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanViewFullProfile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ViewFullProfile_SameClub_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });
            _mockUserService.Setup(u => u.IsUserSameClub(It.IsAny<UserDTO>(), It.IsAny<UserDTO>())).Returns(true);
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanViewFullProfile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ViewFullProfile_AsRegionHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });
            _mockUserService.Setup(u => u.IsUserSameRegion(It.IsAny<UserDTO>(), It.IsAny<UserDTO>())).Returns(true);
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanViewFullProfile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ViewFullProfile_AsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });
            var expected = false;

            //Act
            var result = await _userProfileAccessService.CanViewFullProfile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task EditUserProfile_AsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });
            var expected = false;

            //Act
            var result = await _userProfileAccessService.CanEditUserProfile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task EditUserProfile_AsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanEditUserProfile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task EditUserProfile_AsCityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });
            _mockUserService.Setup(u => u.IsUserSameCity(It.IsAny<UserDTO>(), It.IsAny<UserDTO>())).Returns(true);
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanEditUserProfile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task EditUserProfile_AsRegionHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });
            _mockUserService.Setup(u => u.IsUserSameRegion(It.IsAny<UserDTO>(), It.IsAny<UserDTO>())).Returns(true);
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanEditUserProfile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task EditUserProfile_AsClubHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });
            _mockUserService.Setup(u => u.IsUserSameClub(It.IsAny<UserDTO>(), It.IsAny<UserDTO>())).Returns(true);
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanEditUserProfile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ApproveAsClubHead_AsClubHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });
            _mockUserService.Setup(u => u.IsUserSameClub(It.IsAny<UserDTO>(), It.IsAny<UserDTO>())).Returns(true);
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanApproveAsHead(_fakeUser, It.IsAny<string>(), Roles.KurinHead);

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ApproveAsClubHead_AsClubHead_NotInjgSameClub_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });
            _mockUserService.Setup(u => u.IsUserSameClub(It.IsAny<UserDTO>(), It.IsAny<UserDTO>())).Returns(false);
            var expected = false;

            //Act
            var result = await _userProfileAccessService.CanApproveAsHead(_fakeUser, It.IsAny<string>(), Roles.KurinHead);

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ApproveAsClubHead_AsRegistered_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });
            var expected = false;

            //Act
            var result = await _userProfileAccessService.CanApproveAsHead(_fakeUser, It.IsAny<string>(), Roles.KurinHead);

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test]
        public async Task ApproveAsClubHead_AsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });
            var expected = true;

            //Act
            var result = await _userProfileAccessService.CanApproveAsHead(_fakeUser, It.IsAny<string>(), Roles.KurinHead);

            //Assert
            Assert.AreEqual(result, expected);
        }

        private readonly User _fakeUser = new User { Id = "1" };
    }
}
