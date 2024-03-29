﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.UserArea
{
    public class UserServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUserPersonalDataService> _userPersonalDataService;
        private readonly Mock<IUserBlobStorageRepository> _userBlobStorage;
        private readonly Mock<IWebHostEnvironment> _env;
        private protected readonly Mock<IUserManagerService> _userManagerService;
        private protected readonly Mock<IConfirmedUsersService> _confirmedUserService;
        private readonly Mock<INotificationService> _mockNotificationService;
        private Mock<UserManager<User>> _mockUserManager;

        public UserServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _userPersonalDataService = new Mock<IUserPersonalDataService>();
            _userBlobStorage = new Mock<IUserBlobStorageRepository>();
            _env = new Mock<IWebHostEnvironment>();
            _userManagerService = new Mock<IUserManagerService>();
            _confirmedUserService = new Mock<IConfirmedUsersService>();
            _mockNotificationService = new Mock<INotificationService>();
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private UserService GetService()
        {
            return new UserService(
                _repoWrapper.Object,
                _mapper.Object,
                _userPersonalDataService.Object,
                _userBlobStorage.Object,
                _env.Object,
                _userManagerService.Object,
                _mockNotificationService.Object,
                _mockUserManager.Object
            );
        }
        [Fact]
        public async Task GetUserProfileTest()
        {
            _repoWrapper.SetupSequence(r => r.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(), null)).ReturnsAsync(new User
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
            });

            var service = GetService();
            _mapper.Setup(x => x.Map<User, UserDto>(It.IsAny<User>())).Returns(new UserDto());
            // Act
            var result = await service.GetUserAsync("1");
            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
        }
        [Fact]
        public void GetConfirmedUsersTest()
        {
            UserDto user = new UserDto { ConfirmedUsers = new List<ConfirmedUserDto>() };

            var service = GetService();            // Act
            var result = service.GetConfirmedUsers(user);
            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void GetClubAdminConfirmedUserTest()
        {
            UserDto user = new UserDto { ConfirmedUsers = new List<ConfirmedUserDto>() };

            var service = GetService();            // Act
            var result = service.GetConfirmedUsers(user);
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ConfirmedUserDto>>(result);
        }

        [Fact]
        public void CanApproveTest()
        {
            var conUser = new ConfirmedUserDto { UserID = "1", ConfirmDate = DateTime.Now, ApproveType = ApproveType.PlastMember };
            var appUser = new ApproverDto { UserID = "3", ConfirmedUser = conUser };
            conUser.Approver = appUser;

            var confUsers = new List<ConfirmedUserDto> { conUser, conUser };

            var service = GetService();            
            // Act
            var result = service.CanApprove(confUsers, "2", new User().Id);
            // Assert
            Assert.IsType<bool>(result);
            Assert.True(result);
        }
        [Fact]
        public void CanApproveTestFailure()
        {
            var conUser = new ConfirmedUserDto();
            var confUsers = new List<ConfirmedUserDto> { conUser, conUser, conUser, conUser };

            var service = GetService();            
            // Act
            var result = service.CanApprove(confUsers, "1", "");
            // Assert
            Assert.False(result);
        }
        [Fact]
        public void CheckOrAddPlastunRoleTest()
        {
            var service = GetService();            
            // Act
            var result = service.CheckOrAddPlastunRole("1", DateTime.MinValue);
            // Assert
            Assert.IsType<TimeSpan>(result);
        }
        [Fact]
        public async Task UpdateTest()
        {
            var userDTO = new UserDto
            {
                FirstName = "Vova",
                LastName = "Vermii",
                UserProfile = new UserProfileDto
                {
                    Nationality = new NationalityDto { Name = "Українець" },
                    NationalityId = 1,
                    Religion = new ReligionDto { Name = "Християнство" },
                    ReligionId = 1,
                    Education = new EducationDto() { PlaceOfStudy = "ЛНУ", Speciality = "КН" },
                    EducationId = 1,
                    Degree = new DegreeDto { Name = "Бакалавр" },
                    DegreeId = 1,
                    Work = new WorkDto { PlaceOfwork = "SoftServe", Position = "ProjectManager" },
                    WorkId = 1,
                    Gender = new GenderDto { Name = "Чоловік" },
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
            _repoWrapper.Setup(r => r.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null)).ReturnsAsync(user);
            _repoWrapper.Setup(r => r.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null)).ReturnsAsync(new UserProfile());
            _repoWrapper.Setup(r => r.Education.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Education, bool>>>(), null)).ReturnsAsync(new Education
            {
                PlaceOfStudy = "place",
                Speciality = "spec",
            });
            _repoWrapper.Setup(r => r.Work.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Work, bool>>>(), null)).ReturnsAsync(new Work
            {
                PlaceOfwork = "place",
                Position = "position",
            });
            _mapper.Setup(x => x.Map<UserDto, User>(It.IsAny<UserDto>())).Returns(user);
            var mockFile = new Mock<IFormFile>();

            var service = GetService();            // Act
            await service.UpdateAsyncForFile(userDTO, mockFile.Object, 1, 1, 1, 1);
            // Assert
            _repoWrapper.Verify(r => r.User.Update(It.IsAny<User>()), Times.Once());
            _repoWrapper.Verify(r => r.UserProfile.Update(It.IsAny<UserProfile>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.AtLeast(2));
        }
        [Fact]
        public async Task UpdateAsyncForBase64Test()
        {
            var userDTO = new UserDto
            {
                FirstName = "Vova",
                LastName = "Vermii",
                UserProfile = new UserProfileDto
                {
                    Nationality = new NationalityDto { Name = "Українець" },
                    NationalityId = 1,
                    Religion = new ReligionDto { Name = "Християнство" },
                    ReligionId = 1,
                    Education = new EducationDto() { PlaceOfStudy = "ЛНУ", Speciality = "КН" },
                    EducationId = 1,
                    Degree = new DegreeDto { Name = "Бакалавр" },
                    DegreeId = 1,
                    Work = new WorkDto { PlaceOfwork = "SoftServe", Position = "ProjectManager" },
                    WorkId = 1,
                    Gender = new GenderDto { Name = "Чоловік" },
                    GenderID = 1,
                    FacebookLink = "vovan",
                    TwitterLink = "twitter.com/vovasik",
                    InstagramLink = "https://www.instagram.com/vov4ik",
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
                    Gender = new Gender { Name = "Чоловік" },
                    FacebookLink = "vovan",
                    TwitterLink = "twitter.com/vovasik",
                    InstagramLink = "https://www.instagram.com/vov4ik",
                }
            };
            _repoWrapper.Setup(r => r.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null)).ReturnsAsync(user);
            _repoWrapper.Setup(r => r.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null)).ReturnsAsync(new UserProfile());
            _repoWrapper.Setup(r => r.Education.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Education, bool>>>(), null)).ReturnsAsync(new Education
            {
                PlaceOfStudy = "place",
                Speciality = "spec",
            });
            _repoWrapper.Setup(r => r.Work.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Work, bool>>>(), null)).ReturnsAsync(new Work
            {
                PlaceOfwork = "place",
                Position = "position",
            });
            _mapper.Setup(x => x.Map<UserDto, User>(It.IsAny<UserDto>())).Returns(user);
            _userBlobStorage.Setup(u => u.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _userBlobStorage.Setup(u => u.DeleteBlobAsync(It.IsAny<string>()));

            var service = GetService();            // Act
            await service.UpdateAsyncForBase64(userDTO, "im/age.png;something,so/me.png;jkjk", 1, 1, 1, 1);
            // Assert
            _repoWrapper.Verify(r => r.User.Update(It.IsAny<User>()), Times.Once());
            _repoWrapper.Verify(r => r.UserProfile.Update(It.IsAny<UserProfile>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.AtLeast(2));
        }
    }
}