using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.GoverningBody.Sector;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using System;
using EPlast.BLL.DTO.GoverningBody.Announcement;

namespace EPlast.Tests.Controllers
{
    internal class GoverningBodySectorsControllerTests
    {
        private Mock<ISectorService> _sectorService;
        private Mock<ISectorAdministrationService> _sectorAdministrationService;
        private Mock<ISectorDocumentsService> _sectorDocumentsService;
        private Mock<ISectorAnnouncementsService> _sectorAnnouncementsService;
        private Mock<IMapper> _mapper;
        private Mock<ILoggerService<GoverningBodiesController>> _logger;
        private GoverningBodySectorsController _controller;

        [SetUp]
        public void SetUp()
        {
            _sectorService = new Mock<ISectorService>();
            _sectorAdministrationService = new Mock<ISectorAdministrationService>();
            _sectorDocumentsService = new Mock<ISectorDocumentsService>();
            _sectorAnnouncementsService = new Mock<ISectorAnnouncementsService>();
            _logger = new Mock<ILoggerService<GoverningBodiesController>>();
            _mapper = new Mock<IMapper>();

            _controller = new GoverningBodySectorsController(
                _sectorService.Object,
                _logger.Object,
                _sectorAdministrationService.Object,
                _mapper.Object,
                _sectorDocumentsService.Object,
                _sectorAnnouncementsService.Object);
        }

        [TestCase(1)]
        public async Task GetSectors_ReturnsSectorsList(int governingBodyId)
        {
            //Arrange
            var testSectorsList = new List<SectorDTO>();
            _sectorService
                .Setup(x => x.GetSectorsByGoverningBodyAsync(It.IsAny<int>()))
                .ReturnsAsync(testSectorsList);

            //Act
            var result = await _controller.GetSectors(governingBodyId);
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(testSectorsList, resultValue as List<SectorDTO>);
        }

