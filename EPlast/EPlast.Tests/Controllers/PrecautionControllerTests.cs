using System;
using EPlast.BLL;
using EPlast.BLL.DTO.UserProfiles;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.PrecautionsDTO;
using MediatR;
using EPlast.BLL.Queries.Precaution;
using System.Threading;
using EPlast.BLL.Commands.Precaution;

namespace EPlast.Tests.Controllers
{
    internal class PrecautionControllerTests
    {        
        private Mock<IMediator> _mediator;
        private Mock<IUserPrecautionService> _userPrecautionService;
        private Mock<UserManager<User>> _userManager;
        private Mock<HttpContext> _httpContext = new Mock<HttpContext>();

        private PrecautionController _PrecautionController;
        private ControllerContext _context;
        private Mock<SuggestedUserDto> _availableUserDTOMock;

        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<IMediator>();
            _userPrecautionService = new Mock<IUserPrecautionService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _httpContext = new Mock<HttpContext>();
            _httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);

            _PrecautionController = new PrecautionController(
                _userPrecautionService.Object,
                _userManager.Object,
                _mediator.Object
                );
            _context = new ControllerContext(
                new ActionContext(
                    _httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));

            _availableUserDTOMock = new Mock<SuggestedUserDto>();
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
        public void GetUsersPrecautionsForTable_ReturnsOkObjectResult()
        {
            //Arrange
            _userPrecautionService
                .Setup(s => s.GetUserPrecautionsForTableAsync(It.IsAny<PrecautionTableSettings>()))
                .ReturnsAsync(new UserPrecautionsTableInfo());

            //Act
            var result = _PrecautionController.GetUsersPrecautionsForTable(It.IsAny<PrecautionTableSettings>()).Result;
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<UserPrecautionsTableInfo>(resultValue);
        }

