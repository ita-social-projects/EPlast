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
        }

        [Fact]
        public void UserProfileTest()
        {
            _repoWrapper.Setup(r => r.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>{new User
            {
                FirstName = "Vova",
                LastName = "Vermii",
                UserProfile = new UserProfile
                {
                    Nationality = new Nationality { Name = "Українець" },
                    Religion = new Religion { Name = "Християнство" },
                    Education = new Education() { PlaceOfStudy = "ЛНУ", Speciality = "КН"  },
                    Degree = new Degree { Name = "Бакалавр" },
                    Work = new Work { PlaceOfwork = "SoftServe", Position = "ProjectManager" },
                    Gender = new Gender { Name = "Чоловік" }
                }
            } }.AsQueryable());

            _repoWrapper.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>())).Returns(new List<CityAdministration>{new CityAdministration
            {
                AdminType=new AdminType{ AdminTypeName="Admin"},
                City=new City{ Name="City", HouseNumber="1", Street="Street"}
            } }.AsQueryable());

            _repoWrapper.Setup(r => r.Gender.FindAll()).Returns(new List<Gender>().AsQueryable());
            _repoWrapper.Setup(r => r.Nationality.FindAll()).Returns(new List<Nationality>().AsQueryable());
            _repoWrapper.Setup(r => r.Education.FindAll()).Returns(new List<Education>().AsQueryable());
            _repoWrapper.Setup(r => r.Work.FindAll()).Returns(new List<Work>().AsQueryable());
            _repoWrapper.Setup(r => r.Degree.FindAll()).Returns(new List<Degree>().AsQueryable());
            _repoWrapper.Setup(r => r.Religion.FindAll()).Returns(new List<Religion>().AsQueryable());

            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");

            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, null);
            // Act
            var result = controller.UserProfile("1");
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UserViewModel>(viewResult.Model);
        }

        [Fact]
        public void UserProfileTestFailure()
        {
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, null);
            // Act
            var result = controller.UserProfile("1");
            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void EditTest()
        {
            // Arrange
            var expected = new User
            {
                FirstName = "Vova",
                LastName = "Vermii",
                UserProfile = new UserProfile
                {
                    Nationality = new Nationality { Name = "Українець" },
                    Religion = new Religion { Name = "Християнство" },
                    Education = new Education() { PlaceOfStudy = "ЛНУ", Speciality = "КН" },
                    Degree = new Degree { Name = "Бакалавр" },
                    Work = new Work { PlaceOfwork = "SoftServe", Position = "ProjectManager" },
                    Gender = new Gender { Name = "Чоловік" }
                }
            };

            _repoWrapper.Setup(r => r.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>{new User
            {
                FirstName = "Vova",
                LastName = "Vermii",
                UserProfile = new UserProfile
                {
                    Nationality = new Nationality { Name = "Українець" },
                    Religion = new Religion { Name = "Християнство" },
                    Education = new Education() { PlaceOfStudy = "ЛНУ", Speciality = "КН" },
                    Degree = new Degree { Name = "Бакалавр" },
                    Work = new Work { PlaceOfwork = "SoftServe", Position = "ProjectManager" },
                    Gender = new Gender { Name = "Чоловік" }
                }
            } }.AsQueryable());

            _repoWrapper.Setup(r => r.Gender.FindAll()).Returns(new List<Gender>().AsQueryable());
            _repoWrapper.Setup(r => r.Nationality.FindAll()).Returns(new List<Nationality>().AsQueryable());
            _repoWrapper.Setup(r => r.Education.FindAll()).Returns(new List<Education>().AsQueryable());
            _repoWrapper.Setup(r => r.Work.FindAll()).Returns(new List<Work>().AsQueryable());
            _repoWrapper.Setup(r => r.Degree.FindAll()).Returns(new List<Degree>().AsQueryable());
            _repoWrapper.Setup(r => r.Religion.FindAll()).Returns(new List<Religion>().AsQueryable());
            _repoWrapper.Setup(r => r.UserProfile.FindAll()).Returns(new List<UserProfile>().AsQueryable());

            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(expected.Id);
            _userManager.Setup(x => x.CreateSecurityTokenAsync(expected));

            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, null);
            var mockFile = new Mock<IFormFile>();
            var user = new EditUserViewModel { User = expected, EducationView = new EducationViewModel(), WorkView = new WorkViewModel(), Birthday = "18-04-2020" };

            // Act
            var resultPost = controller.Edit(user, mockFile.Object);

            // Assert
            _repoWrapper.Verify(r => r.User.Update(It.IsAny<User>()), Times.Once());
            _repoWrapper.Verify(r => r.UserProfile.Update(It.IsAny<UserProfile>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
        }

        [Fact]
        public void EditTestFailure()
        {
            // Arrange
            var controller = new AccountController(_userManager.Object, _signInManager.Object, _repoWrapper.Object, _logger.Object, _emailConfirm.Object, _hostEnv.Object,
                _userAccessManager.Object, null);
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
                _userAccessManager.Object, null);

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
                _userAccessManager.Object, null);

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
                _userAccessManager.Object, null);

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
                _userAccessManager.Object, null);

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
                _userAccessManager.Object, null);

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