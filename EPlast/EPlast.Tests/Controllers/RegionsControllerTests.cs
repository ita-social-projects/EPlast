using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Realizations.Base;
using EPlast.Resources;
using System.Text;

namespace EPlast.Tests.Controllers
{
    internal class RegionsControllerTests
    {
        private Mock<IDistributedCache> _cache;
        private Mock<ILoggerService<CitiesController>> _logger;
        private Mock<IRegionAdministrationService> _regionAdministrationService;
        private Mock<IRegionAnnualReportService> _regionAnnualReportService;
        private RegionsController _regionController;
        private Mock<IRegionService> _regionService;
        private Mock<UserManager<User>> _userManager;

        [Test]
        public async Task AddAdministrator_CorrectData_ReturnsNoContentResult()
        {
            // Arrange
            RegionAdministrationDTO admin = new RegionAdministrationDTO() { ID = 2 };
            _regionAdministrationService.Setup(x => x.AddRegionAdministrator(admin));
            // Act
            var result = await _regionController.AddAdministrator(admin);
            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
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
        public async Task CreateRegion_NewRegion_ReturnsOkResult()
        {
            // Arrange
            RegionDTO reg = new RegionDTO() { ID = 3, City = "Lviv" };
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
        public async Task CreateRegionAnnualReportById_Invalid_NullExcept_Test()
        {
            // Arrange
            _regionAnnualReportService
                .Setup(x => x.CreateByNameAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<RegionAnnualReportQuestions>()))
                .Throws(new NullReferenceException());

            // Act
            var expected = StatusCodes.Status404NotFound;
            var result = await _regionController.CreateRegionAnnualReportById(1, 2021, new RegionAnnualReportQuestions());
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task CreateRegionAnnualReportById_Invalid_UnathorizExcept_Test()
        {
            // Arrange
            _regionAnnualReportService
                .Setup(x => x.CreateByNameAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<RegionAnnualReportQuestions>()))
                .Throws(new UnauthorizedAccessException());

            // Act
            var expected = StatusCodes.Status403Forbidden;
            var result = await _regionController.CreateRegionAnnualReportById(1, 2021, new RegionAnnualReportQuestions());
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task CreateRegionAnnualReportById_InvalidOperationException_Test()
        {
            // Arrange
            _regionAnnualReportService
                .Setup(x => x.CreateByNameAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<RegionAnnualReportQuestions>()))
                .Throws(new InvalidOperationException());

            // Act
            var expected = StatusCodes.Status400BadRequest;
            var result = await _regionController.CreateRegionAnnualReportById(1, 2021, new RegionAnnualReportQuestions());
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task CreateRegionAnnualReportById_Valid_Test()
        {
            // Arrange
            _regionAnnualReportService
                .Setup(x => x.CreateByNameAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<RegionAnnualReportQuestions>()))
                .ReturnsAsync(new RegionAnnualReportDTO());

            // Act
            var expected = StatusCodes.Status201Created;
            var result = await _regionController.CreateRegionAnnualReportById(1, 2021, new RegionAnnualReportQuestions());
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAdministrator_CorrectData_ReturnsNoContentResult()
        {
            // Arrange
            RegionAdministrationDTO admin = new RegionAdministrationDTO() { ID = 2 };
            _regionAdministrationService.Setup(x => x.EditRegionAdministrator(admin));
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
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
        public async Task EditRegion_NullInt_ReturnsOkResult()
        {
            // Act
            var result = await _regionController.EditRegion(1, null);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
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
        public async Task GetAllRegionsReportsAsync_TakesParameters_OkObjectResult()
        {
            //Arrange
            var report = new RegionAnnualReportTableObject() { Id = 1 };
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() {"Admin"});
            _regionAnnualReportService.Setup(r => r.GetAllRegionsReportsAsync(It.IsAny<User>(), It.IsAny<bool>(),
                    It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<RegionAnnualReportTableObject>() {report});

            // Act
            var result = await _regionController.GetAllRegionsReportsAsync("",1,1,1, true);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<RegionAnnualReportTableObject>>((result as ObjectResult).Value);
            Assert.IsNotEmpty((result as ObjectResult).Value as List<RegionAnnualReportTableObject>);
            Assert.True(((result as ObjectResult).Value as List<RegionAnnualReportTableObject>).Contains(report));
        }

        [Test]
        public async Task GetRegionsNameThatUserHasAccessTo_Succeeded()
        {
            // Arrange
            _regionAnnualReportService.Setup(x => x.GetAllRegionsIdAndName(It.IsAny<User>()))
                .ReturnsAsync(new List<RegionForAdministrationDTO>());

            // Act
            var result = await _regionController.GetRegionsNameThatUserHasAccessTo();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAdminTypeId_TypeNameString_ReturnsAdminTypeId()
        {
            // Arrange
            string TypeName = Roles.Admin;
            _regionAdministrationService.Setup(x => x.GetAdminType(TypeName)).ReturnsAsync(2);
            // Act
            var actual = await _regionController.GetAdminTypeId(TypeName);

            // Assert

            Assert.AreEqual(2, actual );
        }

        [Test]
        public async Task GetAdminTypes_ReturnsOkObjectResult()
        {
            // Arrange
            var list = new List<AdminTypeDTO>() { new AdminTypeDTO() };
            _regionAdministrationService.Setup(x => x.GetAllAdminTypes())
                .ReturnsAsync(list);
            // Act
            var result = await _regionController.GetAdminTypes();
            var actual = ((IEnumerable<AdminTypeDTO>)(result as ObjectResult).Value);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(actual, list);
        }

        [Test]
        public async Task GetAllRegionsAnnualReportsAsync_ReturnsReportDTO()
        {
            // Arrange
            _userManager
                .Setup(x => x.GetUserAsync(new System.Security.Claims.ClaimsPrincipal())).ReturnsAsync(new User());
            _regionAnnualReportService
                .Setup(x => x.GetAllAsync(It.IsAny<User>())).ReturnsAsync(new List<RegionAnnualReportDTO>());

            // Act
            var result = await _regionController.GetAllRegionAnnualReports();
            // Assert
            _userManager.Verify();
            _regionAdministrationService.Verify();
            Assert.IsInstanceOf<ObjectResult>(result);
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
            _regionAnnualReportService.Verify();
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<RegionAnnualReportDTO>>(actual);
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
        public async Task GetPhotoBase64_NullString_ReturnsOkObjResult()
        {
            // Act
            var result = await _regionController.GetPhotoBase64(null);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNull((result as ObjectResult).Value);
        }

        [Test]
        public async Task GetProfile_InValid_Except_Test()
        {
            // Arrange
            _regionService
                .Setup(x => x.GetRegionProfileByIdAsync(It.IsAny<int>(), It.IsAny<User>()))
                .Throws(new Exception());

            // Act
            var result = await _regionController.GetProfile(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetProfile_InValid_regionsNotFound_Test()
        {
            // Arrange
            _regionService
               .Setup(x => x.GetRegionProfileByIdAsync(It.IsAny<int>(), It.IsAny<User>()));

            // Act
            var result = await _regionController.GetProfile(1);

            // Assert
            _regionService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetProfile_Valid_Test()
        {
            // Arrange
            _regionService
                .Setup(x => x.GetRegionProfileByIdAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(new RegionProfileDTO() { City = "Lviv" });

            // Act
            var result = await _regionController.GetProfile(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
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
        public async Task GetRegionHead_Int_ReturnsOkObjResult()
        {
            // Arrange
            int id = 2;
            var head = new RegionAdministrationDTO() { ID = 2 };
            _regionAdministrationService.Setup(x => x.GetHead(id)).ReturnsAsync(head);
            // Act
            var result = await _regionController.GetRegionHead(id);
            var actual = ((result as ObjectResult).Value as RegionAdministrationDTO).ID;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<RegionAdministrationDTO>((result as ObjectResult).Value);
            Assert.AreEqual(2, actual);
        }

        [Test]
        public async Task GetRegionHeadDeputy_Int_ReturnsOkObjResult()
        {
            // Arrange
            int id = 2;
            var headDeputy = new RegionAdministrationDTO() { ID = 2 };
            _regionAdministrationService.Setup(x => x.GetHeadDeputy(id)).ReturnsAsync(headDeputy);
            // Act
            var result = await _regionController.GetRegionHeadDeputy(id);
            var actual = ((result as ObjectResult).Value as RegionAdministrationDTO).ID;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<RegionAdministrationDTO>((result as ObjectResult).Value);
            Assert.AreEqual(2, actual);
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
            _regionService.Verify();
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<RegionForAdministrationDTO>>(actual);
        }

        [Test]
        public async Task GetRegionsBoardAsync_ReturnsRegionsBoard()
        {
            // Arrange
            _userManager
                .Setup(x => x.GetUserAsync(new System.Security.Claims.ClaimsPrincipal())).ReturnsAsync(new User());
            _regionService
                .Setup(x => x.GetRegionByNameAsync(EnumExtensions.GetDescription(RegionsStatusType.RegionBoard), It.IsAny<User>()))
                .ReturnsAsync(new RegionProfileDTO() { Status = RegionsStatusTypeDTO.RegionBoard });
            // Act
            var result = await _regionController.GetRegionsBoardAsync();
            var actual = (result as ObjectResult).Value;
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<RegionProfileDTO>(actual);
        }

        [Test]
        public async Task GetRegionsBoardAsync_GetDescription_string_empty()
        {
            // Arrange
            _userManager
                .Setup(x => x.GetUserAsync(new System.Security.Claims.ClaimsPrincipal())).ReturnsAsync(new User());
            _regionService
                .Setup(x => x.GetRegionByNameAsync(EnumExtensions.GetDescription(RegionsStatusType.RegionBoard), It.IsAny<User>()))
                .ReturnsAsync(new RegionProfileDTO());
            
            // Act
            var result = await _regionController.GetRegionsBoardAsync();
            var actual = (result as ObjectResult).Value;
            
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<RegionProfileDTO>(actual);
        }

        [Test]
        public async Task GetReportByIdAsync_ReturnsReportDTO()
        {
            // Arrange
            _regionAnnualReportService.Setup(x => x.GetReportByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new RegionAnnualReportDTO());
            
            // Act
            var result = await _regionController.GetReportByIdAsync(1, 2);
            var actual = (result as ObjectResult).Value;
            
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<RegionAnnualReportDTO>(actual); 
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
            string id = "Admin";
            _regionAdministrationService.Setup(x => x.GetUsersAdministrations(It.IsAny<string>())).ReturnsAsync(GetAdmins());
            
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
            string id = "Admin";
            _regionAdministrationService.Setup(x => x.GetUsersPreviousAdministrations(It.IsAny<string>())).ReturnsAsync(GetAdmins());
            
            // Act
            var result = await _regionController.GetUserAdministrations(id);
            var actual = (result as ObjectResult).Value;
            
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<RegionAdministrationDTO>>(actual);
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
        public async Task GetRegionMembersInfo_ReturnsOk()
        {
            // Arrange
            _regionAnnualReportService.Setup(x =>
                    x.GetRegionMembersInfoAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<RegionMembersInfoTableObject>());
            // Act
            var result = await _regionController.GetRegionMembersInfo(1, 1, 1, 1);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Delete_NullReferenceException()
        {
            // Arrange
            _regionAnnualReportService.Setup(x => x.DeleteAsync(It.IsAny<int>()));
            // Act
            var result = await _regionController.Delete(1);
            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.IsTrue((result as ObjectResult).StatusCode==404);
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
        public async Task Delete_Status200OK()
        {
            // Arrange
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { "Admin" });
            RegionAdministrationDTO admin = new RegionAdministrationDTO() { ID = 2 };

            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));

            _regionAnnualReportService.Setup(x => x.DeleteAsync(It.IsAny<int>()));
            
            // Act
            var result = await _regionController.Delete(1);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("{ message = Річний звіт округи видалено }", ((ObjectResult)result).Value.ToString());
        }


        [Test]
        public async Task EditRegionReport_Status404NotFound()
        {
            // Arrange
            int reportID = 0;

           // Act
            var result = await _regionController.EditRegionReport(reportID, null);
           
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("{ message = Річний звіт округи не знайдено }", ((ObjectResult)result).Value.ToString());
        }

        [Test]
        public async Task EditRegionReport_Status200OK()
        {

            // Arrange
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { "Admin" });

            _regionAnnualReportService.Setup(x => x.GetRegionMembersInfo(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<RegionMembersInfo>());
            int reportID = 1;
            
            // Act
            var result = await _regionController.EditRegionReport(reportID, fakeRegionAnnualReportQuestions());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("{ message = Річний звіт округи змінено }", ((ObjectResult)result).Value.ToString());
        }

        [Test]
        public async Task EditRegionReport_InvalidOperation()
        {

            // Arrange
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { "Admin" });

            _regionAnnualReportService.Setup(x => x.EditAsync(It.IsAny<int>(), It.IsAny<RegionAnnualReportQuestions>()))
                .ThrowsAsync(new InvalidOperationException());
            
            int reportID = 1;
            
            // Act
            var result = await _regionController.EditRegionReport(reportID, fakeRegionAnnualReportQuestions());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("{ message = Виникла помилка при внесенні змін до річного звіту округи }", ((ObjectResult)result).Value.ToString());
        }

        [Test]
        public async Task EditRegionReport_ModelState_NotValid()
        {
            // Arrange
            _regionController.ModelState.AddModelError("InvalidModel", "Required");
            int reportID = 1;

            // Act
            var result = await _regionController.EditRegionReport(reportID, fakeRegionAnnualReportQuestions());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public async Task Confirm_OkStatus()
        {
            // Arrange
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { "Admin" });
            RegionAdministrationDTO admin = new RegionAdministrationDTO() { ID = 2 };
 
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            _regionAnnualReportService.Setup(x => x.GetRegionMembersInfo(It.IsAny<int>(), It.IsAny<int>()))
                  .ReturnsAsync(new List<RegionMembersInfo>());
            int id = 0;
            
            // Act
            var result = await _regionController.Confirm(id);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("{ message = Річний звіт округи підтверджено }", ((ObjectResult)result).Value.ToString());
        }


        [Test]
        public async Task Cancel_OkStatus()
        {
            // Arrange
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { "Admin" });
            RegionAdministrationDTO admin = new RegionAdministrationDTO() { ID = 2 };

            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
      
            int id = 0;
            
            // Act
            var result = await _regionController.Cancel(id);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("{ message = Річний звіт округи скасовано }", ((ObjectResult)result).Value.ToString());
        }


        [Test]
        public async Task Cancel_NullReferenceException()
        {
            // Arrange
            _logger.Setup(x => x.LogError(It.IsAny<string>()));

            int id = -1;
            
            // Act
            var result = await _regionController.Cancel(id);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("{ message = Річний звіт округи не знайдено }", ((ObjectResult)result).Value.ToString());
        }


        [Test]
        public async Task GetRegions_Ok()
        {
            // Arrange
            int page = 0;
            int pageSize = 1;
            string regionName = "Lviv";

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(new ClaimsPrincipal());

            _regionController.ControllerContext.HttpContext = mockHttpContext.Object;
        
            byte[] bytes = Encoding.ASCII.GetBytes("[]");
            _cache.Setup(x => x.GetAsync(It.IsAny<string>(), default)).ReturnsAsync(bytes);

            // Act
            var result = await _regionController.GetRegions(page, pageSize, regionName);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }



        [Test]
        public async Task GetRegions_regionsIsnull()
        {
            // Arrange
            int page = 0;
            int pageSize = 1;
            string regionName = "Lviv";

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(new ClaimsPrincipal());

            _regionController.ControllerContext.HttpContext = mockHttpContext.Object;

            byte[] bytes = new byte[2];
            _cache.Setup(x => x.GetAsync(It.IsAny<string>(), default)).ReturnsAsync((byte[])null);
       
            // Act
            var result = await _regionController.GetRegions(page, pageSize, regionName);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

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
                _logger.Object,
                _regionService.Object, 
                _regionAdministrationService.Object,
                _regionAnnualReportService.Object, 
                _userManager.Object, 
                _cache.Object);
        }

        private RegionAnnualReportQuestions fakeRegionAnnualReportQuestions()
        {
            return new RegionAnnualReportQuestions()
            {
                StateOfPreparation = " ",

                Characteristic = " ",

                ChurchCooperation = " ",

                InvolvementOfVolunteers = " ",

                ImportantNeeds = " ",

                SocialProjects = " ",

                StatusOfStrategy = " ",

                SuccessStories = " ",

                ProblemSituations = " ",

                TrainedNeeds = " ",

                PublicFunding = " ",

                Fundraising = " ",
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

        private IEnumerable<RegionDTO> GetRegions()
        {
            return new List<RegionDTO>()
            {
                new RegionDTO(){ ID =2, RegionName="Lviv"},
                new RegionDTO(){ ID =3},
                new RegionDTO(){ ID =4},
                new RegionDTO(){ ID =5}
            };
        }
    }
}
