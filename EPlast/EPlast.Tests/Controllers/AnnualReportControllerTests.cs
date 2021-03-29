using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    internal class AnnualReportControllerTest
    {
        private readonly Mock<IAnnualReportService> _annualReportService;
        private readonly Mock<IClubAnnualReportService> _clubAnnualReportService;
        private readonly Mock<IStringLocalizer<AnnualReportControllerMessage>> _localizer;
        private readonly Mock<ILoggerService<AnnualReportController>> _loggerService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<UserManager<User>> _userManager;

        public AnnualReportControllerTest()
        {
            _annualReportService = new Mock<IAnnualReportService>();
            _loggerService = new Mock<ILoggerService<AnnualReportController>>();
            _localizer = new Mock<IStringLocalizer<AnnualReportControllerMessage>>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _clubAnnualReportService = new Mock<IClubAnnualReportService>();
            _mapper = new Mock<IMapper>();
        }

        private AnnualReportController CreateAnnualReportController => new AnnualReportController(
            _annualReportService.Object,
            _loggerService.Object,
            _localizer.Object,
            _userManager.Object,
            _clubAnnualReportService.Object,
            _mapper.Object
            );

        [Test]
        public async Task Cancel_Invalid_NullReferenceException_Test()
        {
            _annualReportService.Setup(a => a.CancelAsync(It.IsAny<User>(), It.IsAny<int>()))
                 .Throws(new NullReferenceException());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["NotFound"])
               .Returns(GetNotFound());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act

            var result = await annualController.Cancel(5);
            var expected = StatusCodes.Status404NotFound;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            _localizer
              .Verify(s => s["NotFound"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Cancel_Invalid_UnAuthorisedException_Test()
        {
            _annualReportService.Setup(a => a.CancelAsync(It.IsAny<User>(), It.IsAny<int>()))
                .Throws(new UnauthorizedAccessException());

            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["NoAccess"])
               .Returns(GetNoAccess());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var result = await annualController.Cancel(5);
            var expected = StatusCodes.Status403Forbidden;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            _localizer
              .Verify(s => s["NoAccess"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Cancel_Valid_Test()
        {
            _annualReportService.Setup(a => a.CancelAsync(It.IsAny<User>(), It.IsAny<int>()));

            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            _loggerService.Setup(l => l.LogInformation(It.IsAny<string>()));

            _localizer
               .Setup(s => s["Canceled"])
               .Returns(GetCanceled());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act

            var result = await annualController.Cancel(5);
            var expected = StatusCodes.Status200OK;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            _localizer
              .Verify(s => s["Canceled"]);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task CancelClubAnnualReport_Invalid_NullReferenceFoundException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.CancelAsync(It.IsAny<User>(), It.IsAny<int>()))
                .Throws(new NullReferenceException());

            AnnualReportController _annualReportController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status404NotFound;
            var result = await _annualReportController.CancelClubAnnualReport(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task CancelClubAnnualReport_Invalid_UnauthorisedAccessException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.CancelAsync(It.IsAny<User>(), It.IsAny<int>()))
                .Throws(new UnauthorizedAccessException());
            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            AnnualReportController annualReportController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status403Forbidden;
            var result = await annualReportController.CancelClubAnnualReport(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task CancelClubAnnualReport_Valid_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.CancelAsync(It.IsAny<User>(), It.IsAny<int>()));
            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status200OK;
            var result = await annualController.CancelClubAnnualReport(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _clubAnnualReportService.Verify();
            _userManager.Verify();
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task CheckCreated_Invalid_NotCreated_Test()
        {
            _annualReportService.Setup(a => a.CheckCreated(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var result = await annualController.CheckCreated(5);
            var expected = StatusCodes.Status200OK;
            var actual = (result as ObjectResult).StatusCode;
            var hasCreatedValue = (result as ObjectResult).Value.GetType()
                .GetProperty("hasCreated").GetValue((result as ObjectResult).Value);

            // Assert
            _clubAnnualReportService.Verify();
            Assert.AreEqual(expected, actual);
            Assert.IsFalse((bool)hasCreatedValue);
            Assert.NotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task CheckCreated_Invalid_NullReferenceException_Test()
        {
            _annualReportService.Setup(a => a.CheckCreated(It.IsAny<User>(), It.IsAny<int>()))
                .Throws(new NullReferenceException());

            _localizer
              .Setup(s => s["CityNotFound"])
              .Returns(GetCityNotFound());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var result = await annualController.CheckCreated(5);
            var expected = StatusCodes.Status404NotFound;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            _localizer
             .Verify(s => s["CityNotFound"], Times.Once);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task CheckCreated_Invalid_UnauthorizedAccessException_Test()
        {
            _annualReportService.Setup(a => a.CheckCreated(It.IsAny<User>(), It.IsAny<int>()))
                .Throws(new UnauthorizedAccessException());

            _localizer
              .Setup(s => s["CityNoAccess"])
              .Returns(GetCityNoAccess());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var result = await annualController.CheckCreated(5);
            var expected = StatusCodes.Status403Forbidden;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            _localizer
             .Verify(s => s["CityNoAccess"], Times.Once);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task CheckCreated_Valid_Test()
        {
            _annualReportService.Setup(a => a.CheckCreated(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            _localizer
               .Setup(s => s["HasReport"])
               .Returns(GetHasReport());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var result = await annualController.CheckCreated(5);
            var expected = StatusCodes.Status200OK;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            _localizer
              .Verify(s => s["HasReport"], Times.Once);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Confirm_Invalid_NullReferenceException_Test()
        {
            _annualReportService.Setup(a => a.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>()))
                 .Throws(new NullReferenceException());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["NotFound"])
               .Returns(GetNotFound());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act

            var result = await annualController.Confirm(5);
            var expected = StatusCodes.Status404NotFound;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            _localizer
              .Verify(s => s["NotFound"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Confirm_Invalid_UnAuthorisedException_Test()
        {
            _annualReportService.Setup(a => a.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>()))
                .Throws(new UnauthorizedAccessException());

            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["NoAccess"])
               .Returns(GetNoAccess());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var result = await annualController.Confirm(5);
            var expected = StatusCodes.Status403Forbidden;
            var actual = (result as ObjectResult).StatusCode;

            // Assert

            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            _localizer
              .Verify(s => s["NoAccess"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Confirm_Valid_Test()
        {
            _annualReportService.Setup(a => a.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>()));

            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            _loggerService.Setup(l => l.LogInformation(It.IsAny<string>()));

            _localizer
               .Setup(s => s["Confirmed"])
               .Returns(GetConfirmed());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var result = await annualController.Confirm(5);
            var expected = StatusCodes.Status200OK;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            _localizer
              .Verify(s => s["Confirmed"]);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task ConfirmClubAnnualReport_Invalid_NullReferenceFoundException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>()))
                .Throws(new NullReferenceException());

            AnnualReportController _annualReportController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status404NotFound;
            var result = await _annualReportController.ConfirmClubAnnualReport(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task ConfirmClubAnnualReport_Invalid_UnauthorisedAccessException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>()))
               .Throws(new UnauthorizedAccessException());
            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            AnnualReportController annualReportController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status403Forbidden;
            var result = await annualReportController.ConfirmClubAnnualReport(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task ConfirmClubAnnualReport_Valid_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>()));
            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            AnnualReportController _annualReportController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status200OK;
            var result = await _annualReportController.ConfirmClubAnnualReport(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _clubAnnualReportService.Verify(a => a.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>()));
            _userManager.Verify(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>()));
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task Create_Invalid_InvalidOperationException_Test()
        {
            _annualReportService.Setup(a => a.CreateAsync(It.IsAny<User>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new InvalidOperationException());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["HasReport"])
               .Returns(GetHasReport());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            AnnualReportDTO rdto = new AnnualReportDTO();
            var result = await annualController.Create(rdto);
            var expected = StatusCodes.Status400BadRequest;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            _localizer
              .Verify(s => s["HasReport"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Create_Invalid_NullReferenceFoundException_Test()
        {
            _annualReportService.Setup(a => a.CreateAsync(It.IsAny<User>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new NullReferenceException());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["CityNotFound"])
               .Returns(GetCityNotFound());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            AnnualReportDTO rdto = new AnnualReportDTO();
            var result = await annualController.Create(rdto);
            var expected = StatusCodes.Status404NotFound;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            _localizer
              .Verify(s => s["CityNotFound"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Create_Invalid_UnAuthorisedException_Test()
        {
            _annualReportService.Setup(a => a.CreateAsync(It.IsAny<User>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new UnauthorizedAccessException());

            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["CityNoAccess"])
               .Returns(GetCityNoAccess());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            AnnualReportDTO rdto = new AnnualReportDTO();
            var result = await annualController.Create(rdto);
            var expected = StatusCodes.Status403Forbidden;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            _localizer
              .Verify(s => s["CityNoAccess"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_Test()
        {
            _annualReportService.Setup(a => a.CreateAsync(It.IsAny<User>(), It.IsAny<AnnualReportDTO>()));

            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            _loggerService.Setup(l => l.LogInformation(It.IsAny<string>()));

            _localizer
               .Setup(s => s["Created"])
               .Returns(GetCreated());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            AnnualReportDTO rdto = new AnnualReportDTO();
            var result = await annualController.Create(rdto);
            var expected = StatusCodes.Status201Created;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            _localizer
              .Verify(s => s["Created"]);
            _loggerService.Verify(l => l.LogInformation(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task CreateClubAnnualReport_Invalid_InvalidModelState_Test()
        {
            // Arrange
            AnnualReportController _annualReportController = CreateAnnualReportController;
            var _clubAnnualReportViewModel = new ClubAnnualReportViewModel();
            _annualReportController.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await _annualReportController.CreateClubAnnualReport(_clubAnnualReportViewModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task CreateClubAnnualReport_Invalid_InvalidOperationException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.CreateAsync(It.IsAny<User>(), It.IsAny<ClubAnnualReportDTO>()))
                .Throws(new InvalidOperationException());

            AnnualReportController _annualReportController = CreateAnnualReportController;
            var _clubAnnualReportViewModel = new ClubAnnualReportViewModel();

            // Act
            var result = await _annualReportController.CreateClubAnnualReport(_clubAnnualReportViewModel);
            var expected = StatusCodes.Status400BadRequest;
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task CreateClubAnnualReport_Invalid_NullReferenceFoundException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.CreateAsync(It.IsAny<User>(), It.IsAny<ClubAnnualReportDTO>()))
                .Throws(new NullReferenceException());

            AnnualReportController _annualReportController = CreateAnnualReportController;

            // Act
            var _clubAnnualReportViewModel = new ClubAnnualReportViewModel();
            var result = await _annualReportController.CreateClubAnnualReport(_clubAnnualReportViewModel);
            var expected = StatusCodes.Status404NotFound;
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task CreateClubAnnualReport_Valid_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.CreateAsync(It.IsAny<User>(), It.IsAny<ClubAnnualReportDTO>()));
            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            AnnualReportController _annualReportController = CreateAnnualReportController;
            var _clubAnnualReportViewModel = new ClubAnnualReportViewModel();

            // Act
            var result = await _annualReportController.CreateClubAnnualReport(_clubAnnualReportViewModel);
            var expected = StatusCodes.Status201Created;
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _clubAnnualReportService.Verify();
            _userManager.Verify();
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task CreateModelState_Invalid_Test()
        {
            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            AnnualReportDTO rdto = new AnnualReportDTO();
            annualController.ModelState.AddModelError("NameError", "Required");
            var result = await annualController.Create(rdto);

            // Assert
            Assert.NotNull(result);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Delete_Invalid_NullReferenceException_Test()
        {
            // Arrange
            _annualReportService.Setup(a => a.DeleteAsync(It.IsAny<User>(), It.IsAny<int>()))
                 .Throws(new NullReferenceException());
            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));
            _localizer
               .Setup(s => s["NotFound"])
               .Returns(GetNotFound());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var result = await annualController.Delete(5);

            // Assert
            Assert.NotNull(result);
            _localizer
              .Verify(s => s["NotFound"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Delete_Invalid_UnAuthorisedException_Test()
        {
            _annualReportService.Setup(a => a.DeleteAsync(It.IsAny<User>(), It.IsAny<int>()))
                .Throws(new UnauthorizedAccessException());

            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["NoAccess"])
               .Returns(GetNoAccess());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var result = await annualController.Delete(5);
            var expected = StatusCodes.Status403Forbidden;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            _localizer
              .Verify(s => s["NoAccess"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Delete_Valid_Test()
        {
            _annualReportService.Setup(a => a.DeleteAsync(It.IsAny<User>(), It.IsAny<int>()));

            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            _loggerService.Setup(l => l.LogInformation(It.IsAny<string>()));

            _localizer
               .Setup(s => s["Deleted"])
               .Returns(GetDeleted());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act

            var result = await annualController.Delete(5);
            var expected = StatusCodes.Status200OK;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            _localizer
              .Verify(s => s["Deleted"]);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task DeleteClubAnnualReport_Invalid_NullReferenceFoundException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.DeleteClubReportAsync(It.IsAny<User>(), It.IsAny<int>()))
                .Throws(new NullReferenceException());

            AnnualReportController _annualReportController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status404NotFound;
            var result = await _annualReportController.DeleteClubAnnualReport(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task DeleteClubAnnualReport_Invalid_UnauthorisedAccessException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.DeleteClubReportAsync(It.IsAny<User>(), It.IsAny<int>()))
                .Throws(new UnauthorizedAccessException());
            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            AnnualReportController annualReportController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status403Forbidden;
            var result = await annualReportController.DeleteClubAnnualReport(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task DeleteClubAnnualReport_Valid_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.DeleteClubReportAsync(It.IsAny<User>(), It.IsAny<int>()));
            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status200OK;
            var result = await annualController.DeleteClubAnnualReport(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _clubAnnualReportService.Verify();
            _userManager.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task Edit_Invalid_InvalidOperationException_Test()
        {
            _annualReportService.Setup(a => a.EditAsync(It.IsAny<User>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new InvalidOperationException());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["FailedEdit"])
               .Returns(GetFailedEdit());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            AnnualReportDTO rdto = new AnnualReportDTO();
            var result = await annualController.Edit(rdto);
            var expected = StatusCodes.Status400BadRequest;
            var actual = (result as ObjectResult).StatusCode;

            // Assert

            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            _localizer
              .Verify(s => s["FailedEdit"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Edit_Invalid_ModelState_Test()
        {
            AnnualReportController annualController = CreateAnnualReportController;
            AnnualReportDTO annualReport = new AnnualReportDTO();
            annualController.ModelState.AddModelError("NameError", "Required");
            // Act
            var result = await annualController.Edit(annualReport);

            // Assert
            Assert.NotNull(result);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Edit_Invalid_NullReferenceException_Test()
        {
            _annualReportService.Setup(a => a.EditAsync(It.IsAny<User>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new NullReferenceException());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["NotFound"])
               .Returns(GetNotFound());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            AnnualReportDTO rdto = new AnnualReportDTO();
            var result = await annualController.Edit(rdto);
            var expected = StatusCodes.Status404NotFound;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            _localizer
              .Verify(s => s["NotFound"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Edit_Invalid_UnAuthorisedException_Test()
        {
            _annualReportService.Setup(a => a.EditAsync(It.IsAny<User>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new UnauthorizedAccessException());

            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            _localizer
               .Setup(s => s["NoAccess"])
               .Returns(GetNoAccess());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            AnnualReportDTO rdto = new AnnualReportDTO();
            var result = await annualController.Edit(rdto);
            var expected = StatusCodes.Status403Forbidden;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            _localizer
              .Verify(s => s["NoAccess"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Edit_Valid_Test()
        {
            _annualReportService.Setup(a => a.EditAsync(It.IsAny<User>(), It.IsAny<AnnualReportDTO>()));

            _annualReportService.Setup(a => a.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(new AnnualReportDTO());

            _localizer
                .Setup(s => s["Edited"])
                .Returns(GetEdited());
            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            var expected = StatusCodes.Status200OK;

            AnnualReportController annualController = CreateAnnualReportController;
            AnnualReportDTO annualReport = new AnnualReportDTO();
            // Act
            var result = await annualController.Edit(annualReport);

            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            _localizer
                .Verify(s => s["Edited"]);
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task EditClubAnnualReport_Invalid_BadRequestObjectResult_Test()
        {
            // Arrange
            AnnualReportController _annualReportController = CreateAnnualReportController;
            ClubAnnualReportDTO _clubAnnualReportViewModel = new ClubAnnualReportDTO();
            _annualReportController.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await _annualReportController.EditClubAnnualReport(_clubAnnualReportViewModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task EditClubAnnualReport_Invalid_NullReferenceFoundException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.EditClubReportAsync(It.IsAny<User>(), It.IsAny<ClubAnnualReportDTO>()))
                .Throws(new NullReferenceException());

            AnnualReportController annualController = CreateAnnualReportController;
            ClubAnnualReportDTO annualReport = new ClubAnnualReportDTO();

            // Act
            var expected = StatusCodes.Status404NotFound;
            var result = await annualController.EditClubAnnualReport(annualReport);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task EditClubAnnualReport_Invalid_UnauthorisedAccessException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.EditClubReportAsync(It.IsAny<User>(), It.IsAny<ClubAnnualReportDTO>()))
                .Throws(new UnauthorizedAccessException());
            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            AnnualReportController annualController = CreateAnnualReportController;
            ClubAnnualReportDTO annualReport = new ClubAnnualReportDTO();

            // Act
            var expected = StatusCodes.Status403Forbidden;
            var result = await annualController.EditClubAnnualReport(annualReport);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task EditClubAnnualReport_Valid_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.EditClubReportAsync(It.IsAny<User>(), It.IsAny<ClubAnnualReportDTO>()));
            _clubAnnualReportService.Setup(a => a.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()))
               .ReturnsAsync(new ClubAnnualReportDTO());

            AnnualReportController annualController = CreateAnnualReportController;
            ClubAnnualReportDTO annualReport = new ClubAnnualReportDTO();

            // Act
            var expected = StatusCodes.Status200OK;
            var result = await annualController.EditClubAnnualReport(annualReport);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _clubAnnualReportService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task Get_Invalid_NullRefException_Test()
        {
            _annualReportService.Setup(a => a.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()))
               .Throws(new NullReferenceException());

            _localizer
               .Setup(s => s["NotFound"])
               .Returns(GetNotFound());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status404NotFound;
            var result = await annualController.Get(5);
            var actual = (result as ObjectResult).StatusCode;

            // Assert

            _localizer
               .Verify(s => s["NotFound"]);
            Assert.NotNull(result);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Get_InvalidUnauthorisedAccessException_ExceptionTest()
        {
            _annualReportService.Setup(a => a.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()))
               .Throws(new UnauthorizedAccessException());

            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            _localizer
               .Setup(s => s["NoAccess"])
               .Returns(GetNoAccess());

            _loggerService.Setup(l => l.LogError(It.IsAny<string>()));

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status403Forbidden;
            var result = await annualController.Get(5);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            _localizer
               .Verify(s => s["NoAccess"]);
            _loggerService.Verify(l => l.LogError(It.IsAny<string>()));

            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task Get_Valid_Test()
        {
            // Arrange
            _annualReportService.Setup(a => a.GetAllAsync(It.IsAny<User>()))
               .ReturnsAsync(new List<AnnualReportDTO>());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status200OK;
            var result = await annualController.Get();
            var actual = (result as ObjectResult).StatusCode;
            var resultValue = (result as ObjectResult).Value.GetType().GetProperty("annualReports")
                .GetValue((result as ObjectResult).Value).GetType();

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(result);
            Assert.AreEqual("List`1", resultValue.Name);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task GetAllClubAnnualReports_Valid_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(s => s.GetAllAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<ClubAnnualReportDTO>());
            AnnualReportController annualReportController = CreateAnnualReportController;

            // Act
            var result = await annualReportController.GetAllClubAnnualReports();
            var expected = StatusCodes.Status200OK;
            var actual = (result as ObjectResult).StatusCode;
            var resultValue = (result as ObjectResult).Value.GetType().GetProperty("clubAnnualReports")
                .GetValue((result as ObjectResult).Value).GetType();

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual("List`1", resultValue.Name);
            Assert.NotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task GetClubAnnualReportById_Invalid_NUllReferenceException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()))
               .Throws(new NullReferenceException());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status404NotFound;
            var result = await annualController.GetClubAnnualReportById(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task GetClubAnnualReportById_Invalid_UnauthorisedAccessException_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()))
               .Throws(new UnauthorizedAccessException());
            _userManager.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            AnnualReportController annualReportController = CreateAnnualReportController;

            // Act
            var expected = StatusCodes.Status403Forbidden;
            var result = await annualReportController.GetClubAnnualReportById(5);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task GetClubAnnualReportById_Valid_Test()
        {
            // Arrange
            _clubAnnualReportService.Setup(a => a.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(new ClubAnnualReportDTO());

            AnnualReportController _annualReportController = CreateAnnualReportController;

            // Act
            var result = await _annualReportController.GetClubAnnualReportById(5);
            var expected = StatusCodes.Status200OK;
            var actual = (result as ObjectResult).StatusCode;
            var resultValue = (result as ObjectResult).Value.GetType().GetProperty("annualreport")
                .GetValue((result as ObjectResult).Value);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(resultValue);
            Assert.NotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public void GetStatuses_Valid_Test()
        {
            // Arrange
            AnnualReportController annualController = CreateAnnualReportController;

            // Act
            var result = annualController.GetStatuses();
            var expected = StatusCodes.Status200OK;
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task GetWithparam_Valid_Test()
        {
            _annualReportService.Setup(a => a.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()))
               .ReturnsAsync(new AnnualReportDTO());

            AnnualReportController annualController = CreateAnnualReportController;

            // Act

            var result = await annualController.Get(5);
            var expected = StatusCodes.Status200OK;
            var actual = (result as ObjectResult).StatusCode;
            var resultValue = (result as ObjectResult).Value.GetType().GetProperty("annualReport")
                .GetValue((result as ObjectResult).Value);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.NotNull(resultValue);
            Assert.NotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        private LocalizedString GetCanceled()
        {
            var localizedString = new LocalizedString("Canceled",
                "Річний звіт успішно скасовано!");
            return localizedString;
        }

        private LocalizedString GetCityNoAccess()
        {
            var localizedString = new LocalizedString("CityNoAccess",
                "Ви не маєте доступу до даної станиці!");
            return localizedString;
        }

        private LocalizedString GetCityNotFound()
        {
            var localizedString = new LocalizedString("CityNotFound",
                "Не вдалося знайти інформацію про станицю!");
            return localizedString;
        }

        private LocalizedString GetConfirmed()
        {
            var localizedString = new LocalizedString("Confirmed",
                "Річний звіт успішно підтверджено!");
            return localizedString;
        }

        private LocalizedString GetCreated()
        {
            var localizedString = new LocalizedString("Creataed",
                "Річний звіт успішно створено!");
            return localizedString;
        }

        private LocalizedString GetDeleted()
        {
            var localizedString = new LocalizedString("Deleted",
                "Річний звіт успішно видалено!");
            return localizedString;
        }

        private LocalizedString GetEdited()
        {
            var localizedString = new LocalizedString("Edited",
                "Річний звіт успішно відредаговано!");
            return localizedString;
        }

        private LocalizedString GetFailedEdit()
        {
            var localizedString = new LocalizedString("FailedEdit",
                "Не вдалося редагувати річний звіт!");
            return localizedString;
        }

        private LocalizedString GetHasReport()
        {
            var localizedString = new LocalizedString("HasReport",
                "Станиця вже має створений річний звіт!");
            return localizedString;
        }

        private LocalizedString GetNoAccess()
        {
            var localizedString = new LocalizedString("NoAccess",
                "Ви не маєте доступу до даного річного звіту!");
            return localizedString;
        }

        private LocalizedString GetNotFound()
        {
            var localizedString = new LocalizedString("NotFound",
                "Не вдалося знайти річний звіт!");
            return localizedString;
        }
    }
}

