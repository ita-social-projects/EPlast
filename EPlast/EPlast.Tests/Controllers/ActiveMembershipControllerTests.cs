using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

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
            _plastDegreeService.Setup(cs => cs.GetDergeesAsync()).ReturnsAsync(new List<PlastDegreeDto>());

            //Act
            var result = await _activeMembershipController.GetAllDergees();
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<PlastDegreeDto>>(resultValue);
            Assert.AreEqual(0, (resultValue as List<PlastDegreeDto>).Count);
        }

        [TestCase("2")]
        public async Task GetAccessLevel_Valid_Test(string id)
        {
            //Arrange
            _accessLevelService.Setup(m => m.GetUserAccessLevelsAsync(It.IsAny<string>())).ReturnsAsync(new List<string>());

            //Act
            var result = await _activeMembershipController.GetAccessLevel(id);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<string>>(resultValue);
            Assert.AreEqual(0, (resultValue as List<string>).Count);
        }

        [TestCase("2")]
        public async Task GetUserDergee_Valid_Test(string id)
        {
            //Arrange
            _plastDegreeService.Setup(cs => cs.GetUserPlastDegreeAsync(It.IsAny<string>())).ReturnsAsync(new UserPlastDegreeDto());

            //Act
            var result = await _activeMembershipController.GetUserDegree(id);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.IsInstanceOf<UserPlastDegreeDto>(resultValue);
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
            _plastDegreeService.Setup(cs => cs.AddPlastDegreeForUserAsync(It.IsAny<UserPlastDegreePostDto>()))
                .ReturnsAsync(successfulAdded);

            //Act
            var result = await _activeMembershipController.AddPlastDegreeForUser(new UserPlastDegreePostDto() { PlastDegreeId = degreeId, UserId = userId });

            //Assert
            Assert.IsInstanceOf<CreatedResult>(result);
            var cr = (CreatedResult)result;
            Assert.NotNull(cr.Value);
            Assert.IsInstanceOf<int>(cr.Value);
            Assert.AreEqual(degreeId, cr.Value);
        }

        [TestCase(1)]
        public async Task AddPlastDegreeForUser_ValidForCityAdmin(int degreeId)
        {
            //Arrange
            bool successfulAdded = true;
            string userId = "1";
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { Id = userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>() { Roles.CityHead, Roles.CityHeadDeputy });
            _plastDegreeService.Setup(cs => cs.AddPlastDegreeForUserAsync(It.IsAny<UserPlastDegreePostDto>()))
                .ReturnsAsync(successfulAdded);
            _userService.Setup(x => x.IsUserSameCity(_user, _user)).Returns(true);

            //Act
            var result = await _activeMembershipController.AddPlastDegreeForUser(new UserPlastDegreePostDto() { PlastDegreeId = degreeId, UserId = _userId });

            //Assert
            Assert.IsInstanceOf<CreatedResult>(result);
            var cr = (CreatedResult)result;
            Assert.NotNull(cr.Value);
            Assert.IsInstanceOf<int>(cr.Value);
            Assert.AreEqual(degreeId, cr.Value);
        }

        [TestCase(3)]
        public async Task AddPlastDegreeForUser_ValidForCityAdminAndDeputy(int id)
        {
            //Arrange
            bool successfulAdded = false;
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.CityHeadDeputy });
            _plastDegreeService.Setup(cs => cs.AddPlastDegreeForUserAsync(It.IsAny<UserPlastDegreePostDto>())).ReturnsAsync(successfulAdded);
            _plastDegreeService.Setup(ps => ps.CheckDegreeAsync(It.IsAny<int>(), It.IsAny<List<string>>())).ReturnsAsync(true);
            _userService.Setup(x => x.IsUserSameCity(_user, _user)).Returns(true);

            //Act
            var result = await _activeMembershipController.AddPlastDegreeForUser(new UserPlastDegreePostDto() { PlastDegreeId = id });

            //Assert
            Assert.IsInstanceOf<CreatedResult>(result);
        }

        [TestCase(2)]
        public async Task AddPlastDegreeForUser_403Forbidden(int id)
        {
            //Arrange
            string userId = "2";
            var user = new UserDto()
            {
                Id = userId,
                CityMembers = new List<CityMembers>(),
                ClubMembers = new List<ClubMembers>(),
                RegionAdministrations = new List<RegionAdministration>(),
            };
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _activeMembershipController.AddPlastDegreeForUser(new UserPlastDegreePostDto() { PlastDegreeId = id, UserId = userId });
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

            //Act
            var result = await _activeMembershipController.DeletePlastDegreeForUser("", 0);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeletePlastDegreeForUser_InValid_Test()
        {
            //Arrange
            bool successfulDeleted = false;
            _plastDegreeService.Setup(cs => cs.DeletePlastDegreeForUserAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(successfulDeleted);

            //Act
            var result = await _activeMembershipController.DeletePlastDegreeForUser("", 0);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task DeletePlastDegreeForUser_Returns403Forbidden()
        {
            //Arrange
            ArrangeTests(string.Empty, false);
            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _activeMembershipController.DeletePlastDegreeForUser(_userId, 4);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()));
            _userManager.Verify();
            Assert.AreEqual(expected, actual);
        }

        [TestCase("2")]
        public async Task GetUserDates_Valid_ReturnsOK(string id)
        {
            //Arrange
            _userDatesService.Setup(cs => cs.GetUserMembershipDatesAsync(It.IsAny<string>())).ReturnsAsync(new UserMembershipDatesDto());

            //Act
            var result = await _activeMembershipController.GetUserDates(id);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.IsInstanceOf<UserMembershipDatesDto>(((OkObjectResult)result).Value);
        }

        [Test]
        public async Task GetUserDates_InValid_ThrowException()
        {
            //Arrange
            _userDatesService.Setup(cs => cs.GetUserMembershipDatesAsync(It.IsAny<string>())).ThrowsAsync(new InvalidOperationException());

            //Act
            var result = await _activeMembershipController.GetUserDates(null);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetUserFormerDates_ReturnsOK()
        {
            //Arrange
            _userDatesService.Setup(cs => cs.GetUserFormerMembershipDatesTable(It.IsAny<string>()))
                .Returns(CreateTuple);

            //Act
            var result = await _activeMembershipController.GetUserDates(null);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);
        }

        private Tuple<IEnumerable<UserFormerMembershipTable>, int> CreateTuple => new Tuple<IEnumerable<UserFormerMembershipTable>, int>(CreateFormerMemberships, 100);

        private IEnumerable<UserFormerMembershipTable> CreateFormerMemberships => new List<UserFormerMembershipTable>()
        {
            new UserFormerMembershipTable(),
            new UserFormerMembershipTable()
        };

        [Test]
        public async Task ChangeUserEntryAndOathDates_ReturnOK()
        {
            //Arrange
            bool successfulChangedDates = true;
            ArrangeTests(Roles.Admin, successfulChangedDates);

            //Act
            var result = await _activeMembershipController.ChangeUserEntryandOathDatesAsync(new EntryAndOathDatesDto());

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.IsInstanceOf<EntryAndOathDatesDto>(((OkObjectResult)result).Value);
        }

        [Test]
        public async Task ChangeUserEntryAndOathDates_ReturnBadRequest()
        {
            //Arrange
            bool failedChangeDates = false;
            ArrangeTests(Roles.Admin, failedChangeDates);

            //Act
            var result = await _activeMembershipController.ChangeUserEntryandOathDatesAsync(new EntryAndOathDatesDto());

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task ChangeUserEntryAndOathDates_NoAccess_Return403Forbidden()
        {
            //Arrange
            bool failedChangeDates = false;
            ArrangeTests(Roles.PlastMember, failedChangeDates);
            var expected = StatusCodes.Status403Forbidden;

            //Act
            var result = await _activeMembershipController.ChangeUserEntryandOathDatesAsync(new EntryAndOathDatesDto());
            var actual = (result as StatusCodeResult).StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task ChangeUserEntryAndOathDates_Return403Forbidden()
        {
            //Arrange
            ArrangeTests(string.Empty, false);
            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _activeMembershipController.ChangeUserEntryandOathDatesAsync(new EntryAndOathDatesDto());
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()));
            _userManager.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task ChangeUserEntryAndOathDates_RegisteredUser_Return403Forbidden()
        {
            //Arrange
            ArrangeTests(Roles.RegisteredUser, false);
            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _activeMembershipController.ChangeUserEntryandOathDatesAsync(new EntryAndOathDatesDto());
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

            //Act
            var result = await _activeMembershipController.InitializeUserDates("2");

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

            //Act
            var result = await _activeMembershipController.InitializeUserDates("");

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        private void ArrangeTests(string role, bool isChanged)
        {
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User() { Id = _userId });
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { role });
            _userDatesService.Setup(cs => cs.ChangeUserEntryAndOathDateAsync(It.IsAny<EntryAndOathDatesDto>()))
                             .ReturnsAsync(isChanged);
        }

        private string _userId = "1";
        private UserDto _user = new UserDto()
        {
            Id = "1",
            CityMembers = new List<CityMembers>(),
            ClubMembers = new List<ClubMembers>(),
            RegionAdministrations = new List<RegionAdministration>(),
        };
    }
}
