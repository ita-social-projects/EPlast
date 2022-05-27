using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.GoverningBody;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private GoverningBodiesController _governingBodiesController;
        private Mock<IGoverningBodyAnnouncementService> _governingBodyAnnouncementService;

        [SetUp]
        public void SetUp()
        {
            _governingBodiesService = new Mock<IGoverningBodiesService>();
            _governingBodyAdministrationService = new Mock<IGoverningBodyAdministrationService>();
            _governingBodyDocumentsService = new Mock<IGoverningBodyDocumentsService>();
            _logger = new Mock<ILoggerService<GoverningBodiesController>>();
            _mapper = new Mock<IMapper>();
            _governingBodyAnnouncementService = new Mock<IGoverningBodyAnnouncementService>();
            _governingBodiesController = new GoverningBodiesController(
                _governingBodiesService.Object,
                _logger.Object,
                _governingBodyAdministrationService.Object,
                _governingBodyAnnouncementService.Object,
                _mapper.Object,
                _governingBodyDocumentsService.Object
            );
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
            var result = await _governingBodiesController.GetGoverningBodies();
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
            _governingBodiesController.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await _governingBodiesController.Create(testDTO);

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
            var result = await _governingBodiesController.Create(testDTO);
            var resultObject = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(resultObject?.Value, serviceReturnedId);
        }

        [Test]
        public async Task Create_ThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _governingBodiesService
                .Setup(x => x.CreateAsync(It.IsAny<GoverningBodyDTO>()))
                .ThrowsAsync(new ArgumentException());

            // Act
            var result = await _governingBodiesController.Create(testDTO);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Edit_ModelStateNotValid_Test()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _governingBodiesController.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await _governingBodiesController.Edit(testDTO);

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
            var result = await _governingBodiesController.Edit(testDTO);

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
            var result = await _governingBodiesController.GetPhotoBase64(logopath);
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
            var result = await _governingBodiesController.GetPhotoBase64(logopath);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase(1)]
        public async Task Profile_GBExists_Test(int governingBodyId)
        {
            //Arrange
            _governingBodiesService.Setup(x => x.GetGoverningBodyProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateGoverningBodyProfileDto);
            _mapper.Setup(m =>
                    m.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(It.IsAny<GoverningBodyProfileDTO>()))
                .Returns(new GoverningBodyViewModel { Id = CreateGoverningBodyProfileDto.GoverningBody.Id });

            //Act
            var result = await _governingBodiesController.GetProfile(governingBodyId);
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            _mapper.Verify(m => m.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(It.IsAny<GoverningBodyProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
        }

        [TestCase(0)]
        public async Task Profile_GBNotExists_Test(int governingBodyid)
        {
            //Arrange
            _governingBodiesService.Setup(x => x.GetGoverningBodyProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(null as GoverningBodyProfileDTO);

            //Act
            var result = await _governingBodiesController.GetProfile(governingBodyid);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(1)]
        public async Task Remove_Test(int gbId)
        {
            //Act
            var result = await _governingBodiesController.Remove(gbId);

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
            var result = await _governingBodiesController.GetUserAccess(userId);
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
            var result = await _governingBodiesController.GetAdmins(id);

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
            var result = await _governingBodiesController.GetAdmins(id);
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
            var result = await _governingBodiesController.AddAdmin(new GoverningBodyAdministrationDTO { AdminType = new AdminTypeDTO() });
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<GoverningBodyAdministrationDTO>(resultValue);
        }

        [Test]
        public async Task AddAdmin_UserHasRestrictedRoles_ReturnsBadRequest()
        {
            //Arrange
            _governingBodyAdministrationService
                .Setup(x => x.AddGoverningBodyAdministratorAsync(It.IsAny<GoverningBodyAdministrationDTO>()))
                .Throws(new ArgumentException());

            //Act
            var result = await _governingBodiesController.AddAdmin(new GoverningBodyAdministrationDTO());

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task AddMainAdmin_Valid_Test()
        {
            // Arrange
            _governingBodyAdministrationService
                .Setup(c => c.AddGoverningBodyMainAdminAsync(It.IsAny<GoverningBodyAdministrationDTO>()))
                .ReturnsAsync(new GoverningBodyAdministrationDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));

            // Act
            var result = await _governingBodiesController.AddMainAdmin(new GoverningBodyAdministrationDTO { AdminType = new AdminTypeDTO() });
            var resultValue = (result as OkObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(resultValue);
            Assert.IsInstanceOf<GoverningBodyAdministrationDTO>(resultValue);
        }

        [Test]
        public async Task AddMainAdmin_UserHasRestrictedRoles_ReturnsBadRequest()
        {
            //Arrange
            _governingBodyAdministrationService
                .Setup(x => x.AddGoverningBodyMainAdminAsync(It.IsAny<GoverningBodyAdministrationDTO>()))
                .Throws(new ArgumentException());

            //Act
            var result = await _governingBodiesController.AddMainAdmin(new GoverningBodyAdministrationDTO());

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
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
            var result = await _governingBodiesController.EditAdmin(new GoverningBodyAdministrationDTO { AdminType = new AdminTypeDTO() });
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
            var result = await _governingBodiesController.RemoveAdmin(TestId);

            // Assert
            _logger.Verify();
            _governingBodyAdministrationService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task RemoveMainAdmin_Valid_Test()
        {
            // Arrange
            _governingBodyAdministrationService
                .Setup(c => c.RemoveMainAdministratorAsync(It.IsAny<string>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            _governingBodyAdministrationService
                .Setup(c => c.EditGoverningBodyAdministratorAsync(It.IsAny<GoverningBodyAdministrationDTO>()))
                .ReturnsAsync(new GoverningBodyAdministrationDTO());

            // Act
            var result = await _governingBodiesController.RemoveMainAdmin(TestIdString);

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
            var result = await _governingBodiesController.GetDocuments(id);
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
            var result = await _governingBodiesController.GetDocuments(TestId);

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
            var result = await _governingBodiesController.GetDocumentTypesAsync();
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
            var result = await _governingBodiesController.GetFileBase64(fileName);
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
            var result = await _governingBodiesController.AddDocument(new GoverningBodyDocumentsDTO());
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
            var result = await _governingBodiesController.RemoveDocument(TestId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetUserAdministrations_Valid_Test()
        {
            // Arrange
            _governingBodiesService
                .Setup(c => c.GetAdministrationsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<GoverningBodyAdministrationDTO>>());

            // Act
            var result = await _governingBodiesController.GetUserAdministrations(GetStringTestId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserPreviousAdministrations_Valid_Test()
        {
            // Arrange
            _governingBodiesService
                .Setup(c => c.GetPreviousAdministrationsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<GoverningBodyAdministrationDTO>>());

            // Act
            var result = await _governingBodiesController.GetUserPreviousAdministrations(GetStringTestId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddAnnouncement_Valid_Test()
        {
            //Arrange
            int returnId = 1;
            _governingBodyAnnouncementService
                .Setup(c => c.AddAnnouncementAsync(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>())).ReturnsAsync(returnId);

            //Act
            var result = await _governingBodiesController.AddAnnouncement(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>());

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            _governingBodyAnnouncementService.Verify();
        }

        [Test]
        public async Task AddAnnouncement_BadRequest()
        {
            //Arrange
            _governingBodiesController.ModelState.AddModelError("text", "is required");
            _governingBodyAnnouncementService
                .Setup(c => c.AddAnnouncementAsync(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>()));

            //Act
            var result = await _governingBodiesController.AddAnnouncement(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>());

            //Assert
            _governingBodyAnnouncementService.Verify();
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddAnnouncement_TitleOrTextIsWhiteSpace_BadRequest()
        {
            //Arrange
            int? returnId = null;
            _governingBodyAnnouncementService
                .Setup(c => c.AddAnnouncementAsync(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>())).ReturnsAsync(returnId);

            //Act
            var result = await _governingBodiesController.AddAnnouncement(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>());

            //Assert
            _governingBodyAnnouncementService.Verify();
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task DeleteAnnouncement_Valid()
        {
            //Arrange
            _governingBodyAnnouncementService.Setup(d => d.DeleteAnnouncementAsync(It.IsAny<int>()));

            //Act
            var result = await _governingBodiesController.Delete(It.IsAny<int>());

            //Assert
            _governingBodiesService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [TestCase(1, 5, 1)]
        public async Task GetAnnouncementsByPage_Valid(int page, int pageSize, int governingBodyId)
        {
            //Arrange
            _governingBodyAnnouncementService.Setup(g => g.GetAnnouncementsByPageAsync(page, pageSize, governingBodyId));

            //Act
            var result = await _governingBodiesController.GetAnnouncementsByPage(page, pageSize, governingBodyId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllUsers_Valid()
        {
            //Arrange
            _governingBodyAnnouncementService.Setup(a => a.GetAllUserAsync());

            //Act
            var result = await _governingBodiesController.GetAllUserId();

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetById_Valid()
        {
            //Arrange
            _governingBodyAnnouncementService.Setup(g => g.GetAnnouncementByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new GoverningBodyAnnouncementUserWithImagesDTO());

            //Act
            var result = await _governingBodiesController.GetById(It.IsAny<int>());
            var resultValue = (result as ObjectResult).Value;

            //Assert
            _governingBodyAnnouncementService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<GoverningBodyAnnouncementUserWithImagesDTO>(resultValue);
        }

        [Test]
        public async Task GetById_ReturnNoContent()
        {
            //Arrange
            _governingBodyAnnouncementService.Setup(g => g.GetAnnouncementByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as GoverningBodyAnnouncementUserWithImagesDTO);

            //Act
            var result = await _governingBodiesController.GetById(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task EditAnnouncement_ModelStateIsValid_ReturnsOk()
        {
            //Arrange
            _governingBodyAnnouncementService
                .Setup(x => x.EditAnnouncementAsync(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>()))
                .ReturnsAsync(1);

            //Act
            var res = await _governingBodiesController.EditAnnouncement(new GoverningBodyAnnouncementWithImagesDTO());

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public async Task EditAnnouncement_ModeStatIsNotValid_ReturnsBadRequest()
        {
            //Arrange
            _governingBodiesController.ModelState.AddModelError("key", "error message");
            _governingBodyAnnouncementService
                .Setup(x => x.EditAnnouncementAsync(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>()))
                .ReturnsAsync(1);

            //Act
            var res = await _governingBodiesController.EditAnnouncement(new GoverningBodyAnnouncementWithImagesDTO());

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public async Task EditAnnouncement_IdIsNull_ReturnsBadRequest()
        {
            //Arrange

            _governingBodyAnnouncementService
                .Setup(x => x.EditAnnouncementAsync(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>()))
                .ReturnsAsync(null as int?);

            //Act
            var res = await _governingBodiesController.EditAnnouncement(new GoverningBodyAnnouncementWithImagesDTO());

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public async Task EditAnnouncement_TitleOrTextIsWhiteSpace_ReturnsBadRequest()
        {
            //Arrange
            int? returnId = null;
            _governingBodyAnnouncementService
                .Setup(x => x.EditAnnouncementAsync(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>()))
                .ReturnsAsync(returnId);

            //Act
            var res = await _governingBodiesController.EditAnnouncement(new GoverningBodyAnnouncementWithImagesDTO());

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public async Task GetUserAdministrationsForTable_ReturnsOkObjectResult()
        {
            //Arrange
            _governingBodiesService
                .Setup(g => g.GetAdministrationForTableAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync(new Tuple<IEnumerable<GoverningBodyAdministrationDTO>, int>(It.IsAny<IEnumerable<GoverningBodyAdministrationDTO>>(), It.IsAny<int>()));
            _mapper
                .Setup(m =>
                    m.Map<IEnumerable<GoverningBodyAdministrationDTO>, IEnumerable<GoverningBodyTableViewModel>>(
                        It.IsAny<IEnumerable<GoverningBodyAdministrationDTO>>()));

            //Act
            var result = await _governingBodiesController.GetUserAdministrationsForTable(It.IsAny<string>(),
                It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            _governingBodiesService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserAdministrationsForTable_ReturnsBadRequestResult()
        {
            //Arrange
            _governingBodiesService
                .Setup(g => g.GetAdministrationForTableAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(),
                    It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            //Act
            var result = await _governingBodiesController.GetUserAdministrationsForTable(It.IsAny<string>(),
                It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            _governingBodiesService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        private const int TestId = 3;
        private const string TestIdString = "TestId";

        private string GetStringTestId()
        {
            return "1";
        }
        private GoverningBodyDTO CreateGoverningBodyDTO => new GoverningBodyDTO()
        {
            Id = 1,
            GoverningBodyName = "gbName",
            Description = "gbDesc",
            Email = "gbEmail",
            Logo = null,
            PhoneNumber = "12345",
            GoverningBodyDocuments = new List<GoverningBodyDocumentsDTO>()
        };

        private GoverningBodyProfileDTO CreateGoverningBodyProfileDto => new GoverningBodyProfileDTO()
        {
            GoverningBody = CreateGoverningBodyDTO
        };
    }
}
