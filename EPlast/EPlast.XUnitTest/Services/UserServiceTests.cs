using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Xunit;

namespace EPlast.XUnitTest.Services
{
    public class UserServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IUserStore<User>> _userStoreMock;
        private Mock<UserManager<User>> _userManager;
        private Mock<IHostingEnvironment> _hostEnv;
        private Mock<IMapper> _mapper;

        public UserServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _userStoreMock = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _hostEnv = new Mock<IHostingEnvironment>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public void GetUserProfileTest()
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

            var service = new UserService( _repoWrapper.Object, _userManager.Object, _mapper.Object, _hostEnv.Object);
            _mapper.Setup(x => x.Map<User, UserDTO>(It.IsAny<User>())).Returns(new UserDTO());
            // Act
            var result = service.GetUser("1");
            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<UserDTO>(result);
        }
        [Fact]
        public void GetConfirmedUsersTest()
        {
            UserDTO user = new UserDTO { ConfirmedUsers = new List<ConfirmedUserDTO>() };

            var service = new UserService(_repoWrapper.Object, _userManager.Object, _mapper.Object, _hostEnv.Object);
            // Act
            var result = service.GetConfirmedUsers(user);
            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsAssignableFrom<IEnumerable<ConfirmedUserDTO>>(result);
        }
        [Fact]
        public void GetClubAdminConfirmedUserTest()
        {
            UserDTO user = new UserDTO { ConfirmedUsers = new List<ConfirmedUserDTO>() };

            var service = new UserService(_repoWrapper.Object, _userManager.Object, _mapper.Object, _hostEnv.Object);
            // Act
            var result = service.GetConfirmedUsers(user);
            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsAssignableFrom<IEnumerable<ConfirmedUserDTO>>(result);
        }
        [Fact]
        public void GetCityAdminConfirmedUserTest()
        {
            UserDTO user = new UserDTO { ConfirmedUsers = new List<ConfirmedUserDTO>() };

            var service = new UserService(_repoWrapper.Object, _userManager.Object, _mapper.Object, _hostEnv.Object);
            // Act
            var result = service.GetConfirmedUsers(user);
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ConfirmedUserDTO>>(result);
        }
        [Fact]
        public void CanApproveTest()
        {
            var conUser = new ConfirmedUserDTO { UserID = "1", ConfirmDate = DateTime.Now, isClubAdmin = false, isCityAdmin = false };
            var appUser = new ApproverDTO { UserID = "3", ConfirmedUser = conUser };
            conUser.Approver = appUser;

            UserDTO user = new UserDTO { ConfirmedUsers = new List<ConfirmedUserDTO>() };
            var confUsers = new List<ConfirmedUserDTO> { conUser, conUser };
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");

            var service = new UserService(_repoWrapper.Object, _userManager.Object, _mapper.Object, _hostEnv.Object);
            // Act
            var result = service.CanApprove(confUsers,"2", It.IsAny<ClaimsPrincipal>());
            // Assert
            var res=Assert.IsType<bool>(result);
            Assert.True(result);
        }
        [Fact]
        public void CanApproveTestFailure()
        {
            UserDTO user = new UserDTO { ConfirmedUsers = new List<ConfirmedUserDTO>() };
            var conUser = new ConfirmedUserDTO();
            var confUsers = new List<ConfirmedUserDTO> { conUser, conUser, conUser, conUser };
            _userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");

            var service = new UserService(_repoWrapper.Object, _userManager.Object, _mapper.Object, _hostEnv.Object);
            // Act
            var result = service.CanApprove(confUsers, "1", It.IsAny<ClaimsPrincipal>());
            // Assert
            var res = Assert.IsType<bool>(result);
            Assert.False(result);
        }
        [Fact]
        public void CheckOrAddPlastunRoleTest()
        {
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());

            var service = new UserService(_repoWrapper.Object, _userManager.Object, _mapper.Object, _hostEnv.Object);
            // Act
            var result = service.CheckOrAddPlastunRole("1", DateTime.Now);
            // Assert
            var res = Assert.IsType<TimeSpan>(result.Result);
        }
        [Fact]
        public void UpdateTest()
        {
            var userDTO = new UserDTO
            {
                FirstName = "Vova",
                LastName = "Vermii",
                UserProfile = new UserProfileDTO
                {
                    Nationality = new NationalityDTO { Name = "Українець" },
                    NationalityId = 1,
                    Religion = new ReligionDTO { Name = "Християнство" },
                    ReligionId = 1,
                    Education = new EducationDTO() { PlaceOfStudy = "ЛНУ", Speciality = "КН" },
                    EducationId = 1,
                    Degree = new DegreeDTO { Name = "Бакалавр" },
                    DegreeId = 1,
                    Work = new WorkDTO { PlaceOfwork = "SoftServe", Position = "ProjectManager" },
                    WorkId = 1,
                    Gender = new GenderDTO { Name = "Чоловік" },
                    GenderID = 1
                }
            };
            var user = new User
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
            _repoWrapper.Setup(r => r.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>{user}.AsQueryable());
            _repoWrapper.Setup(r => r.UserProfile.FindByCondition(It.IsAny<Expression<Func<UserProfile, bool>>>())).Returns(new List<UserProfile> {  }.AsQueryable());
            _repoWrapper.Setup(r => r.Education.FindByCondition(It.IsAny<Expression<Func<Education, bool>>>())).Returns(new List<Education>{new Education
            {
                PlaceOfStudy="place",
                Speciality="spec",
            } }.AsQueryable());
            _repoWrapper.Setup(r => r.Work.FindByCondition(It.IsAny<Expression<Func<Work, bool>>>())).Returns(new List<Work>{new Work
            {
                PlaceOfwork="place",
                Position="position",
            } }.AsQueryable());
            _mapper.Setup(x => x.Map<UserDTO, User>(It.IsAny<UserDTO>())).Returns(user);
            var mockFile = new Mock<IFormFile>();

            var service = new UserService(_repoWrapper.Object, _userManager.Object, _mapper.Object, _hostEnv.Object);
            // Act
            service.Update(userDTO, mockFile.Object,1,1,1,1);
            // Assert
            _repoWrapper.Verify(r => r.User.Update(It.IsAny<User>()), Times.Once());
            _repoWrapper.Verify(r => r.UserProfile.Update(It.IsAny<UserProfile>()), Times.Once());
            _repoWrapper.Verify(r => r.Save(), Times.Once());
        }
    }
}
