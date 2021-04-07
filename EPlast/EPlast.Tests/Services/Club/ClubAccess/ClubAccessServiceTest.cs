using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Services.Club.ClubAccess;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Repositories;
using DatabaseEntities = EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore.Query;

namespace EPlast.Tests.Services.Club.ClubAccess
{
    class ClubAccessServiceTest
    {
        private ClubAccessService _clubAccessService;
        private Mock<UserManager<DatabaseEntities.User>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private ClubAccessSettings _clubAccessSettings;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        [SetUp]
        public void SetUp()
        {
            Mock<IUserStore<DatabaseEntities.User>> _mockStore = new Mock<IUserStore<DatabaseEntities.User>>();
            _mockUserManager = new Mock<UserManager<DatabaseEntities.User>>(_mockStore.Object, null, null, null, null, null, null, null, null);
            
            _mockMapper = new Mock<IMapper>();
            
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            _mockRepositoryWrapper.Setup(x => x.AdminType.GetFirstAsync(
                    It.IsAny<Expression<Func<DatabaseEntities.AdminType, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.AdminType>, IIncludableQueryable<DatabaseEntities.AdminType, object>>>()))
                .ReturnsAsync(_adminType);

            _clubAccessSettings = new ClubAccessSettings(_mockRepositoryWrapper.Object);
            

            _clubAccessService = new ClubAccessService(_clubAccessSettings, _mockUserManager.Object, _mockMapper.Object);
        }

        [Test]
        public void GetClubsAsync_ReturnsExpectedList()
        {
            //Arrange
            _mockUserManager.Setup(x =>
                x.GetRolesAsync(It.IsAny<DatabaseEntities.User>())).ReturnsAsync(_userRoles);

            _mockRepositoryWrapper.Setup(x => x.Club.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>
                >>())).ReturnsAsync(_clubs);

            _mockMapper.Setup(x =>
                x.Map<IEnumerable<DatabaseEntities.Club>, IEnumerable<ClubDTO>>(
                    It.IsAny<IEnumerable<DatabaseEntities.Club>>())).Returns(_cities);

            //Act
            var result = _clubAccessService.GetClubsAsync(_user);
            var actual = result.Result as IEnumerable<ClubDTO>;

            //Assert
            _mockUserManager.Verify();
            _mockRepositoryWrapper.Verify();
            _mockMapper.Verify();
            Assert.IsInstanceOf<Task<IEnumerable<ClubDTO>>>(result);
            Assert.NotNull(result);
            Assert.AreEqual(_cities.Count(), actual.Count());
            Assert.AreEqual(_cities.FirstOrDefault().ID, actual.FirstOrDefault().ID);
        }

        [Test]
        public void GetClubsAsync_ReturnsEmptyList()
        {
            //Arrange
            _mockUserManager.Setup(x =>
                x.GetRolesAsync(It.IsAny<DatabaseEntities.User>())).ReturnsAsync(_emptyUserRoles);

            //Act
            var result = _clubAccessService.GetClubsAsync(_user);

            //Assert
            _mockUserManager.Verify();
            Assert.IsInstanceOf<Task<IEnumerable<ClubDTO>>>(result);
            Assert.NotNull(result);
            Assert.IsEmpty(result.Result);
        }

        [Test]
        public void HasAccessAsync_ReturnsTrue()
        {
            //Arrange
            _mockUserManager.Setup(x =>
                x.GetRolesAsync(It.IsAny<DatabaseEntities.User>())).ReturnsAsync(_userRoles);

            _mockRepositoryWrapper.Setup(x => x.Club.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>
                >>())).ReturnsAsync(_clubs);

            _mockMapper.Setup(x =>
                x.Map<IEnumerable<DatabaseEntities.Club>, IEnumerable<ClubDTO>>(
                    It.IsAny<IEnumerable<DatabaseEntities.Club>>())).Returns(_cities);

            //Act
            var result = _clubAccessService.HasAccessAsync(_user, _clubId);

            //Assert
            _mockUserManager.Verify();
            _mockRepositoryWrapper.Verify();
            _mockMapper.Verify();
            Assert.NotNull(result);
            Assert.IsTrue(result.Result);

        }

        [Test]
        public void HasAccessAsync_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(x =>
                x.GetRolesAsync(It.IsAny<DatabaseEntities.User>())).ReturnsAsync(_emptyUserRoles);

            //Act
            var result = _clubAccessService.HasAccessAsync(_user, _clubId);

            //Assert
            _mockUserManager.Verify();
            Assert.NotNull(result);
            Assert.IsFalse(result.Result);
        }

        private int _clubId = 3;

        private DatabaseEntities.AdminType _adminType => new DatabaseEntities.AdminType() { };
        private DatabaseEntities.User _user => new DatabaseEntities.User() { };

        private IEnumerable<DatabaseEntities.Club> _clubs => new List<DatabaseEntities.Club>(){};

        private readonly List<string> _userRoles = new List<string>() {"Admin"};

        private readonly List<string> _emptyUserRoles = new List<string>();

        private IEnumerable<ClubDTO> _cities => new List<ClubDTO>() {new ClubDTO() {ID = _clubId}};
    }
}
