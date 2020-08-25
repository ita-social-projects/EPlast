using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class ClubControllerTests
    {
        ClubController _clubController;
        Mock<IClubService> _clubService;
        Mock<IClubAdministrationService> _clubAdministrationService;
        Mock<IClubMembersService> _clubMembersService;
        Mock<IUserManagerService> _userManagerService;
        Mock<IMapper> _mapper;

        [SetUp]
        public void SetUp()
        {
            _clubService = new Mock<IClubService>();
            _clubAdministrationService = new Mock<IClubAdministrationService>();
            _clubMembersService = new Mock<IClubMembersService>();
            _userManagerService = new Mock<IUserManagerService>();
            _mapper = new Mock<IMapper>();

            _clubController = new ClubController(
                _clubService.Object,
                _clubAdministrationService.Object,
                _clubMembersService.Object,
                _userManagerService.Object,
                _mapper.Object
                );
        }

        [Test]
        public async Task Get_ReturnsOkObjectResult()
        {
            //Arrange
            var clubs = new List<ClubDTO>() {
                    new ClubDTO()
                    {
                        Logo = "logo"
                    }
            };
            _clubService
                .Setup(x => x.GetAllClubsAsync())
                .ReturnsAsync(clubs);
            _clubService
                .Setup(x => x.GetImageBase64Async(clubs[0].Logo))
                .ReturnsAsync(clubs[0].Logo);

            //Act
            var result = await _clubController.Get();
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _clubService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<ClubDTO>>(resultValue);
        }

        [Test]
        public async Task GetImage_ReturnsImageName()
        {
            //Arrange
            var imageName = "string";
            _clubService
                .Setup(x => x.GetImageBase64Async(imageName))
                .ReturnsAsync(imageName);

            //Act
            var result = await _clubController.GetImage(imageName);

            //Assert
            _clubService.Verify();
            Assert.IsNotNull(result);
            Assert.AreEqual(imageName, result);
        }

        [Test]
        public async Task Club_ReturnsOkObjectResult()
        {
            //Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _clubController.ControllerContext = context;
            var clubID = GetClubId();
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubProfileViewModel() { Club = new ClubViewModel() });
            _clubService
                .Setup(x => x.GetClubProfileAsync(clubID))
                .ReturnsAsync(new ClubProfileDTO());
            _clubService
                .Setup(x => x.GetImageBase64Async(It.IsAny<string>()))
                .ReturnsAsync("string");

            //Act
            var result = await _clubController.Club(clubID);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _clubService.Verify();
            _mapper.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<ClubProfileViewModel>(resultValue);
        }

        [Test]
        public async Task GetCLubMembers_ReturnsOkObjectResult()
        {
            //Arrange
            var clubId = GetClubId();
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _clubController.ControllerContext = context;
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubProfileViewModel() { Club = new ClubViewModel() });
            _clubService
                .Setup(x => x.GetClubMembersOrFollowersAsync(clubId, true))
                .ReturnsAsync(new ClubProfileDTO());

            //Act
            var result = await _clubController.GetClubMembers(clubId);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mapper.Verify();
            _clubService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<ClubProfileViewModel>(resultValue);
        }

        [Test]
        public async Task GetClubFollowers_ReturnsOkObjectResult()
        {
            //Arrange
            var clubId = GetClubId();
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _clubController.ControllerContext = context;
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubProfileViewModel() { Club = new ClubViewModel() });
            _clubService
                .Setup(x => x.GetClubMembersOrFollowersAsync(clubId, true))
                .ReturnsAsync(new ClubProfileDTO());

            //Act
            var result = await _clubController.GetClubFollowers(clubId);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mapper.Verify();
            _clubService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<ClubProfileViewModel>(resultValue);
        }

        [Test]
        public async Task ClubDesctription_ReturnsOkObjectResult()
        {
            //Arrange
            _clubService
                .Setup(x => x.GetClubInfoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubDTO());

            //Act
            var result = await _clubController.ClubDescription(It.IsAny<int>());
            var resultValue = (result as OkObjectResult).Value as ClubDTO;

            //Assert
            _clubService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<ClubDTO>(resultValue);
        }

        [Test]
        public async Task Edit_ReturnsOkObjectResult()
        {
            //Arrange
            var isValid = true;
            var isClubNameNotChanged = true;

            var expectedValue = "Updated";
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());
            _clubService
                .Setup(x => x.UpdateAsync(It.IsAny<ClubDTO>()))
                .ReturnsAsync(It.IsAny<ClubDTO>);
            _clubService
                .Setup(x => x.Validate(It.IsAny<ClubDTO>()))
                .ReturnsAsync(isValid);
            _clubService
                .Setup(x => x.VerifyClubNameIsNotChanged(It.IsAny<ClubDTO>()))
                .ReturnsAsync(isClubNameNotChanged);

            //Act
            var result = await _clubController.Edit(It.IsAny<ClubViewModel>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mapper.Verify();
            _clubService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(expectedValue, resultValue);
        }

        [Test]
        public async Task Edit_ValidationFailed_ReturnsStatus422UnprocessableEntity()
        {
            //Arrange
            var isValid = false;

            var isClubNameNotChanged = false;

            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());
            _clubService
                .Setup(x => x.Validate(It.IsAny<ClubDTO>()))
                .ReturnsAsync(isValid);
            _clubService
                .Setup(x => x.VerifyClubNameIsNotChanged(It.IsAny<ClubDTO>()))
                .ReturnsAsync(isClubNameNotChanged);

            var expected = StatusCodes.Status422UnprocessableEntity;

            //Act
            var result = await _clubController.Edit(It.IsAny<ClubViewModel>());
            var actual = (result as StatusCodeResult).StatusCode;

            //Assert
            _mapper.Verify();
            _clubService.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Create_ReturnsOkObjectResult()
        {
            //Arrange
            var isValid = true;

            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());
            _clubService
                .Setup(x => x.CreateAsync(It.IsAny<ClubDTO>()))
                .ReturnsAsync(new ClubDTO());
            _clubService
                .Setup(x => x.Validate(It.IsAny<ClubDTO>()))
                .ReturnsAsync(isValid);

            //Act
            var result = await _clubController.Create(It.IsAny<ClubViewModel>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mapper.Verify();
            _clubService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<ClubDTO>(resultValue);
        }

        [Test]
        public async Task Create_ValidationFailed_ReturnsStatus422UnprocessableEntity()
        {
            //Arrange
            var isValid = false;

            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());
            _clubService
                .Setup(x => x.Validate(It.IsAny<ClubDTO>()))
                .ReturnsAsync(isValid);

            var expected = StatusCodes.Status422UnprocessableEntity;

            //Act
            var result = await _clubController.Create(It.IsAny<ClubViewModel>());
            var actual = (result as StatusCodeResult).StatusCode;

            //Assert
            _mapper.Verify();
            _clubService.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetClubAdministrator_ReturnsOkObjectResult()
        {
            //Arrange
            var clubId = GetClubId();
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _clubController.ControllerContext = context;
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubProfileViewModel() { Club = new ClubViewModel() });
            _clubAdministrationService
                .Setup(x => x.GetClubAdministrationByIdAsync(clubId))
                .ReturnsAsync(new ClubProfileDTO());

            //Act
            var result = await _clubController.GetClubAdministration(clubId);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mapper.Verify();
            _clubAdministrationService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<ClubProfileViewModel>(resultValue);
        }

        [Test]
        public async Task DeleteAdministration_ReturnsOkObjectResult()
        {
            //Arrange
            var adminId = GetClubId();
            var expectedValue = $"Club Administrator with id={adminId} deleted.";
            _clubAdministrationService
                .Setup(x => x.DeleteClubAdminAsync(adminId))
                .ReturnsAsync(true);

            //Act
            var result = await _clubController.DeleteAdministration(adminId);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _clubAdministrationService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(expectedValue, resultValue);
        }

        [Test]
        public async Task ChangeApproveStatus_ReturnsOkObjectResult()
        {
            //Arrange
            var memberId = GetClubId();
            var clubId = GetClubId();
            _mapper
                .Setup(m => m.Map<ClubMembersDTO, ClubMembersViewModel>(It.IsAny<ClubMembersDTO>()))
                .Returns(new ClubMembersViewModel());
            _clubMembersService
                .Setup(x => x.ToggleIsApprovedInClubMembersAsync(memberId, clubId))
                .ReturnsAsync(new ClubMembersDTO());

            //Act
            var result = await _clubController.ChangeApproveStatus(clubId, memberId);
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mapper.Verify();
            _clubMembersService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<ClubMembersViewModel>(resultValue);
        }

        [Test]
        public async Task SetClubAdministratorEndDate_ReturnsOkObjectResult()
        {
            //Arrange
            _clubAdministrationService
                .Setup(x => x.SetAdminEndDateAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new ClubAdministrationDTO());

            //Act
            var result = await _clubController.SetClubAdministratorEndDate(It.IsAny<int>(), It.IsAny<DateTime>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _clubAdministrationService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<ClubAdministrationDTO>(resultValue);
        }

        [Test]
        public async Task AddAdmin_ReturnsOkObjectResult()
        {
            //Arrange
            var clubId = GetClubId();
            _clubService
                .Setup(x => x.GetClubInfoByIdAsync(clubId))
                .ReturnsAsync(new ClubDTO());
            _mapper
                .Setup(m => m.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(
                    It.IsAny<ClubAdministrationViewModel>()))
                .Returns(new ClubAdministrationDTO() { ClubId = clubId });
            _mapper
               .Setup(m => m.Map<ClubAdministrationDTO, ClubAdministrationViewModel>(
                   It.IsAny<ClubAdministrationDTO>()))
               .Returns(new ClubAdministrationViewModel());
            _clubAdministrationService
                .Setup(x => x.AddClubAdminAsync(It.IsAny<ClubAdministrationDTO>()))
                .ReturnsAsync(new ClubAdministrationDTO());

            //Act
            var result = await _clubController.AddAdmin(clubId, It.IsAny<ClubAdministrationViewModel>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _clubService.Verify();
            _mapper.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<ClubAdministrationViewModel>(resultValue);

        }

        [Test]
        public async Task AddFollower_ReturnsOkObjectResult()
        {
            //Arrange
            var clubId = GetClubId();
            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(m => m.User.IsInRole("Admin"))
                .Returns(true);
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            _clubController.ControllerContext = context;
            _userManagerService
                .Setup(x => x.GetUserIdAsync(httpContext.Object.User))
                .ReturnsAsync(It.IsAny<string>());
            _mapper
                .Setup(m => m.Map<ClubMembersDTO, ClubMembersViewModel>(It.IsAny<ClubMembersDTO>()))
                .Returns(new ClubMembersViewModel());
            _clubMembersService
                .Setup(x => x.AddFollowerAsync(clubId, It.IsAny<string>()))
                .ReturnsAsync(new ClubMembersDTO());

            //Act
            var result = await _clubController.AddFollower(clubId, It.IsAny<string>());
            var resultValue = (result as OkObjectResult).Value;

            //Assert
            _mapper.Verify();
            _userManagerService.Verify();
            _clubMembersService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<ClubMembersViewModel>(resultValue);
        }

        //Fakes
        private int GetClubId()
        {
            int clubId = 2;
            return clubId;
        }
    }
}
