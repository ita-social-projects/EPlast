using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.City;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{

    class CityControllerTests
    {

        private readonly Mock<ICityService> _cityService;
        private readonly Mock<ICityMembersService> _cityMembersService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService<CitiesController>> _logger;
        private readonly Mock<ICityAdministrationService> _cityAdministrationService;
        private readonly Mock<ICityAccessService> _cityAccessService;
       

        public CityControllerTests()
        {
            _cityAccessService = new Mock<ICityAccessService>();
            _cityService = new Mock<ICityService>();
            _cityMembersService = new Mock<ICityMembersService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILoggerService<CitiesController>>();
            _cityAdministrationService = new Mock<ICityAdministrationService>();
        }

        private CitiesController CreateCityController => new CitiesController(_logger.Object,

             _mapper.Object,
           _cityService.Object,
           _cityMembersService.Object,
           _cityAdministrationService.Object,
           _cityAccessService.Object
          );




        [TestCase(2)]
        public async Task GetMembers_Valid_Test(int id)
        {
            _cityService.Setup(cs => cs.GetCityMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityProfileDTO());

            _mapper.Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            CitiesController cityController = CreateCityController;


            var result = await cityController.GetMembers(id);


            _mapper.Verify(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()));
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
        }



        [TestCase(2)]
        public async Task GetProfile_Valid_Test(int id)
        {

            _cityService.Setup(c => c.GetCityProfileAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new CityProfileDTO());

            _mapper.Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetProfile(id);


            _mapper.Verify(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetFollowers_Valid_Test(int id)
        {

            _cityService.Setup(c => c.GetCityFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityProfileDTO());

            _mapper.Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetFollowers(id);
            _mapper.Verify(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [TestCase(2)]
        public async Task GetFollowers_Invalid_Test(int id)
        {

            _cityService.Setup(c => c.GetCityFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetFollowers(id);


            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [TestCase(2)]
        public async Task GetProfile_Invalid_Test(int id)
        {

            _cityService.Setup(c => c.GetCityProfileAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetProfile(id);

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetMembers_Invalid_Test(int id)
        {
            _cityService.Setup(cs => cs.GetCityMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            CitiesController cityController = CreateCityController;


            var result = await cityController.GetMembers(id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [Test]
        public async Task GetProfile_Invalid_Test()
        {

            _cityService.Setup(c => c.GetCityProfileAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(() => null);

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetProfile(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }





        [TestCase(2)]
        public async Task GetAdmins_Valid_Test(int id)
        {
            _cityService.Setup(c => c.GetCityAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityProfileDTO());

            _mapper.Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetAdmins(id);

            _mapper.Verify(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [TestCase(2)]
        public async Task GetAdmins_Invalid_Test(int id)
        {

            _cityService.Setup(c => c.GetCityAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetAdmins(id);


            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [TestCase(2)]
        public async Task GetDocuments_Valid_Test(int id)
        {

            _cityService.Setup(c => c.GetCityDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityProfileDTO());

            _mapper.Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetDocuments(id);


            _mapper.Verify(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task GetDocumentsInvalidCheck()
        {

            _cityService.Setup(c => c.GetCityDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<CityProfileDTO, CityViewModel>(It.IsAny<CityProfileDTO>()))
                .Returns(new CityViewModel());

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetDocuments(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [Test]
        public async Task Details_Valid_Test()
        {
            _cityService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityDTO());

            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(new CityViewModel());

            CitiesController citycon = CreateCityController;

            var result = await citycon.Details(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task Details_Invalid_Test()
        {
            _cityService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(new CityViewModel());

            CitiesController citycon = CreateCityController;

            var result = await citycon.Details(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [TestCase("logoName")]
        public async Task GetPhotoBase64_Valid_Test(string logoName)
        {
            _cityService.Setup(c => c.GetLogoBase64(It.IsAny<string>()))
                .ReturnsAsync(new string("some string"));

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetPhotoBase64(logoName);

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_Test()
        {
            CityViewModel TestVM = new CityViewModel();

            _cityService.Setup(c => c.CreateAsync(It.IsAny<CityDTO>()))
                .ReturnsAsync(new int());

            _mapper.Setup(m => m.Map<CityViewModel, CityDTO>(It.IsAny<CityViewModel>()))
                .Returns(new CityDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            CitiesController citycon = CreateCityController;

            var result = await citycon.Create(TestVM);

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }



        [Test]
        public async Task Create_InvalidModelState_Valid_Test()
        {
            CityViewModel TestVM = new CityViewModel();

            _cityService.Setup(c => c.CreateAsync(It.IsAny<CityDTO>()))
                .ReturnsAsync(new int());

            _mapper.Setup(m => m.Map<CityViewModel, CityDTO>(It.IsAny<CityViewModel>()))
                .Returns(new CityDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            CitiesController citycon = CreateCityController;
            citycon.ModelState.AddModelError("NameError", "Required");

            var result = await citycon.Create(TestVM);

            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }



        [Test]
        public async Task Edit_InvalidModelState_Valid_Test()
        {
            CityViewModel TestVM = new CityViewModel();

            _cityService.Setup(c => c.EditAsync(It.IsAny<CityDTO>()));

            _mapper.Setup(m => m.Map<CityViewModel, CityDTO>(It.IsAny<CityViewModel>()))
                .Returns(new CityDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            CitiesController citycon = CreateCityController;
            citycon.ModelState.AddModelError("NameError", "Required");


            var result = await citycon.Edit(TestVM);

            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }



        [Test]
        public async Task Edit_Valid_Test()
        {
            CityViewModel TestVM = new CityViewModel();

            _cityService.Setup(c => c.EditAsync(It.IsAny<CityDTO>()));

            _mapper.Setup(m => m.Map<CityViewModel, CityDTO>(It.IsAny<CityViewModel>()))
                .Returns(new CityDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            CitiesController citycon = CreateCityController;

            var result = await citycon.Edit(TestVM);

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }



        [Test]
        public async Task AddFollower_Valid_Test()
        {
            _cityMembersService.Setup(c => c.AddFollowerAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new CityMembersDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            CitiesController citycon = CreateCityController;

            var result = await citycon.AddFollower(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task Remove_Valid_Test()
        {

            _cityService.Setup(c => c.RemoveAsync(It.IsAny<int>()));

            CitiesController citycon = CreateCityController;

            var result = await citycon.Remove(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }



        [Test]
        public async Task RemoveFollower_Valid_Test()
        {
            _cityMembersService.Setup(c => c.RemoveFollowerAsync(It.IsAny<int>()));

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            CitiesController citycon = CreateCityController;

            var result = await citycon.RemoveFollower(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task ChangeApproveStatus_Valid_Test()
        {
            _cityMembersService.Setup(c => c.ToggleApproveStatusAsync(It.IsAny<int>()))
                .ReturnsAsync(new CityMembersDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            CitiesController citycon = CreateCityController;

            var result = await citycon.ChangeApproveStatus(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EditAdmin_Valid_Test()
        {
            CityAdministrationViewModel admin = new CityAdministrationViewModel();

            _mapper.Setup(m => m.Map<CityAdministrationViewModel, CityAdministrationDTO>(It.IsAny<CityAdministrationViewModel>()))
                .Returns(new CityAdministrationDTO());

            _cityAdministrationService.Setup(c => c.EditAdministratorAsync(It.IsAny<CityAdministrationDTO>()));

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            CitiesController citycon = CreateCityController;

            var result = await citycon.EditAdmin(admin);

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RemoveAdmin_Valid_Test()
        {


            _mapper.Setup(m => m.Map<CityAdministrationViewModel, CityAdministrationDTO>(It.IsAny<CityAdministrationViewModel>()))
                .Returns(new CityAdministrationDTO());

            _cityAdministrationService.Setup(c => c.RemoveAdministratorAsync(It.IsAny<int>()));

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            CitiesController citycon = CreateCityController;

            var result = await citycon.RemoveAdmin(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }


        [Test]
        public async Task GetCitiesThatUserHasAccessTo_Valid_Test()
        {

            _cityAccessService.Setup(c => c.GetCitiesAsync(It.IsAny<ClaimsPrincipal>()));

            CitiesController citycon = CreateCityController;

            var result = await citycon.GetCitiesThatUserHasAccessTo();

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


       
        private int GetFakeID()
        {
            return 1;
        }
      


    }
}

