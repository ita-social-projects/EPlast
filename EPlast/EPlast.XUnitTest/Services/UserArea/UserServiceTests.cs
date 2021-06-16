using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.Resources;
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
        private readonly Mock<IUniqueIdService> _uniqueId;
        public UserServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _userPersonalDataService = new Mock<IUserPersonalDataService>();
            _userBlobStorage = new Mock<IUserBlobStorageRepository>();
            _env = new Mock<IWebHostEnvironment>();
            _userManagerService = new Mock<IUserManagerService>();
            _confirmedUserService = new Mock<IConfirmedUsersService>();
            _uniqueId = new Mock<IUniqueIdService>();
        }

        private UserService GetService()
        {
            return new UserService(_repoWrapper.Object, _mapper.Object, _userPersonalDataService.Object, _userBlobStorage.Object, _env.Object, _uniqueId.Object);
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
            _mapper.Setup(x => x.Map<User, UserDTO>(It.IsAny<User>())).Returns(new UserDTO());
            // Act
            var result = await service.GetUserAsync("1");
            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserDTO>(result);
        }
        [Fact]
        public void GetConfirmedUsersTest()
        {
            UserDTO user = new UserDTO { ConfirmedUsers = new List<ConfirmedUserDTO>() };

            var service = GetService();            // Act
            var result = service.GetConfirmedUsers(user);
            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void GetClubAdminConfirmedUserTest()
        {
            UserDTO user = new UserDTO { ConfirmedUsers = new List<ConfirmedUserDTO>() };

            var service = GetService();            // Act
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

            var confUsers = new List<ConfirmedUserDTO> { conUser, conUser };

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
            var conUser = new ConfirmedUserDTO();
            var confUsers = new List<ConfirmedUserDTO> { conUser, conUser, conUser, conUser };

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
            _mapper.Setup(x => x.Map<UserDTO, User>(It.IsAny<UserDTO>())).Returns(user);
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
            _mapper.Setup(x => x.Map<UserDTO, User>(It.IsAny<UserDTO>())).Returns(user);
            _userBlobStorage.Setup(u => u.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _userBlobStorage.Setup(u => u.DeleteBlobAsync(It.IsAny<string>()));
            _uniqueId.Setup(u => u.GetUniqueId()).Returns(It.IsAny<Guid>());

            var service = GetService();            // Act
            await service.UpdateAsyncForBase64(userDTO, "im/age.png;something,so/me.png;jkjk", 1, 1, 1, 1);
            // Assert
            _repoWrapper.Verify(r => r.User.Update(It.IsAny<User>()), Times.Once());
            _repoWrapper.Verify(r => r.UserProfile.Update(It.IsAny<UserProfile>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.AtLeast(2));
        }
    }
}