        [Test]
        public async Task Create_ModelStateNotValid()
        {
            //Arrange
            _controller.ModelState.AddModelError("NameError", "Required");

            //Act
            var result = await _controller.Create(CreateSectorDto());

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_ThrowsArgumentException_BadRequestResult()
        {
            //Arrange
            _sectorService
                .Setup(x => x.CreateAsync(It.IsAny<SectorDTO>()))
                .ThrowsAsync(new ArgumentException());

            //Act
            var result = await _controller.Create(CreateSectorDto());

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Create_ModelStateValid()
        {
            //Arrange
            _sectorService
                .Setup(x => x.CreateAsync(It.IsAny<SectorDTO>()))
                .ReturnsAsync(1);

            //Act
            var result = await _controller.Create(CreateSectorDto());

            //Assert
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(1, (result as ObjectResult)?.Value);
        }

        [Test]
        public async Task Edit_ModelStateNotValid()
        {
            //Arrange
            _controller.ModelState.AddModelError("NameError", "Required");

            //Act
            var result = await _controller.Edit(CreateSectorDto());

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Edit_ModelStateValid()
        {
            //Act
            var result = await _controller.Edit(CreateSectorDto());

            //Assert
            _sectorService.Verify(x => x.EditAsync(It.IsAny<SectorDTO>()), Times.Once);
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetPhotoBase64_LogoNull()
        {
            //Act
            var result = await _controller.GetPhotoBase64(null);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetPhotoBase64_LogoNotNull()
        {
            //Arrange
            string logoName = "test";
            _sectorService
                .Setup(x => x.GetLogoBase64Async(It.IsAny<string>()))
                .ReturnsAsync(logoName);

            //Act
            var result = await _controller.GetPhotoBase64(logoName);
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(logoName, resultValue);
        }

        [TestCase(1)]
        public async Task GetProfile_SectorNotFound(int id)
        {
            //Arrange
            SectorProfileDTO foundSector = null;
            _sectorService
                .Setup(x => x.GetSectorProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(foundSector);

            //Act
            var result = await _controller.GetProfile(id);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(1)]
        public async Task GetProfile_SectorFound(int id)
        {
            //Arrange
            _sectorService
                .Setup(x => x.GetSectorProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(new SectorProfileDTO()
                {
                    Sector = new SectorDTO()
                    {
                        Documents = new List<SectorDocumentsDTO>()
                    }
                });
            var testSectorViewModel = new SectorViewModel();
            _mapper
                .Setup(x => x.Map<SectorProfileDTO, SectorViewModel>(It.IsAny<SectorProfileDTO>()))
                .Returns(testSectorViewModel);

            //Act
            var result = await _controller.GetProfile(id);
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            _mapper.Verify(m => m.Map<SectorProfileDTO, SectorViewModel>(It.IsAny<SectorProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
        }

        [TestCase(1)]
        public async Task Remove_ReturnsOk(int id)
        {
            //Act
            var result = await _controller.Remove(id);

            //Assert
            _sectorService.Verify(x => x.RemoveAsync(It.IsAny<int>()), Times.Once);
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [TestCase(1)]
        public async Task GetAdmins_SectorNotFound(int id)
        {
            //Arrange
            SectorProfileDTO foundSector = null;
            _sectorService
                .Setup(x => x.GetSectorProfileAsync(id))
                .ReturnsAsync(foundSector);

            //Act
            var result = await _controller.GetAdmins(id);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(1)]
        public async Task GetAdmins_SectorFound(int id)
        {
            //Arrange
            _sectorService
                .Setup(x => x.GetSectorProfileAsync(id))
                .ReturnsAsync(new SectorProfileDTO());
            _mapper
                .Setup(x => x.Map<SectorProfileDTO, SectorViewModel>(It.IsAny<SectorProfileDTO>()))
                .Returns(new SectorViewModel());

            //Act
            var result = await _controller.GetAdmins(id);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddAdmin_ReturnsOk()
        {
            //Arrange
            var testAdmin = new SectorAdministrationDTO() { AdminType = new AdminTypeDTO() };

            //Act
            var result = await _controller.AddAdmin(testAdmin);
            var resultValue = (result as OkObjectResult)?.Value;
            
            //Assert
            _sectorAdministrationService.Verify(x => x.AddSectorAdministratorAsync(
                It.IsAny<SectorAdministrationDTO>()), Times.Once);
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()));
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(testAdmin, resultValue);
        }

        [Test]
        public async Task AddAdmin_UserHasRestrictedRoles_ReturnsBadRequest()
        {
            //Arrange
            _sectorAdministrationService
                .Setup(x => x.AddSectorAdministratorAsync(It.IsAny<SectorAdministrationDTO>()))
                .Throws(new ArgumentException());

            //Act
            var result = await _controller.AddAdmin(new SectorAdministrationDTO());

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task EditAdmin_ReturnsOk()
        {
            //Arrange
            var testAdmin = new SectorAdministrationDTO() { AdminType = new AdminTypeDTO() };

            //Act
            var result = await _controller.EditAdmin(testAdmin);
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            _sectorAdministrationService.Verify(x => x.EditSectorAdministratorAsync(
                It.IsAny<SectorAdministrationDTO>()), Times.Once);
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()));
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(testAdmin, resultValue);
        }

        [TestCase(1)]
        public async Task RemoveAdmin_ReturnsOk(int id)
        {
            //Act
            var result = await _controller.RemoveAdmin(id);

            //Assert
            _sectorAdministrationService.Verify(x => x.RemoveAdministratorAsync(
                It.IsAny<int>()), Times.Once);
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()));
            Assert.IsInstanceOf<OkResult>(result);
        }

        [TestCase(1)]
        public async Task GetDocuments_SectorNotFound(int id)
        {
            //Arrange
            SectorProfileDTO foundSector = null;
            _sectorService
                .Setup(x => x.GetSectorDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(foundSector);

            //Act
            var result = await _controller.GetDocuments(id);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(1)]
        public async Task GetDocuments_SectorFound(int id)
        {
            //Arrange
            _sectorService
                .Setup(x => x.GetSectorDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(new SectorProfileDTO());
            _mapper
                .Setup(x => x.Map<SectorProfileDTO, SectorViewModel>(It.IsAny<SectorProfileDTO>()))
                .Returns(new SectorViewModel());

            //Act
            var result = await _controller.GetDocuments(id);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddDocument_ReturnsOk()
        {
            //Arrange
            var testDocument = new SectorDocumentsDTO();

            //Act
            var result = await _controller.AddDocument(testDocument);
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            _sectorDocumentsService.Verify(x => x.AddSectorDocumentAsync(
                It.IsAny<SectorDocumentsDTO>()));
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()));
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(testDocument, resultValue);
        }

        [TestCase("file name")]
        public async Task GetFileBase64_ReturnsOk(string fileName)
        {
            //Arrange
            string fileBase64 = "file";
            _sectorDocumentsService
                .Setup(x => x.DownloadSectorDocumentAsync(It.IsAny<string>()))
                .ReturnsAsync(fileBase64);

            //Act
            var result = await _controller.GetFileBase64(fileName);
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(fileBase64, resultValue);
        }

        [TestCase(1)]
        public async Task RemoveDocument_ReturnsOk(int documentId)
        {
            //Arrange
            _sectorDocumentsService
                .Setup(x => x.DeleteSectorDocumentAsync(It.IsAny<int>()));

            //Act
            var result = await _controller.RemoveDocument(documentId);

            //Assert
            _sectorDocumentsService.Verify(x => x.DeleteSectorDocumentAsync(
                It.IsAny<int>()), Times.Once);
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()));
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetDocumentTypesAsync_ReturnsOk()
        {
            //Arrange
            var testDocuments = new List<SectorDocumentTypeDTO>();
            _sectorDocumentsService
                .Setup(x => x.GetAllSectorDocumentTypesAsync())
                .ReturnsAsync(testDocuments);

            //Act
            var result = await _controller.GetDocumentTypesAsync();
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(testDocuments, resultValue);
        }

        [TestCase("user id")]
        public async Task GetUserAccess_ReturnsOk(string userId)
        {
            //Arrange
            var testAccesses = new Dictionary<string, bool>();
            _sectorService
                .Setup(x => x.GetUserAccessAsync(It.IsAny<string>()))
                .ReturnsAsync(testAccesses);

            //Act
            var result = await _controller.GetUserAccess(userId);
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(testAccesses, resultValue);
        }

        [Test]
        public async Task GetUserAdministrations_Valid_Test()
        {
            // Arrange
            _sectorService
                .Setup(c => c.GetAdministrationsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<SectorAdministrationDTO>>());

            // Act
            var result = await _controller.GetUserAdministrations("1");

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserPreviousAdministrations_Valid_Test()
        {
            // Arrange
            _sectorService
                .Setup(c => c.GetPreviousAdministrationsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<SectorAdministrationDTO>>());

            // Act
            var result = await _controller.GetUserPreviousAdministrations("1");

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserAdministrationsForTable_ReturnsOkObjectResult()
        {
            //Arrange
            _sectorService
                .Setup(s => s.GetAdministrationForTableAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync(
                    new Tuple<IEnumerable<SectorAdministrationDTO>, int>(
                        It.IsAny<IEnumerable<SectorAdministrationDTO>>(), It.IsAny<int>()));
            _mapper
                .Setup(m =>
                    m.Map<IEnumerable<SectorAdministrationDTO>, IEnumerable<SectorTableViewModel>>(
                        It.IsAny<IEnumerable<SectorAdministrationDTO>>()));

            //Act
            var result = await _controller.GetUserAdministrationsForTable(It.IsAny<string>(),
                It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            _sectorService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserAdministrationsForTable_ReturnsBadRequestResult()
        {
            //Arrange
            _sectorService
                .Setup(s => s.GetAdministrationForTableAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(),
                    It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            //Act
            var result = await _controller.GetUserAdministrationsForTable(It.IsAny<string>(),
                It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            _sectorService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddAnnouncement_Valid_Test()
        {
            //Arrange
            _sectorAnnouncementsService
                .Setup(c => c.AddAnnouncementAsync(It.IsAny<SectorAnnouncementWithImagesDTO>()));

            //Act
            var result = await _controller.AddAnnouncement(It.IsAny<SectorAnnouncementWithImagesDTO>());

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            _sectorAnnouncementsService.Verify();
        }

        [Test]
        public async Task AddAnnouncement_BadRequest()
        {
            //Arrange
            _controller.ModelState.AddModelError("text", "is required");
            _sectorAnnouncementsService
                .Setup(c => c.AddAnnouncementAsync(It.IsAny<SectorAnnouncementWithImagesDTO>()));

            //Act
            var result = await _controller.AddAnnouncement(It.IsAny<SectorAnnouncementWithImagesDTO>());

            //Assert
            _sectorAnnouncementsService.Verify();
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task EditAnnouncement_ModelStateIsValid_ReturnsOk()
        {
            //Arrange
            _sectorAnnouncementsService
                .Setup(x => x.EditAnnouncementAsync(It.IsAny<SectorAnnouncementWithImagesDTO>()))
                .ReturnsAsync(1);

            //Act
            var res = await _controller.EditAnnouncement(new SectorAnnouncementWithImagesDTO());

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public async Task EditAnnouncement_ModeStatIsNotValid_ReturnsBadRequest()
        {
            //Arrange
            _controller.ModelState.AddModelError("key", "error message");
            _sectorAnnouncementsService
                .Setup(x => x.EditAnnouncementAsync(It.IsAny<SectorAnnouncementWithImagesDTO>()))
                .ReturnsAsync(1);

            //Act
            var res = await _controller.EditAnnouncement(new SectorAnnouncementWithImagesDTO());

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public async Task EditAnnouncement_IdIsNull_ReturnsBadRequest()
        {
            //Arrange

            _sectorAnnouncementsService
                .Setup(x => x.EditAnnouncementAsync(It.IsAny<SectorAnnouncementWithImagesDTO>()))
                .ReturnsAsync(null as int?);

            //Act
            var res = await _controller.EditAnnouncement(new SectorAnnouncementWithImagesDTO());

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(res);
        }

        [Test]
        public async Task DeleteAnnouncement_Valid()
        {
            //Arrange
            _sectorAnnouncementsService.Setup(d => d.DeleteAnnouncementAsync(It.IsAny<int>()));

            //Act
            var result = await _controller.Delete(It.IsAny<int>());

            //Assert
            _sectorAnnouncementsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetById_Valid()
        {
            //Arrange
            _sectorAnnouncementsService.Setup(g => g.GetAnnouncementByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new SectorAnnouncementUserDTO());

            //Act
            var result = await _controller.GetById(It.IsAny<int>());
            var resultValue = (result as ObjectResult).Value;

            //Assert
            _sectorAnnouncementsService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<SectorAnnouncementUserDTO>(resultValue);
        }

        [Test]
        public async Task GetById_ReturnNoContent()
        {
            //Arrange
            _sectorAnnouncementsService.Setup(g => g.GetAnnouncementByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as SectorAnnouncementUserDTO);

            //Act
            var result = await _controller.GetById(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(1, 5, 1)]
        public async Task GetAnnouncementsByPage_Valid(int page, int pageSize, int governingBodyId)
        {
            //Arrange
            _sectorAnnouncementsService.Setup(g => g.GetAnnouncementsByPageAsync(page, pageSize, governingBodyId));

            //Act
            var result = await _controller.GetAnnouncementsByPage(page, pageSize, governingBodyId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private SectorDTO CreateSectorDto()
        {
            return new SectorDTO()
            {
                Id = 1,
                Description = "description",
                Email = "email",
                GoverningBodyId = 1,
                Name = "name",
                PhoneNumber = "number",
                Administration = new List<SectorAdministrationDTO>() {new SectorAdministrationDTO()},
                AdministrationCount = 1,
                Documents = new List<SectorDocumentsDTO>() {new SectorDocumentsDTO()}
            };
        }
    }
}
