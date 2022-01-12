using System;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.City;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Cache;
using EPlast.BLL.Queries.City;
using EPlast.Resources;
using MediatR;

namespace EPlast.Tests.Controllers
{
    internal class CityControllerTests
    {
        private readonly Mock<ICityAccessService> _cityAccessService;
        private readonly Mock<ICityDocumentsService> _cityDocumentsService;
        private readonly Mock<ICityParticipantsService> _cityParticipantsService;
        private readonly Mock<ICityService> _cityService;
        private readonly Mock<ILoggerService<CitiesController>> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ICacheService> _cache;
        private readonly Mock<Microsoft.AspNetCore.Identity.UserManager<User>> _userManager;
        private readonly Mock<IMediator> _mockMediator;

        public CityControllerTests()
        {
            _cityAccessService = new Mock<ICityAccessService>();
            _cityService = new Mock<ICityService>();
            _cityParticipantsService = new Mock<ICityParticipantsService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILoggerService<CitiesController>>();
            _cityDocumentsService = new Mock<ICityDocumentsService>();
            _cache = new Mock<ICacheService>();
            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<User>>();
            _userManager = new Mock<Microsoft.AspNetCore.Identity.UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _mockMediator = new Mock<IMediator>();
        }

        private CitiesController CreateCityController => new CitiesController(_logger.Object,
            _mapper.Object,
            _cityService.Object,
            _cityDocumentsService.Object,
            _cityAccessService.Object,
            _userManager.Object,
            _cityParticipantsService.Object,
            _cache.Object,
            _mockMediator.Object
          );

