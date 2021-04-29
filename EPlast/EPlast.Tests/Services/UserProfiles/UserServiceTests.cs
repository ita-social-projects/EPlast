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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.UserProfiles
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private Mock<IUserPersonalDataService> _mockUserPersonalDataService;
        private Mock<IUserBlobStorageRepository> _mockUserBlobStorage;
        private Mock<IWebHostEnvironment> _mockEnv;
        private Mock<IUniqueIdService> _mockUniqueId;
        private UserDTO _userDTO;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _mockMapper = new Mock<IMapper>();
            _mockUserPersonalDataService = new Mock<IUserPersonalDataService>();
            _mockUserBlobStorage = new Mock<IUserBlobStorageRepository>();
            _mockEnv = new Mock<IWebHostEnvironment>();
            _mockUniqueId = new Mock<IUniqueIdService>();
            _userService = new UserService(_mockRepoWrapper.Object, _mockUserManager.Object, _mockMapper.Object, _mockUserPersonalDataService.Object, _mockUserBlobStorage.Object, _mockEnv.Object, _mockUniqueId.Object);

            _userDTO = new UserDTO();
        }

        [Test]
        public void GetCityAdminConfirmedUser_Valid_GetConfirmedUserDTO()
        {
            //Arrange
            _userDTO.ConfirmedUsers = new List<ConfirmedUserDTO>()
            {
                new ConfirmedUserDTO()
                {
                    isCityAdmin = true
                }
            };

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
            _userDTO.ConfirmedUsers = new List<ConfirmedUserDTO>()
            {
                new ConfirmedUserDTO()
                {
                    isCityAdmin = false
                }
            };

            // Act
            var result = _userService.GetCityAdminConfirmedUser(_userDTO);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetClubAdminConfirmedUser_Valid_GetConfirmedUserDTO()
        {
            //Arrange
            _userDTO.ConfirmedUsers = new List<ConfirmedUserDTO>()
            {
                new ConfirmedUserDTO()
                {
                    isClubAdmin = true
                }
            };

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
            _userDTO.ConfirmedUsers = new List<ConfirmedUserDTO>()
            {
                new ConfirmedUserDTO()
                {
                    isClubAdmin = false
                }
            };

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
        public async Task GetUserGenderAsync_Valid_GetGenderAsync()
        {
            //Arrange
            string userId = "Id";
            User user = new User()
            {
                UserProfile = new UserProfile()
                {
                    Gender = new Gender()
                    {
                        Name = "Чоловік"
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
            Assert.AreEqual("Чоловік", result);
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
    }
}