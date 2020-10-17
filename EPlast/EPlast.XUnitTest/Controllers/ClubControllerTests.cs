using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.Controllers;
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
        private readonly Mock<IClubService> _ClubService;
        private readonly Mock<IClubMembersService> _ClubMembersService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService<ClubController>> _logger;

        private ClubController CreateClubController => new ClubController(_logger.Object,
            _ClubService.Object,
            _ClubMembersService.Object,
            _mapper.Object);

        public ClubControllerTests()
        {
            _ClubService = new Mock<IClubService>();
            _ClubMembersService = new Mock<IClubMembersService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILoggerService<ClubController>>();
        }

        [Fact]
        public async Task IndexTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetAllDTOAsync(null))
                .ReturnsAsync(new List<ClubDTO>());
            _mapper.Setup(m => m.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(It.IsAny<IEnumerable<ClubDTO>>()))
                .Returns(() => new List<ClubViewModel>());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Index();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ClubProfileTests()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(() => new ClubProfileDTO());
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubProfileViewModel());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubProfile(FakeId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ClubProfileInvalidIdTests()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubProfile(FakeId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task ClubProfileExceptionTests()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubProfile(FakeId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ClubMembersCorrectTest()
        {
            // Arrange
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(() => new ClubProfileViewModel());
            _ClubService.Setup(c => c.GetClubMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubMembers(FakeId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ClubMembersInvalidIdTest()
        {
            // Arrange
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(() => new ClubProfileViewModel());
            _ClubService.Setup(c => c.GetClubMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubMembers(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ClubMembersExceptionTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubMembersAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubMembers(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ClubFollowersCorrectTest()
        {
            // Arrange
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(() => new ClubProfileViewModel());
            _ClubService.Setup(c => c.GetClubFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubFollowers(FakeId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ClubFollowersInvalidIdTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubFollowers(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ClubFollowersExceptionTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubFollowersAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubFollowers(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ClubAdminsCorrectTest()
        {
            // Arrange
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(() => new ClubProfileViewModel());
            _ClubService.Setup(c => c.GetClubAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubAdmins(FakeId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ClubAdminsInvalidIdTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubAdmins(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ClubAdminsExceptionTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubAdminsAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubAdmins(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task EditGetCorrectTest()
        {
            // Arrange
            _mapper.Setup(c => c.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(() => new ClubProfileViewModel());
            _ClubService.Setup(c => c.EditAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Edit(FakeId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task EditGetInvalidIdTest()
        {
            // Arrange
            _ClubService.Setup(c => c.EditAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Edit(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task EditGetExceptionTest()
        {
            // Arrange
            _ClubService.Setup(c => c.EditAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Edit(1);

            // Asser
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task EditWithValidModelStateTest()
        {
            // Arrange
            ClubProfileViewModel model = CreateFakeClubProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<ClubProfileViewModel, ClubProfileDTO>(It.IsAny<ClubProfileViewModel>()))
                .Returns(new ClubProfileDTO());
            _ClubService.Setup(c => c.EditAsync(It.IsAny<ClubProfileDTO>(), It.IsAny<IFormFile>()))
                .Verifiable();
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Edit(model, file);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ClubProfile", viewResult.ActionName);
            Assert.Equal("Club", viewResult.ControllerName);
        }

        [Fact]
        public async Task EditWithInvalidModelStateTest()
        {
            // Arrange
            ClubProfileViewModel model = CreateFakeClubProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<ClubProfileViewModel, ClubProfileDTO>(It.IsAny<ClubProfileViewModel>()))
                .Returns(new ClubProfileDTO());
            _ClubService.Setup(c => c.EditAsync(It.IsAny<ClubProfileDTO>(), It.IsAny<IFormFile>()))
                .Verifiable();
            ClubController ClubController = CreateClubController;
            ClubController.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await ClubController.Edit(model, file);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task EditWithExceptionTest()
        {
            // Arrange
            ClubProfileViewModel model = CreateFakeClubProfileViewModels(1).First();
            model.Club = null;
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _ClubService.Setup(c => c.EditAsync(It.IsAny<ClubProfileDTO>(), It.IsAny<IFormFile>()))
                .ThrowsAsync(new ArgumentException("some message"));
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Edit(model, file);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public void CreateGetCorrectTest()
        {
            // Arrange
            ClubController ClubController = CreateClubController;

            // Act
            var result = ClubController.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ClubDocumentsCorrectTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());
            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubProfileViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubProfileViewModel());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubDocuments(FakeId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ClubDocumentsInvalidIdTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubDocuments(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task ClubDocumentsExceptionTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubDocumentsAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.ClubDocuments(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task DetailsCorrectTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubDTO());
            _mapper.Setup(m => m.Map<ClubDTO, ClubViewModel>(It.IsAny<ClubDTO>()))
                .Returns(new ClubViewModel());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Details(3);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task DetailsInvalidIdTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Details(3);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task DetailsExceptionTest()
        {
            // Arrange
            _ClubService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Details(3);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task CreateWithValidModelStateTest()
        {
            // Arrange
            ClubProfileViewModel model = CreateFakeClubProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<ClubProfileViewModel, ClubProfileDTO>(It.IsAny<ClubProfileViewModel>()))
                .Returns(new ClubProfileDTO());
            _ClubService.Setup(c => c.CreateAsync(It.IsAny<ClubProfileDTO>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(3);
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Create(model, file);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("ClubProfile", viewResult.ActionName);
            Assert.Equal("Club", viewResult.ControllerName);
        }

        [Fact]
        public async Task CreateWithInvalidModelStateTest()
        {
            // Arrange
            ClubProfileViewModel model = CreateFakeClubProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<ClubProfileViewModel, ClubProfileDTO>(It.IsAny<ClubProfileViewModel>()))
                .Returns(new ClubProfileDTO());
            _ClubService.Setup(c => c.CreateAsync(It.IsAny<ClubProfileDTO>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(3);
            ClubController ClubController = CreateClubController;
            ClubController.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await ClubController.Create(model, file);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task CreateWithExceptionTest()
        {
            // Arrange
            ClubProfileViewModel model = CreateFakeClubProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _ClubService.Setup(c => c.CreateAsync(It.IsAny<ClubProfileDTO>(), It.IsAny<IFormFile>()))
                .ThrowsAsync(new ArgumentException("some message"));
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.Create(model, file);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        private int FakeId => 1;
        private IQueryable<ClubProfileViewModel> CreateFakeClubProfileViewModels(int count)
        {
            List<ClubProfileViewModel> ClubProfilesDto = new List<ClubProfileViewModel>();
            for (int i = 0; i < count; i++)
            {
                ClubProfilesDto.Add(new ClubProfileViewModel
                {
                    Club = new ClubViewModel
                    {
                        ID = i,
                        Name = "Name " + i
                    }
                });
            }
            return ClubProfilesDto.AsQueryable();
        }
    }
}