﻿using EPlast.BLL.DTO;
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
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.Resources;
using EPlast.BLL.Interfaces.Cache;

namespace EPlast.Tests.Controllers
{
    internal class RegionsControllerTests
    {

        private Mock<ILoggerService<CitiesController>> _logger;
        private Mock<IRegionAdministrationService> _regionAdministrationService;
        private Mock<IRegionAnnualReportService> _regionAnnualReportService;
        private RegionsController _regionController;
        private Mock<IRegionService> _regionService;
        private Mock<UserManager<User>> _userManager;
        private Mock<ICacheService> _cache;

        [Test]
        public async Task AddAdministrator_CorrectData_ReturnsOkObjectResult()
        {
            // Arrange
            RegionAdministrationDTO admin = new RegionAdministrationDTO() { ID = 2 };
            _regionAdministrationService.Setup(x => x.AddRegionAdministrator(admin));
            // Act
            var result = await _regionController.AddAdministrator(admin);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddAdministrator_Null_ReturnsNoContentResult()
        {
            // Arrange
            RegionAdministrationDTO admin = null;
            // Act
            var result = await _regionController.AddAdministrator(admin);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
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
        public async Task EditAdministrator_CorrectData_ReturnsOkObjectResult()
        {
            // Arrange
            RegionAdministrationDTO admin = new RegionAdministrationDTO() { ID = 2 };
            _regionAdministrationService.Setup(x => x.EditRegionAdministrator(admin));
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            // Act
            var result = await _regionController.EditAdministrator(admin);
            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
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
        public async Task Get_AllActiveRegions_ReturnsAllregionsOkResult()
        {
            // Arrange
            _regionService.Setup(x => x.GetAllActiveRegionsAsync()).ReturnsAsync(GetRegions());
            // Act
            var result = await _regionController.ActiveRegions();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<RegionDTO>>((result as ObjectResult).Value);
        }

        [Test]
        public async Task Get_AllNotActiveRegions_ReturnsAllregionsOkResult()
        {
            // Arrange
            _regionService.Setup(x => x.GetAllNotActiveRegionsAsync()).ReturnsAsync(GetRegions());

            // Act
            var result = await _regionController.NotActiveRegions();

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
        public async Task GetFollowers_ReturnsOkObjectResult()
        {
            // Arrange
            _regionService.Setup(x => x.GetFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<RegionFollowerDTO>());
            // Act
            var result = await _regionController.GetFollowers(It.IsAny<int>());
            var actual = (result as ObjectResult).Value;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<RegionFollowerDTO>>(actual);
        }

        [Test]
        public async Task GetFollower_ReturnsOkObjectResult()
        {
            // Arrange
            _regionService.Setup(x => x.GetFollowerAsync(It.IsAny<int>()))
                .ReturnsAsync(new RegionFollowerDTO());
            // Act
            var result = await _regionController.GetFollower(It.IsAny<int>());
            var actual = (result as ObjectResult).Value;
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<RegionFollowerDTO>(actual);
        }

        [Test]
        public async Task GetFollower_ReturnsNull()
        {
            // Arrange
            RegionFollowerDTO regionFollower = null;
            _regionService
                .Setup(x => x.GetFollowerAsync(It.IsAny<int>()))
                .ReturnsAsync(regionFollower);
            // Act
            var result = await _regionController.GetFollower(It.IsAny<int>());
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task CreateFollower_ReturnsOkObjectResult()
        {
            // Arrange
            RegionFollowerDTO testFollower = new RegionFollowerDTO();
            _regionService.Setup(x => x.CreateFollowerAsync(It.IsAny<RegionFollowerDTO>()));
            // Act
            var result = await _regionController.CreateFollower(testFollower);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task RemoveFollower_ReturnsOkObjectResult()
        {
            // Arrange
            int id = 1;
            _regionService.Setup(x => x.RemoveFollowerAsync(It.IsAny<int>()));
            // Act
            var result = await _regionController.RemoveFollower(id);
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
        public void GetActiveRegionsNames_ReturnsActiveRegionsNames()
        {
            // Arrange
            _regionService.Setup(x => x.GetActiveRegionsNames()).Returns(GetRegionNames());
            
            // Act
            var result = _regionController.GetActiveRegionsNames();
            var actual = (result as ObjectResult).Value;
            
            // Assert
            _regionService.Verify();
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<RegionNamesDTO>>(actual);
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
            _regionAnnualReportService.Setup(x => x.GetReportByIdAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()))
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
        public async Task Archive_AnyInt_ReturnsOkResult()
        {
            // Act
            var result = await _regionController.ArchiveRegion(2);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task UnArchive_AnyInt_ReturnsOkResult()
        {
            // Act
            var result = await _regionController.UnArchiveRegion(2);

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
        public async Task RemoveRegion_ReturnsOkResult()
        {
            // Arrange
           
            int id = 2;
            _regionAdministrationService.Setup(x => x.GetAdministrationAsync(id))
                .ReturnsAsync(new List<RegionAdministrationDTO>() {new RegionAdministrationDTO() {ID = 30, Status = true}});
            _regionAdministrationService.Setup(x => x.DeleteAdminByIdAsync(30));
            _regionService.Setup(x => x.DeleteRegionByIdAsync(id));
            // Act
            var result = await _regionController.RemoveRegion(id);
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

            _regionAnnualReportService.Setup(x => x.EditAsync(It.IsAny<int>(), It.IsAny<RegionAnnualReportQuestions>()));
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
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            _regionAnnualReportService.Setup(x => x.ConfirmAsync(It.IsAny<int>()));

            // Act
            var result = await _regionController.Confirm(0);
            
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
        public async Task GetActiveRegions_PagePageSizeRegionName_ReturnsStatusCodeOKAsync()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string regionName = "Lviv";
            bool isArchive = false;
            _regionService
                .Setup(u => u.GetAllRegionsByPageAndIsArchiveAsync(page, pageSize, regionName, isArchive))
                .ReturnsAsync(CreateTuple);
            var expected = StatusCodes.Status200OK;
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(new ClaimsPrincipal());
            _regionController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await _regionController.GetActiveRegions(page, pageSize, regionName);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            _regionService.Verify((u => u.GetAllRegionsByPageAndIsArchiveAsync(page, pageSize, regionName, isArchive)));
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetActiveRegions_PagePageSize_ReturnsStatusCodeOKAsync()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string regionName = null;
            bool isArchive = false;
            _regionService
                .Setup(u => u.GetAllRegionsByPageAndIsArchiveAsync(page, pageSize, regionName, isArchive))
                .ReturnsAsync(CreateTuple);
            var expected = StatusCodes.Status200OK;
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(new ClaimsPrincipal());

            _regionController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await _regionController.GetActiveRegions(page, pageSize, regionName);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            _regionService.Verify((u => u.GetAllRegionsByPageAndIsArchiveAsync(page, pageSize, regionName, isArchive)));
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetNotActiveRegions_PagePageSizeRegionName_StatusCodesStatus200OKAsync()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string regionName = "Lviv";
            bool isArchive = true;
            _regionService
                .Setup(u => u.GetAllRegionsByPageAndIsArchiveAsync(page, pageSize, regionName, isArchive))
                .ReturnsAsync(CreateTuple);
            var expected = StatusCodes.Status200OK;
            // Act
            var result = await _regionController.GetNotActiveRegions(page, pageSize, regionName);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            _regionService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetNotActiveRegions_PagePageSize_StatusCodesStatus200OKAsync()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string regionName = null;
            bool isArchive = true;
            _regionService
                .Setup(u => u.GetAllRegionsByPageAndIsArchiveAsync(page, pageSize, regionName, isArchive))
                .ReturnsAsync(CreateTuple);
            var expected = StatusCodes.Status200OK;
            // Act
            var result = await _regionController.GetNotActiveRegions(page, pageSize, regionName);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            _regionService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
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
            _cache = new Mock<ICacheService>();
            _regionController = new RegionsController(_cache.Object,
                _logger.Object,
                _regionService.Object, 
                _regionAdministrationService.Object,
                _regionAnnualReportService.Object, 
                _userManager.Object);
        }

        [Test]
        public async Task GetRegionUsers_returnOk()
        {
            // Arrange
            _regionService.Setup(x => x.GetRegionUsersAsync(It.IsAny<int>())).ReturnsAsync(new List<RegionUserDTO>());
            int regionID = 1;
            // Act
            var result = await _regionController.GetRegionUsers(regionID);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<RegionUserDTO>>((result as ObjectResult).Value);
        }

        [Test]
        public async Task CheckIfRegionNameExists_ReturnsOkObjectResult()
        {
            //Arrange
            _regionService.Setup(x => x.CheckIfRegionNameExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            //Act
            var result = await _regionController.CheckIfRegionNameExists(It.IsAny<string>());
            var resultObject = (result as ObjectResult).Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, resultObject);
            Assert.IsInstanceOf<OkObjectResult>(result);
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
                new RegionForAdministrationDTO(){ ID = 2, RegionName="Lviv"},
                new RegionForAdministrationDTO(){ ID = 3 },
                new RegionForAdministrationDTO(){ ID = 4 },
                new RegionForAdministrationDTO(){ ID = 5 }
            };
        }

        private IEnumerable<RegionNamesDTO> GetRegionNames()
        {
            return new List<RegionNamesDTO>()
            {
                new RegionNamesDTO(){ ID = 2, RegionName="Lviv"},
                new RegionNamesDTO(){ ID = 3 },
                new RegionNamesDTO(){ ID = 4 },
                new RegionNamesDTO(){ ID = 5 }
            };
        }

        private IEnumerable<RegionAdministrationDTO> GetAdmins()
        {
            return new List<RegionAdministrationDTO>()
            {
                new RegionAdministrationDTO(){ ID = 2 },
                new RegionAdministrationDTO(){ ID = 3 },
                new RegionAdministrationDTO(){ ID = 4 },
                new RegionAdministrationDTO(){ ID = 5 }
            };
        }

        private IEnumerable<RegionDTO> GetRegions()
        {
            return new List<RegionDTO>()
            {
                new RegionDTO(){ ID = 2, RegionName="Lviv"},
                new RegionDTO(){ ID = 3 },
                new RegionDTO(){ ID = 4 },
                new RegionDTO(){ ID = 5 }
            };
        }
        private Tuple<IEnumerable<RegionObjectsDTO>, int> CreateTuple => new Tuple<IEnumerable<RegionObjectsDTO>, int>(CreateRegionObjects, 100);

        private IEnumerable<RegionObjectsDTO> CreateRegionObjects => new List<RegionObjectsDTO>()
        {
            new RegionObjectsDTO(),
            new RegionObjectsDTO()
        };
    }
}
