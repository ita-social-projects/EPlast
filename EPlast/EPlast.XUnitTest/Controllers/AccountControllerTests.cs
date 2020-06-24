using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using EPlast.BusinessLogicLayer.Interfaces;
using EPlast.BusinessLogicLayer.Interfaces.UserProfiles;
using EPlast.BusinessLogicLayer.Services.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using EPlast.ViewModels.UserInformation;
using EPlast.ViewModels.UserInformation.UserProfile;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BusinessLogicLayer.Interfaces.Logging;
using Xunit;

namespace EPlast.XUnitTest
{
    public class AccountControllerTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IUserStore<User>> _userStoreMock;
        private Mock<IHttpContextAccessor> _contextAccessor;
        private Mock<IUserClaimsPrincipalFactory<User>> _userPrincipalFactory;
        private Mock<UserManager<User>> _userManager;
        private Mock<SignInManager<User>> _signInManager;
        private Mock<ILogger<AccountController>> _logger;
        private Mock<IEmailConfirmation> _emailConfirm;
        private Mock<IWebHostEnvironment> _hostEnv;
        private Mock<IUserService> _userService;
        private Mock<INationalityService> _nationalityService;
        private Mock<IEducationService> _educationService;
        private Mock<IReligionService> _religionService;
        private Mock<IWorkService> _workService;
        private Mock<IGenderService> _genderService;
        private Mock<IDegreeService> _degreeService;
        private Mock<IUserManagerService> _userManagerService;
        private Mock<IConfirmedUsersService> _confirmedUserService;
        private Mock<IMapper> _mapper;
        private Mock<ILoggerService<AccountController>> _loggerService;
        private Mock<IAccountService> _accountService;

