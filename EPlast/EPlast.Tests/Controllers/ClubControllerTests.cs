﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Commands.Club;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Club;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class ClubControllerTests
    {
        private readonly Mock<IClubService> _clubService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILoggerService<ClubController>> _logger;
        private readonly Mock<IClubParticipantsService> _clubParticipantsService;
        private readonly Mock<IClubAccessService> _clubAccessService;
        private readonly Mock<IClubDocumentsService> _clubDocumentsService;
        private readonly Mock<UserManager<User>> _userManager;


        public ClubControllerTests()
        {
            _clubAccessService = new Mock<IClubAccessService>();
            _clubService = new Mock<IClubService>();
            _mapper = new Mock<IMapper>();
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILoggerService<ClubController>>();
            _clubParticipantsService = new Mock<IClubParticipantsService>();
            _clubDocumentsService = new Mock<IClubDocumentsService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        }

        private ClubController CreateClubController => new ClubController(_logger.Object,
             _mapper.Object,
           _clubService.Object,
           _clubParticipantsService.Object,
           _clubDocumentsService.Object,
           _clubAccessService.Object,
           _userManager.Object,
             _mediator.Object
          );

        [TestCase(1, 1, "Курінь")]
        public async Task GetActivClub_Valid_Test(int page, int pageSize, string clubName)
        {
            // Arrange
            ClubController clubcon = CreateClubController;
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            clubcon.ControllerContext = context;
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllClubsByPageAndIsArchiveQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateTuple);

            // Act
            var result = await clubcon.GetActiveClubs(page, pageSize, clubName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(1, 1, "Курінь")]
        public async Task GetNotActivClub_Valid_Test(int page, int pageSize, string clubName)
        {
            // Arrange
            ClubController clubcon = CreateClubController;
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole(Roles.Admin))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            clubcon.ControllerContext = context;
            _mediator
               .Setup(m => m.Send(It.IsAny<GetAllClubsByPageAndIsArchiveQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(CreateTuple);

            // Act
            var result = await clubcon.GetNotActiveClubs(page, pageSize, clubName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetClubs_Valid_Test()
        {
            // Arrange
            ClubController controller = CreateClubController;
            var httpContext = new Mock<HttpContext>();
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            controller.ControllerContext = context;
            _clubService
                .Setup(c => c.GetClubs())
                .ReturnsAsync(GetFakeClubsForAdministration());

            // Act
            var result = await controller.GetClubs();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(((result as ObjectResult).Value as List<ClubForAdministrationDto>)
                .Where(n => n.Name.Equals("Курінь")));
        }

        [Test]
        public async Task IsUserApproved_UserId_ReturnsOk()
        {
            // Arrange
            _clubParticipantsService
                .Setup(x => x.CheckIsUserApproved(It.IsAny<int>()))
                .ReturnsAsync(new bool());
            ClubController controller = CreateClubController;

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
            _clubParticipantsService
                .Setup(x => x.CheckIsUserApproved(It.IsAny<int>()))
                .ReturnsAsync(new bool?());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.IsUserApproved(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetClubUsers_CityId_ReturnsOk()
        {
            // Arrange
            _clubService.Setup(x => x.GetClubUsersAsync(It.IsAny<int>())).ReturnsAsync(new List<ClubUserDto>());
            int cityID = 1;

            // Act
            var result = await CreateClubController.GetCityUsers(cityID);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<List<ClubUserDto>>((result as ObjectResult).Value);
        }

        [TestCase(2)]
        public async Task GetProfile_Valid_Test(int id)
        {

            _clubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(new ClubProfileDto());
            _mapper
                .Setup(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()))
                .Returns(new ClubViewModel());
            ClubController controller = CreateClubController;
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(new ClaimsPrincipal());

            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await controller.GetProfile(id);

            // Assert

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetProfile_Invalid_Test(int id)
        {
            // Arrange
            _clubService.
                Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()))
                .Returns(new ClubViewModel());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetProfile(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetClubAdmins_Int_ReturnsOkObjResult()
        {
            // Arrange
            int id = 2;
            _clubParticipantsService
                .Setup(x => x.GetAdministrationByIdAsync(id))
                .ReturnsAsync(GetAdmins());
            ClubController controller = CreateClubController;
            // Act
            var result = await controller.GetAdministrations(id);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetProfile_Invalid_Test()
        {
            // Arrange
            _clubService
                .Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetProfile(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetClubMembersInfo_Valid_Test(int id)
        {

            // Arrange
            _mediator
                 .Setup(m => m.Send(It.IsAny<GetClubDataForReportQuery>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ClubReportDataDto());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetClubMembersInfo(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetClubMembersInfo_Invalid_Test(int id)
        {
            // Arrange
            _clubService.
                Setup(c => c.GetClubMembersInfoAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetClubMembersInfo(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetClubMembersInfo_Invalid_Test()
        {
            // Arrange
            _clubService
                .Setup(c => c.GetClubMembersInfoAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetClubMembersInfo(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetMembers_Valid_Test(int id)
        {

            _clubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);

            // Arrange
            _clubService
                .Setup(cs => cs.GetClubMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDto());
            _mapper
                .Setup(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()))
                .Returns(new ClubViewModel());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.GetMembers(id);

            // Assert
            _mapper.Verify(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()));
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
        }

        [TestCase(2)]
        public async Task GetMembers_Invalid_Test(int id)
        {
            // Arrange
            _clubService
                .Setup(cs => cs.GetClubMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()))
                .Returns(new ClubViewModel());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.GetMembers(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetFollowers_Valid_Test(int id)
        {
            // Arrange
            _clubService
                .Setup(c => c.GetClubFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDto());
            _mapper
                .Setup(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()))
                .Returns(new ClubViewModel());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetFollowers(id);

            _clubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);
            // Assert
            _mapper.Verify(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetFollowers_Invalid_Test(int id)
        {
            // Arrange
            _clubService
                .Setup(c => c.GetClubFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()))
                .Returns(new ClubViewModel());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetFollowers(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetAdmins_Valid_Test(int id)
        {
            // Arrange
            _clubService
                .Setup(c => c.GetClubAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDto());
            _mapper
                .Setup(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()))
                .Returns(new ClubViewModel());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetAdmins(id);

            // Assert
            _mapper.Verify(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetAdmins_Invalid_Test(int id)
        {
            // Arrange
            _clubService
                .Setup(c => c.GetClubAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()))
                .Returns(new ClubViewModel());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetAdmins(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetDocuments_Valid_Test(int id)
        {
            // Arrange
            _clubService
                .Setup(c => c.GetClubDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDto());
            _mapper
                .Setup(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()))
                .Returns(new ClubViewModel());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetDocuments(id);

            // Assert
            _mapper.Verify(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetDocumentsInvalidCheck()
        {
            // Arrange
            _clubService.Setup(c => c.GetClubDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper.Setup(m => m.Map<ClubProfileDto, ClubViewModel>(It.IsAny<ClubProfileDto>()))
                .Returns(new ClubViewModel());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetDocuments(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_Valid_Test()
        {
            // Arrange
            _clubService
                .Setup(c => c.GetClubProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDto());
            _mapper
                .Setup(m => m.Map<ClubDto, ClubViewModel>(It.IsAny<ClubDto>()))
                .Returns(new ClubViewModel());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.Details(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Details_Invalid_Test()
        {
            // Arrange
            _mediator.Setup(m => m
            .Send(It.IsAny<GetByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<ClubDto, ClubViewModel>(It.IsAny<ClubDto>()))
                .Returns(new ClubViewModel());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.Details(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase("logoName")]
        public async Task GetPhotoBase64_Valid_Test(string logoName)
        {
            // Arrange
            _clubService
                .Setup(c => c.GetLogoBase64(It.IsAny<string>()))
                .ReturnsAsync(new string("some string"));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetPhotoBase64(logoName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_Test()
        {
            // Arrange
            ClubViewModel TestVM = new ClubViewModel();
            _clubService
                .Setup(c => c.CreateAsync(It.IsAny<ClubDto>()))
                .ReturnsAsync(new int());
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDto>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDto());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.Create(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_InvalidModelState_Valid_Test()
        {
            // Arrange
            ClubViewModel TestVM = new ClubViewModel();
            _clubService
                .Setup(c => c.CreateAsync(It.IsAny<ClubDto>()))
                .ReturnsAsync(new int());
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDto>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDto());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;
            controller.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await controller.Create(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_Invalid_Test()
        {
            // Arrange
            ClubViewModel TestVM = new ClubViewModel();
            _clubService
                .Setup(c => c.CreateAsync(It.IsAny<ClubDto>())).ThrowsAsync(new InvalidOperationException());
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDto>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDto());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.Create(TestVM);

            // Assert
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task Edit_Valid_Test()
        {
            // Arrange
            ClubViewModel TestVM = new ClubViewModel();
            _clubService
                .Setup(c => c.EditAsync(It.IsAny<ClubDto>()));
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDto>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDto());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.Edit(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Edit_InvalidModelState_Valid_Test()
        {
            // Arrange
            ClubViewModel TestVM = new ClubViewModel();
            _clubService
                .Setup(c => c.EditAsync(It.IsAny<ClubDto>()));
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDto>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDto());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;
            controller.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await controller.Edit(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Remove_Valid_Test()
        {
            // Arrange
            _clubService
                .Setup(c => c.RemoveAsync(It.IsAny<int>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.Remove(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task AddFollower_Valid_Test()
        {
            // Arrange
            ClubController controller = CreateClubController;

            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { "Admin" });

            _clubParticipantsService.Setup(c => c.AddFollowerInHistoryAsync(It.IsAny<int>(), It.IsAny<string>()));
            _clubParticipantsService.Setup(c => c.AddFollowerAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(new ClubMembersDto());

            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));

            // Act
            var result = await controller.AddFollower(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RemoveFollower_Valid_Test()
        {
            // Arrange
            _clubParticipantsService
                .Setup(c => c.RemoveFollowerAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.RemoveFollower(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task ChangeApproveStatusFalse_Valid_Test()
        {
            // Arrange
            _clubParticipantsService
                .Setup(c => c.ToggleApproveStatusAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubMembersDto() { ClubId = "1", User = new ClubUserDto() { ID = "1" } });
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;
            _clubParticipantsService.Setup(c => c.UpdateStatusFollowerInHistoryAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()));
            _clubParticipantsService.Setup(c => c.AddFollowerInHistoryAsync(It.IsAny<Int32>(), It.IsAny<string>()));
            // Act
            var result = await controller.ChangeApproveStatus(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ChangeApproveStatusTrue_Valid_Test()
        {
            // Arrange
            _clubParticipantsService
                .Setup(c => c.ToggleApproveStatusAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubMembersDto() { ClubId = "1", User = new ClubUserDto() { ID = "1" }, IsApproved = true });
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;
            _clubParticipantsService.Setup(c => c.UpdateStatusFollowerInHistoryAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()));
            _clubParticipantsService.Setup(c => c.AddFollowerInHistoryAsync(It.IsAny<Int32>(), It.IsAny<string>()));
            // Act
            var result = await controller.ChangeApproveStatus(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }




        [Test]
        public async Task GetClubNameOfApprovedMemberTest()
        {
            //Arrange
            _clubParticipantsService
                .Setup(c => c.ClubOfApprovedMember(It.IsAny<string>()));
            ClubController controller = CreateClubController;

            //Act
            var result = await controller.ClubNameOfApprovedMember(GetStringFakeId());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddAdmin_Valid_Test()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<ClubAdministrationViewModel, ClubAdministrationDto>(It.IsAny<ClubAdministrationViewModel>()))
                .Returns(new ClubAdministrationDto() { AdminType = new BLL.DTO.Admin.AdminTypeDto() });
            _clubParticipantsService
                .Setup(c => c.AddAdministratorAsync(It.IsAny<ClubAdministrationDto>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.AddAdmin(new ClubAdministrationViewModel());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RemoveAdmin_Valid_Test()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<ClubAdministrationViewModel, ClubAdministrationDto>(It.IsAny<ClubAdministrationViewModel>()))
                .Returns(new ClubAdministrationDto());
            _clubParticipantsService
                .Setup(c => c.RemoveAdministratorAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.RemoveAdmin(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task EditAdmin_Valid_Test()
        {
            // Arrange
            ClubAdministrationViewModel admin = new ClubAdministrationViewModel();
            _mapper
                .Setup(m => m.Map<ClubAdministrationViewModel, ClubAdministrationDto>(It.IsAny<ClubAdministrationViewModel>()))
                .Returns(new ClubAdministrationDto());
            _clubParticipantsService
                .Setup(c => c.EditAdministratorAsync(It.IsAny<ClubAdministrationDto>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.EditAdmin(admin);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task EditAdmin_DateIsEarlierThanToday_ReturnsBadRequest()
        {
            // Arrange
            ClubAdministrationViewModel admin = new ClubAdministrationViewModel();
            admin.EndDate = DateTime.MinValue;
            _mapper
                .Setup(m => m.Map<ClubAdministrationViewModel, ClubAdministrationDto>(It.IsAny<ClubAdministrationViewModel>()))
                .Returns(new ClubAdministrationDto());
            _clubParticipantsService
                .Setup(c => c.EditAdministratorAsync(It.IsAny<ClubAdministrationDto>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.EditAdmin(admin);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task ArchiveClub_valid_Test()
        {
            //Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<ArchiveCommand>(), It.IsAny<CancellationToken>()));
            ClubController Clubcon = CreateClubController;

            //Act
            var result = await Clubcon.Archive(GetFakeID());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task UnArchiveClub_valid_Test()
        {
            //Arrange
            _mediator
               .Setup(m => m.Send(It.IsAny<UnArchiveCommand>(), It.IsAny<CancellationToken>()));
            ClubController Clubcon = CreateClubController;

            //Act
            var result = await Clubcon.UnArchive(GetFakeID());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        public async Task EditAdmin_OldEndDate_ReturnsBadRequest()
        {
            // Arrange
            var testAdmin = new ClubAdministrationViewModel() { EndDate = DateTime.MinValue };
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.EditAdmin(testAdmin);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task AddDocument_Valid_Test()
        {
            // Arrange
            ClubDocumentsViewModel document = new ClubDocumentsViewModel();
            _mapper
                .Setup(m => m.Map<ClubDocumentsViewModel, ClubDocumentsDto>(It.IsAny<ClubDocumentsViewModel>()))
                .Returns(new ClubDocumentsDto());
            _clubDocumentsService
                .Setup(c => c.AddDocumentAsync(It.IsAny<ClubDocumentsDto>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.AddDocument(document);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetFileBase64_Valid_Test()
        {
            // Arrange
            _clubDocumentsService
                .Setup(c => c.DownloadFileAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<string>());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetFileBase64(GetFakeFileName());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RemoveDocument_Valid_Test()
        {
            // Arrange
            _clubDocumentsService
                .Setup(c => c.DeleteFileAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.RemoveDocument(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }


        [Test]
        public async Task GetDocumentTypesAsync_Valid_Test()
        {
            // Arrange
            _clubDocumentsService
                .Setup(c => c.GetAllClubDocumentTypesAsync())
                .ReturnsAsync(It.IsAny<IEnumerable<ClubDocumentTypeDto>>());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetDocumentTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetClubThatUserHasAccessTo_Valid_Test()
        {
            // Arrange
            _clubAccessService
                .Setup(c => c.GetClubsAsync(It.IsAny<User>()));
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetClubsThatUserHasAccessTo();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserAdministrations_Valid_Test()
        {
            // Arrange
            _clubParticipantsService
                .Setup(c => c.GetAdministrationsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<ClubAdministrationDto>>());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetUserAdministrations(GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserPreviousAdministrations_Valid_Test()
        {
            // Arrange
            _clubParticipantsService
                .Setup(c => c.GetPreviousAdministrationsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<ClubAdministrationDto>>());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetUserPreviousAdministrations(GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllAdministrationStatuses_Valid_Test()
        {
            // Arrange
            _clubParticipantsService
                .Setup(c => c.GetAdministrationStatuses(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<ClubAdministrationStatusDto>>());
            ClubController controller = CreateClubController;

            // Act
            var result = await controller.GetAllAdministrationStatuses(GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetClubsOptions()
        {
            //Arrange
            _userManager.Setup(r => r.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());
            _clubAccessService.Setup(r => r.GetAllClubsIdAndName(It.IsAny<User>()))
                .ReturnsAsync(new List<ClubForAdministrationDto>());
            ClubController controller = CreateClubController;

            //Act
            var result = await controller.GetClubsOptionsThatUserHasAccessTo();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull((result as ObjectResult).Value);
        }

        private int GetFakeID()
        {
            return 1;
        }
        private string GetStringFakeId()
        {
            return "1";
        }
        private string GetFakeFileName()
        {
            return "FileName";
        }
        private int GetFakeClubNumber()
        {
            return 100;
        }

        private List<ClubDto> GetClubsBySearch()
        {
            return new List<ClubDto>()
            {
                new ClubDto()
                {
                    Name = "Курінь",
                }
            };
        }

        private List<ClubObjectDto> GetClubsByPage()
        {
            return new List<ClubObjectDto>()
            {
                new ClubObjectDto()
                {
                    Name = "Курінь",
                }
            };
        }

        private Tuple<IEnumerable<ClubObjectDto>, int> CreateTuple => new Tuple<IEnumerable<ClubObjectDto>, int>(GetClubsByPage(), GetFakeClubNumber());

        private IEnumerable<ClubForAdministrationDto> GetFakeClubsForAdministration()
        {
            return new List<ClubForAdministrationDto>()
            {
                new ClubForAdministrationDto
                {
                    Name = "Курінь"
                }
            }.AsEnumerable();
        }

        private IEnumerable<ClubAdministrationDto> GetAdmins()
        {
            return new List<ClubAdministrationDto>()
            {
                new ClubAdministrationDto() { ID = 2 },
                new ClubAdministrationDto() { ID = 3 },
                new ClubAdministrationDto() { ID = 4 },
                new ClubAdministrationDto() { ID = 5 }
            };
        }
    }
}
