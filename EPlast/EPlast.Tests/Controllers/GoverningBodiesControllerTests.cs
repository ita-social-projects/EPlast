using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.GoverningBody;
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
        private Mock<IGoverningBodyAdministrationService> _governingBodyAdministrationService;
        private Mock<IGoverningBodyDocumentsService> _governingBodyDocumentsService;
        private Mock<IMapper> _mapper;
        private Mock<ILoggerService<GoverningBodiesController>> _logger;
        private GoverningBodiesController _controller;

        [SetUp]
        public void SetUp()
        {
            _governingBodiesService = new Mock<IGoverningBodiesService>();
            _governingBodyAdministrationService = new Mock<IGoverningBodyAdministrationService>();
            _governingBodyDocumentsService = new Mock<IGoverningBodyDocumentsService>();
            _logger = new Mock<ILoggerService<GoverningBodiesController>>();
            _mapper = new Mock<IMapper>();
            _controller = new GoverningBodiesController(
                _governingBodiesService.Object,
                _logger.Object,
                _governingBodyAdministrationService.Object,
                _mapper.Object,
                _governingBodyDocumentsService.Object);
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
            _governingBodiesService.Setup(x => x.GetGoverningBodyProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateGoverningBodyProfileDto);
            _mapper.Setup(m =>
                    m.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(It.IsAny<GoverningBodyProfileDTO>()))
                .Returns(new GoverningBodyViewModel { Id = CreateGoverningBodyProfileDto.GoverningBody.Id });

            //Act
            var result = await _controller.GetProfile(governingBodyid);
            var resultValue = (result as OkObjectResult)?.Value as GoverningBodyViewModel;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(CreateGoverningBodyProfileDto.GoverningBody.Id, resultValue?.Id);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(0)]
        public async Task Profile_GBNotExists_Test(int governingBodyid)
        {
            //Arrange
            _governingBodiesService.Setup(x => x.GetGoverningBodyProfileAsync(It.IsAny<int>()))
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

        [TestCase(2)]
        public async Task GetAdmins_Invalid_Test(int id)
        {
            // Arrange
            _governingBodiesService
                .Setup(c => c.GetGoverningBodyProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper.Setup(m => m.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(It.IsAny<GoverningBodyProfileDTO>()))
                .Returns(new GoverningBodyViewModel());

            // Act
            var result = await _controller.GetAdmins(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetAdmins_Valid_Test(int id)
        {
            // Arrange
            _governingBodiesService
                .Setup(c => c.GetGoverningBodyProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(new GoverningBodyProfileDTO());
            _mapper
                .Setup(m => m.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(It.IsAny<GoverningBodyProfileDTO>()))
                .Returns(new GoverningBodyViewModel());

            // Act
            var result = await _controller.GetAdmins(id);
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            _mapper.Verify(m => m.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(It.IsAny<GoverningBodyProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
        }

        [Test]
        public async Task AddAdmin_Valid_Test()
        {
            // Arrange
            _governingBodyAdministrationService
                .Setup(c => c.AddGoverningBodyAdministratorAsync(It.IsAny<GoverningBodyAdministrationDTO>()))
                .ReturnsAsync(new GoverningBodyAdministrationDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));

            // Act
            var result = await _controller.AddAdmin(new GoverningBodyAdministrationDTO { AdminType = new AdminTypeDTO() });
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<GoverningBodyAdministrationDTO>(resultValue);
        }

        [Test]
        public async Task EditAdmin_Valid_Test()
        {
            // Arrange

            _governingBodyAdministrationService
                .Setup(c => c.EditGoverningBodyAdministratorAsync(It.IsAny<GoverningBodyAdministrationDTO>()))
                    .ReturnsAsync(new GoverningBodyAdministrationDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));

            // Act
            var result = await _controller.EditAdmin(new GoverningBodyAdministrationDTO { AdminType = new AdminTypeDTO() });
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<GoverningBodyAdministrationDTO>(resultValue);
        }

        [Test]
        public async Task RemoveAdmin_Valid_Test()
        {
            // Arrange
            _governingBodyAdministrationService
                .Setup(c => c.RemoveAdministratorAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            _governingBodyAdministrationService
                .Setup(c => c.EditGoverningBodyAdministratorAsync(It.IsAny<GoverningBodyAdministrationDTO>()))
                .ReturnsAsync(new GoverningBodyAdministrationDTO());

            // Act
            var result = await _controller.RemoveAdmin(FakeId);

            // Assert
            _logger.Verify();
            _governingBodyAdministrationService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [TestCase(2)]
        public async Task GetDocuments_Valid_Test(int id)
        {
            // Arrange
            _governingBodiesService
                .Setup(c => c.GetGoverningBodyDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(new GoverningBodyProfileDTO());
            _mapper
                .Setup(m => m.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(It.IsAny<GoverningBodyProfileDTO>()))
                .Returns(new GoverningBodyViewModel());

            // Act
            var result = await _controller.GetDocuments(id);
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            _mapper.Verify(m => m.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(It.IsAny<GoverningBodyProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
        }

        [Test]
        public async Task GetDocuments_Invalid_Test()
        {
            // Arrange
            _governingBodiesService
                .Setup(c => c.GetGoverningBodyDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(It.IsAny<GoverningBodyProfileDTO>()))
                .Returns(new GoverningBodyViewModel());

            // Act
            var result = await _controller.GetDocuments(FakeId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetDocumentTypesAsync_Valid_Test()
        {
            // Arrange
            _governingBodyDocumentsService
                .Setup(c => c.GetAllGoverningBodyDocumentTypesAsync())
                .ReturnsAsync(new List<GoverningBodyDocumentTypeDTO> { new GoverningBodyDocumentTypeDTO() });

            // Act
            var result = await _controller.GetDocumentTypesAsync();
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyDocumentTypeDTO>>(resultValue);
        }

        [TestCase("fileName")]
        public async Task GetFileBase64_Valid_Test(string fileName)
        {
            // Arrange
            _governingBodyDocumentsService
                .Setup(c => c.DownloadGoverningBodyDocumentAsync(It.IsAny<string>()))
                .ReturnsAsync(fileName);

            // Act
            var result = await _controller.GetFileBase64(fileName);
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<string>(resultValue);
        }

        [Test]
        public async Task AddDocument_Valid_Test()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<GoverningBodyDocumentsViewModel, GoverningBodyDocumentsDTO>(It.IsAny<GoverningBodyDocumentsViewModel>()))
                .Returns(new GoverningBodyDocumentsDTO());
            _governingBodyDocumentsService
                .Setup(c => c.AddGoverningBodyDocumentAsync(It.IsAny<GoverningBodyDocumentsDTO>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));

            // Act
            var result = await _controller.AddDocument(new GoverningBodyDocumentsDTO());
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<GoverningBodyDocumentsDTO>(resultValue);
        }

        [Test]
        public async Task RemoveDocument_Valid_Test()
        {
            // Arrange
            _governingBodyDocumentsService
                .Setup(c => c.DeleteGoverningBodyDocumentAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));

            // Act
            var result = await _controller.RemoveDocument(FakeId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        private const int FakeId = 3;

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
