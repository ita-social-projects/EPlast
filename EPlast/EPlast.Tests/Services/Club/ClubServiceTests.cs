﻿using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        private Mock<IUniqueIdService> _uniqueId;
        private Mock<IUserStore<User>> _user;
        private Mock<UserManager<User>> _userManager;
        private string ClubName => "Club";

        private int Count => 2;

        private int Id => 1;

        private string LogoName => "logoName";

        private string StringId => "1";

        [Test]
        public void CreateAsync_InvalidOperationException()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubDTO clubDto = new ClubDTO
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
            ClubDTO clubDto = new ClubDTO
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
            ClubProfileDTO clubProfileDto = new ClubProfileDTO
            {
                Club = new ClubDTO
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
            ClubDTO clubDto = new ClubDTO
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
            ClubDTO clubDto = new ClubDTO
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
            ClubDTO clubDto = new ClubDTO
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
            ClubDTO clubDto = new ClubDTO
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
            ClubProfileDTO clubProfileDto = new ClubProfileDTO
            {
                Club = new ClubDTO
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
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns(new List<ClubDTO>().FirstOrDefault());

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
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task EditAsync_WhereMembersIsNull_ReturnsClubEdited()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithoutMembersWithoutAdminEndDate(Count).FirstOrDefault());

            // Act
            var result = await clubService.EditAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDTO>(result);
            Assert.AreEqual(new List<ClubMembersDTO>(), result.Members);
        }

        [Test]
        public async Task EditAsync_WithModel_ReturnsClubEdited()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            ClubDTO clubDto = new ClubDTO
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
            ClubProfileDTO clubProfileDto = new ClubProfileDTO
            {
                Club = new ClubDTO
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
        public async Task GetAllAsync_ReturnsAllClubs()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.Club.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessClub.Club>, IIncludableQueryable<DataAccessClub.Club, object>>>()))
                .ReturnsAsync(() => new List<DataAccessClub.Club>());

            // Act
            var result = await _clubService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<DataAccessClub.Club>>(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsClubsByName()
        {
            // Arrange
            _repoWrapper.Setup(rw => rw.Club.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessClub.Club>, IIncludableQueryable<DataAccessClub.Club, object>>>()))
                .ReturnsAsync(() => new List<DataAccessClub.Club>());

            // Act
            var result = await _clubService.GetAllAsync(ClubName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<DataAccessClub.Club>>(result);
        }

        [Test]
        public async Task GetAllDTOAsync_ReturnsAllDTO()
        {
            // Arrange
            _mapper
                .Setup(m => m.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubDTO>>(It.IsAny<IEnumerable<DataAccessClub.Club>>()))
                .Returns(GetTestClubDTO());
            _repoWrapper
                .Setup(r => r.Club.GetAllAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccessClub.Club>, IIncludableQueryable<DataAccessClub.Club, object>>>()))
                .ReturnsAsync(GetTestClub());

            // Act
            var result = await _clubService.GetAllDTOAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<ClubDTO>>(result);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsClub()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());
            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns(new ClubDTO());

            // Act
            var result = await _clubService.GetByIdAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubDTO>(result);
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

            // Act
            var result = await clubService.GetClubProfileAsync(Id, It.IsAny<DataAccessClub.User>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubProfileDTO>(result);
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
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task GetClubAdminsAsync_WhereAdminEndDateIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
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
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithExAdmin(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubAdminsAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Head);
            Assert.AreEqual(result.Admins, new List<ClubAdministrationDTO>());
        }

        [Test]
        public async Task GetClubAdminsAsync_WithClubIsNull_ReturnsNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDTO)null);

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
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task GetClubDocumentsAsync_WithClubIsNull_ReturnsNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDTO)null);

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
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task GetClubFollowersAsync_WithClubIsNull_ReturnsNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDTO)null);

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
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task GetClubMembersAsync_WithClubIsNull_ReturnsNull()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDTO)null);

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
            Assert.IsInstanceOf<ClubProfileDTO>(result);
        }

        [Test]
        public async Task GetClubProfileAsync_WhereAdminEndDateIsNull_ReturnClubProfile()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
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
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns((ClubDTO)null);

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
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithExAdmin(Count).FirstOrDefault());

            // Act
            var result = await clubService.GetClubProfileAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Head);
            Assert.AreEqual(result.Admins, new List<ClubAdministrationDTO>());
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
            Assert.IsInstanceOf<IEnumerable<ClubForAdministrationDTO>>(result);
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
        public async Task RemoveAsync()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _clubBlobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.Club.Delete(It.IsAny<DataAccessClub.Club>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await clubService.RemoveAsync(It.IsAny<int>());

            // Assert
            _clubBlobStorage.Verify(c => c.DeleteBlobAsync(It.IsAny<string>()), Times.Once);
            _repoWrapper.Verify(r => r.Club.Delete(It.IsAny<DataAccessClub.Club>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_WithoutLogo()
        {
            // Arrange
            ClubService clubService = CreateClubService();
            _repoWrapper.Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(GetTestClubWithoutLogo());
            _clubBlobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.Club.Delete(It.IsAny<DataAccessClub.Club>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await clubService.RemoveAsync(It.IsAny<int>());

            // Assert
            _clubBlobStorage.Verify(c => c.DeleteBlobAsync(It.IsAny<string>()), Times.Never);
            _repoWrapper.Verify(r => r.Club.Delete(It.IsAny<DataAccessClub.Club>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
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
            _uniqueId = new Mock<IUniqueIdService>();
            _userManager = new Mock<UserManager<User>>(_user.Object, null, null, null, null, null, null, null, null);
            _clubService = new ClubService(_repoWrapper.Object, _mapper.Object, _env.Object, _clubBlobStorage.Object,
                   _clubAccessService.Object, _userManager.Object, _uniqueId.Object);
        }

        private ClubService CreateClubService()
        {
            _mapper.Setup(m => m.Map<IEnumerable<DataAccessClub.Club>,
                    IEnumerable<ClubDTO>>(It.IsAny<IEnumerable<DataAccessClub.Club>>()))
                .Returns(CreateFakeClubDto(10));
            _mapper.Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDto(10).FirstOrDefault());
            _mapper.Setup(m => m.Map<ClubDTO, DataAccessClub.Club>(It.IsAny<ClubDTO>()))
                .Returns(() => new DataAccessClub.Club());
            _repoWrapper.Setup(r => r.Club.FindAll())
                .Returns(CreateFakeCities(10));
            _repoWrapper.Setup(r => r.Club.FindByCondition(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>()))
                .Returns((Expression<Func<DataAccessClub.Club, bool>> condition) =>
                    CreateFakeCities(10).Where(condition));
            _repoWrapper.Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(), null))
               .ReturnsAsync(GetTestRegion());
            _mapper.Setup(m => m.Map<ClubDTO, DataAccessClub.Club>(It.IsAny<ClubDTO>()))
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

            return new ClubService(_repoWrapper.Object, _mapper.Object, _env.Object, _clubBlobStorage.Object, _clubAccessService.Object, _userManager.Object, _uniqueId.Object);
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

        private IQueryable<ClubDTO> CreateFakeClubDto(int count)
        {
            List<ClubDTO> clubs = new List<ClubDTO>();

            for (int i = 0; i < count; i++)
            {
                clubs.Add(new ClubDTO
                {
                    ClubAdministration = GetClubAdministrationDTO(),
                    ClubMembers = new List<ClubMembersDTO>
                    {
                        new ClubMembersDTO
                        {
                            StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null,
                            IsApproved = true,
                            UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                            User=new ClubUserDTO()
                        }
                    },
                    ClubDocuments = new List<ClubDocumentsDTO>
                    {
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO()
                    },
                });
            }
            return clubs.AsQueryable();
        }

        private IQueryable<ClubDTO> CreateFakeClubDtoWithExAdmin(int count)
        {
            List<ClubDTO> clubs = new List<ClubDTO>();

            for (int i = 0; i < count; i++)
            {
                var clubDto = GetClubDto();
                clubDto.ClubAdministration = new List<ClubAdministrationDTO>
                {
                    new ClubAdministrationDTO
                    {
                        UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDTO(),
                        AdminType = new AdminTypeDTO
                        {
                            AdminTypeName = "Голова Куреня"
                        },
                        EndDate = DateTime.Now.AddMonths(-3)
                    },
                    new ClubAdministrationDTO
                    {
                       UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDTO(),
                        AdminType = new AdminTypeDTO
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

        private IQueryable<ClubDTO> CreateFakeClubDtoWithoutMembersWithoutAdminEndDate(int count)
        {
            List<ClubDTO> clubs = new List<ClubDTO>();

            for (int i = 0; i < count; i++)
            {
                var clubDto = GetClubDtoWithoutMembers();
                clubDto.ClubMembers = new List<ClubMembersDTO>();
                clubDto.ClubAdministration = new List<ClubAdministrationDTO>
                {
                    new ClubAdministrationDTO
                    {
                        UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDTO(),
                        AdminType = new AdminTypeDTO
                        {
                            AdminTypeName = "Голова Куреня"
                        }
                    },
                    new ClubAdministrationDTO
                    {
                        UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDTO(),
                        AdminType = new AdminTypeDTO
                        {
                            AdminTypeName = "----------",
                        },
                    }
                };
                clubs.Add(clubDto);
            }
            return clubs.AsQueryable();
        }

        private List<ClubAdministrationDTO> GetClubAdministrationDTO()
        {
            return new List<ClubAdministrationDTO>
            {
                 new ClubAdministrationDTO
                 {
                      UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                      User = new ClubUserDTO(),
                      AdminType = new AdminTypeDTO
                      {
                           AdminTypeName = "Голова Куреня"
                      }
                 },
                 new ClubAdministrationDTO
                 {
                      UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                      User = new ClubUserDTO(),
                      AdminType = new AdminTypeDTO
                      {
                           AdminTypeName = "----------"
                      }
                 }
            };
        }

        private ClubDTO GetClubDto()
        {
            var club = GetClubDtoWithoutMembers();
            club.ClubMembers = new List<ClubMembersDTO>
                {
                    new ClubMembersDTO
                    {
                        UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDTO(),
                        StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null
                    }
                };
            return club;
        }

        private ClubDTO GetClubDtoWithoutMembers()
        {
            return new ClubDTO
            {
                ClubAdministration = GetClubAdministrationDTO(),
                ClubDocuments = new List<ClubDocumentsDTO>
                    {
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO()
                    }
            };
        }

        private IEnumerable<DataAccessClub.Club> GetTestClub()
        {
            return new List<DataAccessClub.Club>
            {
                new DataAccessClub.Club{
                    Name = "Львів",
                    ClubAdministration = new List<ClubAdministration>(),
                    ClubMembers = new List<ClubMembers>(),
                    ClubDocuments = new List<ClubDocuments>(),
                    },
                new DataAccessClub.Club{
                    Name = "Стрий",
                    ClubAdministration = new List<ClubAdministration>(),
                    ClubMembers = new List<ClubMembers>(),
                    ClubDocuments = new List<ClubDocuments>(),
                }
            }.AsEnumerable();
        }

        private IEnumerable<ClubDTO> GetTestClubDTO()
        {
            return new List<ClubDTO>
            {
                new ClubDTO{Name = "Львів"},
                new ClubDTO{Name = "Стрий"},
                new ClubDTO{Name = "Миколаїв"}
            }.AsEnumerable();
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
