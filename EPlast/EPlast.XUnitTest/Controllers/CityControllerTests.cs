using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.Interfaces;
using EPlast.Controllers;
using EPlast.ViewModels.City;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Logging;
using Xunit;

namespace EPlast.XUnitTest
{
    public class CityControllerTests
    {
        private readonly Mock<ICityService> _cityService;
        private readonly Mock<ICityMembersService> _cityMembersService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService<CityController>> _logger;

        private CityController CreateCityController => new CityController(_logger.Object,
            _cityService.Object,
            _cityMembersService.Object,
            _mapper.Object);

        public CityControllerTests()
        {
            _cityService = new Mock<ICityService>();
            _cityMembersService = new Mock<ICityMembersService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILoggerService<CityController>>();
        }

        [Fact]
        public async Task IndexTest()
        {
            // Arrange
            _cityService.Setup(c => c.GetAllDTOAsync(null))
                .ReturnsAsync(new List<CityDTO>());
            _mapper.Setup(m => m.Map<IEnumerable<CityDTO>, IEnumerable<CityViewModel>>(It.IsAny<IEnumerable<CityDTO>>()))
                .Returns(() => new List<CityViewModel>());
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Index();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CityProfileTests()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(() => new CityProfileDTO());
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityProfileViewModel());
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityProfile(FakeId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CityProfileInvalidIdTests()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityProfile(FakeId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task CityProfileExceptionTests()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityProfileAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityProfile(FakeId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task CityMembersCorrectTest()
        {
            // Arrange
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(() => new CityProfileViewModel());
            _cityService.Setup(c => c.GetCityMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityProfileDTO());
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityMembers(FakeId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CityMembersInvalidIdTest()
        {
            // Arrange
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(() => new CityProfileViewModel());
            _cityService.Setup(c => c.GetCityMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityMembers(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task CityMembersExceptionTest()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityMembersAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityMembers(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task CityFollowersCorrectTest()
        {
            // Arrange
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(() => new CityProfileViewModel());
            _cityService.Setup(c => c.GetCityFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityProfileDTO());
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityFollowers(FakeId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CityFollowersInvalidIdTest()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityFollowers(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task CityFollowersExceptionTest()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityFollowersAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityFollowers(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task CityAdminsCorrectTest()
        {
            // Arrange
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(() => new CityProfileViewModel());
            _cityService.Setup(c => c.GetCityAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityProfileDTO());
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityAdmins(FakeId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CityAdminsInvalidIdTest()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityAdmins(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task CityAdminsExceptionTest()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityAdminsAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityAdmins(FakeId);

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
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(() => new CityProfileViewModel());
            _cityService.Setup(c => c.EditAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityProfileDTO());
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Edit(FakeId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task EditGetInvalidIdTest()
        {
            // Arrange
            _cityService.Setup(c => c.EditAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Edit(FakeId);

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
            _cityService.Setup(c => c.EditAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Edit(1);

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
            CityProfileViewModel model = CreateFakeCityProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<CityProfileViewModel, CityProfileDTO>(It.IsAny<CityProfileViewModel>()))
                .Returns(new CityProfileDTO());
            _cityService.Setup(c => c.EditAsync(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
                .Verifiable();
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Edit(model, file);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CityProfile", viewResult.ActionName);
            Assert.Equal("City", viewResult.ControllerName);
        }

        [Fact]
        public async Task EditWithInvalidModelStateTest()
        {
            // Arrange
            CityProfileViewModel model = CreateFakeCityProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<CityProfileViewModel, CityProfileDTO>(It.IsAny<CityProfileViewModel>()))
                .Returns(new CityProfileDTO());
            _cityService.Setup(c => c.EditAsync(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
                .Verifiable();
            CityController cityController = CreateCityController;
            cityController.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await cityController.Edit(model, file);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task EditWithExceptionTest()
        {
            // Arrange
            CityProfileViewModel model = CreateFakeCityProfileViewModels(1).First();
            model.City = null;
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _cityService.Setup(c => c.EditAsync(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
                .ThrowsAsync(new ArgumentException("some message"));
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Edit(model, file);

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
            CityController cityController = CreateCityController;

            // Act
            var result = cityController.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CityDocumentsCorrectTest()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityProfileDTO());
            _mapper.Setup(m => m.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityProfileViewModel());
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityDocuments(FakeId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CityDocumentsInvalidIdTest()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityDocuments(FakeId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        [Fact]
        public async Task CityDocumentsExceptionTest()
        {
            // Arrange
            _cityService.Setup(c => c.GetCityDocumentsAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.CityDocuments(FakeId);

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
            _cityService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityDTO());
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(new CityViewModel());
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Details(3);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task DetailsInvalidIdTest()
        {
            // Arrange
            _cityService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Details(3);

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
            _cityService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("some message"));
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Details(3);

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
            CityProfileViewModel model = CreateFakeCityProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<CityProfileViewModel, CityProfileDTO>(It.IsAny<CityProfileViewModel>()))
                .Returns(new CityProfileDTO());
            _cityService.Setup(c => c.CreateAsync(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(3);
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Create(model, file);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("CityProfile", viewResult.ActionName);
            Assert.Equal("City", viewResult.ControllerName);
        }

        [Fact]
        public async Task CreateWithInvalidModelStateTest()
        {
            // Arrange
            CityProfileViewModel model = CreateFakeCityProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<CityProfileViewModel, CityProfileDTO>(It.IsAny<CityProfileViewModel>()))
                .Returns(new CityProfileDTO());
            _cityService.Setup(c => c.CreateAsync(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(3);
            CityController cityController = CreateCityController;
            cityController.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await cityController.Create(model, file);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task CreateWithExceptionTest()
        {
            // Arrange
            CityProfileViewModel model = CreateFakeCityProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _cityService.Setup(c => c.CreateAsync(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
                .ThrowsAsync(new ArgumentException("some message"));
            CityController cityController = CreateCityController;

            // Act
            var result = await cityController.Create(model, file);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }

        private int FakeId => 1;
        private IQueryable<CityProfileViewModel> CreateFakeCityProfileViewModels(int count)
        {
            List<CityProfileViewModel> cityProfilesDto = new List<CityProfileViewModel>();
            for (int i = 0; i < count; i++)
            {
                cityProfilesDto.Add(new CityProfileViewModel
                {
                    City = new CityViewModel
                    {
                        ID = i,
                        Name = "Name " + i
                    }
                });
            }
            return cityProfilesDto.AsQueryable();
        }
    }
}
