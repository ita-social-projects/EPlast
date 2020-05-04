using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest
{
    public class AdminControllerTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IUserStore<User>> _userStoreMock;
        private Mock<UserManager<User>> _userManager;
        private Mock<ILogger<AdminController>> _logger;
        private Mock<RoleManager<IdentityRole>> _roleManager;
        public AdminControllerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userStoreMock = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _roleManager = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object,
             new IRoleValidator<IdentityRole>[0],
             new Mock<ILookupNormalizer>().Object,
             new Mock<IdentityErrorDescriber>().Object,
             new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            _logger = new Mock<ILogger<AdminController>>();
        }

        [Fact]
        public async Task EditGetTest()
        {
            var user = new User();
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
            _roleManager.Setup(x => x.Roles).Returns(new List<IdentityRole>().AsQueryable());

            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result =await controller.Edit(user.Id);
            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            Assert.IsAssignableFrom<RoleViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task EditGetTestFailure()
        {
            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result = await controller.Edit("1");
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task EditPostTest()
        {
            var user = new User();
            var roles = new List<string>();
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result = await controller.Edit(user.Id,roles);
            // Assert
             
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UsersTable", viewResult.ActionName);
        }

        [Fact]
        public async Task EditPostTestFailure()
        {
            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result = await controller.Edit("1", new List<string>());
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public void ConfirmDeleteTest()
        {
            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result = controller.ConfirmDelete("1");
            // Assert
            Assert.IsType<PartialViewResult>(result);
        }

        [Fact]
        public async Task DeleteTest()
        {
            var user = new List<User> { new User()};
            var roles = new List<string> { "Admin"};
            _repoWrapper.Setup(x => x.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(user.AsQueryable());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result = await controller.Delete(user.First().Id);
            // Assert
            _repoWrapper.Verify(r => r.User.Delete(It.IsAny<User>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
            
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UsersTable", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteTestFailure()
        {
            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result = await controller.Delete(null);
            // Assert
            var viewResult=Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public void RegionsAdminsTest()
        {
            _repoWrapper.Setup(x => x.City.FindAll()).Returns(new List<City>().AsQueryable());
            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result = controller.RegionsAdmins();
            // Assert
            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<RegionsAdmins>(viewResult.Model);
        }

        [Fact]
        public void GetAdminsTest()
        {
            _repoWrapper.Setup(x => x.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>())).Returns(new List<CityAdministration>().AsQueryable());
            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result = controller.GetAdmins(1);
            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            Assert.IsAssignableFrom<IQueryable<CityAdministration>>(viewResult.Model);
        }

        [Fact]
        public async Task UsersTableTest()
        {
            var user = new User();
            _repoWrapper.Setup(x => x.User.FindAll()).Returns(new List<User>().AsQueryable());
            _repoWrapper.Setup(x => x.City.FindAll()).Returns(new List<City>().AsQueryable());
            _repoWrapper.Setup(x => x.ClubMembers.FindAll()).Returns(new List<ClubMembers>().AsQueryable());
            _repoWrapper.Setup(x => x.CityMembers.FindAll()).Returns(new List<CityMembers>().AsQueryable());
            _repoWrapper.Setup(x => x.UserPlastDegrees.FindAll()).Returns(new List<UserPlastDegree>().AsQueryable());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result = await controller.UsersTable();
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<UserTableViewModel>>(viewResult.Model);

        }

        [Fact]
        public async Task UsersTableTestFailure()
        {
            var controller = new AdminController(_roleManager.Object, _userManager.Object, _repoWrapper.Object, _logger.Object);
            // Act
            var result = await controller.UsersTable();
            // Assert
            var viewResult=Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }
    }
}

