using System;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Queries.City;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using EPlast.Resources;
using System.Security.Principal;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace EPlast.Tests.Controllers
{
    internal class AdminControllerTest
    {
        private readonly Mock<IAdminService> _adminService;

        private readonly Mock<ICityParticipantsService> _cityAdministrationService;

        private readonly Mock<ILoggerService<AdminController>> _logger;

        private readonly Mock<IUserManagerService> _userManagerService;

        private readonly Mock<HttpContext> _httpContext;

        private readonly Mock<IMediator> _mediator;

        private readonly ControllerContext _context;

        public AdminControllerTest()
        {
            _logger = new Mock<ILoggerService<AdminController>>();
            _userManagerService = new Mock<IUserManagerService>();
            _adminService = new Mock<IAdminService>();
            _cityAdministrationService = new Mock<ICityParticipantsService>();
            _httpContext = new Mock<HttpContext>();
            _mediator = new Mock<IMediator>();
            
            _context = new ControllerContext(
               new ActionContext(
                   _httpContext.Object,
                   new RouteData(),
                   new ControllerActionDescriptor()
                )
            );
        }

        private AdminController GetAdminController() => new AdminController(
            _logger.Object,
            _userManagerService.Object,
            _adminService.Object,
            _cityAdministrationService.Object,
            _mediator.Object
            );

        [Test]
        public void ChangeUserRoleToExpired_UserExists_Test()
        {
            //Arrange
            AdminController adminController = GetAdminController();
            
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
            AdminController adminController = GetAdminController();
            
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
            AdminController adminController = GetAdminController();
            //Act
            var result = adminController.ChangeCurrentUserRole(null, It.IsAny<string>());
            //Assert
            Assert.NotNull(result);
            _logger.Verify(x => x.LogError(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [TestCase("user")]
        public void ChangeCurrentUserRole_UserExists_Test(string username)
        {
            //Arrange
            AdminController adminController = GetAdminController();
            
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
            AdminController adminController = GetAdminController();

            var result = adminController.ConfirmDelete("");

            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void ConfirmDelete_Valid_Test()
        {
            AdminController adminController = GetAdminController();

            var result = adminController.ConfirmDelete("UserId");

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Delete_Invalid_Test()
        {
            _adminService.Setup(a => a.DeleteUserAsync(It.IsAny<string>()));

            AdminController adminController = GetAdminController();

            var result = await adminController.Delete("");

            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Delete_Valid_Test()
        {
            _adminService.Setup(a => a.DeleteUserAsync(It.IsAny<string>()));

            AdminController adminController = GetAdminController();

            var result = await adminController.Delete("SomeUserId");

            _adminService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Edit_Invalid_Test()
        {
            AdminController adminController = GetAdminController();

            // Act
            var result = await adminController.Edit(null);

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase("user")]
        public async Task Edit_CouldNotFindUser_Test(string username)
        {
            //Arrange
            AdminController adminController = GetAdminController();

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

            AdminController adminController = GetAdminController();

            // Act
            var result = await adminController.Edit("AdminId");

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EditRole_Invalid_Test()
        {
            _adminService.Setup(a => a.EditAsync(It.IsAny<string>(), It.IsAny<List<string>>()));

            AdminController adminController = GetAdminController();

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

            AdminController adminController = GetAdminController();

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

            AdminController adminController = GetAdminController();

            var result = await adminController.GetAdmins(2);

            _adminService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(0)]
        public async Task GetAdmins_WrongCityId_Test(int cityId)
        {
            //Arrange
            AdminController adminController = GetAdminController();

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
            AdminController adminController = GetAdminController();

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
            AdminController adminController = GetAdminController();
            _userManagerService.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDTO());
            
            //Act
            var result = await adminController.GetCityAndRegionAdminsOfUser(username);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            _adminService.Verify(x => x.GetCityRegionAdminsOfUserAsync(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [TestCase("user")]
        public async Task GetCityAndRegionAdminsOfUser_UserNotExists_ReturnsBadRequest(string username)
        {
            //Arrange
            AdminController adminController = GetAdminController();
            _userManagerService.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((UserDTO)null);
            //Act
            var result = await adminController.GetCityAndRegionAdminsOfUser(username);
            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task RegionsAdmins_Invalid_Test()
        {
            _mediator
                .Setup(m => m.Send(
                    It.Is<GetAllCitiesOrByNameQuery>(q => q.CityName == null),
                    default
                ))
                .ReturnsAsync(value: null);

            AdminController adminController = GetAdminController();

            var result = await adminController.RegionsAdmins();

            _adminService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RegionsAdmins_Valid_Test()
        {
            _mediator
                .Setup(m => m.Send(
                It.Is<GetAllCitiesOrByNameQuery>(q => q.CityName == null),
                default
            ))
                .ReturnsAsync(It.IsAny<IEnumerable<CityDTO>>());

            AdminController adminController = GetAdminController();

            var result = await adminController.RegionsAdmins();

            _mediator.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UsersTable_Valid_Test()
        {
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            _httpContext.Setup(t => t.User).Returns(principal);
           
            AdminController adminController = GetAdminController();
            

            //Set your controller ControllerContext with fake context
           adminController.ControllerContext = _context;

            _adminService.Setup(a => a.GetUsersTableAsync(It.IsAny<TableFilterParameters>(), It.IsAny<string>()))
                .ReturnsAsync(CreateTuple);

            
            var expected = StatusCodes.Status200OK;

            // Act
            var result = await adminController.GetUsersTable(CreateTableFilterParameters);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            _adminService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public async Task GetUsers_Valid_Test()
        {
            _adminService.Setup(a => a.GetUsersAsync())
                .ReturnsAsync(new List<ShortUserInformationDTO>());

            AdminController adminController = GetAdminController();

            // Act
            var result = await adminController.GetUsers();
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<List<ShortUserInformationDTO>>(resultValue);
        }

        [TestCase("searchString")]
        public async Task ShortUsersInfo_Valid_Test(string searchString)
        {
            // Arrange
            _adminService.Setup(a => a.GetShortUserInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ShortUserInformationDTO>());

            var adminController = GetAdminController();

            // Act
            var result = await adminController.GetShortUsersInfo(searchString);
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<List<ShortUserInformationDTO>>(resultValue);
        }

        [Test]
        public async Task GetUsersByAllRoles_ReturnsOkObjectResult()
        {
            //Arrange
            _adminService.Setup(x => x.GetUsersByRolesAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Func<IEnumerable<User>, IEnumerable<string>, bool, Task<IEnumerable<ShortUserInformationDTO>>>>()))
                .ReturnsAsync(new List<ShortUserInformationDTO>() { new ShortUserInformationDTO() });
            AdminController adminController = GetAdminController();

            //Act
            var res = await adminController.GetUsersByAllRoles("Roles", true);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public async Task GetUsersByAnyRole_ReturnsOkObjectResult()
        {
            //Arrange
            _adminService.Setup(x => x.GetUsersByRolesAsync(It.IsAny<string>(), It.IsAny<bool>(),It.IsAny<Func<IEnumerable<User>, IEnumerable<string>, bool, Task<IEnumerable<ShortUserInformationDTO>>>>()))
                .ReturnsAsync(new List<ShortUserInformationDTO>() { new ShortUserInformationDTO() });
            AdminController adminController = GetAdminController();

            //Act
            var res = await adminController.GetUsersByAnyRole("Roles", true);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public async Task GetUsersForGoverningBodies_ReturnsOkObjectResult()
        {
            //Arrange
            _adminService.Setup(x => x.GetUsersForGoverningBodiesAsync())
                .ReturnsAsync(new List<ShortUserInformationDTO>() { new ShortUserInformationDTO() });
            AdminController adminController = GetAdminController();

            //Act
            var res = await adminController.GetUsersForGoverningBodies();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public async Task GetUsersByExactRoles_ReturnsOkObjectResult()
        {
            //Arrange
            _adminService.Setup(x => x.GetUsersByRolesAsync(It.IsAny<string>(), It.IsAny<bool>(),It.IsAny<Func<IEnumerable<User>, IEnumerable<string>, bool, Task<IEnumerable<ShortUserInformationDTO>>>>()))
                .ReturnsAsync(new List<ShortUserInformationDTO>() { new ShortUserInformationDTO() });
            AdminController adminController = GetAdminController();

            //Act
            var res = await adminController.GetUsersByExactRoles("Roles", true);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(res);
        }
        [Test]
        public async Task IsCityMember_True_Test()
        {
            //Arrange
            _adminService.Setup(x => x.IsCityMember(It.IsAny<string>())).ReturnsAsync(true);
            AdminController adminController = GetAdminController();

            // Act
            var result = await adminController.IsCityMember(It.IsAny<string>());

            Assert.True(result);
           
        }
        [Test]
        public async Task IsCityMember_False_Test()
        {
            //Arrange
            _adminService.Setup(x => x.IsCityMember(It.IsAny<string>())).ReturnsAsync(false);
            AdminController adminController = GetAdminController();

            // Act
            var result = await adminController.IsCityMember(It.IsAny<string>());

            Assert.False(result);

        }
        private TableFilterParameters CreateTableFilterParameters => new TableFilterParameters()
        {
            Page = 1,
            PageSize = 10,
            TotalRow = 5,
            Tab = "TAB",
            Cities = new List<int>(),
            Regions = new List<int>(),
            Clubs = new List<int>(),
            Degrees = new List<int>(),
            SearchData = "Ольга"
        };
        System.Security.Claims.ClaimsPrincipal claimsPrincipal = new System.Security.Claims.ClaimsPrincipal();
        private Tuple<IEnumerable<UserTableDTO>, int> CreateTuple => new Tuple<IEnumerable<UserTableDTO>, int>(CreateUserTableObjects, 100);

        private IEnumerable<UserTableDTO> CreateUserTableObjects => new List<UserTableDTO>()
        {
            new UserTableDTO(),
            new UserTableDTO()
        };
    }
}
