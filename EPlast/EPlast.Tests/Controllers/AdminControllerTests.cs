﻿using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    internal class AdminControllerTest
    {
        public AdminControllerTest()
        {
            _logger = new Mock<ILoggerService<AdminController>>();
            _userManagerService = new Mock<IUserManagerService>();
            _adminService = new Mock<IAdminService>();
            _cityService = new Mock<ICityService>();
            _cityAdministrationService = new Mock<ICityParticipantsService>();
        }

        [Test]
        public async Task UsersTable_Valid_Test()
        {
            _adminService.Setup(a => a.UsersTableAsync());

            AdminController adminController = CreateAdminController;

            // Act
            var result = await adminController.UsersTable();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Edit_Valid_Test()
        {
            _userManagerService.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
               .ReturnsAsync(new UserDTO());

            _userManagerService.Setup(u => u.GetRolesAsync(It.IsAny<UserDTO>()))
              .ReturnsAsync(It.IsAny<IEnumerable<string>>());

            _adminService.Setup(a => a.GetRolesExceptAdminAsync())
                .ReturnsAsync(It.IsAny<IEnumerable<IdentityRole>>());

            AdminController adminController = CreateAdminController;

            // Act
            var result = await adminController.Edit("AdminId");

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
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

        [Test]
        public async Task EditRole_Valid_Test()
        {
            _adminService.Setup(a => a.EditAsync(It.IsAny<string>(), It.IsAny<List<string>>()));

            AdminController adminController = CreateAdminController;

            List<string> roles = new List<string>();
            roles.Add("rendomRole");

            var result = await adminController.Edit("UserId", roles);

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task EditRole_Invalid_Test()
        {
            _adminService.Setup(a => a.EditAsync(It.IsAny<string>(), It.IsAny<List<string>>()));

            AdminController adminController = CreateAdminController;

            List<string> roles = new List<string>();
            roles.Add("rendomRole");

            var result = await adminController.Edit("", roles);

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_Valid_Test()
        {
            _adminService.Setup(a => a.DeleteUserAsync(It.IsAny<string>()));

            AdminController adminController = CreateAdminController;

            var result = await adminController.Delete("SomeUserId");

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
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
        public async Task RegionsAdmins_Valid_Test()
        {
            _cityService.Setup(c => c.GetAllDTOAsync(null))
                .ReturnsAsync(It.IsAny<IEnumerable<CityDTO>>());

            AdminController adminController = CreateAdminController;

            var result = await adminController.RegionsAdmins();

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RegionsAdmins_Invalid_Test()
        {
            _cityService.Setup(c => c.GetAllDTOAsync(null))
                .ReturnsAsync(() => null);

            AdminController adminController = CreateAdminController;

            var result = await adminController.RegionsAdmins();

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAdmins_Valid_Test()
        {
            _cityAdministrationService.Setup(c => c.GetAdministrationByIdAsync(It.IsAny<int>()));

            AdminController adminController = CreateAdminController;

            var result = await adminController.GetAdmins(2);

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
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
        public void ConfirmDelete_Invalid_Test()
        {
            AdminController adminController = CreateAdminController;

            var result = adminController.ConfirmDelete("");

            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        private readonly Mock<ILoggerService<AdminController>> _logger;
        private readonly Mock<IUserManagerService> _userManagerService;
        private readonly Mock<IAdminService> _adminService;
        private readonly Mock<ICityService> _cityService;
        private readonly Mock<ICityParticipantsService> _cityAdministrationService;

        private AdminController CreateAdminController => new AdminController(
            _logger.Object,
            _userManagerService.Object,
            _adminService.Object,
            _cityService.Object,
            _cityAdministrationService.Object
            );
    }
}
