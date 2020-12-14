using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using EPlast.BLL;
using EPlast.WebApi.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Identity;
using EPlast.DataAccess.Entities;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class DistinctionControllerTests
    {
        private Mock<IDistinctionService> _distinctionService;
        private Mock<IUserDistinctionService> _userDistinctionService;
        private UserManager<User> _userManager;

        private DistinctionController _distinctionController;
        
        [SetUp]
        public void SetUp()
        {
            _distinctionService = new Mock<IDistinctionService>();
            _userDistinctionService = new Mock<IUserDistinctionService>();

            _distinctionController = new DistinctionController(
                _distinctionService.Object,
                _userDistinctionService.Object,
                _userManager
                );
        }

        [Test]
        public async Task GetUserDistinction_DistinctionById_ReturnsOkObjectResult()
        {
            //Arrange
            _userDistinctionService
                .Setup(x => x.GetUserDistinctionAsync(It.IsAny<int>()))
                .ReturnsAsync(new UserDistinctionDTO());
            //Act
            var result = await _distinctionController.GetUserDistinction(It.IsAny<int>());
            var resultValue = (result as ObjectResult).Value;
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<UserDistinctionDTO>(resultValue);
        }

        [Test]
        public async Task GetUserDistinction_DistinctionById_ReturnsNotFoundResult()
        {
            //Arrange
            _userDistinctionService
                .Setup(x => x.GetUserDistinctionAsync(It.IsAny<int>()))
                .ReturnsAsync((UserDistinctionDTO)null);
            //Act
            var result = await _distinctionController.GetUserDistinction(It.IsAny<int>());
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetUserDistinction_ReturnsOkObjectResult()
        {
            //Arrange
            _userDistinctionService
                .Setup(x => x.GetAllUsersDistinctionAsync())
                .ReturnsAsync((new List<UserDistinctionDTO>()).AsEnumerable());
            //Act
            var result = await _distinctionController.GetUserDistinction();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<UserDistinctionDTO>>(resultValue);
        }

        [Test]
        public async Task GetDistinction_DistinctionById_ReturnsOkObjectResult()
        {
            //Arrange
            _distinctionService
                .Setup(x => x.GetDistinctionAsync(It.IsAny<int>()))
                .ReturnsAsync(new DistinctionDTO());
            //Act
            var result = await _distinctionController.GetDistinction(It.IsAny<int>());
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _distinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<DistinctionDTO>(resultValue);
        }

        [Test]
        public async Task GetDistinction_DistinctionById_ReturnsNotFoundResult()
        {
            //Arrange
            _distinctionService
                .Setup(x => x.GetDistinctionAsync(It.IsAny<int>()))
                .ReturnsAsync((DistinctionDTO)null);
            //Act
            var result = await _distinctionController.GetDistinction(It.IsAny<int>());
            //Assert
            _distinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetDistinction_ReturnsOkObjectResult()
        {
            //Arrange
            _distinctionService
                .Setup(x => x.GetAllDistinctionAsync())
                .ReturnsAsync(new List<DistinctionDTO>().AsEnumerable());
            //Act
            var result = await _distinctionController.GetDistinction();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _distinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<DistinctionDTO>>(resultValue);
        }

        [Test]
        public async Task GetDistinctionsOfGivenUser_ReturnsOkObjectResult()
        {
            //Arrange
            _userDistinctionService
                .Setup(x => x.GetUserDistinctionsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<UserDistinctionDTO>().AsEnumerable());
            //Act
            var result = await _distinctionController.GetDistinctionOfGivenUser(It.IsAny<string>());
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<UserDistinctionDTO>>(resultValue);
        }
        
        [Test]
        public async Task DeleteDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionService
                .Setup(x => x.DeleteDistinctionAsync(It.IsAny<int>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.DeleteDistinction(It.IsAny<int>());
            //Assert
            _distinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteUserDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _userDistinctionService
                .Setup(x => x.DeleteUserDistinctionAsync(It.IsAny<int>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.DeleteUserDistinction(It.IsAny<int>());
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddUserDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _userDistinctionService
                .Setup(x => x.AddUserDistinctionAsync(It.IsAny<UserDistinctionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.AddUserDistinction(It.IsAny<UserDistinctionDTO>());
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddUserDistinction_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionController.ModelState.AddModelError("firstName", "First Name field is required");
            _userDistinctionService
                .Setup(x => x.AddUserDistinctionAsync(It.IsAny<UserDistinctionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.AddUserDistinction(It.IsAny<UserDistinctionDTO>());
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionService
                .Setup(x => x.AddDistinctionAsync(It.IsAny<DistinctionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.AddDistinction(It.IsAny<DistinctionDTO>());
            //Assert
            _distinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddDistinction_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionController.ModelState.AddModelError("name", "Name field is required");
            _distinctionService
                .Setup(x => x.AddDistinctionAsync(It.IsAny<DistinctionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.AddDistinction(It.IsAny<DistinctionDTO>());
            //Assert
            _distinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangeUserDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _userDistinctionService
                .Setup(x => x.ChangeUserDistinctionAsync(It.IsAny<UserDistinctionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.EditUserDistinction(It.IsAny<UserDistinctionDTO>());
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task ChangeUserDistinction_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionController.ModelState.AddModelError("firstName", "First Name field is required");
            _userDistinctionService
                .Setup(x => x.ChangeUserDistinctionAsync(It.IsAny<UserDistinctionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.EditUserDistinction(It.IsAny<UserDistinctionDTO>());
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangeDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionService
                .Setup(x => x.ChangeDistinctionAsync(It.IsAny<DistinctionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.EditDistinction(It.IsAny<DistinctionDTO>());
            //Assert
            _distinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task ChangeDistinction_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionController.ModelState.AddModelError("name", "Name field is required");
            _distinctionService
                .Setup(x => x.ChangeDistinctionAsync(It.IsAny<DistinctionDTO>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.EditDistinction(It.IsAny<DistinctionDTO>());
            //Assert
            _distinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }


    }
}
