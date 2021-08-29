using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserProfiles;
using Microsoft.AspNetCore.Identity;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Http;

namespace EPlast.Tests.Controllers
{
    internal class ActiveMembershipControllerTests
    {
        private readonly Mock<IPlastDegreeService> _plastDegreeService;
        private readonly Mock<IAccessLevelService> _accessLevelService;
        private readonly Mock<IUserDatesService> _userDatesService;
        private readonly Mock<ILoggerService<ActiveMembershipController>> _loggerService;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IUserService> _userService;

        private ActiveMembershipController _activeMembershipController =>
            new ActiveMembershipController(_plastDegreeService.Object, _accessLevelService.Object,
                _userDatesService.Object, _loggerService.Object, _userManager.Object, _userService.Object);

        public ActiveMembershipControllerTests()
        {
            _plastDegreeService = new Mock<IPlastDegreeService>();
            _accessLevelService = new Mock<IAccessLevelService>();
            _userDatesService = new Mock<IUserDatesService>();
            _loggerService = new Mock<ILoggerService<ActiveMembershipController>>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _userService = new Mock<IUserService>();
        }

        [Test]
        public async Task GetAllDergees_Valid_Test()
        {
            //Arrange
            _plastDegreeService.Setup(cs => cs.GetDergeesAsync()).ReturnsAsync(new List<PlastDegreeDTO>());

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.GetAllDergees();
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<PlastDegreeDTO>>(resultValue);
            Assert.AreEqual(0, (resultValue as List<PlastDegreeDTO>).Count);
        }

        [TestCase("2")]
        public async Task GetAccessLevel_Valid_Test(string id)
        {
            //Arrange
            _accessLevelService.Setup(m => m.GetUserAccessLevelsAsync(It.IsAny<string>())).ReturnsAsync(new List<string>());

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.GetAccessLevel(id);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<string>>(resultValue);
            Assert.AreEqual(0, (resultValue as List<string>).Count);
        }

        [TestCase("2")]
        public async Task GetUserDergees_Valid_Test(string id)
        {
            //Arrange
            _plastDegreeService.Setup(cs => cs.GetUserPlastDegreesAsync(It.IsAny<string>())).ReturnsAsync(new List<UserPlastDegreeDTO>());

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.GetUserDegrees(id);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<UserPlastDegreeDTO>>(resultValue);
            Assert.AreEqual(0, (resultValue as List<UserPlastDegreeDTO>).Count);
        }

        [TestCase(2)]
        public async Task AddPlastDegreeForUser_Valid_Test(int degreeId)
        {
            //Arrange
            bool successfulAdded = true;
            string userId = "1";
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { Id = userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>() { Roles.Admin });
            _plastDegreeService.Setup(cs => cs.AddPlastDegreeForUserAsync(It.IsAny<UserPlastDegreePostDTO>()))
                .ReturnsAsync(successfulAdded);               

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.AddPlastDegreeForUser(new UserPlastDegreePostDTO() { PlastDegreeId = degreeId, UserId = userId});

            //Assert
            Assert.IsInstanceOf<CreatedResult>(result);
            var cr = (CreatedResult)result;
            Assert.NotNull(cr.Value);
            Assert.IsInstanceOf<int>(cr.Value);
            Assert.AreEqual(degreeId, cr.Value);
        }

