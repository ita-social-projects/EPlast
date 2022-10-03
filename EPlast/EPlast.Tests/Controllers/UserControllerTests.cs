using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.Models.Approver;
using EPlast.WebApi.Models.User;
using EPlast.WebApi.Models.UserModels;
using EPlast.WebApi.Models.UserModels.UserProfileFields;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ClubMembers = EPlast.DataAccess.Entities.ClubMembers;
using StatusCodeResult = Microsoft.AspNetCore.Mvc.StatusCodeResult;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class UserControllerTests
    {
        private Mock<IUserService> _userService;
        private Mock<IUserPersonalDataService> _userPersonalDataService;
        private Mock<IUserManagerService> _userManagerService;
        private Mock<IConfirmedUsersService> _confirmedUserService;
        private Mock<ILoggerService<UserController>> _loggerService;
        private Mock<IMapper> _mapper;
        private Mock<UserManager<User>> _userManager;
        private Mock<IUserAccessService> _userAccessService;

        private UserController _userController;

        [SetUp]
        public void SetUp()
        {
            _userService = new Mock<IUserService>();
            _userPersonalDataService = new Mock<IUserPersonalDataService>();
            _userManagerService = new Mock<IUserManagerService>();
            _confirmedUserService = new Mock<IConfirmedUsersService>();
            _loggerService = new Mock<ILoggerService<UserController>>();
            _mapper = new Mock<IMapper>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _userAccessService = new Mock<IUserAccessService>();

            _userController = new UserController(
                _userService.Object,
                _userPersonalDataService.Object,
                _confirmedUserService.Object,
                _userManagerService.Object,
                _loggerService.Object,
                _mapper.Object,
                _userManager.Object,
                _userAccessService.Object);
        }

        [Test]
        public async Task Get_NullUserIdString_ReturnsNotFoundResult()
        {
            // Arrange
            string nullString = null;

            // Act
            var result = await _userController.Get(nullString);

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Get_User_ReturnsOkObjectResult()
        {
            // Arrange
            string id = "1";

            _userService
                .Setup((x) => x.GetUserAsync(id))
                .ReturnsAsync(CreateFakeUser());

            _userService
                .Setup((x) => x.CheckOrAddPlastunRole(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(It.IsAny<TimeSpan>());

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDto>(), It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<bool>());

            // Act
            var result = await _userController.Get(id);

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_User_ReturnsCreatedModel()
        {
            // Arrange
            var id = "1";
            var isPlastun = true;
            var time = new TimeSpan(1, 1, 1);
            var timeInDays = 0;

            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);

            _userService
                .Setup((x) => x.GetUserAsync(id))
                .ReturnsAsync(CreateFakeUser());

            _userService
                .Setup((x) => x.CheckOrAddPlastunRole(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(time);

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDto>(), It.IsAny<string>()))
                .ReturnsAsync(isPlastun);


            _mapper
                .Setup((x) => x.Map<UserDto, UserViewModel>(It.IsAny<UserDto>()))
                .Returns(CreateFakeUserViewModel());

            var expectedUserId = id;
            var expectedTimeToJoinPlast = timeInDays;
            var expectedIsUserPlastun = isPlastun;

            // Act
            var result = await _userController.Get(id);

            var actual = (result as ObjectResult).Value as PersonalDataViewModel;

            // Assert
            Assert.NotNull(actual);
            Assert.AreEqual(expectedUserId, actual.User.ID);
            Assert.AreEqual(expectedTimeToJoinPlast, actual.TimeToJoinPlast);
            Assert.AreEqual(expectedIsUserPlastun, actual.IsUserPlastun);
        }

        [Test]
        public async Task Get_NullUser_ReturnsNotFoundResult()
        {
            // Arrange
            string id = "1";

            _userService
                .Setup((x) => x.GetUserAsync(id))
                .ReturnsAsync(It.IsAny<UserDto>);

            // Act
            var result = await _userController.Get(id);

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _userService.Verify();
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [Test]
        public async Task Get_Forbidden_Returns403Forbidden()
        {
            // Arrange
            var id = "1";
            var isPlastun = true;
            var time = new TimeSpan(1, 1, 1);
            var currentUser = new UserDto() { Id = "2" };

            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("2");

            _userService
                .Setup((x) => x.GetUserAsync("2"))
                .ReturnsAsync(currentUser);

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(currentUser, Roles.RegisteredUser)).ReturnsAsync(true);

            _userService
                .Setup((x) => x.GetUserAsync(id))
                .ReturnsAsync(CreateFakeUser());

            _userService
                .Setup((x) => x.CheckOrAddPlastunRole(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(time);

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDto>(), It.IsAny<string>()))
                .ReturnsAsync(isPlastun);

            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _userController.Get(id);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _userService.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task PutComment_UserIdIsValidAndExists_ReturnsOk()
        {
            // Arrange
            string userId = Guid.Empty.ToString();
            string comment = "I love unit testing!";

            _userManagerService
                .Setup((svc) => svc.FindByIdAsync(userId))
                .ReturnsAsync(new UserDto() { Id = userId });

            // Act
            var result = await _userController.PutComment(userId, comment);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task PutComment_UserIdIsValidButDoesntExist_ReturnsNotFound()
        {
            // Arrange
            string userId = Guid.Empty.ToString();
            string comment = "I love unit testing!";

            _userManagerService
                .Setup((svc) => svc.FindByIdAsync(userId))
                .ReturnsAsync((UserDto)null);

            // Act
            var result = await _userController.PutComment(userId, comment);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task PutComment_UserIdIsNotValid_ReturnsBadRequest()
        {
            // Arrange
            string userId = null;
            string comment = "I love unit testing!";

            // Act
            var result = await _userController.PutComment(userId, comment);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetUserProfile_UserCanSeeFullProfile_ReturnsFullUserProfile()
        {
            //Arrange
            var focusUserId = "qwerty123456";
            var user = CreateFakeUser();
            var userAccess= new Dictionary<string, bool>()
            {
                {"CanViewUserFullProfile", true}  
            };
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };

            var currentUser = new User();
            var focusUserViewModel = new UserViewModel();
            _userService.Setup(us => us.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(currentUser);
            _userAccessService.Setup(ua =>
                    ua.GetUserProfileAccessAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(userAccess);
            _mapper.Setup(m => m.Map<UserDto, UserViewModel>(It.IsAny<UserDto>()))
                .Returns(focusUserViewModel);

            //Act
            var result= await _userController.GetUserProfile(focusUserId);
            var actual = (result as ObjectResult).Value as PersonalDataViewModel;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(actual);
            Assert.IsNull(actual.ShortUser);
            Assert.IsNotNull(actual.User);
        }

        [Test]
        public async Task GetUserProfile_UserCanNotSeeFullProfile_ReturnsShortUserProfile()
        {
            //Arrange
            var focusUserId = "qwerty123456";
            var user = CreateFakeUser();
            var userAccess = new Dictionary<string, bool>()
            {
                { "CanViewUserFullProfile", false }
            };
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };
            var currentUser = new User();
            var focusUserViewModel = new UserShortViewModel();

            _userService.Setup(us => us.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(currentUser);
            _userAccessService.Setup(ua =>
                    ua.GetUserProfileAccessAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(userAccess);
            _mapper.Setup(m => m.Map<UserDto, UserShortViewModel>(It.IsAny<UserDto>()))
                .Returns(focusUserViewModel);

            //Act
            var result = await _userController.GetUserProfile(focusUserId);
            var actual = (result as ObjectResult).Value as PersonalDataViewModel;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(actual);
            Assert.IsNull(actual.User);
            Assert.IsNotNull(actual.ShortUser);
        }

        [Test]
        public async Task GetUserProfile_FocusUserIsNotFound_ReturnsNotFound()
        {
            //Arrange
            string focusUserId = "qwerty123456";
            UserDto user = null;
            _userService.Setup(us => us.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            //Act
            var result = await _userController.GetUserProfile(focusUserId);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetImage_ReturnsOkObjectResult()
        {
            // Assert
            _userService
                .Setup((x) => x.GetImageBase64Async(It.IsAny<string>()))
                .ReturnsAsync("");

            // Act
            var result = await _userController.GetImage(It.IsAny<string>());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull((result as OkObjectResult).Value);
        }

        [Test]
        public async Task Edit_NullUserIdString_ReturnsNotFoundResult()
        {
            // Arrange
            string nullString = null;

            // Act
            var result = await _userController.Edit(nullString);

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_User_ReturnsOkObjectResult()
        {
            // Arrange
            string currentUserId = "1";
            string focusUserId = "1";
            var userAccess = new Dictionary<string, bool>()
            {
                { "CanEditUserProfile", true }
            };

            _userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());


            _userService
                .Setup((x) => x.GetUserAsync(currentUserId))
                .ReturnsAsync(CreateFakeUser());

            _userAccessService.Setup(ua =>
                    ua.GetUserProfileAccessAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(userAccess);

            _mapper
                .Setup((x) => x.Map<UserDto, UserViewModel>(It.IsAny<UserDto>()))
                .Returns(CreateFakeUserViewModel());

            // Act
            var result = await _userController.Edit(focusUserId);

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Edit_User_ReturnsCreatedModel()
        {
            // Arrange
            var idString = "1";
            var idInt = 1;
            var userAccess = new Dictionary<string, bool>()
            {
                { "CanEditUserProfile", true }
            };

            _userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            _userService
                .Setup((x) => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(CreateFakeUser());
            

            _userPersonalDataService
                .Setup((x) => x.GetAllGendersAsync())
                .ReturnsAsync(new List<GenderDto>());

            _userAccessService.Setup(ua =>
                   ua.GetUserProfileAccessAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
               .ReturnsAsync(userAccess);

            _mapper
                .Setup((x) => x.Map<IEnumerable<GenderDto>, IEnumerable<GenderViewModel>>(new List<GenderDto>()))
                .Returns(new List<GenderViewModel>());

            _userPersonalDataService
               .Setup((x) => x.GetAllEducationsGroupByPlaceAsync())
                .ReturnsAsync(new List<EducationDto>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<EducationDto>, IEnumerable<EducationViewModel>>(new List<EducationDto>()))
                .Returns(new List<EducationViewModel>());

            _userPersonalDataService
               .Setup((x) => x.GetAllEducationsGroupBySpecialityAsync())
                .ReturnsAsync(new List<EducationDto>());

            _userPersonalDataService
               .Setup((x) => x.GetAllWorkGroupByPlaceAsync())
                .ReturnsAsync(new List<WorkDto>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<WorkDto>, IEnumerable<WorkViewModel>>(new List<WorkDto>()))
                .Returns(new List<WorkViewModel>());

            _userPersonalDataService
               .Setup((x) => x.GetAllWorkGroupByPositionAsync())
               .ReturnsAsync(new List<WorkDto>());

            _mapper
                .Setup((x) => x.Map<UserDto, UserViewModel>(It.IsAny<UserDto>()))
                .Returns(CreateFakeUserViewModel());

            _userPersonalDataService
                .Setup((x) => x.GetAllNationalityAsync())
                .ReturnsAsync(new List<NationalityDto>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<NationalityDto>, IEnumerable<NationalityViewModel>>(It.IsAny<List<NationalityDto>>()))
                .Returns(new List<NationalityViewModel>());

            _userPersonalDataService
                .Setup((x) => x.GetAllReligionsAsync())
                .ReturnsAsync(new List<ReligionDto>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<ReligionDto>, IEnumerable<ReligionViewModel>>(It.IsAny<List<ReligionDto>>()))
                .Returns(new List<ReligionViewModel>());

            _userPersonalDataService
                .Setup((x) => x.GetAllDegreesAsync())
                .ReturnsAsync(new List<DegreeDto>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<DegreeDto>, IEnumerable<DegreeViewModel>>(It.IsAny<List<DegreeDto>>()))
                .Returns(new List<DegreeViewModel>());

            var expectedUserId = idString;
            var expectedId = idInt;

            // Act
            var result = await _userController.Edit(idString);

            var actual = (result as ObjectResult).Value as EditUserViewModel;

            // Assert
            Assert.NotNull(actual);
            Assert.AreEqual(expectedUserId, actual.User.ID);
            Assert.AreEqual(expectedId, actual.EducationView.PlaceOfStudyID);
            Assert.AreEqual(expectedId, actual.EducationView.SpecialityID);
            Assert.AreEqual(expectedId, actual.WorkView.PlaceOfWorkID);
            Assert.AreEqual(expectedId, actual.WorkView.PositionID);
        }

        [Test]
        public async Task Edit_NullUser_ReturnsNotFoundResult()
        {
            // Arrange
            string id = null;
            _userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _userService
                .Setup((x) => x.GetUserAsync(id))
                .ReturnsAsync(It.IsAny<UserDto>);

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDto>(), It.IsAny<string[]>()))
                .ReturnsAsync(true);

            // Act
            var result = await _userController.Edit(id);

            // Assert
            _userService.Verify();
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_Forbidden_Returns403Forbidden()
        {
            // Arrange
            string focusUserId = "2";
            var user = CreateFakeUser();
            var userAccess = new Dictionary<string, bool>()
            {
                { "CanEditUserProfile", false }
            };
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };

             _userManager
                .Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _userAccessService
                .Setup(ua =>
                    ua.GetUserProfileAccessAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(userAccess);

            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _userController.Edit(focusUserId);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _userService.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditProfilePhoto_ReturnsOkResult()
        {
            // Arrange
            string focusUserId = "2";
            var user = CreateFakeUser();
            var userAccess = new Dictionary<string, bool>()
            {
                { "CanEditDeleteUserPhoto", true }
            };
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };

            _userManager
                .Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _userAccessService
                .Setup(ua =>
                    ua.GetUserProfileAccessAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(userAccess);
            _userManagerService
                .Setup(m => m.FindByIdAsync(focusUserId))
                .ReturnsAsync(new UserDto());
            // Act
            var result = await _userController.EditProfilePhotoAsync(focusUserId, It.IsAny<string>());

            // Assert
            _userService.Verify();
            _mapper.Verify();
            _loggerService.Verify();
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EditProfilePhoto_Returns403Forbidden()
        {
            // Arrange
            var user = CreateFakeUser();
            var userAccess = new Dictionary<string, bool>()
            {
                { "CanEditDeleteUserPhoto", false }
            };
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };

            _userManagerService
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserDto());
            _userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _userAccessService.Setup(ua =>
                    ua.GetUserProfileAccessAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(userAccess);
            var expected = StatusCodes.Status403Forbidden;
            // Act
            var result = await _userController.EditProfilePhotoAsync(It.IsAny<string>(), It.IsAny<string>());
            var actual = ((StatusCodeResult) result).StatusCode;
            // Assert
            _userService.Verify();
            _mapper.Verify();
            _loggerService.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditProfilePhoto_ReturnsBadRequest()
        {
            // Arrange
            var id = "1";
            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            _userService.Setup(u => u.UpdatePhotoAsyncForBase64(It.IsAny<UserDto>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());
            // Act
            var result = await _userController.EditProfilePhotoAsync(id, It.IsAny<string>());
            // Assert
            _userService.Verify();
            _mapper.Verify();
            _loggerService.Verify();
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task EditBase64_ReturnsOkResult()
        {
            // Arrange
            var user = CreateFakeUser();
            var userAccess = new Dictionary<string, bool>()
            {
                { "CanEditUserProfile", true }
            };
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };
            _userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _userAccessService.Setup(ua =>
                    ua.GetUserProfileAccessAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(userAccess);
            _mapper
                .Setup((x) => x.Map<UserViewModel, UserDto>(It.IsAny<UserViewModel>()))
                .Returns(CreateFakeUser());
            _userService
                .Setup((x) => x.UpdateAsyncForBase64(It.IsAny<UserDto>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()));

            // Act
            var result = await _userController.EditBase64(CreateFakeEditUserViewModel());

            // Assert
            _userService.Verify();
            _mapper.Verify();
            _loggerService.Verify((x) => x.LogInformation(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EditBase64_Returns403Forbidden()
        {
            // Arrange
            var expected = StatusCodes.Status403Forbidden;

            var user = CreateFakeUser();
            var userAccess = new Dictionary<string, bool>()
            {
                { "CanEditUserProfile", false }
            };
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };
            _userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _userAccessService.Setup(ua =>
                    ua.GetUserProfileAccessAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(userAccess);
            _userService
                .Setup((x) => x.UpdateAsyncForBase64(It.IsAny<UserDto>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()));

            // Act
            var result = await  _userController.EditBase64(CreateFakeEditUserViewModel());
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _userService.Verify();
            _mapper.Verify();
            _loggerService.Verify((x) => x.LogInformation(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditBase64_ReturnsBadRequest()
        {
            // Arrange
            var id = "1";
            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            _mapper
                .Setup((x) => x.Map<UserViewModel, UserDto>(It.IsAny<UserViewModel>()))
                .Returns(CreateFakeUser());
            _mapper
                .Setup((x) => x.Map<UserViewModel, UserDto>(It.IsAny<UserViewModel>()))
                .Returns(CreateFakeUser());

            _userManager.Setup(x => x.IsInRoleAsync(It.IsAny<User>(), Roles.Admin)).ReturnsAsync(true);
            _userService
                .Setup((x) => x.UpdateAsyncForBase64(It.IsAny<UserDto>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ThrowsAsync(new ArgumentException("Wrong arguments!"));

            // Act
            var result = await _userController.EditBase64(CreateFakeEditUserViewModel());

            // Assert
            _userService.Verify();
            _mapper.Verify();
            _loggerService.Verify((x) => x.LogInformation(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Approvers_NullUserIdString_ReturnsNotFoundResult()
        {
            // Act
            var result = await _userController.Approvers(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Approvers_User_ReturnsOkObjectResult()
        {
            // Arrange
            var idString = "1";

            _userService
                .Setup((x) => x.GetUserAsync(idString))
                .ReturnsAsync(CreateFakeUser());

            _mapper
                .Setup((x) => x.Map<UserDto, UserInfoViewModel>(It.IsAny<UserDto>()))
                .Returns(new UserInfoViewModel());

            // Act
            var result = await _userController.Approvers(idString, idString);

            var actual = (result as ObjectResult).Value as UserApproversViewModel;

            // Assert
            Assert.NotNull(actual);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull((result as OkObjectResult).Value);
        }

        [Test]
        public async Task Approvers_User_ReturnsCreatedModel()
        {
            // Arrange
            var idString = "1";
            var idInt = 1;
            var canApprove = true;
            var isUserHead = false;
            var time = new TimeSpan(1, 1, 1);
            var timeInDays = 0;
            var listCount = 2;
            var imageString = "SomeImgInBase64";

            _userManagerService.Setup(x => x.GetRolesAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(new List<string> {Roles.KurinHead});

            _userService
                .Setup((x) => x.GetUserAsync(idString))
                .ReturnsAsync(CreateFakeUser());

            _userService
                .Setup((x) => x.GetConfirmedUsers(It.IsAny<UserDto>()))
                .Returns(new List<ConfirmedUserDto>());

            _userService
                .Setup((x) => x.CanApprove(It.IsAny<List<ConfirmedUserDto>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(canApprove);

            _userService
               .Setup((x) => x.CheckOrAddPlastunRole(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(time);

            _userService
                .Setup((x) => x.GetClubAdminConfirmedUser(It.IsAny<UserDto>()))
                .Returns(It.IsAny<ConfirmedUserDto>());

            _userService
                .Setup((x) => x.GetCityAdminConfirmedUser(It.IsAny<UserDto>()))
                .Returns(It.IsAny<ConfirmedUserDto>());

            _mapper
                .Setup((x) => x.Map<UserDto, UserInfoViewModel>(It.IsAny<UserDto>()))
                .Returns(CreateFaceUserInfoViewModel());

            _mapper
                .Setup((x) => x.Map<IEnumerable<ConfirmedUserDto>, IEnumerable<ConfirmedUserViewModel>>(It.IsAny<List<ConfirmedUserDto>>()))
                .Returns(CreateListOfConfirmedUserViewModels());

            _mapper
                .Setup((x) => x.Map<ConfirmedUserDto, ConfirmedUserViewModel>(It.IsAny<ConfirmedUserDto>()))
                .Returns(CreateConfirmedUserViewModel());

            _mapper
                .Setup((x) => x.Map<ConfirmedUserDto, ConfirmedUserViewModel>(It.IsAny<ConfirmedUserDto>()))
                .Returns(CreateConfirmedUserViewModel());

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDto>(), It.IsAny<string>()))
                .ReturnsAsync(isUserHead);

            _userService
                .Setup((x) => x.GetImageBase64Async(It.IsAny<string>()))
                .ReturnsAsync(imageString);

            var expectedUserId = idString;
            var expectedApproverId = idString;
            var expectedTimeToJoinPlast = timeInDays;
            var expectedCanApprove = canApprove;
            var expectedIsUserHead = isUserHead;
            var expectedListCount = listCount;
            var expectedId = idInt;

            // Act
            var result = await _userController.Approvers(idString, idString);

            var actual = (result as ObjectResult).Value as UserApproversViewModel;

            // Assert
            Assert.NotNull(actual);
            Assert.AreEqual(expectedUserId, actual.User.Id);
            Assert.IsTrue(actual.CanApprove);
            Assert.AreEqual(expectedTimeToJoinPlast, actual.TimeToJoinPlast);
            Assert.AreEqual(expectedListCount, (actual.ConfirmedUsers as List<ConfirmedUserViewModel>).Count);
            Assert.AreEqual(expectedId, actual.ClubApprover.ID);
            Assert.AreEqual(expectedId, actual.CityApprover.ID);
            Assert.AreEqual(expectedCanApprove, actual.CanApprove);
            Assert.AreEqual(expectedIsUserHead, actual.IsUserHeadOfCity);
            Assert.IsFalse(actual.IsUserHeadOfCity);
            Assert.IsFalse(actual.IsUserHeadOfClub);
            Assert.IsFalse(actual.IsUserHeadOfRegion);
            Assert.IsTrue(actual.IsUserPlastun);
            Assert.AreEqual(expectedApproverId, actual.CurrentUserId);
        }

        [Test]
        public async Task Approvers_User_ThrowsException()
        {
            // Arrange
            string userId = "1";
            string approverId = "2";
            _userService
                .Setup((x) => x.GetUserAsync(It.IsAny<string>()))
                .Throws(new Exception());

            // Act
            var result = await _userController.Approvers(userId, approverId);

            // Assert
            _userService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task ApproveUser_ForbiddenString_FormerPlastMember_ReturnsStatus403Forbidden()
        {
            // Arrange
            var idString = "1";

            _userManagerService.Setup(x => x.GetRolesAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(new List<string> { Roles.FormerPlastMember });

            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _userController.ApproveUser(idString, ApproveType.PlastMember);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _userManagerService.Verify(x => x.GetRolesAsync(It.IsAny<UserDto>()));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task ApproveUser_ForbiddenString_RegisteredUser_ReturnsStatus403Forbidden()
        {
            // Arrange
            var idString = "1";

            _userManagerService.Setup(x => x.GetRolesAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(new List<string> { Roles.RegisteredUser });

            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _userController.ApproveUser(idString, ApproveType.PlastMember);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _userManagerService.Verify(x => x.GetRolesAsync(It.IsAny<UserDto>()));
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public async Task ApproveUser_NullUserIdString_ReturnsNotFoundResult()
        {
            // Act
            var result = await _userController.ApproveUser(It.IsAny<string>(), It.IsAny<ApproveType>());

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task ApproveUser_ReturnsOkResult()
        {
            // Arrange
            var idString = "1";

            _confirmedUserService
                .Setup((x) => x.CreateAsync(It.IsAny<User>(), idString, It.IsAny<ApproveType>()));
            _userManagerService.Setup(x => x.GetRolesAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(new List<string> {Roles.KurinHead});

            // Act
            var result = await _userController.ApproveUser(idString, It.IsAny<ApproveType>());

            // Assert
            _confirmedUserService.Verify();
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task ApproverDelete_NullUserIdString_ReturnsNotFoundResult()
        {
            // Act
            var result = await _userController.ApproverDelete(It.IsAny<int>());

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task ApproverDelete_ReturnsOkResult()
        {
            // Arrange
            var confirmedId = 1;

            _confirmedUserService
                .Setup((x) => x.DeleteAsync(It.IsAny<User>(), It.IsAny<int>()));

            // Act
            var result = await _userController.ApproverDelete(confirmedId);

            // Assert
            _confirmedUserService.Verify();
            _loggerService.Verify((x) => x.LogInformation(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<OkResult>(result);
        }

        private UserDto CreateFakeUser()
            => new UserDto()
            {
                Id = "1",
                FirstName = "SomeFirstName",
                LastName = "SomeLastName",
                UserProfile = new UserProfileDto()
                {
                    EducationId = 1,
                    WorkId = 1,
                },
            };
        private UserDto CreateFakeUserWithCity()
            => new UserDto()
            {
                Id = "1",
                FirstName = "SomeFirstName",
                LastName = "SomeLastName",
                CityMembers = new List<CityMembers>
                {
                    new CityMembers
                    {
                        CityId = 1
                    }
                },
                ClubMembers = new List<ClubMembers>(),
                RegionAdministrations = new List<RegionAdministration>(),
                UserProfile = new UserProfileDto()
                {
                    EducationId = 1,
                    WorkId = 1,
                },
            };
        private UserDto CreateFakeUserWithoutCity(string userId)
            => new UserDto()
            {
                Id = userId,
                FirstName = "SomeFirstName",
                LastName = "SomeLastName",
                CityMembers = new List<CityMembers>(),
                ClubMembers = new List<ClubMembers>(),
                RegionAdministrations = new List<RegionAdministration>(),
                UserProfile = new UserProfileDto()
                {
                    EducationId = 1,
                    WorkId = 1,
                },
            };
        private UserViewModel CreateFakeUserViewModel()
            => new UserViewModel()
            {
                ID = "1",
            };
        private UserShortViewModel CreateFakeUserShortViewModel()
            => new UserShortViewModel()
            {
                ID = "1",
            };

        private EditUserViewModel CreateFakeEditUserViewModel()
            => new EditUserViewModel()
            {
                User = new UserViewModel()
                {
                    ID = "1",
                },

                ImageBase64 = "SomeImgInBase64",
                
                EducationView = new UserEducationViewModel()
                {
                    PlaceOfStudyID = 1,
                    SpecialityID = 1,
                },
                
                WorkView = new UserWorkViewModel()
                {
                    PlaceOfWorkID = 1,
                    PositionID = 1,
                },
            };

        private UserInfoViewModel CreateFaceUserInfoViewModel()
            => new UserInfoViewModel()
            {
                Id = "1",
            };

        private List<ConfirmedUserViewModel> CreateListOfConfirmedUserViewModels()
            => new List<ConfirmedUserViewModel>()
            {
                new ConfirmedUserViewModel()
                {
                    ID = 1,
                    Approver = new ApproverViewModel()
                    {
                        User = new UserInfoViewModel()
                        {
                            ImagePath = "SomeStringID2",
                        },
                    },
                },
                new ConfirmedUserViewModel()
                {
                    ID = 2,
                    Approver = new ApproverViewModel()
                    {
                        User = new UserInfoViewModel()
                        {
                            ImagePath = "SomeStringID1",
                        },
                    },
                },
            };

        private ConfirmedUserViewModel CreateConfirmedUserViewModel()
            => new ConfirmedUserViewModel()
            {
                ID = 1,
                Approver = new ApproverViewModel()
                {
                    User = new UserInfoViewModel()
                    {
                        ImagePath = "SomeImagePath",
                    },
                },
            };
    }
}
