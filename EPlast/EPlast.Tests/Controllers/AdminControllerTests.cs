using System;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.DataAccess.Entities;

namespace EPlast.Tests.Controllers
{
    internal class AdminControllerTest
    {
        private readonly Mock<IAdminService> _adminService;

        private readonly Mock<ICityParticipantsService> _cityAdministrationService;

        private readonly Mock<ICityService> _cityService;

        private readonly Mock<ILoggerService<AdminController>> _logger;

        private readonly Mock<IUserManagerService> _userManagerService;

        public AdminControllerTest()
        {
            _logger = new Mock<ILoggerService<AdminController>>();
            _userManagerService = new Mock<IUserManagerService>();
            _adminService = new Mock<IAdminService>();
            _cityService = new Mock<ICityService>();
            _cityAdministrationService = new Mock<ICityParticipantsService>();
        }

        private AdminController CreateAdminController => new AdminController(
            _logger.Object,
            _userManagerService.Object,
            _adminService.Object,
            _cityService.Object,
            _cityAdministrationService.Object
            );

        [Test]
        public void ChangeUserRoleToExpired_UserExists_Test()
        {
            //Arrange
            AdminController adminController = CreateAdminController;
            
            //Act
            var result = adminController.ChangeUserRoleToExpired("user");
            
            //Assert
            Assert.NotNull(result);
            _adminService.Verify(x => x.ChangeAsync(It.IsAny<string>()), Times.AtLeastOnce);
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void ChangeUserRoleToExpired_UserNotExists_Test()
        {
            //Arrange
            AdminController adminController = CreateAdminController;
            
            //Act
            var result = adminController.ChangeUserRoleToExpired(null);
            
            //Assert
            Assert.NotNull(result);
            _logger.Verify(x => x.LogError(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void ChangeCurrentUserRole_UserNotExists_Test()
        {
            //Arrange
            AdminController adminController = CreateAdminController;
            //Act
            var result = adminController.ChangeCurrentUserRole(null, It.IsAny<string>());
            //Assert
            Assert.NotNull(result);
            _logger.Verify(x => x.LogError(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [TestCase("user")]
        public void ChangeCurrentUserRole_UserExists_Test( string username)
        {
            //Arrange
            AdminController adminController = CreateAdminController;
            
            //Act
            var result = adminController.ChangeCurrentUserRole(username, It.IsAny<string>());
            
            //Assert
            Assert.NotNull(result);
            _adminService.Verify(x => x.ChangeCurrentRoleAsync(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void ConfirmDelete_Invalid_Test()
        {
            AdminController adminController = CreateAdminController;

            var result = adminController.ConfirmDelete("");

            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void ConfirmDelete_Valid_Test()
        {
            AdminController adminController = CreateAdminController;

            var result = adminController.ConfirmDelete("UserId");

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Delete_Invalid_Test()
        {
            _adminService.Setup(a => a.DeleteUserAsync(It.IsAny<string>()));

            AdminController adminController = CreateAdminController;

            var result = await adminController.Delete("");

            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Delete_Valid_Test()
        {
            _adminService.Setup(a => a.DeleteUserAsync(It.IsAny<string>()));

            AdminController adminController = CreateAdminController;

            var result = await adminController.Delete("SomeUserId");

            _adminService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Edit_Invalid_Test()
        {
            AdminController adminController = CreateAdminController;

            // Act
            var result = await adminController.Edit(null);

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase("user")]
        public async Task Edit_CouldNotFindUser_Test(string username)
        {
            //Arrange
            AdminController adminController = CreateAdminController;

            // Act
            var result = await adminController.Edit(username);
            _userManagerService.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns((Task<UserDTO>)null);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
            _logger.Verify(x => x.LogError(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public async Task Edit_Valid_Test()
        {
            _userManagerService.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
               .ReturnsAsync(new UserDTO());

            _userManagerService.Setup(u => u.GetRolesAsync(It.IsAny<UserDTO>()))
              .ReturnsAsync(It.IsAny<IEnumerable<string>>());

            _adminService.Setup(a => a.GetRolesExceptAdmin())
                .Returns(It.IsAny<IEnumerable<IdentityRole>>());

            AdminController adminController = CreateAdminController;

            // Act
            var result = await adminController.Edit("AdminId");

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EditRole_Invalid_Test()
        {
            _adminService.Setup(a => a.EditAsync(It.IsAny<string>(), It.IsAny<List<string>>()));

            AdminController adminController = CreateAdminController;

            List<string> roles = new List<string>();
            roles.Add("rendomRole");

            var result = await adminController.Edit("", roles);

            _adminService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task EditRole_Valid_Test()
        {
            _adminService.Setup(a => a.EditAsync(It.IsAny<string>(), It.IsAny<List<string>>()));

            AdminController adminController = CreateAdminController;

            List<string> roles = new List<string>();
            roles.Add("rendomRole");

            var result = await adminController.Edit("UserId", roles);

            _adminService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetAdmins_Valid_Test()
        {
            _cityAdministrationService.Setup(c => c.GetAdministrationByIdAsync(It.IsAny<int>()));

            AdminController adminController = CreateAdminController;

            var result = await adminController.GetAdmins(2);

            _adminService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(0)]
        public async Task GetAdmins_WrongCityId_Test(int cityId)
        {
            //Arrange
            AdminController adminController = CreateAdminController;

            //Act
            var result = await adminController.GetAdmins(cityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
            _logger.Verify(x => x.LogError(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public async Task GetCityAndRegionAdminsOfUser_UserNotExists_Test()
        {
            //Arrange
            AdminController adminController = CreateAdminController;

            //Act
            var result = await adminController.GetCityAndRegionAdminsOfUser(null);
            
            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [TestCase("user")]
        public async Task GetCityAndRegionAdminsOfUser_UserExists_Test(string username)
        {
            //Arrange
            AdminController adminController = CreateAdminController;

            //Act
            var result = await adminController.GetCityAndRegionAdminsOfUser(username);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            _adminService.Verify(x => x.GetCityRegionAdminsOfUserAsync(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public async Task RegionsAdmins_Invalid_Test()
        {
            _cityService.Setup(c => c.GetAllDTOAsync(null))
                .ReturnsAsync(() => null);

            AdminController adminController = CreateAdminController;

            var result = await adminController.RegionsAdmins();

            _adminService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RegionsAdmins_Valid_Test()
        {
            _cityService.Setup(c => c.GetAllDTOAsync(null))
                .ReturnsAsync(It.IsAny<IEnumerable<CityDTO>>());

            AdminController adminController = CreateAdminController;

            var result = await adminController.RegionsAdmins();

            _cityService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UsersTable_Valid_Test()
        {
            _adminService.Setup(a => a.GetUsersTableAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(CreateTuple);

            AdminController adminController = CreateAdminController;

            // Act
            var result = await adminController.GetUsersTable(CreateTableFilterParameters);

            // Assert
            _adminService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private TableFilterParameters CreateTableFilterParameters => new TableFilterParameters()
        {
            Page = 1,
            PageSize = 10,
            TotalRow = 5,
            Tab = "TAB",
            Cities = new List<string>(),
            Regions = new List<string>(),
            Clubs = new List<string>(),
            Degrees = new List<string>()
        };

        private Tuple<IEnumerable<UserTableDTO>, int> CreateTuple => new Tuple<IEnumerable<UserTableDTO>, int>(CreateUserTableObjects, 100);

        private IEnumerable<UserTableDTO> CreateUserTableObjects => new List<UserTableDTO>()
        {
            new UserTableDTO(),
            new UserTableDTO()
        };
    }
}
