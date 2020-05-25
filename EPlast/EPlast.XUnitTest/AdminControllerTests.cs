using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest
{
    public class AdminControllerTests
    {
        private Mock<IUserStore<User>> _userStoreMock;
        private Mock<UserManager<User>> _userManager;
        private Mock<ILogger<AdminController>> _logger;
        private Mock<RoleManager<IdentityRole>> _roleManager;
        private  Mock<IUserManagerService> _userManagerService;
        private  Mock<IAdminService> _adminService;
        private Mock<ICityService> _cityService;
        private Mock<ICItyAdministrationService> _cityAdministrationService;
        private  Mock<IMapper> _mapper;
        public AdminControllerTests()
        {
            _userStoreMock = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _roleManager = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object,
             new IRoleValidator<IdentityRole>[0],
             new Mock<ILookupNormalizer>().Object,
             new Mock<IdentityErrorDescriber>().Object,
             new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            _logger = new Mock<ILogger<AdminController>>();
            _userManagerService = new Mock<IUserManagerService>();
            _adminService = new Mock<IAdminService>();
            _mapper = new Mock<IMapper>();
            _cityService = new Mock<ICityService>();
            _cityAdministrationService = new Mock<ICItyAdministrationService>();
        }

        [Fact]
        public async Task EditGetTest()
        {
            var user = new UserDTO();
            _userManagerService.Setup(x => x.FindById(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerService.Setup(x => x.GetRoles(It.IsAny<UserDTO>())).ReturnsAsync(new List<string>());
            _adminService.Setup(x => x.GetRolesExceptAdmin()).Returns(new List<IdentityRole>().AsQueryable());

            var controller = new AdminController( _logger.Object, _userManagerService.Object, _adminService.Object, _mapper.Object,
                _cityService.Object, _cityAdministrationService.Object);
            // Act
            var result =await controller.Edit(user.Id);
            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            Assert.IsAssignableFrom<RoleViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task EditGetTestFailure()
        {
            var controller = new AdminController(_logger.Object, _userManagerService.Object, _adminService.Object, _mapper.Object,
                _cityService.Object, _cityAdministrationService.Object);
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
            var roles = new List<string>();

            var controller = new AdminController(_logger.Object, _userManagerService.Object, _adminService.Object, _mapper.Object,
                _cityService.Object, _cityAdministrationService.Object);
            // Act
            var result = await controller.Edit("1",roles);
            // Assert
             
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UsersTable", viewResult.ActionName);
        }

        [Fact]
        public async Task EditPostTestFailure()
        {
            var controller = new AdminController(_logger.Object, _userManagerService.Object, _adminService.Object, _mapper.Object,
                _cityService.Object, _cityAdministrationService.Object);
            // Act
            var result = await controller.Edit(null, new List<string>());
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public void ConfirmDeleteTest()
        {
            var controller = new AdminController(_logger.Object, _userManagerService.Object, _adminService.Object, _mapper.Object,
                _cityService.Object, _cityAdministrationService.Object);
            // Act
            var result = controller.ConfirmDelete("1");
            // Assert
            Assert.IsType<PartialViewResult>(result);
        }

        [Fact]
        public async Task DeleteTest()
        {
            var user = new List<User> { new User { Id="1"} };
            var controller = new AdminController(_logger.Object, _userManagerService.Object, _adminService.Object, _mapper.Object,
                _cityService.Object, _cityAdministrationService.Object);
            // Act
            var result = await controller.Delete(user.First().Id);
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UsersTable", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteTestFailure()
        {
            var controller = new AdminController(_logger.Object, _userManagerService.Object, _adminService.Object, _mapper.Object,
                _cityService.Object, _cityAdministrationService.Object);
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
            _cityService.Setup(x => x.GetAllDTO()).Returns(new List<CityDTO>());
            var controller = new AdminController(_logger.Object, _userManagerService.Object, _adminService.Object, _mapper.Object,
                _cityService.Object, _cityAdministrationService.Object);
            // Act
            var result = controller.RegionsAdmins();
            // Assert
            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<RegionsAdminsViewModel>(viewResult.Model);
        }

        [Fact]
        public void GetAdminsTest()
        {
            _cityAdministrationService.Setup(x => x.GetByCityId(It.IsAny<int>())).Returns(new List<CityAdministrationDTO>());
            _mapper.Setup(x => x.Map<IEnumerable<CityAdministrationDTO>, IEnumerable<CityAdministrationViewModel>>(It.IsAny<IEnumerable<CityAdministrationDTO>>()));
            var controller = new AdminController(_logger.Object, _userManagerService.Object, _adminService.Object, _mapper.Object,
                _cityService.Object, _cityAdministrationService.Object);
            // Act
            var result = controller.GetAdmins(1);
            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            Assert.IsAssignableFrom<CityAdministrationViewModel[]>(viewResult.Model);
        }

        [Fact]
        public async Task UsersTableTest()
        {

            _adminService.Setup(x => x.UsersTable()).ReturnsAsync(new List<UserTableDTO>());
            _mapper.Setup(x => x.Map<IEnumerable<UserTableDTO>, IEnumerable<UserTableViewModel>>(It.IsAny<IEnumerable<UserTableDTO>>()));
            var controller = new AdminController(_logger.Object, _userManagerService.Object, _adminService.Object, _mapper.Object,
                _cityService.Object, _cityAdministrationService.Object);
            // Act
            var result = await controller.UsersTable();
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<UserTableViewModel[]>(viewResult.Model);
            Assert.NotNull(result);
        }
    }
}