        [Test]
        public async Task AddAdmin_Valid_Test()
        {
            // Arrange
            CityAdministrationViewModel admin = new CityAdministrationViewModel();
            _mapper
                .Setup(m => m.Map<CityAdministrationViewModel, CityAdministrationDTO>(It.IsAny<CityAdministrationViewModel>()))
                .Returns(new CityAdministrationDTO() { AdminType = new BLL.DTO.Admin.AdminTypeDTO() });
            _cityParticipantsService
                .Setup(c => c.AddAdministratorAsync(It.IsAny<CityAdministrationDTO>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.AddAdmin(admin);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetCheckPlastMember_UserId_ReturnOk()
        {
            //Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<PlastMemberCheckQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new bool());

            //Act
            var result = await CreateCityController.GetCheckPlastMember(It.IsAny<string>());
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<bool>(resultValue);
        }

        [Test]
        public async Task IsUserApproved_UserId_ReturnsOk()
        {
            // Arrange
            _cityParticipantsService
                .Setup(x => x.CheckIsUserApproved(It.IsAny<int>()))
                .ReturnsAsync(new bool());
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.IsUserApproved(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task IsUserApproved_UserId_ReturnsBadRequest()
        {
            // Arrange
            _cityParticipantsService
                .Setup(x => x.CheckIsUserApproved(It.IsAny<int>()))
                .ReturnsAsync(new bool?());
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.IsUserApproved(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task AddDocument_Valid_Test()
        {
            // Arrange
            CityDocumentsViewModel document = new CityDocumentsViewModel();
            _mapper
                .Setup(m => m.Map<CityDocumentsViewModel, CityDocumentsDTO>(It.IsAny<CityDocumentsViewModel>()))
                .Returns(new CityDocumentsDTO());
            _cityDocumentsService
                .Setup(c => c.AddDocumentAsync(It.IsAny<CityDocumentsDTO>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.AddDocument(document);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetCityUsers_CityId_ReturnsOk()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CityUserDTO>());
            const int cityId = 1;

            // Act
            var result = await CreateCityController.GetCityUsers(cityId);
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<CityUserDTO>>(resultValue);
        }

        [Test]
        public async Task GetCityAdmins_CityId_ReturnsOkObjResult()
        {
            // Arrange
            const int id = 2;
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetAdministrationQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAdmins());

            // Act
            var result = await CreateCityController.GetAdministrations(id);
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<CityAdministrationGetDTO>>(resultValue);
        }

        [Test]
        public async Task AddFollower_Valid_Test()
        {
            _cityParticipantsService.Setup(c => c.AddFollowerAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(new CityMembersDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.AddFollower(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddFollowerWithId_Valid_Test()
        {
            // Arrange
            _cityParticipantsService
                .Setup(c => c.AddFollowerAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new CityMembersDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.AddFollowerWithId(GetFakeID(), GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ChangeApproveStatus_Valid_Test()
        {
            // Arrange
            _cityParticipantsService
                .Setup(c => c.ToggleApproveStatusAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityMembersDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.ChangeApproveStatus(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetCityNameOfApprovedMemberTest()
        {
            //Arrange
            _cityParticipantsService
                .Setup(c => c.CityOfApprovedMember(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            //Act
            var result = await controller.CityNameOfApprovedMember(GetStringFakeId());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task Create_InvalidModelState_Valid_Test()
        {
            // Arrange
            CityViewModel TestVM = new CityViewModel();
            _cityService
                .Setup(c => c.CreateAsync(It.IsAny<CityDTO>()))
                .ReturnsAsync(new int());
            _mapper
                .Setup(m => m.Map<CityViewModel, CityDTO>(It.IsAny<CityViewModel>()))
                .Returns(new CityDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;
            controller.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await controller.Create(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_Test()
        {
            // Arrange
            CityViewModel TestVM = new CityViewModel();
            _cityService
                .Setup(c => c.CreateAsync(It.IsAny<CityDTO>()))
                .ReturnsAsync(new int());
            _mapper
                .Setup(m => m.Map<CityViewModel, CityDTO>(It.IsAny<CityViewModel>()))
                .Returns(new CityDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.Create(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Details_Invalid_Test()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityByIdWthFullInfoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await CreateCityController.Details(GetFakeID());
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
            Assert.IsNull(resultValue);
        }

        [Test]
        public async Task Details_Valid_Test()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityByIdWthFullInfoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CityDTO());
            _mapper
                .Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(new CityViewModel());

            // Act
            var result = await CreateCityController.Details(GetFakeID());
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<CityViewModel>(resultValue);
        }

        [Test]
        public async Task Edit_InvalidModelState_Valid_Test()
        {
            // Arrange
            CityViewModel TestVM = new CityViewModel();
            _cityService
                .Setup(c => c.EditAsync(It.IsAny<CityDTO>()));
            _mapper
                .Setup(m => m.Map<CityViewModel, CityDTO>(It.IsAny<CityViewModel>()))
                .Returns(new CityDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;
            controller.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await controller.Edit(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Edit_Valid_Test()
        {
            // Arrange
            CityViewModel TestVM = new CityViewModel();
            _cityService
                .Setup(c => c.EditAsync(It.IsAny<CityDTO>()));
            _mapper
                .Setup(m => m.Map<CityViewModel, CityDTO>(It.IsAny<CityViewModel>()))
                .Returns(new CityDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.Edit(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task EditAdmin_Valid_Test()
        {
            // Arrange
            CityAdministrationViewModel admin = new CityAdministrationViewModel();
            _mapper
                .Setup(m => m.Map<CityAdministrationViewModel, CityAdministrationDTO>(It.IsAny<CityAdministrationViewModel>()))
                .Returns(new CityAdministrationDTO());
            _cityParticipantsService
                .Setup(c => c.EditAdministratorAsync(It.IsAny<CityAdministrationDTO>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.EditAdmin(admin);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EditAdmin_OldEndDate_ThrowsException()
        {
            //Arrange
            var testAdmin = new CityAdministrationViewModel() { EndDate = DateTime.MinValue };
            var controller = CreateCityController;

            //Act
            var result = await controller.EditAdmin(testAdmin);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [TestCase(2)]
        public async Task GetAdmins_Invalid_Test(int id)
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityAdminsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);
            
            // Act
            var result = await CreateCityController.GetAdmins(id);
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
            Assert.IsNull(resultValue);
        }

        [TestCase(2)]
        public async Task GetAdmins_Valid_Test(int id)
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityAdminsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CityProfileDTO());
            _mapper
                .Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());
            _userManager
                .Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _cityAccessService
                .Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await CreateCityController.GetAdmins(id);
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
        }

        [Test]
        public async Task GetAdminsIds_ReturnsOkObject()
        {
            //Arrange
            const string adminsIds = "Id1,Id2";
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityAdminsIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(adminsIds);

            //Act
            var result = await CreateCityController.GetAdminsIds(It.IsAny<int>());
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<string>(resultValue);
        }

        [Test]
        public async Task GetAdminsIds_ReturnsNotFound()
        {
            //Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityAdminsIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await CreateCityController.GetAdminsIds(It.IsAny<int>());
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
            Assert.IsNull(resultValue);
        }

        [Test]
        public async Task GetAllAdministrationStatuses_Valid_Test()
        {
            // Arrange
            _cityParticipantsService
                .Setup(c => c.GetAdministrationStatuses(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<CityAdministrationStatusDTO>>());
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.GetAllAdministrationStatuses(GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(1, 1, "Львів")]
        public async Task GetAllCities_Valid_Test(int page, int pageSize, string cityName)
        {
            // Arrange
            var controller = CreateCityController;
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            controller.ControllerContext = context;
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetAllCitiesOrByNameQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetCitiesBySearch());

            // Act
            var result = await controller.GetCities(page, pageSize, cityName);
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<CitiesViewModel>(resultValue);
        }

        [Test]
        public async Task GetActiveCities_PagePageSizeName_ReturnsStatusCodeOKAsync()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string name = "Lviv";
            bool isArchive = false;
            _cityService
                .Setup(u => u.GetAllCitiesByPageAndIsArchiveAsync(page, pageSize, name, isArchive))
                .ReturnsAsync(CreateTuple);
            var expected = StatusCodes.Status200OK;
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(new ClaimsPrincipal());
            CreateCityController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await CreateCityController.GetActiveCities(page, pageSize, name);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            _cityService.Verify((u => u.GetAllCitiesByPageAndIsArchiveAsync(page, pageSize, name, isArchive)));
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetActiveCities_PagePageSize_ReturnsStatusCodeOKAsync()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string name = null;
            bool isArchive = false;
            _cityService
                .Setup(u => u.GetAllCitiesByPageAndIsArchiveAsync(page, pageSize, name, isArchive))
                .ReturnsAsync(CreateTuple);
            var expected = StatusCodes.Status200OK;
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(new ClaimsPrincipal());

            // Act
            var result = await CreateCityController.GetActiveCities(page, pageSize, name);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            _cityService.Verify((u => u.GetAllCitiesByPageAndIsArchiveAsync(page, pageSize, name, isArchive)));
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetNotActiveCities_PagePageSizeName_StatusCodesStatus200OKAsync()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string name = "Lviv";
            bool isArchive = true;
            _cityService
                .Setup(u => u.GetAllCitiesByPageAndIsArchiveAsync(page, pageSize, name, isArchive))
                .ReturnsAsync(CreateTuple);
            var expected = StatusCodes.Status200OK;

            // Act
            var result = await CreateCityController.GetNotActiveCities(page, pageSize, name);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            _cityService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetNotActiveCities_PagePageSize_StatusCodesStatus200OKAsync()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string name = null;
            bool isArchive = true;
            _cityService
                .Setup(u => u.GetAllCitiesByPageAndIsArchiveAsync(page, pageSize, name, isArchive))
                .ReturnsAsync(CreateTuple);
            var expected = StatusCodes.Status200OK;

            // Act
            var result = await CreateCityController.GetNotActiveCities(page, pageSize, name);
            var actual = (result as ObjectResult).StatusCode;

            // Assert
            _cityService.Verify();
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetCities_Valid_Test()
        {
            // Arrange
            CitiesController controller = CreateCityController;
            var httpContext = new Mock<HttpContext>();
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            controller.ControllerContext = context;
            _cityService
                .Setup(c => c.GetCities())
                .ReturnsAsync(GetFakeCitiesForAdministration());

            // Act
            var result = await controller.GetCities();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(((result as ObjectResult).Value as List<CityForAdministrationDTO>)
                .Where(n => n.Name.Equals("Львів")));
        }

        [Test]
        public async Task GetCitiesThatUserHasAccessTo_Valid_Test()
        {
            // Arrange
            _cityAccessService
                .Setup(c => c.GetCitiesAsync(It.IsAny<User>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.GetCitiesThatUserHasAccessTo();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetDocuments_Valid_Test(int id)
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityDocumentsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CityProfileDTO());
            _mapper
                .Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());
            _userManager
                .Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _cityAccessService
                .Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await CreateCityController.GetDocuments(id);
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
        }

        [Test]
        public async Task GetDocumentsInvalidCheck()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityDocumentsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);
            
            // Act
            var result = await CreateCityController.GetDocuments(GetFakeID());
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
            Assert.IsNull(resultValue);
        }

        [Test]
        public async Task GetDocumentTypesAsync_Valid_Test()
        {
            // Arrange
            _cityDocumentsService
                .Setup(c => c.GetAllCityDocumentTypesAsync())
                .ReturnsAsync(It.IsAny<IEnumerable<CityDocumentTypeDTO>>());
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.GetDocumentTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetFileBase64_Valid_Test()
        {
            // Arrange
            _cityDocumentsService
                .Setup(c => c.DownloadFileAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<string>());
            CitiesController controller = CreateCityController;

            _cityParticipantsService.Setup(c => c.RemoveAdministratorAsync(It.IsAny<int>()));
            // Act
            var result = await controller.GetFileBase64(GetFakeFileName());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetFollowers_Invalid_Test(int id)
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityFollowersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await CreateCityController.GetFollowers(id);
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
            Assert.IsNull(resultValue);
        }

        [TestCase(2)]
        public async Task GetFollowers_Valid_Test(int id)
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityFollowersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CityProfileDTO());
            _mapper
                .Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());
            _userManager
                .Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _cityAccessService
                .Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await CreateCityController.GetFollowers(id);
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
        }

        [Test]
        public void GetLegalStatuses_Valid_Test()
        {
            // Arrange
            CitiesController controller = CreateCityController;

            // Act
            var result = controller.GetLegalStatuses();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetMembers_Invalid_Test(int id)
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityMembersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await CreateCityController.GetMembers(id);
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
            Assert.IsNull(resultValue);
        }

        [TestCase(2)]
        public async Task GetMembers_Valid_Test(int id)
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityMembersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CityProfileDTO());
            _mapper
                .Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());
            _userManager
                .Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _cityAccessService
                .Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await CreateCityController.GetMembers(id);
            var resultValue = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
        }

        [TestCase("logoName")]
        public async Task GetPhotoBase64_Valid_Test(string logoName)
        {
            // Arrange
            _cityService
                .Setup(c => c.GetLogoBase64(It.IsAny<string>()))
                .ReturnsAsync(new string("some string"));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.GetPhotoBase64(logoName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetProfile_Invalid_NotFound_Test(int id)
        {
            //Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityProfileQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await CreateCityController.GetProfile(id);
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
            Assert.IsNull(resultValue);
        }

        [TestCase(2)]
        public async Task GetProfile_Valid_Test(int id)
        {
            //Arrange
            _userManager
                .Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityProfileQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CityProfileDTO());
            _mapper
                .Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            //Act
            var result = await CreateCityController.GetProfile(id);
            var resultValue = (result as ObjectResult)?.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<CityViewModel>(resultValue);
        }

        [Test]
        public async Task GetUserAdministrations_Valid_Test()
        {
            // Arrange
            _cityParticipantsService
                .Setup(c => c.GetAdministrationsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<CityAdministrationDTO>>());
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.GetUserAdministrations(GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Archive_Valid_Test()
        {
            // Arrange
            _cityService
                .Setup(c => c.ArchiveAsync(It.IsAny<int>()));
            CitiesController citycon = CreateCityController;

            // Act
            var result = await citycon.Archive(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task UnArchive_Valid_Test()
        {
            // Arrange
            _cityService
                .Setup(c => c.UnArchiveAsync(It.IsAny<int>()));
            CitiesController citycon = CreateCityController;

            // Act
            var result = await citycon.UnArchive(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetUserPreviousAdministrations_Valid_Test()
        {
            // Arrange
            _cityParticipantsService
                .Setup(c => c.GetPreviousAdministrationsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<CityAdministrationDTO>>());
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.GetUserPreviousAdministrations(GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Remove_Valid_Test()
        {
            // Arrange
            _cityService
                .Setup(c => c.RemoveAsync(It.IsAny<int>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.Remove(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task RemoveAdmin_Valid_Test()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<CityAdministrationViewModel, CityAdministrationDTO>(It.IsAny<CityAdministrationViewModel>()))
                .Returns(new CityAdministrationDTO());
            _cityParticipantsService
                .Setup(c => c.RemoveAdministratorAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            _cityParticipantsService.Setup(c => c.EditAdministratorAsync(It.IsAny<CityAdministrationDTO>()));
            // Act
            var result = await controller.RemoveAdmin(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task RemoveDocument_Valid_Test()
        {
            // Arrange
            _cityDocumentsService
                .Setup(c => c.DeleteFileAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.RemoveDocument(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task RemoveFollower_Valid_Test()
        {
            // Arrange
            _cityParticipantsService
                .Setup(c => c.RemoveFollowerAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.RemoveFollower(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetCitiesNameThatUserHasAccessTo_Succeeded()
        {
            // Arrange
            _cityAccessService
                .Setup(c => c.GetAllCitiesIdAndName(It.IsAny<User>()));
            CitiesController controller = CreateCityController;

            // Act
            var result = await controller.GetCitiesNameThatUserHasAccessTo();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private List<CityDTO> GetCitiesBySearch()
        {
            return new List<CityDTO>()
            {
                new CityDTO()
                {
                    Name = "Львів",
                }
            };
        }

        private IEnumerable<CityForAdministrationDTO> GetFakeCitiesForAdministration()
        {
            return new List<CityForAdministrationDTO>()
            {
                new CityForAdministrationDTO
                {
                    Name = "Львів"
                }
            }.AsEnumerable();
        }

        private string GetFakeFileName()
        {
            return "FileName";
        }

        private int GetFakeID()
        {
            return 1;
        }

        private Tuple<IEnumerable<CityObjectDTO>, int> CreateTuple => new Tuple<IEnumerable<CityObjectDTO>, int>(CreateCityObjects, 100);

        private IEnumerable<CityObjectDTO> CreateCityObjects => new List<CityObjectDTO>()
        {
            new CityObjectDTO(),
            new CityObjectDTO()
        };

        private IEnumerable<CityAdministrationGetDTO> GetAdmins()
        {
            return new List<CityAdministrationGetDTO>()
            {
                new CityAdministrationGetDTO(){ Id = 2 },
                new CityAdministrationGetDTO(){ Id = 3 },
                new CityAdministrationGetDTO(){ Id = 4 },
                new CityAdministrationGetDTO(){ Id = 5 }
            };
        }
        
        private string GetStringFakeId()
        {
            return "1";
        }
    }
}

