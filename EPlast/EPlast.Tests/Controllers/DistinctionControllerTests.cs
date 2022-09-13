using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL;
using EPlast.BLL.Commands.Distinction;
using EPlast.BLL.DTO.Distinction;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    internal class DistinctionControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IUserDistinctionService> _userDistinctionService;
        private Mock<UserManager<User>> _userManager;

        private DistinctionController _distinctionController;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _userDistinctionService = new Mock<IUserDistinctionService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _distinctionController = new DistinctionController(                
                _userDistinctionService.Object,
                _userManager.Object,
                _mockMediator.Object
                );
        }

        [Test]
        public async Task GetUserDistinction_DistinctionById_ReturnsOkObjectResult()
        {
            //Arrange
            _userDistinctionService
                .Setup(x => x.GetUserDistinctionAsync(It.IsAny<int>()))
                .ReturnsAsync(new UserDistinctionDto());
            //Act
            var result = await _distinctionController.GetUserDistinction(It.IsAny<int>());
            var resultValue = (result as ObjectResult).Value;
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<UserDistinctionDto>(resultValue);
        }

        [Test]
        public async Task GetUserDistinction_DistinctionById_ReturnsNotFoundResult()
        {
            //Arrange
            _userDistinctionService
                .Setup(x => x.GetUserDistinctionAsync(It.IsAny<int>()))
                .ReturnsAsync((UserDistinctionDto)null);
            //Act
            var result = await _distinctionController.GetUserDistinction(It.IsAny<int>());
            var resultObject = (result as ObjectResult)?.Value;

            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(resultObject);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetUserDistinction_ReturnsOkObjectResult()
        {
            //Arrange
            _userDistinctionService
                .Setup(x => x.GetAllUsersDistinctionAsync())
                .ReturnsAsync((new List<UserDistinctionDto>()).AsEnumerable());
            //Act
            var result = await _distinctionController.GetUserDistinction();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<UserDistinctionDto>>(resultValue);
        }

        [Test]
        public async Task GetDistinction_DistinctionById_ReturnsOkObjectResult()
        {
            //Arrange
            _mockMediator
                .Setup(x => x.Send(It.IsAny<GetDistinctionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DistinctionDto());
            //Act
            var result = await _distinctionController.GetDistinction(It.IsAny<int>());
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<DistinctionDto>(resultValue);
        }

        [Test]
        public async Task GetDistinction_DistinctionById_ReturnsNotFoundResult()
        {
            //Arrange
            _mockMediator
                 .Setup(x => x.Send(It.IsAny<GetDistinctionQuery>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((DistinctionDto)null);
            //Act
            var result = await _distinctionController.GetDistinction(It.IsAny<int>());
            var resultObject = (result as ObjectResult)?.Value;
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(resultObject);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetDistinction_ReturnsOkObjectResult()
        {
            //Arrange
            _mockMediator
                .Setup(x => x.Send(It.IsAny<GetAllDistinctionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DistinctionDto>().AsEnumerable());
            //Act
            var result = await _distinctionController.GetDistinction();
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<DistinctionDto>>(resultValue);
        }

        [Test]
        public void GetUsersDistinctionsForTable_ReturnsOkObjectResult()
        {
            DistictionTableSettings TestDTS = new DistictionTableSettings();
            //Arrange
            _mockMediator
                 .Setup(x => x.Send(It.IsAny<GetUsersDistinctionsForTableQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(CreateTuple);

            //Act
            var result = _distinctionController.GetUsersDistinctionsForTable(It.IsAny<DistictionTableSettings>()).Result;
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<UserDistinctionsTableObject>>(resultValue);
        }

        [Test]
        public async Task GetDistinctionsOfGivenUser_ReturnsOkObjectResult()
        {
            //Arrange
            _userDistinctionService
                .Setup(x => x.GetUserDistinctionsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<UserDistinctionDto>().AsEnumerable());
            //Act
            var result = await _distinctionController.GetDistinctionOfGivenUser(It.IsAny<string>());
            var resultValue = (result as OkObjectResult).Value;
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<UserDistinctionDto>>(resultValue);
        }

        [Test]
        public async Task GetDistinctionsOfGivenUser_ReturnsNotFoundResult()
        {
            //Arrange
            _userDistinctionService
                .Setup(x => x.GetUserDistinctionsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync((List<UserDistinctionDto>)null);
            //Act
            var result = await _distinctionController.GetDistinctionOfGivenUser(It.IsAny<string>());
            var resultValue = (result as OkObjectResult)?.Value;
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(resultValue);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _mockMediator
                .Setup(x => x.Send(It.IsAny<DeleteDistinctionCommand>(), It.IsAny<CancellationToken>()));
            //Act
            var result = await _distinctionController.DeleteDistinction(It.IsAny<int>());
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteDistinction_ReturnsNullReferenceException()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _mockMediator
                .Setup(x => x.Send(It.IsAny<DeleteDistinctionCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new NullReferenceException());
            //Act
            var result = await _distinctionController.DeleteDistinction(It.IsAny<int>());
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteUserDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
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
        public async Task DeleteUserDistinction_ReturnsNullReferenceException()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _userDistinctionService
                .Setup(x => x.DeleteUserDistinctionAsync(It.IsAny<int>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());
            //Act
            var result = await _distinctionController.DeleteUserDistinction(It.IsAny<int>());
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task AddUserDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _userDistinctionService
                .Setup(x => x.AddUserDistinctionAsync(It.IsAny<UserDistinctionDto>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.AddUserDistinction(It.IsAny<UserDistinctionDto>());
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddUserDistinction_ReturnsNullReferenceException()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _userDistinctionService
                .Setup(x => x.AddUserDistinctionAsync(It.IsAny<UserDistinctionDto>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());
            //Act
            var result = await _distinctionController.AddUserDistinction(It.IsAny<UserDistinctionDto>());
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task AddUserDistinction_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionController.ModelState.AddModelError("firstName", "First Name field is required");
            _userDistinctionService
                .Setup(x => x.AddUserDistinctionAsync(It.IsAny<UserDistinctionDto>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.AddUserDistinction(It.IsAny<UserDistinctionDto>());
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
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _mockMediator
                .Setup(x => x.Send(It.IsAny<AddDistinctionCommand>(), It.IsAny<CancellationToken>()));
            //Act
            var result = await _distinctionController.AddDistinction(It.IsAny<DistinctionDto>());
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddDistinction_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionController.ModelState.AddModelError("name", "Name field is required");
            _mockMediator
                .Setup(x => x.Send(It.IsAny<AddDistinctionCommand>(), It.IsAny<CancellationToken>()));
            //Act
            var result = await _distinctionController.AddDistinction(It.IsAny<DistinctionDto>());
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangeUserDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor(), new ModelStateDictionary()));
            _distinctionController.ControllerContext = context;
            _userDistinctionService
                .Setup(x => x.ChangeUserDistinctionAsync(It.IsAny<UserDistinctionDto>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.EditUserDistinction(It.IsAny<UserDistinctionDto>());

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
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionController.ModelState.AddModelError("firstName", "First Name field is required");
            _userDistinctionService
                .Setup(x => x.ChangeUserDistinctionAsync(It.IsAny<UserDistinctionDto>(), It.IsAny<User>()));
            //Act
            var result = await _distinctionController.EditUserDistinction(It.IsAny<UserDistinctionDto>());
            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangeUserDistinction_ReturnsNullReferenceException()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor(), new ModelStateDictionary()));
            _distinctionController.ControllerContext = context;
            _userDistinctionService
                .Setup(x => x.ChangeUserDistinctionAsync(It.IsAny<UserDistinctionDto>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());
            //Act
            var result = await _distinctionController.EditUserDistinction(It.IsAny<UserDistinctionDto>());

            //Assert
            _userDistinctionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task ChangeDistinction_ReturnsNoContentResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _mockMediator
                .Setup(x => x.Send(It.IsAny<ChangeDistinctionCommand>(), It.IsAny<CancellationToken>()));
            //Act
            var result = await _distinctionController.EditDistinction(It.IsAny<DistinctionDto>());
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task ChangeDistinction_ReturnsBadRequestResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _distinctionController.ModelState.AddModelError("name", "Name field is required");
            _mockMediator
                .Setup(x => x.Send(It.IsAny<ChangeDistinctionCommand>(), It.IsAny<CancellationToken>()));
            //Act
            var result = await _distinctionController.EditDistinction(It.IsAny<DistinctionDto>());
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangeDistinction_ReturnsNullReferenceException()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _distinctionController.ControllerContext = context;
            _mockMediator
                .Setup(x => x.Send(It.IsAny<ChangeDistinctionCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new NullReferenceException());
            //Act
            var result = await _distinctionController.EditDistinction(It.IsAny<DistinctionDto>());
            //Assert
            _mockMediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(1)]
        public async Task CheckNumberExisting_ReturnsOkObjectResult_Test(int number)
        {
            //Arrange
            _userDistinctionService.Setup(x => x.IsNumberExistAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            //Act
            var result = await _distinctionController.CheckNumberExisting(number);
            var resultObject = (result as ObjectResult).Value;
            

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, resultObject);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private List<UserDistinctionsTableObject> GetUsersDistinctionByPage()
        {
            return new List<UserDistinctionsTableObject>()
            {
                new UserDistinctionsTableObject()
                {
                    Number = 34,
                }
            };
        }

        private int GetFakeUserDistinctionNumber()
        {
            return 100;
        }

        private Tuple<IEnumerable<UserDistinctionsTableObject>, int> CreateTuple => new Tuple<IEnumerable<UserDistinctionsTableObject>, int>(GetUsersDistinctionByPage(), GetFakeUserDistinctionNumber());
    }
}