        [Test]
        public async Task GetPrecaution_PrecautionById_ReturnsOkObjectResult()
        {
            //Arrange
            _mediator
                .Setup(x => x.Send(It.IsAny<GetPrecautionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PrecautionDTO());

            //Act
            var result = await _PrecautionController.GetPrecaution(It.IsAny<int>());
            var resultValue = (result as OkObjectResult).Value as PrecautionDTO;

            //Assert
            _mediator.Verify();
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PrecautionDTO>(resultValue);
        }

        [Test]
        public async Task GetPrecaution_PrecautionById_ReturnsNotFoundResult()
        {
            //Arrange
            _mediator
                .Setup(x => x.Send(It.IsAny<GetPrecautionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PrecautionDTO)null);
            PrecautionController precautionController = _PrecautionController;

            //Act
            var result = await precautionController.GetPrecaution(It.IsAny<int>());

            //Assert
            _mediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetAllPrecaution_ReturnsOkObjectResult()
        {
            //Arrange
            _mediator
                .Setup(x => x.Send(It.IsAny<GetAllPrecautionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PrecautionDTO>().AsEnumerable());

            //Act
            var result = await _PrecautionController.GetPrecaution();
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mediator.Verify();
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
        public async Task GetPrecautionsOfGivenUser_ReturnsNotFoundResult()
        {
            //Arrange
            _userPrecautionService
                .Setup(x => x.GetUserPrecautionsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync((List<UserPrecautionDTO>)null);
            //Act
            var result = await _PrecautionController.GetPrecautionOfGivenUser(It.IsAny<string>());
            var resultValue = (result as OkObjectResult)?.Value;
            //Assert
            _userPrecautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsNull(resultValue);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeletePrecaution_ReturnsNoContentResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _mediator
                .Setup(x => x.Send(It.IsAny<DeletePrecautionCommand>(), It.IsAny<CancellationToken>()));
            PrecautionController precautionController = _PrecautionController;

            //Act
            var result = await precautionController.DeletePrecaution(It.IsAny<int>());

            //Assert
            _mediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeletePrecaution_ReturnsNullReferenceException()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _mediator
                .Setup(x => x.Send(It.IsAny<DeletePrecautionCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new NullReferenceException());
            PrecautionController precautionController = _PrecautionController;

            //Act
            var result = await precautionController.DeletePrecaution(It.IsAny<int>());

            //Assert
            _mediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteUserPrecaution_ReturnsNoContentResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _userPrecautionService
                .Setup(x => x.DeleteUserPrecautionAsync(It.IsAny<int>(), It.IsAny<User>()));
            //Act
            var result = await _PrecautionController.DeleteUserPrecaution(It.IsAny<int>());
            //Assert
            _userPrecautionService.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteUserPrecaution_ReturnsNullReferenceException()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _userPrecautionService
                .Setup(x => x.DeleteUserPrecautionAsync(It.IsAny<int>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());
            //Act
            var result = await _PrecautionController.DeleteUserPrecaution(It.IsAny<int>());
            //Assert
            _userPrecautionService.Verify();
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task AddUserPrecaution_ReturnsNoContentResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _userPrecautionService
                .Setup(x => x.AddUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>())).ReturnsAsync(true);
            
            //Act
            var result = await _PrecautionController.AddUserPrecaution(It.IsAny<UserPrecautionDTO>());
           
            //Assert
            _userManager.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddUserPrecaution_InvalidModel_ReturnsBadRequestResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _PrecautionController.ModelState.AddModelError("firstName", "First Name field is required");
            _userPrecautionService
                .Setup(x => x.AddUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>()));
            
            //Act
            var result = await _PrecautionController.AddUserPrecaution(It.IsAny<UserPrecautionDTO>());
           
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddUserPrecaution_PrecautionWasNotCreated_ReturnsBadRequestResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _userPrecautionService
                .Setup(x => x.AddUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>())).ReturnsAsync(false);
            
            //Act
            var result = await _PrecautionController.AddUserPrecaution(It.IsAny<UserPrecautionDTO>());
            
            //Assert
            _userManager.Verify();
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task AddPrecaution_ReturnsNoContentResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _mediator
                .Setup(x => x.Send(It.IsAny<AddPrecautionCommand>(), It.IsAny<CancellationToken>()));

            //Act
            var result = await _PrecautionController.AddPrecaution(It.IsAny<PrecautionDTO>());

            //Assert
            _mediator.Verify();
            _userManager.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddPrecaution_ReturnsBadRequestResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _PrecautionController.ModelState.AddModelError("name", "Name field is required");
            _mediator
                .Setup(x => x.Send(It.IsAny<AddPrecautionCommand>(), It.IsAny<CancellationToken>()));

            //Act
            var result = await _PrecautionController.AddPrecaution(It.IsAny<PrecautionDTO>());

            //Assert
            _mediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangeUserPrecaution_Successful_ReturnsNoContentResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _userPrecautionService
                .Setup(x => x.ChangeUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>())).ReturnsAsync(true);
            
            //Act
            var result = await _PrecautionController.EditUserPrecaution(It.IsAny<UserPrecautionDTO>());
            
            //Assert
            _userManager.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task ChangeUserPrecaution_InvalidModel_ReturnsBadRequestResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _PrecautionController.ModelState.AddModelError("firstName", "First Name field is required");
            _userPrecautionService
                .Setup(x => x.ChangeUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>())).ReturnsAsync(true);
            
            //Act
            var result = await _PrecautionController.EditUserPrecaution(It.IsAny<UserPrecautionDTO>());
            
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangeUserPrecaution_CannotChangePrecaution_ReturnsNotFound()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _userPrecautionService
                .Setup(x => x.ChangeUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>())).ReturnsAsync(false);

            //Act
            var result = await _PrecautionController.EditUserPrecaution(It.IsAny<UserPrecautionDTO>());

            //Assert
            _userManager.Verify();
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task ChangePrecaution_ReturnsNoContentResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _mediator
                .Setup(x => x.Send(It.IsAny<ChangePrecautionCommand>(), It.IsAny<CancellationToken>()));

            //Act
            var result = await _PrecautionController.EditPrecaution(It.IsAny<PrecautionDTO>());

            //Assert
            _mediator.Verify();
            _userManager.Verify();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task ChangePrecaution_ReturnsBadRequestResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _PrecautionController.ModelState.AddModelError("name", "Name field is required");
            _mediator
                .Setup(x => x.Send(It.IsAny<AddPrecautionCommand>(), It.IsAny<CancellationToken>()));

            //Act
            var result = await _PrecautionController.EditPrecaution(It.IsAny<PrecautionDTO>());

            //Assert
            _mediator.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangePrecaution_ReturnsNullReferenceException()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _mediator
                .Setup(x => x.Send(It.IsAny<ChangePrecautionCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new NullReferenceException());

            //Act
            var result = await _PrecautionController.EditPrecaution(It.IsAny<PrecautionDTO>());

            //Assert
            _mediator.Verify();
            _userManager.Verify();
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase("tab")]
        public async Task UsersTableWithoutPrecautions_Valid_Test(string tab)
        {
            _userPrecautionService.Setup(a => a.UsersTableWithoutPrecautionAsync());

            // Act
            var result = await _PrecautionController.UsersWithoutPrecautionsTable(tab);
            var resultValue = (result as OkObjectResult).Value;

            // Assert
            _userPrecautionService.Verify();
            Assert.IsInstanceOf<IEnumerable<ShortUserInformationDTO>>(resultValue);
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(1)]
        public async Task CheckNumberExisting_ReturnsOkObjectResult_Test(int number)
        {
            //Arrange
            _userPrecautionService.Setup(x => x.IsNumberExistAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            //Act
            var result = await _PrecautionController.CheckNumberExisting(number);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            Assert.IsInstanceOf<bool>(resultValue);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase("a84473c3-140b-4cae-ac80-b7cd5759d3b5", "За силу")]
        public async Task CheckUserPrecautionsType_ReturnsOkObjectResult_Test(string userId, string type)
        {
            //Arrange
            _userPrecautionService
                .Setup(x => x.GetUserPrecautionsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<UserPrecautionDTO>().AsEnumerable());

            //Act
            var result = await _PrecautionController.CheckUserPrecautionsType(userId, type);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            Assert.IsInstanceOf<bool>(resultValue);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserActivePrecautionEndDate_Test()
        {
            //Arrange
            _userPrecautionService
                .Setup(x => x.GetUserActivePrecaution(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new UserPrecautionDTO());

            //Act
            var result = await _PrecautionController.GetUserActivePrecautionEndDate(It.IsAny<string>(), It.IsAny<string>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            Assert.IsInstanceOf<string>(resultValue);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUsersForPrecaution_ReturnsOkObjectResult()
        {
            //Arrange
            _userPrecautionService.Setup(s => s.GetUsersForPrecautionAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<SuggestedUserDto>());

            //Act
            var result = await _PrecautionController.GetUsersForPrecaution();
            
            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        
        


    }
}