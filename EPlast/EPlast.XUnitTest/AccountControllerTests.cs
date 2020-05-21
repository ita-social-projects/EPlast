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
    }
        [Fact]
        public void UserProfileTest()
        {
            //_repoWrapper.Setup(r => r.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>{new User
            //{
            //    FirstName = "Vova",
            //    LastName = "Vermii",
            //    UserProfile = new UserProfile
            //    {
            //        Nationality = new Nationality { Name = "Українець" },
            //        Religion = new Religion { Name = "Християнство" },
            //        Education = new Education() { PlaceOfStudy = "ЛНУ", Speciality = "КН"  },
            //        Degree = new Degree { Name = "Бакалавр" },
            //        Work = new Work { PlaceOfwork = "SoftServe", Position = "ProjectManager" },
            //        Gender = new Gender { Name = "Чоловік" }
            //    }
            //} }.AsQueryable());

            //_repoWrapper.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>())).Returns(new List<CityAdministration>{new CityAdministration
            //{
            //    AdminType=new AdminType{ AdminTypeName="Admin"},
            //    City=new City{ Name="City", HouseNumber="1", Street="Street"}
            //} }.AsQueryable());

            //_repoWrapper.Setup(r => r.Gender.FindAll()).Returns(new List<Gender>().AsQueryable());
            //_repoWrapper.Setup(r => r.Nationality.FindAll()).Returns(new List<Nationality>().AsQueryable());
            //_repoWrapper.Setup(r => r.Education.FindAll()).Returns(new List<Education>().AsQueryable());
            //_repoWrapper.Setup(r => r.Work.FindAll()).Returns(new List<Work>().AsQueryable());
            //_repoWrapper.Setup(r => r.Degree.FindAll()).Returns(new List<Degree>().AsQueryable());
            //_repoWrapper.Setup(r => r.Religion.FindAll()).Returns(new List<Religion>().AsQueryable());

            //_userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");

            _userService.Setup(x => x.GetUserProfile(It.IsAny<string>())).Returns(new UserDTO
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
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object);
            // Act
            var result = controller.UserProfile("1");
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<PersonalDataViewModel>(viewResult.Model);
        }

        [Fact]
        public void UserProfileTestFailure()
        {
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                  _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                  _confirmedUserService.Object, _userManagerService.Object, _mapper.Object);
            // Act
            var result = controller.UserProfile("");
            // Assert
            Assert.IsType<RedirectToActionResult>(result.Result);
        }
        [Fact]
        public void EditTest()
        {
            _mapper.Setup(x => x.Map<UserViewModel, UserDTO>(It.IsAny<UserViewModel>())).Returns(new UserDTO());
            var mockFile = new Mock<IFormFile>();
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object);
            // Act
            var resultPost = controller.Edit(new EditUserViewModel { User=new UserViewModel(),EducationView=new EducationUserViewModel(),WorkView=new WorkUserViewModel()}, mockFile.Object);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultPost);
        }

        [Fact]
        public void EditTestFailure()
        {
            // Arrange
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object);
            var mockFile = new Mock<IFormFile>();
            var user = new EditUserViewModel();

            // Act
            var result = controller.Edit(user, mockFile.Object);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }
        [Fact]
        public void DeletePositionTrueRemoveRoleTrueTest()
        {
            // Arrange
            var cityAdministrations = new List<CityAdministration>
            {
                new CityAdministration
                {
                    ID = 1,
                    User = new User(),
                    AdminType = new AdminType(),
                },
            };
            _repoWrapper.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(cityAdministrations.AsQueryable());
            _userAccessManager.Setup(uam => uam.HasAccess(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object);

            // Act
            var result = controller.DeletePosition(cityAdministrations[0].ID);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void DeletePositionTrueRemoveRoleFalseTest()
        {
            // Arrange
            var cityAdministrations = new List<CityAdministration>
            {
                new CityAdministration
                {
                    ID = 1,
                    User = new User(),
                    AdminType = new AdminType(),
                    EndDate = DateTime.Now,
                },
            };
            _repoWrapper.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(cityAdministrations.AsQueryable());
            _userAccessManager.Setup(uam => uam.HasAccess(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object);

            // Act
            var result = controller.DeletePosition(cityAdministrations[0].ID);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            _userManager.Verify(u => u.RemoveFromRoleAsync(cityAdministrations[0].User, cityAdministrations[0].AdminType.AdminTypeName), Times.Never);
        }

        [Fact]
        public void DeletePositionFalseTest()
        {
            // Arrange
            _repoWrapper.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(new List<CityAdministration>().AsQueryable());
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object);

            // Act
            var result = controller.DeletePosition(0);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            _repoWrapper.Verify(r => r.CityAdministration.Delete(It.IsAny<CityAdministration>()), Times.Never);
            _repoWrapper.Verify(r => r.Save(), Times.Never);
            _userManager.Verify(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void EndPositionTrueTest()
        {
            // Arrange
            var cityAdministrations = new List<CityAdministration>
            {
                new CityAdministration
                {
                    ID = 1,
                    User = new User(),
                    AdminType = new AdminType(),
                    StartDate = DateTime.Now
                },
            };
            _repoWrapper.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(cityAdministrations.AsQueryable());
            _userAccessManager.Setup(uam => uam.HasAccess(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object);

            // Act
            var result = controller.EndPosition(cityAdministrations[0].ID);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(cityAdministrations[0].EndDate);
        }

        [Fact]
        public void EndPositionFalseTest()
        {
            // Arrange
            _repoWrapper.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(new List<CityAdministration>().AsQueryable());
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, _userService.Object, _nationalityService.Object, _educationService.Object, _religionService.Object, _workService.Object, _genderService.Object, _degreeService.Object,
                _confirmedUserService.Object, _userManagerService.Object, _mapper.Object);

            // Act
            var result = controller.EndPosition(0);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            _repoWrapper.Verify(r => r.CityAdministration.Update(It.IsAny<CityAdministration>()), Times.Never);
            _repoWrapper.Verify(r => r.Save(), Times.Never);
            _userManager.Verify(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }
    }
}
