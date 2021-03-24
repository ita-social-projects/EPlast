using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using EPlast.Resources;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    internal class PrecautionControllerTests
    {
        private Mock<IPrecautionService> _precautionService;
        private Mock<IUserPrecautionService> _userPrecautionService;
        private Mock<UserManager<User>> _userManager;

        private PrecautionController _PrecautionController;

        [SetUp]
        public void SetUp()
        {
            _precautionService = new Mock<IPrecautionService>();
            _userPrecautionService = new Mock<IUserPrecautionService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _PrecautionController = new PrecautionController(
                _precautionService.Object,
                _userPrecautionService.Object,
                _userManager.Object
                );
        }

        [Test]
        public async Task GetUserPrecaution_PrecautionById_ReturnsOkObjectResult()
        {
            //Arrange
            _userPrecautionService
                .Setup(x => x.GetUserPrecautionAsync(It.IsAny<int>()))
                .ReturnsAsync(new UserPrecautionDTO());
            //Act
            var result = await _PrecautionController.GetUserPrecaution(It.IsAny<int>());
            var resultValue = (result as ObjectResult).Value;
            //Assert
            _userPrecautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<UserPrecautionDTO>(resultValue);
        }

        [Test]
        public async Task GetUserPrecaution_PrecautionById_ReturnsNotFoundResult()
        {
            //Arrange
            int id = 0;
            _userPrecautionService
                .Setup(x => x.GetUserPrecautionAsync(id))
                .ReturnsAsync((UserPrecautionDTO)null);
            //Act
            var result = await _PrecautionController.GetUserPrecaution(id);
            
            //Assert
            _userPrecautionService.Verify();
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetUserPrecaution_ReturnsOkObjectResult()
        {
            //Arrange
            _userPrecautionService
                .Setup(x => x.GetAllUsersPrecautionAsync())
                .ReturnsAsync((new List<UserPrecautionDTO>()).AsEnumerable());
            //Act
            var result = await _PrecautionController.GetUserPrecaution();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _userPrecautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<UserPrecautionDTO>>(resultValue);
        }

        [Test]
        public async Task GetPrecaution_PrecautionById_ReturnsOkObjectResult()
        {
            //Arrange
            _precautionService
                .Setup(x => x.GetPrecautionAsync(It.IsAny<int>()))
                .ReturnsAsync(new PrecautionDTO());
            //Act
            var result = await _PrecautionController.GetPrecaution(It.IsAny<int>());
            var resultValue = (result as OkObjectResult).Value as PrecautionDTO;
            //Assert
            _precautionService.Verify();
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PrecautionDTO>(resultValue);
        }

        [Test]
        public async Task GetPrecaution_PrecautionById_ReturnsNotFoundResult()
        {
            //Arrange
            _precautionService
                .Setup(x => x.GetPrecautionAsync(It.IsAny<int>()))
                .ReturnsAsync((PrecautionDTO)null);
            PrecautionController precautionController = _PrecautionController;
            //Act
            var result = await precautionController.GetPrecaution(It.IsAny<int>());
            //Assert
            _precautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetPrecaution_ReturnsOkObjectResult()
        {
            //Arrange
            _precautionService
                .Setup(x => x.GetAllPrecautionAsync())
                .ReturnsAsync(new List<PrecautionDTO>().AsEnumerable());
            //Act
            var result = await _PrecautionController.GetPrecaution();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _precautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<PrecautionDTO>>(resultValue);
        }

        [Test]
        public async Task GetPrecautionsOfGivenUser_ReturnsOkObjectResult()
        {
            //Arrange
            _userPrecautionService
                .Setup(x => x.GetUserPrecautionsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<UserPrecautionDTO>().AsEnumerable());
            //Act
            var result = await _PrecautionController.GetPrecautionOfGivenUser(It.IsAny<string>());
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _userPrecautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<UserPrecautionDTO>>(resultValue);
        }

        [Test]
        public async Task DeletePrecaution_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _PrecautionController.ControllerContext = context;
            _precautionService
                .Setup(x => x.DeletePrecautionAsync(It.IsAny<int>(), It.IsAny<User>()));
            PrecautionController precautionController = _PrecautionController;
            //Act
            var result = await precautionController.DeletePrecaution(It.IsAny<int>());
            //Assert
            _precautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteUserPrecaution_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _PrecautionController.ControllerContext = context;
            _userPrecautionService
                .Setup(x => x.DeleteUserPrecautionAsync(It.IsAny<int>(), It.IsAny<User>()));
            //Act
            var result = await _PrecautionController.DeleteUserPrecaution(It.IsAny<int>());
            //Assert
            _userPrecautionService.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddUserPrecaution_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _PrecautionController.ControllerContext = context;
            _userPrecautionService
                .Setup(x => x.AddUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _PrecautionController.AddUserPrecaution(It.IsAny<UserPrecautionDTO>());
            //Assert
            _userPrecautionService.Verify();
            _userManager.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddUserPrecaution_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _PrecautionController.ControllerContext = context;
            _PrecautionController.ModelState.AddModelError("firstName", "First Name field is required");
            _userPrecautionService
                .Setup(x => x.AddUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _PrecautionController.AddUserPrecaution(It.IsAny<UserPrecautionDTO>());
            //Assert
            _userPrecautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddPrecaution_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _PrecautionController.ControllerContext = context;
            _precautionService
                .Setup(x => x.AddPrecautionAsync(It.IsAny<PrecautionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _PrecautionController.AddPrecaution(It.IsAny<PrecautionDTO>());
            //Assert
            _precautionService.Verify();
            _userManager.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddPrecaution_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _PrecautionController.ControllerContext = context;
            _PrecautionController.ModelState.AddModelError("name", "Name field is required");
            _precautionService
                .Setup(x => x.AddPrecautionAsync(It.IsAny<PrecautionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _PrecautionController.AddPrecaution(It.IsAny<PrecautionDTO>());
            //Assert
            _precautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangeUserPrecaution_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _PrecautionController.ControllerContext = context;
            _userPrecautionService
                .Setup(x => x.ChangeUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _PrecautionController.EditUserPrecaution(It.IsAny<UserPrecautionDTO>());
            //Assert
            _userPrecautionService.Verify();
            _userManager.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task ChangeUserPrecaution_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _PrecautionController.ControllerContext = context;
            _PrecautionController.ModelState.AddModelError("firstName", "First Name field is required");
            _userPrecautionService
                .Setup(x => x.ChangeUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _PrecautionController.EditUserPrecaution(It.IsAny<UserPrecautionDTO>());
            //Assert
            _userPrecautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangePrecaution_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _PrecautionController.ControllerContext = context;
            _precautionService
                .Setup(x => x.ChangePrecautionAsync(It.IsAny<PrecautionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _PrecautionController.EditPrecaution(It.IsAny<PrecautionDTO>());
            //Assert
            _precautionService.Verify();
            _userManager.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task ChangePrecaution_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _PrecautionController.ControllerContext = context;
            _PrecautionController.ModelState.AddModelError("name", "Name field is required");
            _precautionService
                .Setup(x => x.ChangePrecautionAsync(It.IsAny<PrecautionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _PrecautionController.EditPrecaution(It.IsAny<PrecautionDTO>());
            //Assert
            _precautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UsersTableWithoutPrecautions_Valid_Test()
        {
            _userPrecautionService.Setup(a => a.UsersTableWithotPrecautionAsync());

            // Act
            var result = await _PrecautionController.UsersWithoutPrecautionsTable();
            var resultValue = (result as OkObjectResult).Value;

            // Assert
            _userPrecautionService.Verify();
            Assert.IsInstanceOf<IEnumerable<UserTableDTO>>(resultValue);
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        
        [TestCase(1)]
        public async Task CheckNumberExisting_ReturnsOkObjectResult_Test(int number)
        {
            //Arrange
            _userPrecautionService.Setup(x => x.IsNumberExistAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            //Act
            var result = await _PrecautionController.CheckNumberExisting(number);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            Assert.IsInstanceOf<bool>(resultValue);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
