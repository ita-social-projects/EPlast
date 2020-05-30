using EPlast.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.Interfaces.City;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.ViewModels.City;
using Microsoft.AspNetCore.Http.Internal;
using Xunit;

namespace EPlast.XUnitTest
{
    public class CityControllerTests
    {

        private readonly Mock<ICityService> _cityService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService<CityController>> _logger;
        private CityController CreateCityController => new CityController(_logger.Object, _cityService.Object, _mapper.Object);
        public CityControllerTests()
        {
            _cityService = new Mock<ICityService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILoggerService<CityController>>();
        }

        [Fact]
        public void IndexTest()
        {
            _cityService.Setup(c => c.GetAllDTO())
                .Returns(new List<CityDTO>());
            _mapper.Setup(
                    m => m.Map<IEnumerable<CityDTO>, IEnumerable<CityViewModel>>(It.IsAny<IEnumerable<CityDTO>>()))
                .Returns(() => new List<CityViewModel>());
            CityController cityController = CreateCityController;

            var result = cityController.Index();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void CityProfileTests()
        {

            _cityService.Setup(c => c.CityProfile(It.IsAny<int>()))
                .Returns(() => new CityProfileDTO());
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityProfileViewModel());
            CityController cityController = CreateCityController;

            var result = cityController.CityProfile(1);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void EditWithValidModelStateTest()
        {
            CityProfileViewModel model = createFakeCityProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<CityProfileViewModel, CityProfileDTO>(It.IsAny<CityProfileViewModel>()))
                .Returns(new CityProfileDTO());
            _cityService.Setup(c => c.Edit(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
                .Verifiable();
            CityController cityController = CreateCityController;

            var result = cityController.Edit(model, file);

            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }
        [Fact]
        public void EditWithInvalidModelStateTest()
        {
            CityProfileViewModel model = createFakeCityProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<CityProfileViewModel, CityProfileDTO>(It.IsAny<CityProfileViewModel>()))
                .Returns(new CityProfileDTO());
            _cityService.Setup(c => c.Edit(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
                .Verifiable();
            CityController cityController = CreateCityController;
            cityController.ModelState.AddModelError("NameError", "Required");
            var result = cityController.Edit(model, file);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
        //[Fact]
        //public void EditWithExceptionTest()
        //{
        //    CityProfileViewModel model = createFakeCityProfileViewModels(1).First();
        //    model.City = null;
        //    IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
        //    _mapper.Setup(m => m.Map<CityProfileViewModel, CityProfileDTO>(It.IsAny<CityProfileViewModel>()))
        //        .Returns(new CityProfileDTO());
        //    _cityService.Setup(c => c.Edit(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
        //        .Verifiable();
        //    CityController cityController = CreateCityController;
        //    cityController.ModelState.AddModelError("NameError", "Required");

        //    var result = cityController.Edit(model, file);

        //    var viewResult = Assert.IsType<RedirectToActionResult>(result);
        //    Assert.NotNull(viewResult);
        //    Assert.Equal("HandleError", viewResult.ActionName);
        //    Assert.Equal("Error", viewResult.ControllerName);
        //}
        [Fact]
        public void EditGetCorrectTest()
        {
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(() => new CityProfileViewModel());
            _cityService.Setup(c => c.Edit(It.IsAny<int>()))
                .Returns(new CityProfileDTO());
            CityController cityController = CreateCityController;

            var result = cityController.Edit(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CityAdminsCorrectTest()
        {
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(() => new CityProfileViewModel());
            _cityService.Setup(c => c.CityAdmins(It.IsAny<int>()))
                .Returns(new CityProfileDTO());
            CityController cityController = CreateCityController;

            var result = cityController.CityAdmins(1);

            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void CityFollowersCorrectTest()
        {
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(() => new CityProfileViewModel());
            _cityService.Setup(c => c.CityFollowers(It.IsAny<int>()))
                .Returns(new CityProfileDTO());
            CityController cityController = CreateCityController;

            var result = cityController.CityFollowers(1);

            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void CityMembersCorrectTest()
        {
            _mapper.Setup(c => c.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(() => new CityProfileViewModel());
            _cityService.Setup(c => c.CityMembers(It.IsAny<int>()))
                .Returns(new CityProfileDTO());
            CityController cityController = CreateCityController;

            var result = cityController.CityMembers(1);

            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void CityDocumentsCorrectTest()
        {
            _cityService.Setup(c => c.CityDocuments(It.IsAny<int>()))
                .Returns(new CityProfileDTO());
            _mapper.Setup(m => m.Map<CityProfileDTO, CityProfileViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityProfileViewModel());
            CityController cityController = CreateCityController;

            var result = cityController.CityDocuments(3);

            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void DetailsDocumentsCorrectTest()
        {
            _cityService.Setup(c => c.GetById(It.IsAny<int>()))
                .Returns(new CityDTO());
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(new CityViewModel());
            CityController cityController = CreateCityController;

            var result = cityController.CityDocuments(3);

            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void CreateWithValidModelStateTest()
        {
            CityProfileViewModel model = createFakeCityProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<CityProfileViewModel, CityProfileDTO>(It.IsAny<CityProfileViewModel>()))
                .Returns(new CityProfileDTO());
            _cityService.Setup(c => c.Create(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
                .Returns(3);
            CityController cityController = CreateCityController;

            var result = cityController.Create(model, file);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("CityProfile", viewResult.ActionName);
            Assert.Equal("City", viewResult.ControllerName);
        }
        [Fact]
        public void CreateWithInvalidModelStateTest()
        {
            CityProfileViewModel model = createFakeCityProfileViewModels(1).First();
            IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
            _mapper.Setup(m => m.Map<CityProfileViewModel, CityProfileDTO>(It.IsAny<CityProfileViewModel>()))
                .Returns(new CityProfileDTO());
            _cityService.Setup(c => c.Create(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
                .Returns(3);
            CityController cityController = CreateCityController;
            cityController.ModelState.AddModelError("NameError", "Required");

            var result = cityController.Create(model, file);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }
        [Fact]
        public void CreateGetCorrectTest()
        {
            CityController cityController = CreateCityController;

            var result = cityController.Create();

            Assert.IsType<ViewResult>(result);

        }
        //[Fact]
        //public void CreateWithExceptionTest()
        //{
        //    CityProfileViewModel model = createFakeCityProfileViewModels(1).First();
        //    IFormFile file = new FormFile(null, 1234, 1134, "fdd", "dfsdf");
        //    _mapper.Setup(m => m.Map<CityProfileViewModel, CityProfileDTO>(It.IsAny<CityProfileViewModel>()))
        //        .Returns(() => null);
        //    _cityService.Setup(c => c.Create(It.IsAny<CityProfileDTO>(), It.IsAny<IFormFile>()))
        //        .Returns(3);
        //    CityController cityController = CreateCityController;

        //    var result = cityController.Create(model, file);

        //    var viewResult = Assert.IsType<RedirectToActionResult>(result);
        //    Assert.NotNull(viewResult);
        //    Assert.Equal("HandleError", viewResult.ActionName);
        //    Assert.Equal("Error", viewResult.ControllerName);
        //}

        private IQueryable<CityProfileDTO> createFakeCityProfilesDto(int count)
        {
            List<CityProfileDTO> cityProfilesDto = new List<CityProfileDTO>();
            for (int i = 0; i < count; i++)
            {
                cityProfilesDto.Add(new CityProfileDTO
                {
                    City = new CityDTO
                    {
                        ID = i,
                        Name = "Name " + i
                    }
                });
            }
            return cityProfilesDto.AsQueryable();
        }
        private IQueryable<CityProfileViewModel> createFakeCityProfileViewModels(int count)
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
