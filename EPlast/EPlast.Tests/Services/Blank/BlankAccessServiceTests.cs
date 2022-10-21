using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Blank;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Blank
{
    [TestFixture]
    public class BlankAccessServiceTests
    {
        private IBlankAccessService _blankAccessService;
        private Mock<IUserService> _mockUserService;
        private Mock<UserManager<User>> _mockUserManager;
        private readonly User _fakeUser = new User { Id = "1" };

        [SetUp]
        public void SetUp()
        {
            Mock<IUserStore<User>> _mockStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(_mockStore.Object, null, null, null, null, null, null, null, null);
            _mockUserService = new Mock<IUserService>();
            _blankAccessService = new BlankAccessService(_mockUserService.Object, _mockUserManager.Object);
        }

        [Test]
        public async Task AddAchievementAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanAddAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test] 
        public async Task AddBiographyAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanAddBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewAddDownloadDeleteExtractUPUAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanViewAddDownloadDeleteExtractUPU(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAchievementAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanDeleteAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteBiographyAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanDeleteBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DownloadAchievementAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanDownloadAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DownloadBiographyAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanDownloadBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewAchievementAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanViewAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBiographyAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanViewBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBlankTabAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanViewBlankTab(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewListOfAchievementsAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanViewListOfAchievements(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GenerateFileAsAdmin_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.GoverningBodyAdmin });

            //Act
            var result = await _blankAccessService.CanGenerateFile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddAchievementAsOkrugaHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanAddAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddBiographyAsOkrugaHead_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanAddBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewAddDownloadDeleteExtractUPUAsOkrugaHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanViewAddDownloadDeleteExtractUPU(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAchievementAsOkrugaHead_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanDeleteAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteBiographyAsOkrugaHead_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanDeleteBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DownloadAchievementAsOkrugaHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanDownloadAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DownloadBiographyAsOkrugaHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanDownloadBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewAchievementAsOkrugaHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanViewAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBiographyAsOkrugaHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanViewBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBlankTabAsOkrugaHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanViewBlankTab(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewListOfAchievementsAsOkrugaHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanViewListOfAchievements(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddBiographyAsOkrugaHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });
            _mockUserService.Setup(u => u.IsUserInSameCellAsync(It.IsAny<UserDto>(), It.IsAny<UserDto>(), CellType.Region)).ReturnsAsync(true);

            //Act
            var result = await _blankAccessService.CanAddBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GenerateFileAsOkrugaHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.OkrugaHead });

            //Act
            var result = await _blankAccessService.CanGenerateFile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddAchievementAsСityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanAddAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddBiographyAsСityHead_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanAddBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewAddDownloadDeleteExtractUPUAsСityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanViewAddDownloadDeleteExtractUPU(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAchievementAsСityHead_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanDeleteAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteBiographyAsСityHead_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanDeleteBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DownloadAchievementAsСityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanDownloadAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DownloadBiographyAsСityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanDownloadBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewAchievementAsСityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanViewAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBiographyAsСityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanViewBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBlankTabAsСityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanViewBlankTab(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewListOfAchievementsAsСityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanViewListOfAchievements(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddBiographyAsСityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });
            _mockUserService.Setup(u => u.IsUserInSameCellAsync(It.IsAny<UserDto>(), It.IsAny<UserDto>(), CellType.City)).ReturnsAsync(true);

            //Act
            var result = await _blankAccessService.CanAddBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GenerateFileAsСityHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHead });

            //Act
            var result = await _blankAccessService.CanGenerateFile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddAchievementAsKurinHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanAddAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddBiographyAsKurinHead_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanAddBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewAddDownloadDeleteExtractUPUAsKurinHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanViewAddDownloadDeleteExtractUPU(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAchievementAsKurinHead_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanDeleteAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteBiographyAsKurinHead_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanDeleteBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DownloadAchievementAsKurinHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanDownloadAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DownloadBiographyAsKurinHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanDownloadBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewAchievementAsKurinHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanViewAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBiographyAsKurinHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanViewBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBlankTabAsKurinHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanViewBlankTab(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewListOfAchievementsAsKurinHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanViewListOfAchievements(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GenerateFileAsKurinHead_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            //Act
            var result = await _blankAccessService.CanGenerateFile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddAchievementAsRegisteredUser_ReturnsFalseTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanAddAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddBiographyAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanAddBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewAddDownloadDeleteExtractUPUAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanViewAddDownloadDeleteExtractUPU(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteAchievementAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanDeleteAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteBiographyAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanDeleteBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DownloadAchievementAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanDownloadAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DownloadBiographyAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanDownloadBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewAchievementAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanViewAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewBiographyAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanViewBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewBlankTabAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanViewBlankTab(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewListOfAchievementsAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanViewListOfAchievements(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GenerateFileAsRegisteredUser_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.RegisteredUser });

            //Act
            var result = await _blankAccessService.CanGenerateFile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddAchievementAsSupporter_ReturnsFalseTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanAddAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddBiographyAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanAddBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewAddDownloadDeleteExtractUPUAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanViewAddDownloadDeleteExtractUPU(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteAchievementAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanDeleteAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteBiographyAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanDeleteBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DownloadAchievementAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanDownloadAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DownloadBiographyAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanDownloadBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewAchievementAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanViewAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewBiographyAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanViewBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewBlankTabAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanViewBlankTab(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewListOfAchievementsAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanViewListOfAchievements(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GenerateFileAsSupporter_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Supporter });

            //Act
            var result = await _blankAccessService.CanGenerateFile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddAchievementAsPlastMember_ReturnsFalseTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanAddAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddBiographyAsPlastMember_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanAddBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewAddDownloadDeleteExtractUPUAsPlastMember_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanViewAddDownloadDeleteExtractUPU(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteAchievementAsPlastMember_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanDeleteAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteBiographyAsPlastMember_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanDeleteBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DownloadAchievementAsPlastMember_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanDownloadAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DownloadBiographyAsPlastMember_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanDownloadBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewAchievementAsPlastMember_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanViewAchievement(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBiographyAsPlastMember_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanViewBiography(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ViewBlankTabAsPlastMember_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanViewBlankTab(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewListOfAchievementsAsPlastMember_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanViewListOfAchievements(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GenerateFileAsPlastMember_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanGenerateFile(_fakeUser, It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddAchievementAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanAddAchievement(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddBiographyAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanAddBiography(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewAddDownloadDeleteExtractUPUAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanViewAddDownloadDeleteExtractUPU(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAchievementAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanDeleteAchievement(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteBiographyAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.PlastMember });

            //Act
            var result = await _blankAccessService.CanDeleteBiography(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DownloadAchievementAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanDownloadAchievement(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DownloadBiographyAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanDownloadBiography(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewAchievementAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanViewAchievement(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBiographyAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanViewBiography(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewBlankTabAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanViewBlankTab(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ViewListOfAchievementsAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanViewListOfAchievements(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GenerateFileAsSameUser_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { });

            //Act
            var result = await _blankAccessService.CanGenerateFile(_fakeUser, _fakeUser.Id);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
