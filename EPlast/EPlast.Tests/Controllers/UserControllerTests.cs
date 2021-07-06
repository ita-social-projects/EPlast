using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.WebApi.Models.UserModels;
using EPlast.WebApi.Models.UserModels.UserProfileFields;
using EPlast.BLL.DTO;
using System.Security.Claims;
using EPlast.WebApi.Models.User;
using EPlast.WebApi.Models.Approver;
using Microsoft.AspNetCore.Identity;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Http;

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

            _userController = new UserController(
                _userService.Object,
                _userPersonalDataService.Object,
                _confirmedUserService.Object,
                _userManagerService.Object,
                _loggerService.Object,
                _mapper.Object,
                _userManager.Object);
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
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
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
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
                .ReturnsAsync(isPlastun);


            _mapper
                .Setup((x) => x.Map<UserDTO, UserViewModel>(It.IsAny<UserDTO>()))
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
                .ReturnsAsync(It.IsAny<UserDTO>);

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
            var currentUser = new UserDTO() {Id = "2"};

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
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
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
        public async Task GetUserProfile_FocusUserIdIsNull_ReturnsNotFoundResult()
        {
            // Arrange
            string focusUserId = "";

            // Act
            var result = await _userController.GetUserProfile(It.IsAny<string>(), focusUserId);
            var resultObject = (result as ObjectResult)?.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(resultObject);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetUserProfile_NullCurrentUserIdString_ReturnsNotFoundResult()
        {
            // Arrange
            string nullString = null;
            string focusUserId = "1";

            // Act
            var result = await _userController.GetUserProfile(nullString, focusUserId);

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetUserProfile_ShortUser_ReturnsOkObjectResult()
        {
            // Arrange
            string currentUserId = "1";
            string focusUserId = "2";
            var currentUser = CreateFakeUserWithoutCity(currentUserId);
            var focusUser = CreateFakeUserWithoutCity(focusUserId);


            _userService
                .Setup((x) => x.GetUserAsync(currentUserId))
                .ReturnsAsync(currentUser);

            _userService
                .Setup((x) => x.GetUserAsync(focusUserId))
                .ReturnsAsync(focusUser);

            _userService
                .Setup((x) => x.CheckOrAddPlastunRole(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(It.IsAny<TimeSpan>());

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<bool>());

            _userManagerService.Setup((x) => x.IsInRoleAsync(currentUser, Roles.PlastMember)).ReturnsAsync(true);

            // Act
            var result = await _userController.GetUserProfile(currentUserId, focusUserId);

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserProfile_ShortUser_ReturnsCreatedModel()
        {
            // Arrange
            string currentUserId = "2";
            string focusUserId = "1";
            var isPlastun = false;
            var time = new TimeSpan(1, 1, 1);
            var timeInDays = 0;

            var currentUser = CreateFakeUserWithoutCity(currentUserId);
            var focusUser = CreateFakeUserWithoutCity(focusUserId);


            _userService
                .Setup((x) => x.GetUserAsync(currentUserId))
                .ReturnsAsync(currentUser);

            _userService
                .Setup((x) => x.GetUserAsync(focusUserId))
                .ReturnsAsync(focusUser);

            _userService
                .Setup((x) => x.CheckOrAddPlastunRole(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(time);

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
                .ReturnsAsync(isPlastun);

            _userManagerService.Setup((x) => x.IsInRoleAsync(currentUser, Roles.PlastMember)).ReturnsAsync(true);

            _mapper
                .Setup((x) => x.Map<UserDTO, UserShortViewModel>(It.IsAny<UserDTO>()))
                .Returns(CreateFakeUserShortViewModel());

            var expectedUserId = focusUserId;
            var expectedTimeToJoinPlast = timeInDays;
            var expectedIsUserPlastun = !isPlastun;

            // Act
            var result = await _userController.GetUserProfile(currentUserId, focusUserId);

            var actual = (result as ObjectResult).Value as PersonalDataViewModel;

            // Assert
            Assert.NotNull(actual);
            Assert.AreEqual(expectedUserId, actual.ShortUser.ID);
            Assert.AreEqual(expectedTimeToJoinPlast, actual.TimeToJoinPlast);
            Assert.AreEqual(expectedIsUserPlastun, actual.IsUserPlastun);
        }

        [Test]
        public async Task GetUserProfile_NullUser_ReturnsNotFoundResult()
        {
            // Arrange
            string currentUserId = "1";
            string focusUserId = "1";

            _userService
                .Setup((x) => x.GetUserAsync(focusUserId))
                .ReturnsAsync(It.IsAny<UserDTO>);

            // Act
            var result = await _userController.GetUserProfile(currentUserId, focusUserId);

            // Assert
            _userService.Verify();
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetUserProfile_UserIsAdmin_ReturnsOkObjectResult()
        {
            // Arrange
            string currentUserId = "1";
            string focusUserId = "1";
            bool isAdmin = true;

            _userService
                .Setup((x) => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(CreateFakeUserWithoutCity(focusUserId));

            _userService
                .Setup((x) => x.CheckOrAddPlastunRole(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(It.IsAny<TimeSpan>());

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
                .ReturnsAsync(isAdmin);

            // Act
            var result = await _userController.GetUserProfile(currentUserId, focusUserId);

            // Assert
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserProfile_Forbidden_Returns403Forbidden()
        {
            // Arrange
            string currentUserId = "2";
            string focusUserId = "1";
            bool isAdmin = true;

            _userService
                .Setup((x) => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(CreateFakeUserWithoutCity(focusUserId));

            _userService
                .Setup((x) => x.CheckOrAddPlastunRole(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(It.IsAny<TimeSpan>());

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
                .ReturnsAsync(isAdmin);

            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _userController.GetUserProfile(currentUserId, focusUserId);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _userService.Verify();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUserProfile_User_ReturnsCreatedModel()
        {
            // Arrange
            string currentUserId = "1";
            string focusUserId = "1";
            var isPlastun = true;
            var time = new TimeSpan(1, 1, 1);
            var timeInDays = 0;

            _userService
                .Setup((x) => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(CreateFakeUserWithoutCity(focusUserId));

            _userService
                .Setup((x) => x.CheckOrAddPlastunRole(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(time);

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
                .ReturnsAsync(isPlastun);

            _mapper
                .Setup((x) => x.Map<UserDTO, UserViewModel>(It.IsAny<UserDTO>()))
                .Returns(CreateFakeUserViewModel());

            var expectedUserId = focusUserId;
            var expectedTimeToJoinPlast = timeInDays;
            var expectedIsUserPlastun = isPlastun;

            // Act
            var result = await _userController.GetUserProfile(currentUserId, focusUserId);

            var actual = (result as ObjectResult).Value as PersonalDataViewModel;

            // Assert
            Assert.NotNull(actual);
            Assert.AreEqual(expectedUserId, actual.User.ID);
            Assert.AreEqual(expectedTimeToJoinPlast, actual.TimeToJoinPlast);
            Assert.AreEqual(expectedIsUserPlastun, actual.IsUserPlastun);
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

            _userService
                .Setup((x) => x.GetUserAsync(currentUserId))
                .ReturnsAsync(CreateFakeUser());

            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(currentUserId);

            _mapper
                .Setup((x) => x.Map<UserDTO, UserViewModel>(It.IsAny<UserDTO>()))
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
            string currentUserId = "1";

            _userService
                .Setup((x) => x.GetUserAsync(currentUserId))
                .ReturnsAsync(CreateFakeUser());

            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(currentUserId);
            

            _userPersonalDataService
                .Setup((x) => x.GetAllGendersAsync())
                .ReturnsAsync(new List<GenderDTO>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<GenderDTO>, IEnumerable<GenderViewModel>>(new List<GenderDTO>()))
                .Returns(new List<GenderViewModel>());

            _userPersonalDataService
               .Setup((x) => x.GetAllEducationsGroupByPlaceAsync())
                .ReturnsAsync(new List<EducationDTO>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<EducationDTO>, IEnumerable<EducationViewModel>>(new List<EducationDTO>()))
                .Returns(new List<EducationViewModel>());

            _userPersonalDataService
               .Setup((x) => x.GetAllEducationsGroupBySpecialityAsync())
                .ReturnsAsync(new List<EducationDTO>());

            _userPersonalDataService
               .Setup((x) => x.GetAllWorkGroupByPlaceAsync())
                .ReturnsAsync(new List<WorkDTO>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<WorkDTO>, IEnumerable<WorkViewModel>>(new List<WorkDTO>()))
                .Returns(new List<WorkViewModel>());

            _userPersonalDataService
               .Setup((x) => x.GetAllWorkGroupByPositionAsync())
               .ReturnsAsync(new List<WorkDTO>());

            _mapper
                .Setup((x) => x.Map<UserDTO, UserViewModel>(It.IsAny<UserDTO>()))
                .Returns(CreateFakeUserViewModel());

            _userPersonalDataService
                .Setup((x) => x.GetAllNationalityAsync())
                .ReturnsAsync(new List<NationalityDTO>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<NationalityDTO>, IEnumerable<NationalityViewModel>>(It.IsAny<List<NationalityDTO>>()))
                .Returns(new List<NationalityViewModel>());

            _userPersonalDataService
                .Setup((x) => x.GetAllReligionsAsync())
                .ReturnsAsync(new List<ReligionDTO>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<ReligionDTO>, IEnumerable<ReligionViewModel>>(It.IsAny<List<ReligionDTO>>()))
                .Returns(new List<ReligionViewModel>());

            _userPersonalDataService
                .Setup((x) => x.GetAllDegreesAsync())
                .ReturnsAsync(new List<DegreeDTO>());

            _mapper
                .Setup((x) => x.Map<IEnumerable<DegreeDTO>, IEnumerable<DegreeViewModel>>(It.IsAny<List<DegreeDTO>>()))
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
            string id = "1";

            _userService
                .Setup((x) => x.GetUserAsync(id))
                .ReturnsAsync(It.IsAny<UserDTO>);

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
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

            _userService
                .Setup((x) => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(CreateFakeUserWithoutCity(focusUserId));

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
                .ReturnsAsync(false);

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
        public async Task EditBase64_ReturnsOkResult()
        {
            // Assert
            _mapper
                .Setup((x) => x.Map<UserViewModel, UserDTO>(It.IsAny<UserViewModel>()))
                .Returns(CreateFakeUser());

            _userService
                .Setup((x) => x.UpdateAsyncForBase64(It.IsAny<UserDTO>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()));

            // Act
            var result = await _userController.EditBase64(CreateFakeEditUserViewModel());

            // Assert
            _userService.Verify();
            _mapper.Verify();
            _loggerService.Verify((x) => x.LogInformation(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<OkResult>(result);
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
                .Setup((x) => x.Map<UserDTO, UserInfoViewModel>(It.IsAny<UserDTO>()))
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

            _userManagerService.Setup(x => x.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(new List<string> {Roles.KurinHead});

            _userService
                .Setup((x) => x.GetUserAsync(idString))
                .ReturnsAsync(CreateFakeUser());

            _userService
                .Setup((x) => x.GetConfirmedUsers(It.IsAny<UserDTO>()))
                .Returns(new List<ConfirmedUserDTO>());

            _userService
                .Setup((x) => x.CanApprove(It.IsAny<List<ConfirmedUserDTO>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(canApprove);

            _userService
               .Setup((x) => x.CheckOrAddPlastunRole(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(time);

            _userService
                .Setup((x) => x.GetClubAdminConfirmedUser(It.IsAny<UserDTO>()))
                .Returns(It.IsAny<ConfirmedUserDTO>());

            _userService
                .Setup((x) => x.GetCityAdminConfirmedUser(It.IsAny<UserDTO>()))
                .Returns(It.IsAny<ConfirmedUserDTO>());

            _mapper
                .Setup((x) => x.Map<UserDTO, UserInfoViewModel>(It.IsAny<UserDTO>()))
                .Returns(CreateFaceUserInfoViewModel());

            _mapper
                .Setup((x) => x.Map<IEnumerable<ConfirmedUserDTO>, IEnumerable<ConfirmedUserViewModel>>(It.IsAny<List<ConfirmedUserDTO>>()))
                .Returns(CreateListOfConfirmedUserViewModels());

            _mapper
                .Setup((x) => x.Map<ConfirmedUserDTO, ConfirmedUserViewModel>(It.IsAny<ConfirmedUserDTO>()))
                .Returns(CreateConfirmedUserViewModel());

            _mapper
                .Setup((x) => x.Map<ConfirmedUserDTO, ConfirmedUserViewModel>(It.IsAny<ConfirmedUserDTO>()))
                .Returns(CreateConfirmedUserViewModel());

            _userManagerService
                .Setup((x) => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>()))
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
            // Assert
            var idString = "1";
            
            _userManagerService.Setup(x => x.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(new List<string> { Roles.FormerPlastMember });

            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _userController.ApproveUser(idString);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _userManagerService.Verify(x => x.GetRolesAsync(It.IsAny<UserDTO>()));
            Assert.AreEqual(expected, actual);
            
        }

        [Test]
        public async Task ApproveUser_ForbiddenString_RegisteredUser_ReturnsStatus403Forbidden()
        {
            // Assert
            var idString = "1";

            _userManagerService.Setup(x => x.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(new List<string> { Roles.RegisteredUser });

            var expected = StatusCodes.Status403Forbidden;

            // Act
            var result = await _userController.ApproveUser(idString);
            var actual = (result as StatusCodeResult).StatusCode;

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            _userManagerService.Verify(x => x.GetRolesAsync(It.IsAny<UserDTO>()));
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public async Task ApproveUser_NullUserIdString_ReturnsNotFoundResult()
        {
            // Act
            var result = await _userController.ApproveUser(It.IsAny<string>());

            // Assert
            _loggerService.Verify((x) => x.LogError(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task ApproveUser_ReturnsOkResult()
        {
            // Assert
            var idString = "1";

            _confirmedUserService
                .Setup((x) => x.CreateAsync(It.IsAny<User>(), idString, It.IsAny<bool>(), It.IsAny<bool>()));
            _userManagerService.Setup(x => x.GetRolesAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(new List<string> {Roles.KurinHead});

            // Act
            var result = await _userController.ApproveUser(idString, It.IsAny<bool>(), It.IsAny<bool>());

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
            // Assert
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

        private UserDTO CreateFakeUser()
            => new UserDTO()
            {
                Id = "1",
                FirstName = "SomeFirstName",
                LastName = "SomeLastName",
                UserProfile = new UserProfileDTO()
                {
                    EducationId = 1,
                    WorkId = 1,
                },
            };
        private UserDTO CreateFakeUserWithCity()
            => new UserDTO()
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
                UserProfile = new UserProfileDTO()
                {
                    EducationId = 1,
                    WorkId = 1,
                },
            };
        private UserDTO CreateFakeUserWithoutCity(string userId)
            => new UserDTO()
            {
                Id = userId,
                FirstName = "SomeFirstName",
                LastName = "SomeLastName",
                CityMembers = new List<CityMembers>(),
                ClubMembers = new List<ClubMembers>(),
                RegionAdministrations = new List<RegionAdministration>(),
                UserProfile = new UserProfileDTO()
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