        [TestCase(1)]
        public async Task AddPlastDegreeForUser_InValidForCityAdmin(int degreeId)
        {
            //Arrange
            bool successfulAdded = true;
            string userId = "1";
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { Id = userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>() { Roles.CityHead,Roles.CityHeadDeputy});
            _plastDegreeService.Setup(cs => cs.AddPlastDegreeForUserAsync(It.IsAny<UserPlastDegreePostDTO>()))
                .ReturnsAsync(successfulAdded); 
            _userService.Setup(x => x.IsUserSameCity(_user, _user)).Returns(true);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.AddPlastDegreeForUser(new UserPlastDegreePostDTO() { PlastDegreeId = degreeId, UserId = _userId });

            //Assert
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [TestCase(3)]
        public async Task AddPlastDegreeForUser_ValidForCityAdminAndDeputy(int id)
        {
            //Arrange
            bool successfulAdded = false;
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHeadDeputy});
            _plastDegreeService.Setup(cs => cs.AddPlastDegreeForUserAsync(It.IsAny<UserPlastDegreePostDTO>())).ReturnsAsync(successfulAdded);
            _plastDegreeService.Setup(ps => ps.GetDergeeAsync(It.IsAny<int>(), It.IsAny<List<string>>())).ReturnsAsync(true);
            _userService.Setup(x => x.IsUserSameCity(_user, _user)).Returns(true);
            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.AddPlastDegreeForUser(new UserPlastDegreePostDTO() { PlastDegreeId = id });

            //Assert
            Assert.IsInstanceOf<CreatedResult>(result);
        }

