using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Xunit;
using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.BussinessLayer.Services.Interfaces;
using AutoMapper;
using EPlast.BussinessLayer.DTO;
using System.Threading.Tasks;
using EPlast.ViewModels.UserInformation;
using EPlast.ViewModels.UserInformation.UserProfile;

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
        private Mock<IHostingEnvironment> _hostEnv;
        private Mock<IUserAccessManager> _userAccessManager;
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
            _hostEnv = new Mock<IHostingEnvironment>();
            _userAccessManager = new Mock<IUserAccessManager>();
            _userService=new Mock<IUserService>();
            _nationalityService=new Mock<INationalityService>() ;
            _educationService=new Mock<IEducationService> ();
            _religionService=new Mock<IReligionService> ();
            _workService=new Mock<IWorkService> ();
            _genderService=new  Mock<IGenderService>() ;
            _degreeService=new Mock<IDegreeService> ();
            _userManagerService=new Mock<IUserManagerService>() ;
            _confirmedUserService=new Mock<IConfirmedUsersService> ();
            _mapper=new Mock<IMapper> ();
            _loggerService = new Mock<ILoggerService<AccountController>>();
    }
        [Fact]
        public void UserProfileTest()
        {
            _userService.Setup(x => x.GetUser(It.IsAny<string>())).Returns(new UserDTO
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
            _userService.Setup(x => x.CheckOrAddPlastunRole(It.IsAny<string>(),DateTime.Now)).ReturnsAsync(TimeSpan.Zero);

            _userManagerService.Setup(x => x.IsInRole(It.IsAny<UserDTO>(), It.IsAny<string>())).ReturnsAsync(true);
            UserViewModel a = new UserViewModel { Id="1"};
            _mapper.Setup(x => x.Map<UserDTO, UserViewModel>(It.IsAny<UserDTO>())).Returns(a);
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object,_userService.Object, _nationalityService.Object,_educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object,_loggerService.Object);
            // Act
            var result = controller.UserProfile("1");
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<PersonalDataViewModel>(viewResult.Model);
            Assert.NotNull(result);
        }

        [Fact]
        public void UserProfileTestFailure()
        {
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object);
            // Act
            var result = controller.UserProfile("");
            // Assert
            var viewResult=Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.NotNull(result);

        }

        [Fact]
        public void ApproversTest()
        {
            _userService.Setup(x => x.GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _userService.Setup(x => x.GetConfirmedUsers(It.IsAny<UserDTO>())).Returns(new List<ConfirmedUserDTO>());
            _userService.Setup(x => x.GetCityAdminConfirmedUser(It.IsAny<UserDTO>())).Returns(new ConfirmedUserDTO());
            _userService.Setup(x => x.GetClubAdminConfirmedUser(It.IsAny<UserDTO>())).Returns(new ConfirmedUserDTO());
            _userService.Setup(x => x.CheckOrAddPlastunRole(It.IsAny<string>(), DateTime.Now)).ReturnsAsync(TimeSpan.Zero);
            _userService.Setup(x => x.CanApprove(new List<ConfirmedUserDTO>(),It.IsAny<string>(),ClaimsPrincipal.Current)).Returns(true);
            _mapper.Setup(x => x.Map<UserDTO, UserViewModel>(It.IsAny<UserDTO>())).Returns(new UserViewModel());
            _mapper.Setup(x => x.Map<IEnumerable<ConfirmedUserDTO>, IEnumerable<ConfirmedUserViewModel>>(new List<ConfirmedUserDTO>())).Returns(new List<ConfirmedUserViewModel>());
            _mapper.Setup(x => x.Map<ConfirmedUserDTO, ConfirmedUserViewModel>(It.IsAny<ConfirmedUserDTO>())).Returns(new ConfirmedUserViewModel());
            _userManagerService.Setup(x => x.IsInRole(It.IsAny<UserDTO>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManagerService.Setup(x => x.GetUserId(ClaimsPrincipal.Current)).Returns(It.IsAny<string>());
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object);
            // Acts
            var result = controller.Approvers("q");
            // Assert
            var viewResult=Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<UserApproversViewModel>(viewResult.Model);
            Assert.NotNull(result);

        }

        [Fact]
        public void ApproversTestFailure()
        {
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object);
            // Acts
            var result = controller.Approvers("");
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.NotNull(result);

        }
        [Fact]
        public void ApproveUserTest()
        {
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object);
            // Acts
            var result = controller.ApproveUser("1", false, false);
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Approvers", viewResult.ActionName);
            Assert.Equal("Account", viewResult.ControllerName);
            Assert.NotNull(result);

        }
        [Fact]
        public void ApproveUserTestFailure()
        {
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object);
            // Acts
            var result = controller.ApproveUser(null,false,false);
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.NotNull(result);

        }
        [Fact]
        public void ApproverDeleteTest()
        {
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object);
            // Acts
            var result = controller.ApproverDelete(1,"");
            // Assert
            var viewResult= Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UserProfile", viewResult.ActionName);
            Assert.Equal("Account", viewResult.ControllerName);
            Assert.NotNull(result);

        }

        [Fact]
        public void EditGetTest()
        {
            var userDTO = new UserDTO { UserProfile=new UserProfileDTO { EducationId = 1, WorkId = 1 } };
            _userService.Setup(x => x.GetUser(It.IsAny<string>())).Returns(userDTO);
            _genderService.Setup(x => x.GetAll()).Returns(new List<GenderDTO>());
            _educationService.Setup(x => x.GetAllGroupByPlace()).Returns(new List<EducationDTO>());
            _educationService.Setup(x => x.GetAllGroupBySpeciality()).Returns(new List<EducationDTO>());
            _nationalityService.Setup(x => x.GetAll()).Returns(new List<NationalityDTO>());
            _degreeService.Setup(x => x.GetAll()).Returns(new List<DegreeDTO>());
            _workService.Setup(x => x.GetAllGroupByPlace()).Returns(new List<WorkDTO>());
            _workService.Setup(x => x.GetAllGroupByPosition()).Returns(new List<WorkDTO>());
            _religionService.Setup(x => x.GetAll()).Returns(new List<ReligionDTO>());
            _mapper.Setup(x => x.Map<IEnumerable<EducationDTO>, IEnumerable<EducationViewModel>>(new List<EducationDTO>())).Returns(new List<EducationViewModel>());
            _mapper.Setup(x => x.Map<IEnumerable<WorkDTO>, IEnumerable<WorkViewModel>>(new List<WorkDTO>())).Returns(new List<WorkViewModel>());
            _mapper.Setup(x => x.Map<UserDTO, UserViewModel>(It.IsAny<UserDTO>())).Returns(new UserViewModel());
            _mapper.Setup(x => x.Map<IEnumerable<NationalityDTO>, IEnumerable<NationalityViewModel>>(new List<NationalityDTO>())).Returns(new List<NationalityViewModel>());
            _mapper.Setup(x => x.Map<IEnumerable<ReligionDTO>, IEnumerable<ReligionViewModel>>(new List<ReligionDTO>())).Returns(new List<ReligionViewModel>());
            _mapper.Setup(x => x.Map<IEnumerable<DegreeDTO>, IEnumerable<DegreeViewModel>>(new List<DegreeDTO>())).Returns(new List<DegreeViewModel>());

            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object);
            // Act
            var result = controller.Edit("");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EditUserViewModel>(viewResult.Model);
            Assert.NotNull(result);
        }
        [Fact]
        public void EditGetTestFailure()
        {
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object);
            // Act
            var result = controller.Edit(null);

            // Assert
            var viewResult=Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.NotNull(result);
        }
        [Fact]
        public void EditPostTest()
        {
            _mapper.Setup(x => x.Map<UserViewModel, UserDTO>(It.IsAny<UserViewModel>())).Returns(new UserDTO());
            var mockFile = new Mock<IFormFile>();
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object);
            // Act
            var result = controller.Edit(new EditUserViewModel { User=new UserViewModel(),EducationView=new EducationUserViewModel(),WorkView=new WorkUserViewModel()}, mockFile.Object);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UserProfile", viewResult.ActionName);
            Assert.NotNull(result);
        }

        [Fact]
        public void EditPostTestFailure()
        {
            // Arrange
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object, _loggerService.Object);
            var mockFile = new Mock<IFormFile>();
            var user = new EditUserViewModel();

            // Act
            var result = controller.Edit(user, mockFile.Object);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.NotNull(result);
        }
    }
}
