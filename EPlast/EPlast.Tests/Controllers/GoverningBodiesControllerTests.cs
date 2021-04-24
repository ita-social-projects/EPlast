using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    internal class GoverningBodiesControllerTests
    {
        private Mock<IGoverningBodiesService> _governingBodiesService;
        private Mock<ILoggerService<GoverningBodiesController>> _logger;
        private Mock<UserManager<User>> _userManager;
        private GoverningBodiesController _controller;

        [SetUp]
        public void SetUp()
        {
            _governingBodiesService = new Mock<IGoverningBodiesService>();
            _logger = new Mock<ILoggerService<GoverningBodiesController>>();
            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _controller = new GoverningBodiesController(
                _governingBodiesService.Object,
                _logger.Object,
                _userManager.Object);
        }

        [Test]
        public async Task GetGoverningBodies_ReturnsOrganizationsList()
        {
            //Arrange
            List<GoverningBodyDTO> list = new List<GoverningBodyDTO>();
            list.Add(new Mock<GoverningBodyDTO>().Object);
            _governingBodiesService
                .Setup(x => x.GetGoverningBodiesListAsync()).ReturnsAsync(list);
            //Act
            var result = await _controller.GetGoverningBodies();
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotEmpty(resultValue as List<GoverningBodyDTO>);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyDTO>>(resultValue);
        }

        [Test]
        public async Task Create_ModelStateNotValid_Test()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _controller.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await _controller.Create(testDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase(2)]
        public async Task Create_Valid_Test(int serviceReturnedId)
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _governingBodiesService.Setup(x => x.CreateAsync(It.IsAny<GoverningBodyDTO>())).ReturnsAsync(serviceReturnedId);

            // Act
            var result = await _controller.Create(testDTO);
            var resultObject = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(resultObject?.Value, serviceReturnedId);
        }

        [Test]
        public async Task Edit_ModelStateNotValid_Test()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _controller.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await _controller.Edit(testDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase(2)]
        public async Task Edit_Valid_Test(int serviceReturnedId)
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _governingBodiesService = new Mock<IGoverningBodiesService>();

            // Act
            var result = await _controller.Edit(testDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [TestCase("logopath", "logo64path")]
        public async Task GetPhotoBase64_Valid_Test(string logopath, string logo64Path)
        {
            //Arrange
            _governingBodiesService.Setup(x => x.GetLogoBase64Async(It.IsAny<string>())).ReturnsAsync(logo64Path);

            //Act
            var result = await _controller.GetPhotoBase64(logopath);
            var resultObject = result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(resultObject?.Value, logo64Path);
        }

        [TestCase(null, "logo64path")]
        public async Task GetPhotoBase64_NullGot_Test(string logopath, string logo64Path)
        {
            //Act
            var result = await _controller.GetPhotoBase64(logopath);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase(1)]
        public async Task Profile_GBExists_Test(int governingBodyid)
        {
            //Arrange
            _governingBodiesService.Setup(x => x.GetProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(CreateGoverningBodyProfileDto);

            //Act
            var result = await _controller.GetProfile(governingBodyid);
            var resultValue = (result as OkObjectResult).Value as GoverningBodyProfileDTO;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(CreateGoverningBodyProfileDto.GoverningBody.Id, resultValue.GoverningBody.Id);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(0)]
        public async Task Profile_GBNotExists_Test(int governingBodyid)
        {
            //Arrange
            _governingBodiesService.Setup(x => x.GetProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(null as GoverningBodyProfileDTO);

            //Act
            var result = await _controller.GetProfile(governingBodyid);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(1)]
        public async Task Remove_Test(int gbId)
        {
            //Act
            var result = await _controller.Remove(gbId);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [TestCase("userId")]
        public async Task GetUserAccess_Test(string userId)
        {
            //Arrange
            var dict = new Dictionary<string, bool>()
            {
                {"action", true}
            };
            _governingBodiesService.Setup(x => x.GetUserAccessAsync(It.IsAny<string>())).ReturnsAsync(dict);

            //Act
            var result = await _controller.GetUserAccess(userId);
            var resultValue = (result as OkObjectResult).Value as Dictionary<string, bool>;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(dict.Count, resultValue.Count);
        }

        private GoverningBodyDTO CreateGoverningBodyDTO => new GoverningBodyDTO()
        {
            Id = 1,
            GoverningBodyName = "gbName",
            Description = "gbDesc",
            Email = "gbEmail",
            Logo = null,
            PhoneNumber = "12345"
        };

        private GoverningBodyProfileDTO CreateGoverningBodyProfileDto => new GoverningBodyProfileDTO()
        {
            GoverningBody = CreateGoverningBodyDTO
        };
    }
}