        [TestCase(2)]
        public async Task AddPlastDegreeForUser_403Forbidden(int id)
        {
            //Arrange
            string userId = "2";
            var user = new UserDTO()
            {
                Id = userId,
                CityMembers = new List<CityMembers>(),
                ClubMembers = new List<ClubMembers>(),
                RegionAdministrations = new List<RegionAdministration>(),
            };
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            ActiveMembershipController activeMembershipController = _activeMembershipController;
            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await activeMembershipController.AddPlastDegreeForUser(new UserPlastDegreePostDTO() { PlastDegreeId = id, UserId = userId });
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()));
            _userManager.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task DeletePlastDegreeForUser_Valid_Test()
        {
            //Arrange
            bool successfulDeleted = true;
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });
            _plastDegreeService.Setup(cs => cs.DeletePlastDegreeForUserAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(successfulDeleted);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.DeletePlastDegreeForUser("", 0);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeletePlastDegreeForUser_InValid_Test()
        {
            //Arrange
            bool successfulDeleted = false;
            _plastDegreeService.Setup(cs => cs.DeletePlastDegreeForUserAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(successfulDeleted);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.DeletePlastDegreeForUser("", 0);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task DeletePlastDegreeForUser_Returns403Forbidden()
        {
            //Arrange
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            ActiveMembershipController activeMembershipController = _activeMembershipController;
            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await activeMembershipController.DeletePlastDegreeForUser(_userId, 4);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()));
            _userManager.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SetPlastDegreeAsCurrent_Valid_Test()
        {
            //Arrange
            bool successfulSetPD = true;
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });
            _plastDegreeService.Setup(cs => cs.SetPlastDegreeForUserAsCurrentAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(successfulSetPD);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.SetPlastDegreeAsCurrent("", 0);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task SetPlastDegreeAsCurrent_InValid_Test()
        {
            //Arrange
            bool successfulSetPD = false;
            _plastDegreeService.Setup(cs => cs.SetPlastDegreeForUserAsCurrentAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(successfulSetPD);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.SetPlastDegreeAsCurrent("", 0);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task SetPlastDegreeAsCurrent_Returns403Forbidden()
        {
            //Arrange
            bool successfulSetPD = true;
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
            _plastDegreeService.Setup(cs => cs.SetPlastDegreeForUserAsCurrentAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(successfulSetPD);

            ActiveMembershipController activeMembershipController = _activeMembershipController;
            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await activeMembershipController.SetPlastDegreeAsCurrent("", 0); 
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()));
            _userManager.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task AddEndDatePlastDegreeForUser_Valid_Test()
        {
            //Arrange
            bool successfulAddedEndDate = true;
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });
            _plastDegreeService.Setup(cs => cs.AddEndDateForUserPlastDegreeAsync(It.IsAny<UserPlastDegreePutDTO>()))
                               .ReturnsAsync(successfulAddedEndDate);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.AddEndDatePlastDegreeForUser(new UserPlastDegreePutDTO());

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddEndDatePlastDegreeForUser_InValid_Test()
        {
            //Arrange
            bool successfulAddedEndDate = false;
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });
            _plastDegreeService.Setup(cs => cs.AddEndDateForUserPlastDegreeAsync(It.IsAny<UserPlastDegreePutDTO>()))
                               .ReturnsAsync(successfulAddedEndDate);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.AddEndDatePlastDegreeForUser(new UserPlastDegreePutDTO());

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task AddEndDatePlastDegreeForUser_403Forbidden()
        {
            //Arrange
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            ActiveMembershipController activeMembershipController = _activeMembershipController;
            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await activeMembershipController.AddEndDatePlastDegreeForUser(new UserPlastDegreePutDTO());
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _userManager.Verify();
            Assert.AreEqual(expected, actual);
        }

        [TestCase("2")]
        public async Task GetUserDates_Valid_ReturnsOK(string id)
        {
            //Arrange
            _userDatesService.Setup(cs => cs.GetUserMembershipDatesAsync(It.IsAny<string>())).ReturnsAsync(new UserMembershipDatesDTO());

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.GetUserDates(id);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.IsInstanceOf<UserMembershipDatesDTO>(((OkObjectResult)result).Value);
        }

        [Test]
        public async Task GetUserDates_InValid_ThrowException()
        {
            //Arrange
            _userDatesService.Setup(cs => cs.GetUserMembershipDatesAsync(It.IsAny<string>())).ThrowsAsync(new InvalidOperationException());

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.GetUserDates(null);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangeUserDates_Valid_Test()
        {
            //Arrange
            bool successfulChangedDates = true;
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });
            _userDatesService.Setup(cs => cs.ChangeUserMembershipDatesAsync(It.IsAny<UserMembershipDatesDTO>()))
                             .ReturnsAsync(successfulChangedDates);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.ChangeUserDates(new UserMembershipDatesDTO());

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.IsInstanceOf<UserMembershipDatesDTO>(((OkObjectResult)result).Value);
        }

        [Test]
        public async Task ChangeUserDates_InValid_Test()
        {
            //Arrange
            bool successfulChangedDates = false;
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });
            _userDatesService.Setup(cs => cs.ChangeUserMembershipDatesAsync(It.IsAny<UserMembershipDatesDTO>()))
                             .ReturnsAsync(successfulChangedDates);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.ChangeUserDates(new UserMembershipDatesDTO());

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task ChangeUserDates_403Forbidden()
        {
            //Arrange
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            ActiveMembershipController activeMembershipController = _activeMembershipController;
            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await activeMembershipController.ChangeUserDates(new UserMembershipDatesDTO());
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()));
            _userManager.Verify();
            Assert.AreEqual(expected, actual);
        }

        [TestCase("2")]
        public async Task InitializeUserDates_Valid_Test(string userId)
        {
            //Arrange
            bool successfulInitedDates = true;
            _userDatesService.Setup(cs => cs.AddDateEntryAsync(It.IsAny<string>()))
                             .ReturnsAsync(successfulInitedDates);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.InitializeUserDates("2");

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.IsInstanceOf<string>(((OkObjectResult)result).Value);
        }

        [Test]
        public async Task InitializeUserDates_InValid_Test()
        {
            //Arrange
            bool successfulInitedDates = false;
            _userDatesService.Setup(cs => cs.AddDateEntryAsync(It.IsAny<string>()))
                             .ReturnsAsync(successfulInitedDates);

            ActiveMembershipController activeMembershipController = _activeMembershipController;

            //Act
            var result = await activeMembershipController.InitializeUserDates("");

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        private string _userId = "1";
        private UserDTO _user = new UserDTO()
        {
            Id = "1",
            CityMembers = new List<CityMembers>(),
            ClubMembers = new List<ClubMembers>(),
            RegionAdministrations = new List<RegionAdministration>(),
        };
    }
}
