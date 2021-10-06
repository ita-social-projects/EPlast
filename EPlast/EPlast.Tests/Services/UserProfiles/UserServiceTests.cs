﻿using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.Resources;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;

namespace EPlast.Tests.Services.UserProfiles
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private Mock<IUserPersonalDataService> _mockUserPersonalDataService;
        private Mock<IUserBlobStorageRepository> _mockUserBlobStorage;
        private Mock<IWebHostEnvironment> _mockEnv;
        private Mock<IUniqueIdService> _mockUniqueId;
        private UserDTO _userDTO;
        private ConfirmedUserDTO _confirmedUserDTO;
        private Mock<IUserManagerService> _mockUserManageService;
        Mock<UserDTO> _currentUser;
        Mock<UserDTO> _focusUser;

        [SetUp]
        public void SetUp()
        {
            _currentUser = new Mock<UserDTO>();
            _focusUser = new Mock<UserDTO>();
            _currentUser.Object.RegionAdministrations = new List<RegionAdministration>() { new RegionAdministration() { RegionId = 1 }, };
            _focusUser.Object.RegionAdministrations = new List<RegionAdministration>() { new RegionAdministration() { RegionId = 2 }, };
            _currentUser.Object.CityMembers = new List<CityMembers>() { new CityMembers() { City = new DataAccess.Entities.City() { RegionId = 1 } }, };
            _focusUser.Object.CityMembers = new List<CityMembers>() { new CityMembers() { City = new DataAccess.Entities.City() { RegionId = 2 } }, };
            _currentUser.Object.ClubMembers = new List<ClubMembers>() { new ClubMembers() { Club = new DataAccess.Entities.Club(), UserId = "1", }, };
            _focusUser.Object.ClubMembers = new List<ClubMembers>() { new ClubMembers() { Club = new DataAccess.Entities.Club(), UserId = "1" }, };
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockUserPersonalDataService = new Mock<IUserPersonalDataService>();
            _mockUserBlobStorage = new Mock<IUserBlobStorageRepository>();
            _mockEnv = new Mock<IWebHostEnvironment>();
            _mockUniqueId = new Mock<IUniqueIdService>();
            _mockUserManageService = new Mock<IUserManagerService>();
            _userService = new UserService(_mockRepoWrapper.Object, _mockMapper.Object, _mockUserPersonalDataService.Object, _mockUserBlobStorage.Object, _mockEnv.Object,_mockUserManageService.Object , _mockUniqueId.Object);
            _confirmedUserDTO = new ConfirmedUserDTO();
            _userDTO = new UserDTO()
            {
                ConfirmedUsers = new List<ConfirmedUserDTO>()
                {
                    _confirmedUserDTO
                }
            };
        }

        [Test]
        public void GetCityAdminConfirmedUser_Valid_GetConfirmedUserDTO()
        {
            //Arrange
            _confirmedUserDTO.isCityAdmin = true;

            // Act
            var result = _userService.GetCityAdminConfirmedUser(_userDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ConfirmedUserDTO>(result);
        }

        [Test]
        public void GetCityAdminConfirmedUser_InValid_GetConfirmedUserDTO()
        {
            //Arrange
            _confirmedUserDTO.isCityAdmin = false;

            // Act
            var result = _userService.GetCityAdminConfirmedUser(_userDTO);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetClubAdminConfirmedUser_Valid_GetConfirmedUserDTO()
        {
            //Arrange
            _confirmedUserDTO.isClubAdmin = true;

            //Act
            var result = _userService.GetClubAdminConfirmedUser(_userDTO);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ConfirmedUserDTO>(result);
        }

        [Test]
        public void GetClubAdminConfirmedUser_InValid_GetConfirmedUserDTO()
        {
            //Arrange
            _confirmedUserDTO.isCityAdmin = false;

            //Act
            var result = _userService.GetClubAdminConfirmedUser(_userDTO);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task IsApprovedCityMember_Valid_GetTrueAsync()
        {
            //Arrange
            string userId = "Id";
            CityMembers cityMembers = new CityMembers() { IsApproved = true };
            _mockRepoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>,
                IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(cityMembers);

            //Act
            var result = await _userService.IsApprovedCityMember(userId);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsApprovedCityMember_Valid_GetFalseAsync()
        {
            //Arrange
            string userId = "Id";
            CityMembers cityMembers = new CityMembers() { IsApproved = false };
            _mockRepoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>,
                IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(cityMembers);

            //Act
            var result = await _userService.IsApprovedCityMember(userId);

            //Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task IsApprovedCLubMember_Valid_GetFalseAsync()
        {
            //Arrange
            string userId = "Id";
            ClubMembers clubMembers = new ClubMembers() { IsApproved = false };
            _mockRepoWrapper
                .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubMembers>,
                        IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(clubMembers);

            //Act
            var result = await _userService.IsApprovedCLubMember(userId);

            //Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task IsApprovedCLubMember_Valid_GetTrueAsync()
        {
            //Arrange
            string userId = "Id";
            ClubMembers clubMembers = new ClubMembers() { IsApproved = true };
            _mockRepoWrapper
                .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubMembers>,
                        IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(clubMembers);

            //Act
            var result = await _userService.IsApprovedCLubMember(userId);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetUserGenderAsync_Valid_GetGenderAsync()
        {
            //Arrange
            string userId = "Id";
            var user = new User
            {
                UserProfile = new UserProfile
                {
                    Gender = new Gender
                    {
                        Name = UserGenders.Male
                    }
                }
            };
            _mockRepoWrapper
                .Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(user);

            //Act
            var result = await _userService.GetUserGenderAsync(userId);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(UserGenders.Male, result);
        }

        [Test]
        public async Task GetUserGenderAsync_Null_GetGenderAsync()
        {
            //Arrange
            var user = new User
            {
                UserProfile = new UserProfile()
            };
            _mockRepoWrapper
                .Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(user);

            //Act
            var result = await _userService.GetUserGenderAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(UserGenders.Undefined, result);
        }

        [Test]
        public async Task GetImageBase64Async_Valid_GetImageAsync()
        {
            //Arrange
            string fileName = "name";
            _mockUserBlobStorage
                .Setup(x => x.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync("ImageBase64");

            //Act
            var result = await _userService.GetImageBase64Async(fileName);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void IsUserSameCity_ReturnsTrue()
        {
            //Arrange
            Mock<UserDTO> _currentUser = new Mock<UserDTO>();
            Mock<UserDTO> _focusUser = new Mock<UserDTO>();

            _currentUser.Object.CityMembers = new List<CityMembers>() {new CityMembers() {CityId = 1},};
            _focusUser.Object.CityMembers = new List<CityMembers>() { new CityMembers() { CityId = 1 }, };

            //Act
            var result = _userService.IsUserSameCity(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsUserSameCity_ReturnsFalse()
        {
            //Arrange
            Mock<UserDTO> _currentUser = new Mock<UserDTO>();
            Mock<UserDTO> _focusUser = new Mock<UserDTO>();

            _currentUser.Object.CityMembers = new List<CityMembers>() { new CityMembers() { CityId = 1 }, };
            _focusUser.Object.CityMembers = new List<CityMembers>() { new CityMembers() { CityId = 2 }, };

            //Act
            var result = _userService.IsUserSameCity(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsUserSameClub_ReturnsTrue()
        {
            //Arrange
            Mock<UserDTO> _currentUser = new Mock<UserDTO>();
            Mock<UserDTO> _focusUser = new Mock<UserDTO>();

            _currentUser.Object.ClubMembers = new List<ClubMembers>() { new ClubMembers() { ClubId = 1 }, };
            _focusUser.Object.ClubMembers = new List<ClubMembers>() { new ClubMembers() { ClubId = 1 }, };

            //Act
            var result = _userService.IsUserSameClub(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsUserSameClub_ReturnsFalse()
        {
            //Arrange
            Mock<UserDTO> _currentUser = new Mock<UserDTO>();
            Mock<UserDTO> _focusUser = new Mock<UserDTO>();

            _currentUser.Object.ClubMembers = new List<ClubMembers>() { new ClubMembers() { ClubId = 1 }, };
            _focusUser.Object.ClubMembers = new List<ClubMembers>() { new ClubMembers() { ClubId = 2 }, };

            //Act
            var result = _userService.IsUserSameClub(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsUserSameRegion_ReturnsTrue()
        {
            //Arrange
            Mock<UserDTO> _currentUser = new Mock<UserDTO>();
            Mock<UserDTO> _focusUser = new Mock<UserDTO>();

            _currentUser.Object.RegionAdministrations = new List<RegionAdministration>() { new RegionAdministration() { RegionId = 1 }, };
            _focusUser.Object.RegionAdministrations = new List<RegionAdministration>() { new RegionAdministration() { RegionId = 1 }, };

            //Act
            var result = _userService.IsUserSameRegion(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsUserSameRegion_ReturnsFalse()
        {
            //Arrange
            Mock<UserDTO> _currentUser = new Mock<UserDTO>();
            Mock<UserDTO> _focusUser = new Mock<UserDTO>();

            _currentUser.Object.RegionAdministrations = new List<RegionAdministration>() { new RegionAdministration() { RegionId = 1 }, };
            _focusUser.Object.RegionAdministrations = new List<RegionAdministration>() { new RegionAdministration() { RegionId = 2 }, };

            _currentUser.Object.CityMembers = new List<CityMembers>() { new CityMembers() { City = new DataAccess.Entities.City() { RegionId = 1 } }, };
            _focusUser.Object.CityMembers = new List<CityMembers>() { new CityMembers() { City = new DataAccess.Entities.City() { RegionId = 2 } }, };

            //Act
            var result = _userService.IsUserSameRegion(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckOrAddPlastunRoleTest_Valid()
        {
            //Arrange
            DateTime registeredOn = DateTime.MinValue;
            var timeToJoinPlast = registeredOn.AddYears(1) - DateTime.Now;
            _mockRepoWrapper.Setup(x => x.ConfirmedUser.FindByCondition(It.IsAny<Expression<Func<ConfirmedUser, bool>>>()))
               .Returns(new List<ConfirmedUser>().AsQueryable());

            // Act
            var result = _userService.CheckOrAddPlastunRole("1", DateTime.MinValue);

            // Assert
            Assert.IsInstanceOf<TimeSpan>(result);
        }
    
        [Test]
        public void CheckOrAddPlastunRole_ReturnsTimeToJoinPlast()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(x => (x.ConfirmedUser.FindByCondition(It.IsAny<Expression<Func<ConfirmedUser, bool>>>())))
                .Returns(new List<ConfirmedUser>() { new ConfirmedUser() { isClubAdmin = false } }.AsQueryable());

            var registeredOn = DateTime.Now - new TimeSpan(90, 0, 0, 0);

            //Act
            var res = _userService.CheckOrAddPlastunRole("0", registeredOn);


            //Assert
            Assert.IsTrue(res > TimeSpan.Zero);
        }


        [Test]
        public void CheckOrAddPlastunRole_UserIsAdmin_ReturnsTimeToJoinPlast()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(x => (x.ConfirmedUser.FindByCondition(It.IsAny<Expression<Func<ConfirmedUser, bool>>>())))
                .Returns(new List<ConfirmedUser>() { new ConfirmedUser() { isClubAdmin = true } }.AsQueryable());

            var registeredOn = DateTime.Now - new TimeSpan(160, 0, 0, 0);

            //Act
            var res = _userService.CheckOrAddPlastunRole("0", registeredOn);


            //Assert
            Assert.IsTrue(res > TimeSpan.Zero);
        }

        [Test]
        public void CheckOrAddPlastunRole_ThrowsException_ReturnsTimeSpanZero()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(x => (x.ConfirmedUser.FindByCondition(It.IsAny<Expression<Func<ConfirmedUser, bool>>>())))
                .Throws(new Exception());

            //Act
            var res = _userService.CheckOrAddPlastunRole("0", DateTime.Now);


            //Assert
            Assert.AreEqual(res,TimeSpan.Zero);
        }

        [Test]
        public async Task IsUserInClubAsync_ReturnsFalse()
        {
            //Arrange
            var user = new User
            {
                UserProfile = new UserProfile()
            };
            _mockRepoWrapper
                .Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(user);
            ClubMembers clubMembers = new ClubMembers() { IsApproved = true };
            _mockRepoWrapper
                .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubMembers>,
                        IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(clubMembers);
            _currentUser.SetupGet(x => x.Id).Returns("1");
            _currentUser.SetupSet(s => s.Id = It.IsAny<string>());
            _focusUser.SetupGet(x => x.Id).Returns("2");
            _focusUser.SetupSet(s => s.Id = It.IsAny<string>());
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_currentUser.Object, It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<bool>());
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_focusUser.Object, It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<bool>());

            //Act
            var result = await _userService.IsUserInClubAsync(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }
        [Test]
        public async Task IsUserInClubAsync_ReturnsTrue()
        {
            //Arrange
            var user = new User
            {
                UserProfile = new UserProfile()
            };
            _mockRepoWrapper
                .Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(user);
            ClubMembers clubMembers = new ClubMembers() { IsApproved = true };
            _mockRepoWrapper
                .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubMembers>,
                        IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(clubMembers);
            _currentUser.SetupGet(x => x.Id).Returns("1");
            _currentUser.SetupSet(s => s.Id = It.IsAny<string>());
            _focusUser.SetupGet(x => x.Id).Returns("2");
            _focusUser.SetupSet(s => s.Id = It.IsAny<string>());
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_currentUser.Object, "Дійсний член організації"))
                .ReturnsAsync(true);
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_focusUser.Object, "Дійсний член організації"))
                .ReturnsAsync(true);

            //Act
            var result = await _userService.IsUserInClubAsync(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }
        [Test]
        public async Task IsUserInCityAsync_ReturnsTrue()
        {
            //Arrange
            var user = new User
            {
                UserProfile = new UserProfile()
            };
            _mockRepoWrapper
                .Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(user);
            CityMembers cityMembers = new CityMembers() { IsApproved = true };
            _mockRepoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>,
                        IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(cityMembers);
            _currentUser.SetupGet(x => x.Id).Returns("1");
            _currentUser.SetupSet(s => s.Id = It.IsAny<string>());
            _focusUser.SetupGet(x => x.Id).Returns("2");
            _focusUser.SetupSet(s => s.Id = It.IsAny<string>());
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_currentUser.Object, "Дійсний член організації"))
                .ReturnsAsync(true);
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_focusUser.Object, "Дійсний член організації"))
                .ReturnsAsync(true);

            //Act
            var result = await _userService.IsUserInCityAsync(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }
        [Test]
        public async Task IsUserInCityAsync_ReturnsFalse()
        {
            //Arrange
            var user = new User
            {
                UserProfile = new UserProfile()
            };
            _mockRepoWrapper
                .Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(user);
            CityMembers cityMembers = new CityMembers() { IsApproved = true };
            _mockRepoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>,
                        IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(cityMembers);
            _currentUser.SetupGet(x => x.Id).Returns("1");
            _currentUser.SetupSet(s => s.Id = It.IsAny<string>());
            _focusUser.SetupGet(x => x.Id).Returns("2");
            _focusUser.SetupSet(s => s.Id = It.IsAny<string>());
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_currentUser.Object, It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<bool>());
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_focusUser.Object, It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<bool>());

            //Act
            var result = await _userService.IsUserInCityAsync(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }
        [Test]
        public async Task IsUserInRegionAsync_ReturnsFalse()
        {
            //Arrange
            var user = new User
            {
                UserProfile = new UserProfile()
            };
            _mockRepoWrapper
                .Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(user);
            RegionFollowers regionFollowers = new RegionFollowers();
            _mockRepoWrapper
                .Setup(x => x.RegionFollowers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionFollowers, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionFollowers>,
                        IIncludableQueryable<RegionFollowers, object>>>()))
                .ReturnsAsync(regionFollowers);
            _currentUser.SetupGet(x => x.Id).Returns("1");
            _currentUser.SetupSet(s => s.Id = It.IsAny<string>());
            _focusUser.SetupGet(x => x.Id).Returns("2");
            _focusUser.SetupSet(s => s.Id = It.IsAny<string>());
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_currentUser.Object, It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<bool>());
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_focusUser.Object, It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<bool>());

            //Act
            var result = await _userService.IsUserInRegionAsync(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }
        [Test]
        public async Task IsUserInRegionAsync_ReturnsTrue()
        {
            //Arrange
            var user = new User
            {
                UserProfile = new UserProfile()
            };
            _mockRepoWrapper
                .Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(user);
            RegionFollowers regionFollowers = new RegionFollowers();
            _mockRepoWrapper
                .Setup(x => x.RegionFollowers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionFollowers, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionFollowers>,
                        IIncludableQueryable<RegionFollowers, object>>>()))
                .ReturnsAsync(regionFollowers);
            _currentUser.Object.CityMembers = new List<CityMembers>() { new CityMembers() { City = new DataAccess.Entities.City() { RegionId = 1 } }, };
            _focusUser.Object.CityMembers = new List<CityMembers>() { new CityMembers() { City = new DataAccess.Entities.City() { RegionId = 1 } }, };
            _currentUser.SetupGet(x => x.Id).Returns("1");
            _currentUser.SetupSet(s => s.Id = It.IsAny<string>());
            _focusUser.SetupGet(x => x.Id).Returns("2");
            _focusUser.SetupSet(s => s.Id = It.IsAny<string>());
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_currentUser.Object, "Голова Округи"))
                .ReturnsAsync(true);
            _mockUserManageService
                .Setup((x) => x.IsInRoleAsync(_focusUser.Object, "Голова Округи"))
                .ReturnsAsync(true);

            //Act
            var result = await _userService.IsUserInRegionAsync(_currentUser.Object, _focusUser.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }
    }
}
