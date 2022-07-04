using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.Tests.Services.Club
{
    [TestFixture]
    public class ClubServiceTests
    {
        private Mock<IClubAccessService> _clubAccessService;
        private Mock<IClubBlobStorageRepository> _clubBlobStorage;
        private ClubService _clubService;
        private Mock<IWebHostEnvironment> _env;
        private Mock<IMapper> _mapper;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IUserStore<User>> _user;
        private Mock<UserManager<User>> _userManager;

        private int Count => 2;

        private int Id => 1;

        private string LogoName => "logoName";

        private string StringId => "1";

        [Test]
        public void CreateAsync_InvalidOperationException()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubDto clubDto = new ClubDto
            {
                ID = 0
            };
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());

            // Act // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await clubService.CreateAsync(clubDto));
        }

        [Test]
        public async Task CreateAsync_ReturnClubDtoID()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubDto clubDto = new ClubDto
            {
                ID = 0
            };
            _clubBlobStorage.Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync((DataAccessClub.Club)null);

            // Act
            var result = await clubService.CreateAsync(clubDto);

            // Assert
            Assert.AreEqual(clubDto.ID, result);
            _clubBlobStorage.Verify(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task CreateAsync_WhereFormFileIsNull_ReturnClubDtoID()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubProfileDto clubProfileDto = new ClubProfileDto
            {
                Club = new ClubDto
                {
                    ID = 0
                }
            };

            // Act
            var result = await clubService.CreateAsync(clubProfileDto, null);

            // Assert
            Assert.AreEqual(clubProfileDto.Club.ID, result);
        }

        [Test]
        public async Task CreateAsync_WithClubDtoLogo_ClubIsNull_ReturnClubDtoID()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubDto clubDto = new ClubDto
            {
                ID = 0,
                Logo = "data:application/.jpeg;base64,/9j/"
            };
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync((DataAccessClub.Club)null);
            _clubBlobStorage.Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));

            // Act
            var result = await clubService.CreateAsync(clubDto);

            // Assert
            Assert.AreEqual(clubDto.ID, result);
            _clubBlobStorage.Verify(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _clubBlobStorage.Verify(b => b.DeleteBlobAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task CreateAsync_WithClubDtoLogo_ExtensionIsEmpty_ReturnClubDtoID()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubDto clubDto = new ClubDto
            {
                ID = 0,
                Logo = "data:application/,/9j/"
            };
            _clubBlobStorage.Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync((DataAccessClub.Club)null);

            // Act
            var result = await clubService.CreateAsync(clubDto);

            // Assert
            Assert.AreEqual(clubDto.ID, result);
            _clubBlobStorage.Verify(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WithClubDtoLogo_ExtensionWithoutPoint_ReturnClubDtoID()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubDto clubDto = new ClubDto
            {
                ID = 0,
                Logo = "data:application/base64,/9j/"
            };
            _clubBlobStorage.Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync((DataAccessClub.Club)null);

            // Act
            var result = await clubService.CreateAsync(clubDto);

            // Assert
            Assert.AreEqual(clubDto.ID, result);
            _clubBlobStorage.Verify(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WithClubDtoLogo_ReturnClubDtoID()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubDto clubDto = new ClubDto
            {
                ID = 0,
                Logo = "data:application/.jpeg;base64,/9j/"
            };
            _clubBlobStorage.Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync((DataAccessClub.Club)null);

            // Act
            var result = await clubService.CreateAsync(clubDto);

            // Assert
            Assert.AreEqual(clubDto.ID, result);
            _clubBlobStorage.Verify(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WithOldImageName_WhereFormFileIsNull_ReturnClubDtoID()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubProfileDto clubProfileDto = new ClubProfileDto
            {
                Club = new ClubDto
                {
                    ID = 0
                }
            };
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync((DataAccessClub.Club)null);

            // Act
            var result = await clubService.CreateAsync(clubProfileDto, null);

            // Assert
            Assert.AreEqual(clubProfileDto.Club.ID, result);
        }

        [Test]
        public async Task EditAsync_ReturnNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(new List<ClubDto>().FirstOrDefault());

            // Act
            var result = await clubService.EditAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task EditAsync_ReturnsClubEdited()
        {
            // Arrange
            ClubService clubService = CreateClubService();

            // Act
            var result = await clubService.EditAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDto>(result);
        }

        [Test]
        public async Task EditAsync_WhereMembersIsNull_ReturnsClubEdited()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithoutMembersWithoutAdminEndDate(Count).FirstOrDefault());

            // Act
            var result = await clubService.EditAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDto>(result);
            Assert.AreEqual(new List<ClubMembersDto>(), result.Members);
        }

        [Test]
        public async Task EditAsync_WithModel_ReturnsClubEdited()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubDto clubDto = new ClubDto
            {
                ID = 0
            };
            _repoWrapper.Setup(r => r.Club.Attach(It.IsAny<DataAccessClub.Club>()));
            _repoWrapper.Setup(r => r.Club.Update(It.IsAny<DataAccessClub.Club>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await clubService.EditAsync(clubDto);

            // Assert
            _repoWrapper.Verify(r => r.Club.Attach(It.IsAny<DataAccessClub.Club>()), Times.Once);
            _repoWrapper.Verify(r => r.Club.Update(It.IsAny<DataAccessClub.Club>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task EditAsync_WithModelAndFormFile_ReturnsClubEdited()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubProfileDto clubProfileDto = new ClubProfileDto
            {
                Club = new ClubDto
                {
                    ID = 0
                }
            };
            _repoWrapper.Setup(r => r.Club.Attach(It.IsAny<DataAccessClub.Club>()));
            _repoWrapper.Setup(r => r.Club.Update(It.IsAny<DataAccessClub.Club>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await clubService.EditAsync(clubProfileDto, It.IsAny<IFormFile>());

            // Assert
            _repoWrapper.Verify(r => r.Club.Attach(It.IsAny<DataAccessClub.Club>()), Times.Once);
            _repoWrapper.Verify(r => r.Club.Update(It.IsAny<DataAccessClub.Club>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsClub()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());
            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(new ClubDto());

            // Act
            var result = await _clubService.GetByIdAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubDto>(result);
        }

        [Test]
        public async Task GetClubUsersAsync_CityId_ReturnsCityUsers()
        {
            // Arrange
            _repoWrapper
                .Setup(u => u.ClubMembers.GetAllAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(), null));

            // Act
            var result = await _clubService.GetClubUsersAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubUserDto[]>(result);
        }

        [Test]
        public async Task GetCityProfileAsync_WithUser_ReturnsCityProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _userManager
                .Setup(u => u.GetUserIdAsync(It.IsAny<DataAccessClub.User>()))
                .ReturnsAsync(StringId);
            _userManager
                .Setup(u => u.GetRolesAsync(It.IsAny<DataAccessClub.User>()))
                .ReturnsAsync(new List<string>());
            var mockList = new Mock<IList<string>>();
            mockList
                .Setup(m => m.Contains(It.IsAny<string>()))
                .Returns(true);
            _clubAccessService
                .Setup(c => c.HasAccessAsync(It.IsAny<DataAccessClub.User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repoWrapper
                .Setup(r => r.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubMembers>, IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(new ClubMembers());
            _repoWrapper.Setup(x => x.UserPlastDegree.GetAllAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new List<UserPlastDegree>()
                    {new UserPlastDegree() { UserId = "12345", PlastDegree = new PlastDegree() {Id = 1, Name = ""}}});

            // Act
            var result = await clubService.GetClubProfileAsync(Id, new User() { Id = "1" });

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDto>(result);
        }

        [Test]
        public async Task GetCityProfileAsync_WithUserNullDegree_ReturnsCityProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _userManager
                .Setup(u => u.GetUserIdAsync(It.IsAny<DataAccessClub.User>()))
                .ReturnsAsync(StringId);
            _userManager
                .Setup(u => u.GetRolesAsync(It.IsAny<DataAccessClub.User>()))
                .ReturnsAsync(new List<string>());
            var mockList = new Mock<IList<string>>();
            mockList
                .Setup(m => m.Contains(It.IsAny<string>()))
                .Returns(true);
            _clubAccessService
                .Setup(c => c.HasAccessAsync(It.IsAny<DataAccessClub.User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repoWrapper
                .Setup(r => r.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubMembers>, IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(new ClubMembers());
            _repoWrapper.Setup(x => x.UserPlastDegree.GetAllAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new List<UserPlastDegree>()
                    {new UserPlastDegree()});

            // Act
            var result = await clubService.GetClubProfileAsync(Id, new User() { Id = "1" });

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDto>(result);
        }

        [Test]
        public async Task GetClubAdminsAsync_ReturnsClubAdmins()
        {
            // Arrange
            ClubService clubService = CreateClubService();

            // Act
            var result = await clubService.GetClubAdminsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDto>(result);
        }

        [Test]
        public async Task GetClubAdminsAsync_WhereAdminEndDateIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithoutMembersWithoutAdminEndDate(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubAdminsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Head);
            Assert.NotNull(result.Admins);
            Assert.AreEqual(0, result.Club.AdministrationCount);
        }

        [Test]
        public async Task GetClubAdminsAsync_WhereHeadIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithExAdmin(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubAdminsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Head);
            Assert.AreEqual(result.Admins, new List<ClubAdministrationDto>());
        }

        [Test]
        public async Task GetClubAdminsAsync_WhereHeadDeputyIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithExAdmin(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubAdminsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.HeadDeputy);
            Assert.AreEqual(result.Admins, new List<ClubAdministrationDto>());
        }

        [Test]
        public async Task GetClubAdminsAsync_WithClubIsNull_ReturnsNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDto)null);

            // Act
            var result = await clubService.GetClubAdminsAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetClubDocumentsAsync_ReturnsClubDocuments()
        {
            // Arrange
            ClubService clubService = CreateClubService();

            // Act
            var result = await clubService.GetClubDocumentsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDto>(result);
        }

        [Test]
        public async Task GetClubDocumentsAsync_WithClubIsNull_ReturnsNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDto)null);

            // Act
            var result = await clubService.GetClubDocumentsAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetClubFollowersAsync_ReturnsClubFollowers()
        {
            // Arrange
            ClubService clubService = CreateClubService();

            // Act
            var result = await clubService.GetClubFollowersAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDto>(result);
        }

        [Test]
        public async Task GetClubFollowersAsync_WithClubIsNull_ReturnsNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDto)null);

            // Act
            var result = await clubService.GetClubFollowersAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetClubMembersAsync_ReturnsClubMembers()
        {
            // Arrange
            ClubService clubService = CreateClubService();

            // Act
            var result = await clubService.GetClubMembersAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDto>(result);
        }

        [Test]
        public async Task GetClubMembersAsync_WithClubIsNull_ReturnsNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDto)null);

            // Act
            var result = await clubService.GetClubMembersAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetClubProfileAsync_ReturnsClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();

            // Act
            var result = await clubService.GetClubProfileAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDto>(result);
        }

        [Test]
        public async Task GetClubProfileAsync_WhereAdminEndDateIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithoutMembersWithoutAdminEndDate(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubProfileAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Head);
            Assert.NotNull(result.Admins);
            Assert.AreNotEqual(0, result.Club.AdministrationCount);
        }

        [Test]
        public async Task GetClubProfileAsync_WhereClubIsNull_ReturnNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDto)null);

            // Act
            var result = await clubService.GetClubProfileAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetClubProfileAsync_WhereHeadIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithExAdmin(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubProfileAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Head);
            Assert.AreEqual(result.Admins, new List<ClubAdministrationDto>());
        }

        [Test]
        public async Task GetClubProfileAsync_WhereHeadDeputyIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithExAdmin(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubProfileAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.HeadDeputy);
            Assert.AreEqual(result.Admins, new List<ClubAdministrationDto>());
        }

        [Test]
        public async Task GetClubMembersInfoAsync_ReturnsClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();

            // Act
            var result = await clubService.GetClubMembersInfoAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDto>(result);
        }

        [Test]
        public async Task GetClubMembersInfoAsync_WhereAdminEndDateIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithoutMembersWithoutAdminEndDate(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubMembersInfoAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Head);
            Assert.NotNull(result.Admins);
            Assert.AreNotEqual(0, result.Club.AdministrationCount);
        }

        [Test]
        public async Task GetClubMembersInfoAsync_WhereClubIsNull_ReturnNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDto)null);

            // Act
            var result = await clubService.GetClubMembersInfoAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetClubMembersInfoAsync_WhereHeadIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithExAdmin(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubMembersInfoAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Head);
            Assert.AreEqual(result.Admins, new List<ClubAdministrationDto>());
        }

        [Test]
        public async Task GetClubMembersInfoAsync_WhereHeadDeputyIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithExAdmin(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubMembersInfoAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.HeadDeputy);
            Assert.AreEqual(result.Admins, new List<ClubAdministrationDto>());
        }

        [Test]
        public async Task GetClubMembersInfoAsync_WhereCountIsNull_ReturnClubProfile()
        {
            // Arrange
            var fakeClubDTO = CreateFakeClubDto(1).FirstOrDefault();
            fakeClubDTO.ClubAdministration = null;
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(fakeClubDTO);

            // Act
            var result = await clubService.GetClubMembersInfoAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Head);
            Assert.Null(result.HeadDeputy);
            Assert.Null(result.Admins);
        }

        [Test]
        public async Task GetClubs_ReturnClubForAdministrationDTOs()
        {
            // Arrange
            ClubService clubService = CreateClubService();

            // Act
            var result = await clubService.GetClubs();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<ClubForAdministrationDto>>(result);
        }

        [Test]
        public async Task GetLogoBase64_ReturnLogoBase64()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _clubBlobStorage.Setup(c => c.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(LogoName);

            // Act
            var result = await clubService.GetLogoBase64(LogoName);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(LogoName, result);
        }
        [Test]
        public async Task DeleteClubMemberHistory_SingleNumber_DeletesClubMember()
        {
            // Arrange
            _repoWrapper
                .Setup(u => u.ClubMemberHistory.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.ClubMemberHistory, bool>>>(), null))
                    .ReturnsAsync(new List<DataAccessClub.ClubMemberHistory>() { new ClubMemberHistory() { } });
            _repoWrapper
                .Setup(u => u.SaveAsync());
            _repoWrapper
                .Setup(u => u.ClubMemberHistory.Delete(It.IsAny<ClubMemberHistory>()));
            // Act
            await _clubService.DeleteClubMemberHistory(It.IsAny<int>());
            // Assert
            _repoWrapper.Verify(u => u.ClubMemberHistory.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.ClubMemberHistory, bool>>>(), null), Times.Once);
            _repoWrapper.Verify(u => u.ClubMemberHistory.Delete(It.IsAny<ClubMemberHistory>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }
        [Test]
        public async Task RemoveAsync_SingleNumber_DeletesClub()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _repoWrapper
                .Setup(
                    u => u.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());

            _clubBlobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.Club.Delete(It.IsAny<DataAccessClub.Club>()));

            _repoWrapper
                .Setup(u => u.ClubMemberHistory.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.ClubMemberHistory, bool>>>(), null))
                .ReturnsAsync(new List<DataAccessClub.ClubMemberHistory>() { new ClubMemberHistory() { } });
            _repoWrapper
                .Setup(u => u.SaveAsync());
            _repoWrapper
                .Setup(u => u.ClubMemberHistory.Delete(It.IsAny<ClubMemberHistory>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await clubService.RemoveAsync(It.IsAny<int>());

            // Assert
            _repoWrapper.Verify(u => u.ClubMemberHistory.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.ClubMemberHistory, bool>>>(), null), Times.Once);
            _repoWrapper.Verify(u => u.ClubMemberHistory.Delete(It.IsAny<ClubMemberHistory>()), Times.Once);
            _repoWrapper.Verify(
                u => u.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null),
                Times.Once);
            _clubBlobStorage.Verify(c => c.DeleteBlobAsync(It.IsAny<string>()), Times.Never);
            _repoWrapper.Verify(r => r.Club.Delete(It.IsAny<DataAccessClub.Club>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.AtMost(2));
        }

        [Test]
        public async Task RemoveAsyncWithoutLogo_SingleNumber_DeletesClubWithotLogo()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _repoWrapper
                .Setup(
                    u => u.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(GetTestNewClub());

            _clubBlobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.Club.Delete(It.IsAny<DataAccessClub.Club>()));

            _repoWrapper
                .Setup(u => u.ClubMemberHistory.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.ClubMemberHistory, bool>>>(), null))
                .ReturnsAsync(new List<DataAccessClub.ClubMemberHistory>() { new ClubMemberHistory() { } });
            _repoWrapper
                .Setup(u => u.SaveAsync());
            _repoWrapper
                .Setup(u => u.ClubMemberHistory.Delete(It.IsAny<ClubMemberHistory>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await clubService.RemoveAsync(It.IsAny<int>());

            // Assert
            _repoWrapper.Verify(u => u.ClubMemberHistory.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.ClubMemberHistory, bool>>>(), null), Times.Once);
            _repoWrapper.Verify(u => u.ClubMemberHistory.Delete(It.IsAny<ClubMemberHistory>()), Times.Once);
            _repoWrapper.Verify(
                u => u.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null),
                Times.Once);
            _clubBlobStorage.Verify(c => c.DeleteBlobAsync(It.IsAny<string>()), Times.Once);
            _repoWrapper.Verify(r => r.Club.Delete(It.IsAny<DataAccessClub.Club>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.AtMost(2));
        }

        [Test]
        public async Task GetClubHead_ReturnsClubHead()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());
            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubWithRole(Roles.KurinHead));

            // Act
            var result = await _clubService.GetClubHeadAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubAdministrationDto>(result);
        }

        [Test]
        public async Task GetClubHead_ReturnsNull()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());
            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(new ClubDto());

            // Act
            var result = await _clubService.GetClubHeadAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetClubHeadDeputy_ReturnsClubHeadDeputy()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());
            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubWithRole(Roles.KurinHeadDeputy));

            // Act
            var result = await _clubService.GetClubHeadDeputyAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubAdministrationDto>(result);
        }

        [Test]
        public async Task GetClubHeadDeputy_ReturnsNull()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());
            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(new ClubDto());

            // Act
            var result = await _clubService.GetClubHeadDeputyAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetClubAdmins_ReturnsClubAdmins()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());
            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubWithRole(Roles.KurinHeadDeputy));

            // Act
            var result = await _clubService.GetAdminsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<ClubAdministrationDto>>(result);
        }

        [Test]
        public async Task GetClubAdmins_ReturnsNull()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());
            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(new ClubDto());

            // Act
            var result = await _clubService.GetAdminsAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _env = new Mock<IWebHostEnvironment>();
            _clubBlobStorage = new Mock<IClubBlobStorageRepository>();
            _clubAccessService = new Mock<IClubAccessService>();
            _user = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_user.Object, null, null, null, null, null, null, null, null);
            _clubService = new ClubService(
                _repoWrapper.Object,
                _mapper.Object,
                _env.Object,
                _clubBlobStorage.Object,
                _userManager.Object
            );
        }

        private ClubService CreateClubService()
        {
            _mapper.Setup(m => m.Map<IEnumerable<DataAccessClub.Club>,
                    IEnumerable<ClubDto>>(It.IsAny<IEnumerable<DataAccessClub.Club>>()))
                .Returns(CreateFakeClubDto(10));
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDto(10).FirstOrDefault());
            _mapper.Setup(m => m.Map<ClubDto, DataAccessClub.Club>(It.IsAny<ClubDto>()))
                .Returns(() => new DataAccessClub.Club());
            _repoWrapper.Setup(r => r.Club.FindAll())
                .Returns(CreateFakeCities(10));
            _repoWrapper.Setup(r => r.Club.FindByCondition(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>()))
                .Returns((Expression<Func<DataAccessClub.Club, bool>> condition) =>
                    CreateFakeCities(10).Where(condition));
            _repoWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(), null))
               .ReturnsAsync(GetTestRegion());
            _mapper.Setup(m => m.Map<ClubDto, DataAccessClub.Club>(It.IsAny<ClubDto>()))
                .Returns(() => new DataAccessClub.Club());
            _repoWrapper.Setup(m => m.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(), null))
                .ReturnsAsync(new CityMembers() { });
            _repoWrapper.Setup(r => r.City.GetFirstAsync(It.IsAny<Expression<Func<DataAccessClub.City, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.City() { Name = "Львів" });
            _repoWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(), null))
               .ReturnsAsync(GetTestRegion());
            _repoWrapper.Setup(r => r.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
               .ReturnsAsync(new User());
            _repoWrapper.Setup(r => r.Club.Update(It.IsAny<DataAccessClub.Club>()))
                .Verifiable();
            _repoWrapper.Setup(r => r.Club.Create(It.IsAny<DataAccessClub.Club>()))
                .Verifiable();
            _repoWrapper.Setup(r => r.Save())
                .Verifiable();
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(GetTestNewClub());

            return new ClubService(
                _repoWrapper.Object,
                _mapper.Object,
                _env.Object,
                _clubBlobStorage.Object,
                _userManager.Object
            );
        }

        private IQueryable<DataAccessClub.Club> CreateFakeCities(int count)
        {
            List<DataAccessClub.Club> cities = new List<DataAccessClub.Club>();
            for (int i = 0; i < count; i++)
            {
                cities.Add(new DataAccessClub.Club());
            }
            return cities.AsQueryable();
        }

        private IQueryable<ClubDto> CreateFakeClubDto(int count)
        {
            List<ClubDto> clubs = new List<ClubDto>();

            for (int i = 0; i < count; i++)
            {
                clubs.Add(new ClubDto
                {
                    ClubAdministration = GetClubAdministrationDTO(),
                    ClubMembers = new List<ClubMembersDto>
                    {
                        new ClubMembersDto
                        {
                            StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null,
                            IsApproved = true,
                            UserId = "12345",
                            User=new ClubUserDto()
                        },
                        new ClubMembersDto
                        {
                            StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null,
                            IsApproved = false,
                            UserId = "12345",
                            User=new ClubUserDto()
                        }
                    },
                    ClubDocuments = new List<ClubDocumentsDto>
                    {
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto()
                    },
                });
            }
            return clubs.AsQueryable();
        }

        private IQueryable<ClubDto> CreateFakeClubDtoWithExAdmin(int count)
        {
            List<ClubDto> clubs = new List<ClubDto>();

            for (int i = 0; i < count; i++)
            {
                var clubDto = GetClubDto();
                clubDto.ClubAdministration = new List<ClubAdministrationDto>
                {
                    new ClubAdministrationDto
                    {
                        UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDto(),
                        AdminType = new AdminTypeDto
                        {
                            AdminTypeName = Roles.KurinHead
                        },
                        EndDate = DateTime.Now.AddMonths(-3)
                    },
                    new ClubAdministrationDto
                    {
                       UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDto(),
                        AdminType = new AdminTypeDto
                        {
                            AdminTypeName = "----------",
                        },
                        EndDate = DateTime.Now.AddMonths(-3)
                    }
                };
                clubs.Add(clubDto);
            }

            return clubs.AsQueryable();
        }

        private ClubDto CreateFakeClubWithRole(string role)
        {
            return new ClubDto()
            {
                ID = Id,
                ClubAdministration = new List<ClubAdministrationDto>
                    {
                        new ClubAdministrationDto
                        {
                            AdminType = new AdminTypeDto
                            {
                                AdminTypeName = role
                            },
                            Status = true
                        }
                    }
            };
        }

        private IQueryable<ClubDto> CreateFakeClubDtoWithoutMembersWithoutAdminEndDate(int count)
        {
            List<ClubDto> clubs = new List<ClubDto>();

            for (int i = 0; i < count; i++)
            {
                var clubDto = GetClubDtoWithoutMembers();
                clubDto.ClubMembers = new List<ClubMembersDto>();
                clubDto.ClubAdministration = new List<ClubAdministrationDto>
                {
                    new ClubAdministrationDto
                    {
                        UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDto(),
                        AdminType = new AdminTypeDto
                        {
                            AdminTypeName = Roles.KurinHead
                        },
                        Status=true
                    },
                    new ClubAdministrationDto
                    {
                        UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDto(),
                        AdminType = new AdminTypeDto
                        {
                            AdminTypeName = "----------",
                        },
                        Status=true
                    }
                };
                clubs.Add(clubDto);
            }
            return clubs.AsQueryable();
        }

        private List<ClubAdministrationDto> GetClubAdministrationDTO()
        {
            return new List<ClubAdministrationDto>
            {
                 new ClubAdministrationDto
                 {
                      UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                      User = new ClubUserDto(),
                      AdminType = new AdminTypeDto
                      {
                           AdminTypeName = Roles.KurinHead
                      }
                 },
                 new ClubAdministrationDto
                 {
                      UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                      User = new ClubUserDto(),
                      AdminType = new AdminTypeDto
                      {
                           AdminTypeName = "----------"
                      }
                 }
            };
        }

        private ClubDto GetClubDto()
        {
            var club = GetClubDtoWithoutMembers();
            club.ClubMembers = new List<ClubMembersDto>
                {
                    new ClubMembersDto
                    {
                        UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDto(),
                        StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null
                    }
                };
            return club;
        }

        private ClubDto GetClubDtoWithoutMembers()
        {
            return new ClubDto
            {
                ClubAdministration = GetClubAdministrationDTO(),
                ClubDocuments = new List<ClubDocumentsDto>
                    {
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto()
                    }
            };
        }

        private DataAccessClub.Club GetTestClubWithoutLogo()
        {
            var club = new DataAccessClub.Club
            {
                Name = "club"
            };
            return club;
        }

        private DataAccessClub.Club GetTestNewClub()
        {
            var club = GetTestClubWithoutLogo();
            club.Logo = "710b8b06-6869-45db-894f-7a0b131e6c6b.jpg";

            return club;
        }

        private Region GetTestRegion()
        {
            return new Region()
            {
                ID = 1,
                RegionName = "Lviv",
                Description = "Lviv region"
            };
        }
    }
}
