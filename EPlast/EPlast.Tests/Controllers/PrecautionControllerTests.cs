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

namespace EPlast.Tests.Controllers
{
    internal class PrecautionControllerTests
    {
        private Mock<IPrecautionService> _precautionService;
        private Mock<IUserPrecautionService> _userPrecautionService;
        private Mock<UserManager<User>> _userManager;
        private Mock<HttpContext> _httpContext = new Mock<HttpContext>();

        private PrecautionController _PrecautionController;
        private ControllerContext _context;

        [SetUp]
        public void SetUp()
        {
            _precautionService = new Mock<IPrecautionService>();
            _userPrecautionService = new Mock<IUserPrecautionService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _httpContext = new Mock<HttpContext>();
            _httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);

            _PrecautionController = new PrecautionController(
                _precautionService.Object,
                _userPrecautionService.Object,
                _userManager.Object
                );
            _context = new ControllerContext(
                new ActionContext(
                    _httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
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
            PrecautionTableSettings TestPTS = new PrecautionTableSettings();
            _precautionService
                .Setup(x => x.GetUsersPrecautionsForTableAsync(It.IsAny<PrecautionTableSettings>())).ReturnsAsync(CreateTuple);

            PrecautionController controller = _PrecautionController;
            //Act
            var result = _PrecautionController.GetUsersPrecautionsForTable(It.IsAny<PrecautionTableSettings>()).Result;
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            _precautionService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<UserPrecautionsTableObject>>(resultValue);
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
        public async Task DeletePrecaution_ReturnsNullReferenceException()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _precautionService
                .Setup(x => x.DeletePrecautionAsync(It.IsAny<int>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());
            PrecautionController precautionController = _PrecautionController;
            //Act
            var result = await precautionController.DeletePrecaution(It.IsAny<int>());
            //Assert
            _precautionService.Verify();
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
            _PrecautionController.ControllerContext = _context;
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
        public async Task AddUserPrecaution_ReturnsNullReferenceException()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _userPrecautionService
                .Setup(x => x.AddUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());
            //Act
            var result = await _PrecautionController.AddUserPrecaution(It.IsAny<UserPrecautionDTO>());
            //Assert
            _userPrecautionService.Verify();
            _userManager.Verify();
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task AddPrecaution_ReturnsNoContentResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
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
            _PrecautionController.ControllerContext = _context;
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
            _PrecautionController.ControllerContext = _context;
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
            _PrecautionController.ControllerContext = _context;
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
        public async Task ChangeUserPrecaution_ReturnsNullReferenceException()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _userPrecautionService
                .Setup(x => x.ChangeUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());
            //Act
            var result = await _PrecautionController.EditUserPrecaution(It.IsAny<UserPrecautionDTO>());
            //Assert
            _userPrecautionService.Verify();
            _userManager.Verify();
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task ChangePrecaution_ReturnsNoContentResult()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
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
            _PrecautionController.ControllerContext = _context;
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
        public async Task ChangePrecaution_ReturnsNullReferenceException()
        {
            //Arrange
            _PrecautionController.ControllerContext = _context;
            _precautionService
                .Setup(x => x.ChangePrecautionAsync(It.IsAny<PrecautionDTO>(), It.IsAny<User>()))
                .Throws(new NullReferenceException());
            //Act
            var result = await _PrecautionController.EditPrecaution(It.IsAny<PrecautionDTO>());
            //Assert
            _precautionService.Verify();
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

        private List<UserPrecautionsTableObject> GetUsersPrecautionByPage()
        {
            return new List<UserPrecautionsTableObject>()
            {
                new UserPrecautionsTableObject()
                {
                    Number = 34,
                }
            };
        }
        private int GetFakeUserPrecautionNumber()
        {
            return 100;
        }

        private Tuple<IEnumerable<UserPrecautionsTableObject>, int> CreateTuple => new Tuple<IEnumerable<UserPrecautionsTableObject>, int>(GetUsersPrecautionByPage(), GetFakeUserPrecautionNumber());
    }
}