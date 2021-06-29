using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.Resources;

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
        private readonly Mock<UserManager<User>> _userManager;


        public ClubControllerTests()
        {
            _ClubAccessService = new Mock<IClubAccessService>();
            _ClubService = new Mock<IClubService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILoggerService<ClubController>>();
            _ClubParticipantsService = new Mock<IClubParticipantsService>();
            _ClubDocumentsService = new Mock<IClubDocumentsService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        }

        private ClubController CreateClubController => new ClubController(_logger.Object,
             _mapper.Object,
           _ClubService.Object,
           _ClubParticipantsService.Object,
           _ClubDocumentsService.Object,
           _ClubAccessService.Object,
           _userManager.Object
          );

        

        [Test]
        public async Task GetCities_Valid_Test()
        {
            // Arrange
            ClubController clubcon = CreateClubController;
            var httpContext = new Mock<HttpContext>();
            var context = new ControllerContext(
                new ActionContext(
                    httpContext.Object, new RouteData(),
                    new ControllerActionDescriptor()));
            clubcon.ControllerContext = context;
            _ClubService
                .Setup(c => c.GetClubs())
                .ReturnsAsync(GetFakeClubsForAdministration());

            // Act
            var result = await clubcon.GetClubs();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(((result as ObjectResult).Value as List<ClubForAdministrationDTO>)
                .Where(n => n.Name.Equals("Курінь")));
        }

        [TestCase(2)]
        public async Task GetProfile_Valid_Test(int id)
        {

            _ClubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(new ClubProfileDTO());
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetProfile(id);

            // Assert
            _mapper.Verify(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetProfile_Invalid_Test(int id)
        {
            // Arrange
            _ClubService.
                Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetProfile(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetProfile_Invalid_Test()
        {
            // Arrange
            _ClubService
                .Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetProfile(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetMembers_Valid_Test(int id)
        {

            _ClubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);

            // Arrange
            _ClubService
                .Setup(cs => cs.GetClubMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());
            ClubController ClubController = CreateClubController;

            // Act
            var result = await ClubController.GetMembers(id);

            // Assert
            _mapper.Verify(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()));
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
        }

        [TestCase(2)]
        public async Task GetMembers_Invalid_Test(int id)
        {
            // Arrange
            _ClubService
                .Setup(cs => cs.GetClubMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
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
            _ClubService
                .Setup(c => c.GetClubFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetFollowers(id);

            _ClubService.Setup(c => c.GetClubProfileAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);
            // Assert
            _mapper.Verify(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetFollowers_Invalid_Test(int id)
        {
            // Arrange
            _ClubService
                .Setup(c => c.GetClubFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetFollowers(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetAdmins_Valid_Test(int id)
        {
            // Arrange
            _ClubService
                .Setup(c => c.GetClubAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetAdmins(id);

            // Assert
            _mapper.Verify(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(2)]
        public async Task GetAdmins_Invalid_Test(int id)
        {
            // Arrange
            _ClubService
                .Setup(c => c.GetClubAdminsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetAdmins(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2)]
        public async Task GetDocuments_Valid_Test(int id)
        {
            // Arrange
            _ClubService
                .Setup(c => c.GetClubDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());
            _mapper
                .Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetDocuments(id);

            // Assert
            _mapper.Verify(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()));
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetDocumentsInvalidCheck()
        {
            // Arrange
            _ClubService.Setup(c => c.GetClubDocumentsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper.Setup(m => m.Map<ClubProfileDTO, ClubViewModel>(It.IsAny<ClubProfileDTO>()))
                .Returns(new ClubViewModel());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetDocuments(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_Valid_Test()
        {
            // Arrange
            _ClubService
                .Setup(c => c.GetClubProfileAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubProfileDTO());
            _mapper
                .Setup(m => m.Map<ClubDTO, ClubViewModel>(It.IsAny<ClubDTO>()))
                .Returns(new ClubViewModel());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.Details(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Details_Invalid_Test()
        {
            // Arrange
            _ClubService
                .Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            _mapper
                .Setup(m => m.Map<ClubDTO, ClubViewModel>(It.IsAny<ClubDTO>()))
                .Returns(new ClubViewModel());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.Details(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase("logoName")]
        public async Task GetPhotoBase64_Valid_Test(string logoName)
        {
            // Arrange
            _ClubService
                .Setup(c => c.GetLogoBase64(It.IsAny<string>()))
                .ReturnsAsync(new string("some string"));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetPhotoBase64(logoName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_Test()
        {
            // Arrange
            ClubViewModel TestVM = new ClubViewModel();
            _ClubService
                .Setup(c => c.CreateAsync(It.IsAny<ClubDTO>()))
                .ReturnsAsync(new int());
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.Create(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_InvalidModelState_Valid_Test()
        {
            // Arrange
            ClubViewModel TestVM = new ClubViewModel();
            _ClubService
                .Setup(c => c.CreateAsync(It.IsAny<ClubDTO>()))
                .ReturnsAsync(new int());
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;
            Clubcon.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await Clubcon.Create(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_Invalid_Test()
        {
            // Arrange
            ClubViewModel TestVM = new ClubViewModel();
            _ClubService
                .Setup(c => c.CreateAsync(It.IsAny<ClubDTO>())).ThrowsAsync(new InvalidOperationException());
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.Create(TestVM);

            // Assert
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task Edit_Valid_Test()
        {
            // Arrange
            ClubViewModel TestVM = new ClubViewModel();
            _ClubService
                .Setup(c => c.EditAsync(It.IsAny<ClubDTO>()));
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.Edit(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Edit_InvalidModelState_Valid_Test()
        {
            // Arrange
            ClubViewModel TestVM = new ClubViewModel();
            _ClubService
                .Setup(c => c.EditAsync(It.IsAny<ClubDTO>()));
            _mapper
                .Setup(m => m.Map<ClubViewModel, ClubDTO>(It.IsAny<ClubViewModel>()))
                .Returns(new ClubDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;
            Clubcon.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await Clubcon.Edit(TestVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Remove_Valid_Test()
        {
            // Arrange
            _ClubService
                .Setup(c => c.RemoveAsync(It.IsAny<int>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.Remove(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task AddFollower_Valid_Test()
        {
            _ClubParticipantsService.Setup(c => c.AddFollowerAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(new ClubMembersDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.AddFollower(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddFollowerWithId_Valid_Test()
        {
            // Arrange
            _ClubParticipantsService
                .Setup(c => c.AddFollowerAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new ClubMembersDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.AddFollowerWithId(GetFakeID(), GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RemoveFollower_Valid_Test()
        {
            // Arrange
            _ClubParticipantsService
                .Setup(c => c.RemoveFollowerAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.RemoveFollower(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task ChangeApproveStatus_Valid_Test()
        {
            // Arrange
            _ClubParticipantsService
                .Setup(c => c.ToggleApproveStatusAsync(It.IsAny<int>()))
                .ReturnsAsync(new ClubMembersDTO());
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.ChangeApproveStatus(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetClubNameOfApprovedMemberTest()
        {
            //Arrange
            _ClubParticipantsService
                .Setup(c => c.ClubOfApprovedMember(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            //Act
            var result = await Clubcon.ClubNameOfApprovedMember(GetStringFakeId());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddAdmin_Valid_Test()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(It.IsAny<ClubAdministrationViewModel>()))
                .Returns(new ClubAdministrationDTO() { AdminType = new BLL.DTO.Admin.AdminTypeDTO() });
            _ClubParticipantsService
                .Setup(c => c.AddAdministratorAsync(It.IsAny<ClubAdministrationDTO>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.AddAdmin(new ClubAdministrationViewModel());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RemoveAdmin_Valid_Test()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(It.IsAny<ClubAdministrationViewModel>()))
                .Returns(new ClubAdministrationDTO());
            _ClubParticipantsService
                .Setup(c => c.RemoveAdministratorAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.RemoveAdmin(GetFakeID());

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
                .Setup(m => m.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(It.IsAny<ClubAdministrationViewModel>()))
                .Returns(new ClubAdministrationDTO());
            _ClubParticipantsService
                .Setup(c => c.EditAdministratorAsync(It.IsAny<ClubAdministrationDTO>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.EditAdmin(admin);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddDocument_Valid_Test()
        {
            // Arrange
            ClubDocumentsViewModel document = new ClubDocumentsViewModel();
            _mapper
                .Setup(m => m.Map<ClubDocumentsViewModel, ClubDocumentsDTO>(It.IsAny<ClubDocumentsViewModel>()))
                .Returns(new ClubDocumentsDTO());
            _ClubDocumentsService
                .Setup(c => c.AddDocumentAsync(It.IsAny<ClubDocumentsDTO>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.AddDocument(document);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetFileBase64_Valid_Test()
        {
            // Arrange
            _ClubDocumentsService
                .Setup(c => c.DownloadFileAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<string>());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetFileBase64(GetFakeFileName());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task RemoveDocument_Valid_Test()
        {
            // Arrange
            _ClubDocumentsService
                .Setup(c => c.DeleteFileAsync(It.IsAny<int>()));
            _logger
                .Setup(l => l.LogInformation(It.IsAny<string>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.RemoveDocument(GetFakeID());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }


        [Test]
        public async Task GetDocumentTypesAsync_Valid_Test()
        {
            // Arrange
            _ClubDocumentsService
                .Setup(c => c.GetAllClubDocumentTypesAsync())
                .ReturnsAsync(It.IsAny<IEnumerable<ClubDocumentTypeDTO>>());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetDocumentTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetClubThatUserHasAccessTo_Valid_Test()
        {
            // Arrange
            _ClubAccessService
                .Setup(c => c.GetClubsAsync(It.IsAny<User>()));
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetClubsThatUserHasAccessTo();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserAdministrations_Valid_Test()
        {
            // Arrange
            _ClubParticipantsService
                .Setup(c => c.GetAdministrationsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<ClubAdministrationDTO>>());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetUserAdministrations(GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserPreviousAdministrations_Valid_Test()
        {
            // Arrange
            _ClubParticipantsService
                .Setup(c => c.GetPreviousAdministrationsOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<ClubAdministrationDTO>>());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetUserPreviousAdministrations(GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllAdministrationStatuses_Valid_Test()
        {
            // Arrange
            _ClubParticipantsService
                .Setup(c => c.GetAdministrationStatuses(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<IEnumerable<ClubAdministrationStatusDTO>>());
            ClubController Clubcon = CreateClubController;

            // Act
            var result = await Clubcon.GetAllAdministrationStatuses(GetStringFakeId());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetClubsOptions()
        {
            //Arrange
            _userManager.Setup(r => r.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());
            _ClubAccessService.Setup(r => r.GetAllClubsIdAndName(It.IsAny<User>()))
                .ReturnsAsync(new List<ClubForAdministrationDTO>());
            ClubController Clubcon = CreateClubController;

            //Act
            var result = await Clubcon.GetClubsOptionsThatUserHasAccessTo();

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

        private List<ClubDTO> GetClubsBySearch()
        {
            return new List<ClubDTO>()
            {
                new ClubDTO()
                {
                    Name = "Курінь",
                }
            };
        }

        private IEnumerable<ClubForAdministrationDTO> GetFakeClubsForAdministration()
        {
            return new List<ClubForAdministrationDTO>()
            {
                new ClubForAdministrationDTO
                {
                    Name = "Курінь"
                }
            }.AsEnumerable();
        }
    }
}
