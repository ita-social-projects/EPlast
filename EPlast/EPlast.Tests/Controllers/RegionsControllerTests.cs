﻿using EPlast.BLL.DTO.Admin;
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
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace EPlast.Tests.Controllers
{
    class RegionsControllerTests
    {
        private  Mock<ILoggerService<CitiesController>> _logger;
        private  Mock<IRegionService> _regionService;
        private  Mock<IRegionAdministrationService> _regionAdministrationService;
        private  Mock<IRegionAnnualReportService> _regionAnnualReportService;
        private Mock<UserManager<User>> _userManager;
        private RegionsController _regionController;
        private Mock<IDistributedCache> _cache;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILoggerService<CitiesController>>();
            _regionService = new Mock<IRegionService>();
            _regionAdministrationService = new Mock<IRegionAdministrationService>();
            _regionAnnualReportService = new Mock<IRegionAnnualReportService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _cache = new Mock<IDistributedCache>();
            _regionController = new RegionsController(
                _logger.Object, _regionService.Object, _regionAdministrationService.Object, _regionAnnualReportService.Object, _userManager.Object, _cache.Object);
        }

        [Test]
        public async Task Get_AllRegions_ReturnsAllregionsOkResult()
        {
            // Arrange
            _regionService.Setup(x => x.GetAllRegionsAsync()).ReturnsAsync(GetRegions());
            // Act
            var result = await _regionController.Index();
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
            var result = await _regionController.CreateRegion(reg);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task CreateRegion_NullRegion_ReturnsOkResult()
        {
            // Act
            var result = await _regionController.CreateRegion(null);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetPhotoBase64_NullString_ReturnsOkObjResult()
        {
           // Act
            var result = await _regionController.GetPhotoBase64(null);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNull((result as ObjectResult).Value);
        }

        [Test]
        public async Task RedirectCities_AnyInt_ReturnsOkResult()
        {
            // Act
            var result = await _regionController.RedirectCities(1, 2);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task EditRegion_NullInt_ReturnsOkResult()
        {
            // Act
            var result = await _regionController.EditRegion(1, null);
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
            var result = await _regionController.GetRegionAdmins(id);
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
            var result = await _regionController.GetRegionHead(id);
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
            var result = await _regionController.AddAdministrator(admin);
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
            var result = await _regionController.AddAdministrator(admin);
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
            var result = await _regionController.EditAdministrator(admin);
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
            var result = await _regionController.EditAdministrator(admin);
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetRegions_ReturnsRegions()
        {
            // Arrange
            _regionService.Setup(x => x.GetRegions()).ReturnsAsync(GetAdminRegions());
            // Act
            var result = await _regionController.GetRegions();
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
            var result = await _regionController.Remove(id);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task AddDocument_NewDocument_ReturnsNullOkObjectResult()
        {
            // Arrange
            RegionDocumentDTO document = new RegionDocumentDTO();
             // Act
            var result = await _regionController.AddDocument(document);
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
            var result = await _regionController.RemoveAdmin(id);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
         }

        [Test]
        public async Task GetUserAdministrations_NullString_ReturnsOkResult()
        {
            // Arrange
            string id = null;
            // Act
            var result = await _regionController.GetUserAdministrations(id);
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
            var result = await _regionController.GetUserAdministrations(id);
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
            var result = await _regionController.GetUserPrevAdministrations(id);
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
            var result = await _regionController.GetUserAdministrations(id);
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
            var result = await _regionController.RemoveDocument(id);
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
            var result = await _regionController.GetFileBase64("file");
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
            var result = await _regionController.GetRegionDocs(id);
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
            var result = await _regionController.AddFollower(id, idCity);
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
            var result = await _regionController.GetMembers(id);
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
            var result = await _regionController.GetAdminTypes();
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
            var result = await _regionController.GetAdminTypeId(TypeName);

            // Assert

            Assert.AreEqual(result, 2);
           
        }

        [Test]
        public async Task GetReportByIdAsync_ReturnsReportDTO()
        {
            // Arrange
            _regionAnnualReportService.Setup(x => x.GetReportByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new RegionAnnualReportDTO());
            // Act
            var result = await _regionController.GetReportByIdAsync(1,2);
            var actual = (result as ObjectResult).Value;
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<RegionAnnualReportDTO>(actual);

        }

        [Test]
        public async Task GetAllRegionsReportsAsync_ReturnsReportDTO()
        {
            // Arrange
            _regionAnnualReportService.Setup(x => x.GetAllRegionsReportsAsync()).ReturnsAsync(new List<RegionAnnualReportDTO>());
            // Act
            var result = await _regionController.GetAllRegionsReportsAsync();
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
