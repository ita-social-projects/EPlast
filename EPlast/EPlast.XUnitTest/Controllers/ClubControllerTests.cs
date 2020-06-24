using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using EPlast.BusinessLogicLayer.DTO.Admin;
using EPlast.BusinessLogicLayer.DTO.Club;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using EPlast.BusinessLogicLayer.Interfaces.Club;
using EPlast.BusinessLogicLayer.Interfaces.Logging;
using EPlast.BusinessLogicLayer.Services.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using EPlast.ViewModels.Club;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Controllers
{
    public class ClubControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IClubService> _clubService;
        private readonly Mock<IClubMembersService> _clubMembersService;
        private readonly Mock<IClubAdministrationService> _clubAdministrationService;
        private readonly Mock<IUserManagerService> _userManagerService;
        private readonly ClubController controller;

        public ClubControllerTests()
        {
            _mapper = new Mock<IMapper>();
            var loggerService = new Mock<ILoggerService<ClubController>>();
            _clubService = new Mock<IClubService>();
            _clubMembersService = new Mock<IClubMembersService>();
            _clubAdministrationService = new Mock<IClubAdministrationService>();

            var user = new UserDTO();
            _userManagerService = new Mock<IUserManagerService>();
            _userManagerService.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerService.Setup(x => x.GetRolesAsync(It.IsAny<UserDTO>())).ReturnsAsync(new List<string>());
            _userManagerService.Setup(x => x.GetUserIdAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(It.IsAny<string>());

            controller = new ClubController(_clubService.Object,
                _clubAdministrationService.Object,
                _clubMembersService.Object,
                _mapper.Object,
                loggerService.Object,
                _userManagerService.Object);

            var context = new Mock<HttpContext>();
            var identity = new GenericIdentity("username");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
            var principal = new GenericPrincipal(identity, new[] {"user"});
            context.Setup(s => s.User).Returns(principal);
            controller.ControllerContext.HttpContext = context.Object;
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfClubs()
        {
            //arrange
            _clubService.Setup(c => c.GetAllClubsAsync())
                .ReturnsAsync(new List<ClubDTO>());
            _mapper.Setup(
                    m => m.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(It.IsAny<IEnumerable<ClubDTO>>()))
                .Returns(() => new List<ClubViewModel>());

            //action
            var result = await controller.Index();

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<ClubViewModel>>(viewResult.Model);
        }

        [Fact]
        public async Task Club_FindByID_ReturnsAView()
        {
            _clubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(() => new ClubProfileDTO {Club = new ClubDTO()});
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubProfileViewModel());

            //action
            var result = await controller.Club(It.IsAny<int>());

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ClubProfileViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task Club_InvalidID_ReturnsHandleError()
        {
            //arrange
            _clubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //action
            var result = await controller.Club(It.IsAny<int>());

            //assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ClubAdminsReturnsView()
        {
            //Arrange
            _mapper
                .Setup(s => s.Map<ClubProfileDTO>(It.IsAny<ClubProfileViewModel>()))
                .Returns(GetTestClubProfileDTO());

            //Act
            var result = await controller.ClubAdmins(1);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ClubAdminsReturnsHandleError()
        {
            //Arrange
            _mapper
                .Setup(s => s.Map<ClubProfileDTO>(It.IsAny<ClubProfileViewModel>()))
                .Returns((ClubProfileDTO) null);

            //Act
            var result = await controller.ClubAdmins(1);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ClubMembers_Correct_ReturnsAView()
        {
            //Arrange
            _clubService.Setup(c => c.GetClubMembersOrFollowersAsync(It.IsAny<int>(), true))
                .ReturnsAsync(() => new ClubProfileDTO());
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubProfileViewModel());

            //Act
            var result = await controller.ClubMembers(It.IsAny<int>());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ClubProfileViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task ClubMembers_InvalidID_ReturnsHandleError()
        {
            //Arrange
            _clubService.Setup(c => c.GetClubMembersOrFollowersAsync(It.IsAny<int>(), true))
                .ReturnsAsync(() => null);
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(null))
                .Returns((ClubProfileViewModel) null);

            //Act
            var result = await controller.ClubMembers(It.IsAny<int>());

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ClubFollowers_Correct_ReturnsAView()
        {
            //Arrange
            _clubService.Setup(c => c.GetClubMembersOrFollowersAsync(It.IsAny<int>(), false))
                .ReturnsAsync(() => new ClubProfileDTO());
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubProfileViewModel());

            //Act
            var result = await controller.ClubFollowers(It.IsAny<int>());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ClubProfileViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task ClubFollowers_InvalidID_ReturnsHandleError()
        {
            //Arrange
            _clubService.Setup(c => c.GetClubMembersOrFollowersAsync(It.IsAny<int>(), false))
                .ReturnsAsync(() => null);
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(null))
                .Returns((ClubProfileViewModel) null);

            //Act
            var result = await controller.ClubFollowers(It.IsAny<int>());

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ClubDescription_Correct_ReturnsAView()
        {
            //Arrange
            _clubService.Setup(c => c.GetClubInfoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => new ClubDTO());
            _mapper.Setup(c => c.Map<ClubDTO, ClubViewModel>(It.IsAny<ClubDTO>()))
                .Returns(new ClubViewModel());

            //Act
            var result = await controller.ClubDescription(It.IsAny<int>());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ClubViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task ClubDescription_InvalidID_ReturnsHandleError()
        {
            //Arrange
            _clubService.Setup(c => c.GetClubInfoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper.Setup(c => c.Map<ClubDTO, ClubViewModel>(null))
                .Returns((ClubViewModel) null);

            //Act
            var result = await controller.ClubDescription(It.IsAny<int>());

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task EditGet_Correct_ReturnsAView()
        {
            //Arrange
            _clubService.Setup(c => c.GetClubInfoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => new ClubDTO());
            _mapper.Setup(c => c.Map<ClubDTO, ClubViewModel>(It.IsAny<ClubDTO>()))
                .Returns(new ClubViewModel());

            //Act
            var result = await controller.EditClub(It.IsAny<int>());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ClubViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task EditGet_InvalidID_ReturnsHandleError()
        {
            //Arrange
            _clubService.Setup(c => c.GetClubInfoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper.Setup(c => c.Map<ClubDTO, ClubViewModel>(null))
                .Returns((ClubViewModel) null);

            //Act
            var result = await controller.EditClub(It.IsAny<int>());

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task EditPost_Correct_ReturnsAView()
        {
            //Arrange
            _clubService.Setup(c => c.UpdateAsync(It.IsAny<ClubDTO>(), It.IsAny<IFormFile>()))
                .Verifiable();
            _mapper.Setup(c => c.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());

            //Act
            var result = await controller.EditClub(new ClubViewModel(), It.IsAny<IFormFile>());

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Club", viewResult.ActionName);
        }

        [Fact]
        public async Task EditPost_InvalidModel_ReturnsHandleError()
        {
            //Arrange
            _clubService.Setup(c => c.UpdateAsync(It.IsAny<ClubDTO>(), It.IsAny<IFormFile>()))
                .Verifiable();
            _mapper.Setup(c => c.Map<ClubViewModel, ClubDTO>(null))
                .Returns((ClubDTO) null);

            //Act
            var result = await controller.EditClub(It.IsAny<ClubViewModel>(), It.IsAny<IFormFile>());

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ChangeIsApprovedStatusClubReturnClub()
        {
            //Act
            var result = await controller.ChangeIsApprovedStatusClub(1, 1);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("Club", viewResult.ActionName);
        }

        [Fact]
        public async Task ChangeIsApprovedStatusClubReturnHandleError()
        {
            //Arrange
            _clubMembersService
                .Setup(s => s.ToggleIsApprovedInClubMembersAsync(1, 1))
                .Returns((Task) null);

            //Act
            var result = await controller.ChangeIsApprovedStatusClub(1, 1);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteFromAdminsReturnsClubAdmins()
        {
            //Arrange
            _clubAdministrationService
                .Setup(s => s.DeleteClubAdminAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            //Act
            var result = await controller.DeleteFromAdmins(1, 1);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("ClubAdmins", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteFromAdminsReturnsHandleError()
        {
            //Arrange
            _clubAdministrationService
                .Setup(s => s.DeleteClubAdminAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            //Act
            var result = await controller.DeleteFromAdmins(1, 1);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
        }

        [Fact]
        public async Task AddEndDateReturns1()
        {
            //Act
            var result = await controller.AddEndDate(GetTestAdminEndDateDTO());

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task AddEndDateReturns0()
        {
            //Arrange
            _clubAdministrationService
                .Setup(s => s.SetAdminEndDateAsync(It.IsAny<AdminEndDateDTO>()))
                .Returns((Task) null);

            //Act
            var result = await controller.AddEndDate(GetTestAdminEndDateDTO());

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task AddToClubAdministrationReturnJsonTrue()
        {
            //Act
            var result = await controller.AddToClubAdministration(GetTestClubAdministrationDTO());

            //Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Contains("True", jsonResult.Value.ToString());
        }

        [Fact]
        public async Task AddToClubAdministrationReturnJsonFalse()
        {
            //Arrange
            _clubAdministrationService
                .Setup(s => s.AddClubAdminAsync(It.IsAny<ClubAdministrationDTO>()))
                .Returns((Task) null);

            //Act
            var result = await controller.AddToClubAdministration(GetTestClubAdministrationDTO());

            //Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Contains("False", jsonResult.Value.ToString());
        }

        [Fact]
        public async Task ChooseAClub_Correct_ReturnsAView()
        {
            //Arrange
            _clubService.Setup(c => c.GetAllClubsAsync())
                .ReturnsAsync(() => new List<ClubDTO>());
            _mapper.Setup(
                    c => c.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(It.IsAny<IEnumerable<ClubDTO>>()))
                .Returns(new List<ClubViewModel>());

            //Act
            var result = await controller.ChooseAClub(It.IsAny<string>());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ClubChooseAClubViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task AddAsClubFollowerReturnsUserProfile()
        {
            //Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() {User = user}
            };

            _userManagerService
                .Setup(s => s.GetUserIdAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync("aaaa-bbbb-cccc");

            //Act
            var result = await controller.AddAsClubFollower(1, "aaaa-bbbb-cccc");

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("UserProfile", viewResult.ActionName);
        }

        [Fact]
        public void CreateGet_Correct_ReturnsAView()
        {
            //Act
            var result = controller.CreateClub();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ClubViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task CreatePost_Correct_ReturnsAView()
        {
            //Arrange
            _clubService.Setup(c => c.CreateAsync(It.IsAny<ClubDTO>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(new ClubDTO());
            _mapper.Setup(c => c.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());

            //Act
            var result = await controller.CreateClub(new ClubViewModel(), It.IsAny<IFormFile>());

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Club", viewResult.ActionName);
        }

        [Fact]
        public async Task CreatePost_InvalidModel_ReturnsHandleError()
        {
            //Arrange
            _clubService.Setup(c => c.CreateAsync(It.IsAny<ClubDTO>(), It.IsAny<IFormFile>()))
                .ReturnsAsync((ClubDTO) null);
            _mapper.Setup(c => c.Map<ClubViewModel, ClubDTO>(null))
                .Returns((ClubDTO) null);

            //Act
            var result = await controller.EditClub(It.IsAny<ClubViewModel>(), It.IsAny<IFormFile>());

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        private static ClubProfileDTO GetTestClubProfileDTO()
        {
            return new ClubProfileDTO
            {
                Club = new ClubDTO()
                {
                    ClubName = "Club",
                    Description = "New club",
                    ID = 1
                }
            };
        }

        private static AdminEndDateDTO GetTestAdminEndDateDTO()
        {
            return new AdminEndDateDTO
            {
                AdminId = 1,
                ClubIndex = 1,
                EndDate = new DateTime(2030, 10, 5)
            };
        }

        private static ClubAdministrationDTO GetTestClubAdministrationDTO()
        {
            return new ClubAdministrationDTO
            {
                AdminType = new AdminType()
                {
                    AdminTypeName = "Admin",
                },
                AdminTypeName = "Admin",
                AdminTypeId = 1,
                ClubId = 1,
                ID = 1,
                StartDate = new DateTime(2020, 5, 10),
                EndDate = new DateTime(2030, 10, 5)
            };
        }
    }
}