        public AccountControllerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userStoreMock = new Mock<IUserStore<User>>();
            _contextAccessor = new Mock<IHttpContextAccessor>();
            _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _userManager = new Mock<UserManager<User>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _signInManager = new Mock<SignInManager<User>>(_userManager.Object, _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null);
            _logger = new Mock<ILogger<AccountController>>();
            _emailConfirm = new Mock<IEmailConfirmation>();
            _hostEnv = new Mock<IWebHostEnvironment>();
            _userService = new Mock<IUserService>();
            _nationalityService = new Mock<INationalityService>();
            _educationService = new Mock<IEducationService>();
            _religionService = new Mock<IReligionService>();
            _workService = new Mock<IWorkService>();
            _genderService = new Mock<IGenderService>();
            _degreeService = new Mock<IDegreeService>();
            _userManagerService = new Mock<IUserManagerService>();
            _confirmedUserService = new Mock<IConfirmedUsersService>();
            _mapper = new Mock<IMapper>();
            _loggerService = new Mock<ILoggerService<AccountController>>();
            _accountService = new Mock<IAccountService>();
        }
        [Fact]
        public async Task UserProfileTest()
        {
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(new UserDTO
            {
                FirstName = "Vova",
                LastName = "Vermii",
                UserProfile = new UserProfileDTO
                {
                    Nationality = new NationalityDTO { Name = "Українець" },
                    Religion = new ReligionDTO { Name = "Християнство" },
                    Education = new EducationDTO() { PlaceOfStudy = "ЛНУ", Speciality = "КН" },
                    Degree = new DegreeDTO { Name = "Бакалавр" },
                    Work = new WorkDTO { PlaceOfwork = "SoftServe", Position = "ProjectManager" },
                    Gender = new GenderDTO { Name = "Чоловік" }
                }
            });
            _userService.Setup(x => x.CheckOrAddPlastunRoleAsync(It.IsAny<string>(), DateTime.Now)).ReturnsAsync(TimeSpan.Zero);

            _userManagerService.Setup(x => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>())).ReturnsAsync(true);
            UserViewModel a = new UserViewModel { Id = "1" };
            _mapper.Setup(x => x.Map<UserDTO, UserViewModel>(It.IsAny<UserDTO>())).Returns(a);
            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            // Act
            var result = await controller.UserProfile("1");
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PersonalDataViewModel>(viewResult.Model);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UserProfileTestFailure()
        {
            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            // Act
            var result = await controller.UserProfile("");
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.NotNull(result);

        }

        [Fact]
        public async Task ApproversTest()
        {
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(new UserDTO());
            _userService.Setup(x => x.GetConfirmedUsers(It.IsAny<UserDTO>())).Returns(new List<ConfirmedUserDTO>());
            _userService.Setup(x => x.GetCityAdminConfirmedUser(It.IsAny<UserDTO>())).Returns(new ConfirmedUserDTO());
            _userService.Setup(x => x.GetClubAdminConfirmedUser(It.IsAny<UserDTO>())).Returns(new ConfirmedUserDTO());
            _userService.Setup(x => x.CheckOrAddPlastunRoleAsync(It.IsAny<string>(), DateTime.Now)).ReturnsAsync(TimeSpan.Zero);
            _userService.Setup(x => x.CanApproveAsync(new List<ConfirmedUserDTO>(), It.IsAny<string>(), ClaimsPrincipal.Current)).ReturnsAsync(true);
            _mapper.Setup(x => x.Map<UserDTO, UserViewModel>(It.IsAny<UserDTO>())).Returns(new UserViewModel());
            _mapper.Setup(x => x.Map<IEnumerable<ConfirmedUserDTO>, IEnumerable<ConfirmedUserViewModel>>(new List<ConfirmedUserDTO>())).Returns(new List<ConfirmedUserViewModel>());
            _mapper.Setup(x => x.Map<ConfirmedUserDTO, ConfirmedUserViewModel>(It.IsAny<ConfirmedUserDTO>())).Returns(new ConfirmedUserViewModel());
            _userManagerService.Setup(x => x.IsInRoleAsync(It.IsAny<UserDTO>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManagerService.Setup(x => x.GetUserIdAsync(ClaimsPrincipal.Current)).ReturnsAsync(It.IsAny<string>());
            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            // Acts
            var result = await controller.Approvers("q");
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UserApproversViewModel>(viewResult.Model);
            Assert.NotNull(result);

        }

        [Fact]
        public async Task ApproversTestFailure()
        {
            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            // Acts
            var result = await  controller.Approvers("");
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.NotNull(result);

        }
        [Fact]
        public async Task ApproveUserTest()
        {
            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            // Acts
            var result = await controller.ApproveUser("1", false, false);
            // Assert
            
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Approvers", viewResult.ActionName);
            Assert.Equal("Account", viewResult.ControllerName);
            Assert.NotNull(result);

        }
        [Fact]
        public async Task ApproveUserTestFailure()
        {
            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            // Acts
            var result = await  controller.ApproveUser(null, false, false);
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.NotNull(result);

        }
        [Fact]
        public async Task ApproverDeleteTest()
        {
            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            // Acts
            var result = await controller.ApproverDelete(1, "");
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Approvers", viewResult.ActionName);
            Assert.Equal("Account", viewResult.ControllerName);
            Assert.NotNull(result);

        }

        [Fact]
        public async Task EditGetTest()
        {
            var userDTO = new UserDTO { UserProfile = new UserProfileDTO { EducationId = 1, WorkId = 1 } };
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(userDTO);
            _genderService.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<GenderDTO>());
            _educationService.Setup(x => x.GetAllGroupByPlaceAsync()).ReturnsAsync(new List<EducationDTO>());
            _educationService.Setup(x => x.GetAllGroupBySpecialityAsync()).ReturnsAsync(new List<EducationDTO>());
            _nationalityService.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<NationalityDTO>());
            _degreeService.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<DegreeDTO>());
            _workService.Setup(x => x.GetAllGroupByPlaceAsync()).ReturnsAsync(new List<WorkDTO>());
            _workService.Setup(x => x.GetAllGroupByPositionAsync()).ReturnsAsync(new List<WorkDTO>());
            _religionService.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<ReligionDTO>());
            _mapper.Setup(x => x.Map<IEnumerable<EducationDTO>, IEnumerable<EducationViewModel>>(new List<EducationDTO>())).Returns(new List<EducationViewModel>());
            _mapper.Setup(x => x.Map<IEnumerable<WorkDTO>, IEnumerable<WorkViewModel>>(new List<WorkDTO>())).Returns(new List<WorkViewModel>());
            _mapper.Setup(x => x.Map<UserDTO, UserViewModel>(It.IsAny<UserDTO>())).Returns(new UserViewModel());
            _mapper.Setup(x => x.Map<IEnumerable<NationalityDTO>, IEnumerable<NationalityViewModel>>(new List<NationalityDTO>())).Returns(new List<NationalityViewModel>());
            _mapper.Setup(x => x.Map<IEnumerable<ReligionDTO>, IEnumerable<ReligionViewModel>>(new List<ReligionDTO>())).Returns(new List<ReligionViewModel>());
            _mapper.Setup(x => x.Map<IEnumerable<DegreeDTO>, IEnumerable<DegreeViewModel>>(new List<DegreeDTO>())).Returns(new List<DegreeViewModel>());

            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            // Act
            var result = await controller.Edit("");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EditUserViewModel>(viewResult.Model);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task EditGetTestFailure()
        {
            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            // Act
            var result = await controller.Edit(null);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task EditPostTest()
        {
            _mapper.Setup(x => x.Map<UserViewModel, UserDTO>(It.IsAny<UserViewModel>())).Returns(new UserDTO());
            var mockFile = new Mock<IFormFile>();
            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            // Act
            var result = await controller.Edit(new EditUserViewModel { User = new UserViewModel(), EducationView = new EducationUserViewModel(), WorkView = new WorkUserViewModel() }, mockFile.Object);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UserProfile", viewResult.ActionName);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EditPostTestFailure()
        {
            // Arrange
            var controller = new AccountController(_userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object, _accountService.Object, null);
            var mockFile = new Mock<IFormFile>();
            var user = new EditUserViewModel();

            // Act
            var result = await controller.Edit(user, mockFile.Object);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.NotNull(result);
        }
    }
}
