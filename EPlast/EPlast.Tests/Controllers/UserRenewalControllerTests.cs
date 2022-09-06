using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    internal class UserRenewalControllerTests
    {
        private Mock<IUserRenewalService> _mockUserRenewalService;
        private Mock<IUserManagerService> _mockUserManagerService;
        private Mock<HttpContext> _mockHttpContext = new Mock<HttpContext>();
        private Mock<UserManager<User>> _mockUserManager;

        private UserRenewalController _userRenewalController;
        private ControllerContext _context;

        [SetUp]
        public void SetUp()
        {
            _mockUserRenewalService = new Mock<IUserRenewalService>();
            _mockUserManagerService = new Mock<IUserManagerService>();
            var mockUser = new Mock<IUserStore<User>>();
            _mockUserManager =
                new Mock<UserManager<User>>(mockUser.Object, null, null, null, null, null, null, null, null);
            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpContext
                .Setup(u => u.User.IsInRole(Roles.AdminAndCityHeadAndCityHeadDeputy))
                .Returns(true);

            _userRenewalController = new UserRenewalController(
                _mockUserRenewalService.Object,
                _mockUserManager.Object);
            _context = new ControllerContext(
                new ActionContext(
                    _mockHttpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
        }

        [Test]
        public void GetUserRenewalsForTable_ReturnsOkObjectResult()
        {
            //Arrange
            _mockUserRenewalService
                .Setup(r => r.GetUserRenewalsTableObject(It.IsAny<string>(), 
                    It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<UserRenewalsTableObject>());

            //Act
            var result =
                _userRenewalController.GetUserRenewalsForTable(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<int>());
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            _mockUserRenewalService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<UserRenewalsTableObject>>(resultValue);
        }

        [Test]
        public async Task FormerCheck_ReturnsOkObjectResult()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            //Act
            var result = await _userRenewalController.FormerCheck(It.IsAny<string>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mockUserManagerService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<string>(resultValue);
        }

        [Test]
        public async Task FormerCheck_ReturnsBadRequestResult()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            //Act
            var result = await _userRenewalController.FormerCheck(It.IsAny<string>());

            //Assert
            _mockUserManagerService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddUserRenewal_ReturnsBadRequestResult()
        {
            //Arrange
            _userRenewalController.ModelState.AddModelError("Bad request", "Cannot create renewal");
            _mockUserRenewalService
                .Setup(r => r.AddUserRenewalAsync(It.IsAny<UserRenewalDto>()));

            //Act
            var result = await _userRenewalController.AddUserRenewalAsync(It.IsAny<UserRenewalDto>());

            //Assert
            _mockUserRenewalService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddUserRenewal_ThrowsException_ReturnsNotFound()
        {
            //Arrange
            _mockUserRenewalService
                .Setup(r => r.AddUserRenewalAsync(It.IsAny<UserRenewalDto>()))
                .Throws(new Exception());
            _mockUserRenewalService
                .Setup(r => r.IsValidUserRenewalAsync(It.IsAny<UserRenewalDto>()))
                .ReturnsAsync(true);

            //Act
            var result = await _userRenewalController.AddUserRenewalAsync(new UserRenewalDto());

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task AddUserRenewal_ReturnsNoContentResult()
        {
            //Arrange
            _mockUserRenewalService
                .Setup(r => r.AddUserRenewalAsync(It.IsAny<UserRenewalDto>()));
            _mockUserRenewalService
                .Setup(r => r.IsValidUserRenewalAsync(It.IsAny<UserRenewalDto>()))
                .ReturnsAsync(true);

            //Act
            var result = await _userRenewalController.AddUserRenewalAsync(It.IsAny<UserRenewalDto>());

            //Assert
            _mockUserRenewalService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task RenewUser_ReturnsOkObjectResult()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockUserRenewalService
                .Setup(r => r.IsValidAdminAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockUserRenewalService
                .Setup(r => r.RenewFormerMemberUserAsync(It.IsAny<UserRenewalDto>()))
                .ReturnsAsync(new CityMembersDto());
            _mockUserRenewalService
                .Setup(r => r.SendRenewalConfirmationEmailAsync(It.IsAny<string>(), It.IsAny<int>()));

            //Act
            var result = await _userRenewalController.RenewUser(userRenewalDTO);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mockUserRenewalService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<CityMembersDto>(resultValue);
        }

        [Test]
        public async Task RenewUser_ReturnsBadRequestResult()
        {
            //Arrange
            _userRenewalController.ModelState.AddModelError("Model", "Unable to apply the renewal");
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockUserRenewalService
                .Setup(r => r.IsValidAdminAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockUserRenewalService
                .Setup(r => r.RenewFormerMemberUserAsync(It.IsAny<UserRenewalDto>()))
                .ReturnsAsync(new CityMembersDto());
            _mockUserRenewalService
                .Setup(r => r.SendRenewalConfirmationEmailAsync(It.IsAny<string>(), It.IsAny<int>()));

            //Act
            var result = await _userRenewalController.RenewUser(userRenewalDTO);

            //Assert
            _mockUserRenewalService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task RenewUser_ReturnsBadRequestResultUnfortunateRenewal()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockUserRenewalService
                .Setup(r => r.IsValidAdminAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockUserRenewalService
                .Setup(r => r.RenewFormerMemberUserAsync(It.IsAny<UserRenewalDto>()))
                .ThrowsAsync(new ArgumentException());
            _mockUserRenewalService
                .Setup(r => r.SendRenewalConfirmationEmailAsync(It.IsAny<string>(), It.IsAny<int>()));

            //Act
            var result = await _userRenewalController.RenewUser(userRenewalDTO);

            //Assert
            _mockUserRenewalService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task RenewUser_ReturnsUnauthorized()
        {
            //Arrange
            _mockUserManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockUserRenewalService
                .Setup(r => r.IsValidAdminAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(false);
            _mockUserRenewalService
                .Setup(r => r.RenewFormerMemberUserAsync(It.IsAny<UserRenewalDto>()))
                .ReturnsAsync(new CityMembersDto());
            _mockUserRenewalService
                .Setup(r => r.SendRenewalConfirmationEmailAsync(It.IsAny<string>(), It.IsAny<int>()));

            //Act
            var result = await _userRenewalController.RenewUser(userRenewalDTO);

            //Assert
            _mockUserRenewalService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }

        private readonly UserRenewalDto userRenewalDTO = new UserRenewalDto
        {
            Id = 1,
            CityId = 13,
            Approved = true,
            UserId = "660c4f26-760d-46f9-b780-5b5c7c153b25",
            RequestDate = new DateTime()
        };
    }
}
