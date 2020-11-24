using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class ClubControllerTests
    {
        private readonly Mock<IClubService> _ClubService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService<ClubController>> _logger;
        private readonly Mock<IClubParticipantsService> _ClubParticipantsService;
        private readonly Mock<IClubAccessService> _ClubAccessService;
        private readonly Mock<IClubDocumentsService> _ClubDocumentsService;
        private readonly Mock<IClubAnnualReportService> _ClubAnnualReportService;


        public ClubControllerTests()
        {
            _ClubAccessService = new Mock<IClubAccessService>();
            _ClubService = new Mock<IClubService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILoggerService<ClubController>>();
            _ClubParticipantsService = new Mock<IClubParticipantsService>();
            _ClubDocumentsService = new Mock<IClubDocumentsService>();
            _ClubAnnualReportService = new Mock<IClubAnnualReportService>();
        }

        private ClubController CreateClubController => new ClubController(_logger.Object,
             _mapper.Object,
           _ClubService.Object,
           _ClubParticipantsService.Object,
           _ClubDocumentsService.Object,
           _ClubAccessService.Object,
           _ClubAnnualReportService.Object
          );




        [TestCase(2)]
        public async Task GetMembers_Valid_Test(int id)
        {
            _ClubService.Setup(cs => cs.GetClubMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());

            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());

            ClubController ClubController = CreateClubController;


            var result = await ClubController.GetMembers(id);


            _mapper.Verify(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()));
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
        }



        [TestCase(2)]
        public async Task GetProfile_Valid_Test(int id)
        {

            _ClubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new ClubProfileDTO());

            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetProfile(id);


            _mapper.Verify(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetFollowers_Valid_Test(int id)
        {

            _ClubService.Setup(c => c.GetClubFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());

            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetFollowers(id);
            _mapper.Verify(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [TestCase(2)]
        public async Task GetFollowers_Invalid_Test(int id)
        {

            _ClubService.Setup(c => c.GetClubFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetFollowers(id);


            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [TestCase(2)]
        public async Task GetProfile_Invalid_Test(int id)
        {

            _ClubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetProfile(id);

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetMembers_Invalid_Test(int id)
        {
            _ClubService.Setup(cs => cs.GetClubMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());

            ClubController ClubController = CreateClubController;


            var result = await ClubController.GetMembers(id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [Test]
        public async Task GetProfile_Invalid_Test()
        {

            _ClubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(() => null);

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetProfile(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetAdmins_Valid_Test(int id)
        {
            _ClubService.Setup(c => c.GetClubAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());

            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetAdmins(id);

            _mapper.Verify(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [TestCase(2)]
        public async Task GetAdmins_Invalid_Test(int id)
        {

            _ClubService.Setup(c => c.GetClubAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetAdmins(id);


            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [TestCase(2)]
        public async Task GetDocuments_Valid_Test(int id)
        {

            _ClubService.Setup(c => c.GetClubDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());

            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetDocuments(id);


            _mapper.Verify(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task GetDocumentsInvalidCheck()
        {

            _ClubService.Setup(c => c.GetClubDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetDocuments(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [Test]
        public async Task Details_Valid_Test()
        {
            _ClubService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubDTO());

            _mapper.Setup(m => m.Map<ClubDTO, ClubViewModel>(It.IsAny<ClubDTO>()))
                .Returns(new ClubViewModel());

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.Details(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task Details_Invalid_Test()
        {
            _ClubService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _mapper.Setup(m => m.Map<ClubDTO, ClubViewModel>(It.IsAny<ClubDTO>()))
                .Returns(new ClubViewModel());

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.Details(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [TestCase("logoName")]
        public async Task GetPhotoBase64_Valid_Test(string logoName)
        {
            _ClubService.Setup(c => c.GetLogoBase64(It.IsAny<string>()))
                .ReturnsAsync(new string("some string"));

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetPhotoBase64(logoName);

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_Test()
        {
            ClubViewModel TestVM = new ClubViewModel();

            _ClubService.Setup(c => c.CreateAsync(It.IsAny<ClubDTO>()))
                .ReturnsAsync(new int());

            _mapper.Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.Create(TestVM);

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_InvalidModelState_Valid_Test()
        {
            ClubViewModel TestVM = new ClubViewModel();

            _ClubService.Setup(c => c.CreateAsync(It.IsAny<ClubDTO>()))
                .ReturnsAsync(new int());

            _mapper.Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            ClubController Clubcon = CreateClubController;
            Clubcon.ModelState.AddModelError("NameError", "Required");

            var result = await Clubcon.Create(TestVM);

            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }



        [Test]
        public async Task Edit_InvalidModelState_Valid_Test()
        {
            ClubViewModel TestVM = new ClubViewModel();

            _ClubService.Setup(c => c.EditAsync(It.IsAny<ClubDTO>()));

            _mapper.Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            ClubController Clubcon = CreateClubController;
            Clubcon.ModelState.AddModelError("NameError", "Required");


            var result = await Clubcon.Edit(TestVM);

            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Edit_Valid_Test()
        {
            ClubViewModel TestVM = new ClubViewModel();

            _ClubService.Setup(c => c.EditAsync(It.IsAny<ClubDTO>()));

            _mapper.Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.Edit(TestVM);

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task AddFollower_Valid_Test()
        {
            _ClubParticipantsService.Setup(c => c.AddFollowerAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new ClubMembersDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.AddFollower(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Remove_Valid_Test()
        {

            _ClubService.Setup(c => c.RemoveAsync(It.IsAny<int>()));

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.Remove(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task RemoveFollower_Valid_Test()
        {
            _ClubParticipantsService.Setup(c => c.RemoveFollowerAsync(It.IsAny<int>()));

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.RemoveFollower(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task ChangeApproveStatus_Valid_Test()
        {
            _ClubParticipantsService.Setup(c => c.ToggleApproveStatusAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubMembersDTO());

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.ChangeApproveStatus(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EditAdmin_Valid_Test()
        {
            ClubAdministrationViewModel admin = new ClubAdministrationViewModel();

            _mapper.Setup(m => m.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(It.IsAny<ClubAdministrationViewModel>()))
                .Returns(new ClubAdministrationDTO());

            _ClubParticipantsService.Setup(c => c.EditAdministratorAsync(It.IsAny<ClubAdministrationDTO>()));

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.EditAdmin(admin);

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RemoveAdmin_Valid_Test()
        {


            _mapper.Setup(m => m.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(It.IsAny<ClubAdministrationViewModel>()))
                .Returns(new ClubAdministrationDTO());

            _ClubParticipantsService.Setup(c => c.RemoveAdministratorAsync(It.IsAny<int>()));

            _logger.Setup(l => l.LogInformation(It.IsAny<string>()));

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.RemoveAdmin(GetFakeID());

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetClubThatUserHasAccessTo_Valid_Test()
        {

            _ClubAccessService.Setup(c => c.GetClubsAsync(It.IsAny<ClaimsPrincipal>()));

            ClubController Clubcon = CreateClubController;

            var result = await Clubcon.GetClubsThatUserHasAccessTo();

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private int GetFakeID()
        {
            return 1;
        }
    }
}
