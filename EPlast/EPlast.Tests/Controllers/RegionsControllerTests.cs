using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Region;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    class RegionsControllerTests
    {
        private  Mock<ILoggerService<CitiesController>> _logger;
        private  Mock<IRegionService> _regionService;
        private  Mock<IRegionAdministrationService> _regionAdministrationService;
        private  Mock<IRegionAnnualReportService> _RegionAnnualReportService;

        private RegionsController _RegionController;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILoggerService<CitiesController>>();
            _regionService = new Mock<IRegionService>();
            _regionAdministrationService = new Mock<IRegionAdministrationService>();
            _RegionAnnualReportService = new Mock<IRegionAnnualReportService>();

            _RegionController = new RegionsController(
                _logger.Object, _regionService.Object, _regionAdministrationService.Object, _RegionAnnualReportService.Object);
        }

        [Test]
        public async Task Get_AllRegions_ReturnsAllregionsOkResult()
        {
            // Arrange
            _regionService.Setup(x => x.GetAllRegionsAsync()).ReturnsAsync(GetRegions());
            // Act
            var result = await _RegionController.Index();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<RegionDTO>>((result as ObjectResult).Value);
        }

        [Test]
        public async Task CreateRegion_NewRegion_ReturnsOkResult()
        {
            // Arrange
            RegionDTO reg = new RegionDTO() { ID=3, City="Lviv"};
            // Act
            var result = await _RegionController.CreateRegion(reg);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task CreateRegion_NullRegion_ReturnsOkResult()
        {
            // Act
            var result = await _RegionController.CreateRegion(null);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetPhotoBase64_NullString_ReturnsOkObjResult()
        {
           // Act
            var result = await _RegionController.GetPhotoBase64(null);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNull((result as ObjectResult).Value);
        }

        [Test]
        public async Task RedirectCities_AnyInt_ReturnsOkResult()
        {
            // Act
            var result = await _RegionController.RedirectCities(1, 2);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task EditRegion_NullInt_ReturnsOkResult()
        {
            // Act
            var result = await _RegionController.EditRegion(1, null);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetRegionAdmins_Int_ReturnsOkObjResult()
        {
            // Arrange
            int id = 2;
            _regionAdministrationService.Setup(x => x.GetAdministrationAsync(id)).ReturnsAsync(GetAdmins());
            // Act
            var result = await _RegionController.GetRegionAdmins(id);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<RegionAdministrationDTO>>((result as ObjectResult).Value);
        }

        [Test]
        public async Task GetRegionHead_Int_ReturnsOkObjResult()
        {
            // Arrange
            int id = 2;
            var head = new RegionAdministrationDTO() { ID = 2 };
            _regionAdministrationService.Setup(x => x.GetHead(id)).ReturnsAsync(head);
            // Act
            var result = await _RegionController.GetRegionHead(id);
            var actual = (result as ObjectResult).Value as RegionAdministrationDTO;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<RegionAdministrationDTO>((result as ObjectResult).Value);
            Assert.AreEqual(actual.ID, 2);
        }

        [Test]
        public async Task AddAdministrator_Null_ReturnsNoContentResult()
        {
            // Arrange
            RegionAdministrationDTO admin = null;
            // Act
            var result = await _RegionController.AddAdministrator(admin);
            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddAdministrator_CorrectData_ReturnsNoContentResult()
        {
            // Arrange
            RegionAdministrationDTO admin = new RegionAdministrationDTO() { ID=2};
            _regionAdministrationService.Setup(x => x.AddRegionAdministrator(admin));
            // Act
            var result = await _RegionController.AddAdministrator(admin);
            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task EditAdministrator_CorrectData_ReturnsNoContentResult()
        {
            // Arrange
            RegionAdministrationDTO admin = new RegionAdministrationDTO() { ID = 2 };
            _regionAdministrationService.Setup(x => x.EditRegionAdministrator(admin));
            _logger.Setup(x=> x.LogInformation(It.IsAny<string>()));
            // Act
            var result = await _RegionController.EditAdministrator(admin);
            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
         }

        [Test]
        public async Task EditAdministrator_Null_ReturnsNoContentResult()
        {
            // Arrange
            RegionAdministrationDTO admin = null;
            _regionAdministrationService.Setup(x => x.EditRegionAdministrator(admin));
            _logger.Setup(x => x.LogError(It.IsAny<string>()));
            // Act
            var result = await _RegionController.EditAdministrator(admin);
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetRegions_ReturnsRegions()
        {
            // Arrange
            _regionService.Setup(x => x.GetRegions()).ReturnsAsync(GetAdminRegions());
            // Act
            var result = await _RegionController.GetRegions();
             var actual = (result as ObjectResult).Value;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<RegionForAdministrationDTO>>(actual);
        }

        [Test]
        public async Task Remove_ReturnsRegions()
        {
            // Arrange
            int id = 2;
            // Act
            var result = await _RegionController.Remove(id);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task AddDocument_NewDocument_ReturnsNullOkObjectResult()
        {
            // Arrange
            RegionDocumentDTO document = new RegionDocumentDTO();
             // Act
            var result = await _RegionController.AddDocument(document);
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            var actual = (result as ObjectResult).Value;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<RegionDocumentDTO>(actual);
        }

        [Test]
        public async Task RemoveAdmin_ReturnsOkResult()
        {
            // Arrange
            int id = 2;
            // Act
            var result = await _RegionController.RemoveAdmin(id);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
         }

        [Test]
        public async Task GetUserAdministrations_NullString_ReturnsOkResult()
        {
            // Arrange
            string id = null;
            // Act
            var result = await _RegionController.GetUserAdministrations(id);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserAdministrations_String_ReturnsOkResult()
        {
            // Arrange
            string id = "admin";
            _regionAdministrationService.Setup(x=>x.GetUsersAdministrations(It.IsAny<string>())).ReturnsAsync(GetAdmins());
            // Act
            var result = await _RegionController.GetUserAdministrations(id);
            var actual = (result as ObjectResult).Value;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<RegionAdministrationDTO>>(actual);
        }
        [Test]
        public async Task GetUserPrevAdministrations_NullString_ReturnsOkResult()
        {
            // Arrange
            string id = null;
            // Act
            var result = await _RegionController.GetUserPrevAdministrations(id);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserPrevAdministrations_String_ReturnsOkResult()
        {
            // Arrange
            string id = "admin";
            _regionAdministrationService.Setup(x => x.GetUsersPreviousAdministrations(It.IsAny<string>())).ReturnsAsync(GetAdmins());
            // Act
            var result = await _RegionController.GetUserAdministrations(id);
            var actual = (result as ObjectResult).Value;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<RegionAdministrationDTO>>(actual);
        }

        [Test]
        public async Task RemoveDocument_Id_ReturnsOkResult()
        {
            // Arrange
            int id = 2;
            // Act
            var result = await _RegionController.RemoveDocument(id);
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }


        [Test]
        public async Task GetFileBase64_ReturnsString()
        {
            // Arrange
            _regionService.Setup(x => x.DownloadFileAsync(It.IsAny<string>())).ReturnsAsync("file");
            // Act
            var result = await _RegionController.GetFileBase64("file");
            var actual = (result as ObjectResult).Value;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<string>(actual);
        }

        [Test]
        public async Task GetRegionDocs_Id_ReturnsOkResult()
        {
            // Arrange
            int id = 2;
            _regionService.Setup(x => x.GetRegionDocsAsync(It.IsAny<int>())).ReturnsAsync(new List<RegionDocumentDTO>());
            // Act
            var result = await _RegionController.GetRegionDocs(id);
            var actual = (result as ObjectResult).Value;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<RegionDocumentDTO>>(actual);
        }

        [Test]
        public async Task AddFollower_ReturnsOkResult()
        {
            // Arrange
            int id = 2;
            int idCity = 2;
            _regionService.Setup(x => x.AddFollowerAsync(It.IsAny<int>(), It.IsAny<int>()));
            // Act
            var result = await _RegionController.AddFollower(id, idCity);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }


        [Test]
        public async Task GetMembers_ReturnsOkObjectResult()
        {
            // Arrange
            int id = 2;
            _regionService.Setup(x => x.GetMembersAsync(It.IsAny<int>())).ReturnsAsync(new List<CityDTO>());
            // Act
            var result = await _RegionController.GetMembers(id);
            var actual = (result as ObjectResult).Value;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<CityDTO>>(actual);
        }

        [Test]
        public async Task GetAdminTypes_ReturnsOkObjectResult()
        {
            // Arrange
            _regionAdministrationService.Setup(x => x.GetAllAdminTypes()).ReturnsAsync(new List<AdminTypeDTO>());
            // Act
            var result = await _RegionController.GetAdminTypes();
            var actual = (result as ObjectResult).Value;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<AdminTypeDTO>>(actual);
        }

        [Test]
        public async Task GetAdminTypeId_TypeNameString_ReturnsAdminTypeId()
        {
            // Arrange
            string TypeName = "Admin";
            _regionAdministrationService.Setup(x => x.GetAdminType(TypeName)).ReturnsAsync(2);
            // Act
            var result = await _RegionController.GetAdminTypeId(TypeName);

            // Assert

            Assert.AreEqual(result, 2);
           
        }

        [Test]
        public async Task GetReportByIdAsync_ReturnsReportDTO()
        {
            // Arrange
            _RegionAnnualReportService.Setup(x => x.GetReportByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new RegionAnnualReportDTO());
            // Act
            var result = await _RegionController.GetReportByIdAsync(1,2);
            var actual = (result as ObjectResult).Value;
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<RegionAnnualReportDTO>(actual);

        }

        [Test]
        public async Task GetAllRegionsReportsAsync_ReturnsReportDTO()
        {
            // Arrange
            _RegionAnnualReportService.Setup(x => x.GetAllRegionsReportsAsync()).ReturnsAsync(new List<RegionAnnualReportDTO>());
            // Act
            var result = await _RegionController.GetAllRegionsReportsAsync();
            var actual = (result as ObjectResult).Value;
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<RegionAnnualReportDTO>>(actual);

        }
       private IEnumerable<RegionDTO> GetRegions() {
            return new List<RegionDTO>()
            {
                new RegionDTO(){ ID =2, RegionName="Lviv"},
                new RegionDTO(){ ID =3},
                new RegionDTO(){ ID =4},
                new RegionDTO(){ ID =5}
            };
        
        }
        private IEnumerable<RegionAdministrationDTO> GetAdmins()
        {
            return new List<RegionAdministrationDTO>()
            {
                new RegionAdministrationDTO(){ ID =2 },
                new RegionAdministrationDTO(){ ID =3 },
                new RegionAdministrationDTO(){ ID =4 },
                new RegionAdministrationDTO(){ ID =5 }
            };

        }
        private IEnumerable<RegionForAdministrationDTO> GetAdminRegions()
        {
            return new List<RegionForAdministrationDTO>()
            {
                new RegionForAdministrationDTO(){ ID =2, RegionName="Lviv"},
                new RegionForAdministrationDTO(){ ID =3},
                new RegionForAdministrationDTO(){ ID =4},
                new RegionForAdministrationDTO(){ ID =5}
            };

        }
    }
}
