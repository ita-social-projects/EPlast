using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
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
using EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.Tests.Services.Club.ClubAccess
{
    class ClubAccessServiceTest
    {
        private ClubAccessService _clubAccessService;
        private Mock<UserManager<DatabaseEntities.User>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private ClubAccessSettings _clubAccessSettings;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        private const string AdminRoleName = Roles.Admin;
        private const string ClubAdminRoleName = Roles.KurinHead;

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
        public async Task GetClubsAsync_Admin_Passed()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { AdminRoleName });
            _mockRepositoryWrapper.Setup(x => x.Club.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>
                >>())).ReturnsAsync(new List<DatabaseEntities.Club> { new DatabaseEntities.Club() });

            // Act
            await _clubAccessService.GetClubsAsync(new User());

            // Assert
            _mockMapper.Verify(x =>
                x.Map<IEnumerable<DatabaseEntities.Club>, IEnumerable<ClubDTO>>(
                    It.IsAny<IEnumerable<DatabaseEntities.Club>>()));
        }

        [Test]
        public async Task GetClubsAsync_ClubAdmin_Passed()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { ClubAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.ClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.ClubAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.ClubAdministration());
            _mockRepositoryWrapper.Setup(r => r.Club.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>>>()))
                .ReturnsAsync(new List<DatabaseEntities.Club> { new DatabaseEntities.Club() });


            // Act
            await _clubAccessService.GetClubsAsync(new User());

            // Assert
            _mockMapper.Verify(x =>
                x.Map<IEnumerable<DatabaseEntities.Club>, IEnumerable<ClubDTO>>(
                    It.IsAny<IEnumerable<DatabaseEntities.Club>>()));
        }

        [Test]
        public async Task GetClubsAsync_ClubAdmin_EmptyList()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { ClubAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.ClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.ClubAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.ClubAdministration)null);
            _mockRepositoryWrapper.Setup(r => r.Club.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>>>()))
                .ReturnsAsync(new List<DatabaseEntities.Club> { new DatabaseEntities.Club() });


            // Act
            await _clubAccessService.GetClubsAsync(new User());

            // Assert
            _mockMapper.Verify(x =>
                x.Map<IEnumerable<DatabaseEntities.Club>, IEnumerable<ClubDTO>>(
                    It.IsAny<IEnumerable<DatabaseEntities.Club>>()));
        }

        [Test]
        public async Task GetClubsAsync_NoRoles()
        {
            //Arrange
            _mockUserManager.Setup(x =>
                x.GetRolesAsync(It.IsAny<DatabaseEntities.User>())).ReturnsAsync(Enumerable.Empty<string>().ToList());

            //Act
            var result = await _clubAccessService.GetClubsAsync(new User());

            //Assert
            _mockUserManager.Verify(x =>
                x.GetRolesAsync(It.IsAny<DatabaseEntities.User>()));
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllClubsIdAndName_Admin_Passed()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { AdminRoleName });
            _mockRepositoryWrapper.Setup(x => x.Club.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>
                >>())).ReturnsAsync(new List<DatabaseEntities.Club> { new DatabaseEntities.Club(){Name = "TestClub"} });

            // Act
            await _clubAccessService.GetAllClubsIdAndName(new User());

            // Assert
            _mockRepositoryWrapper.Verify(x => x.Club.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>
                >>()), Times.Once);
        }

        [Test]
        public async Task GetAllClubsIdAndName_ClubAdmin_Passed()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { ClubAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.ClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.ClubAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.ClubAdministration());
            _mockRepositoryWrapper.Setup(r => r.Club.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>>>()))
                .ReturnsAsync(new List<DatabaseEntities.Club> { new DatabaseEntities.Club() });


            // Act
            await _clubAccessService.GetAllClubsIdAndName(new User());

            // Assert
            _mockRepositoryWrapper.Verify(r => r.Club.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>>>()), Times.Once);
        }

        [Test]
        public async Task GetAllClubsIdAndName_ClubAdmin_EmptyList()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { ClubAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.ClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.ClubAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.ClubAdministration)null);
            _mockRepositoryWrapper.Setup(r => r.Club.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>>>()))
                .ReturnsAsync(new List<DatabaseEntities.Club> { new DatabaseEntities.Club() });


            // Act
            await _clubAccessService.GetAllClubsIdAndName(new User());

            // Assert
            _mockRepositoryWrapper.Verify(r => r.Club.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.Club>, IIncludableQueryable<DatabaseEntities.Club, object>>>()), Times.Never);
        }

        [Test]
        public async Task GetAllClubsIdAndName_NoRoles()
        {
            //Arrange
            _mockUserManager.Setup(x =>
                x.GetRolesAsync(It.IsAny<DatabaseEntities.User>())).ReturnsAsync(Enumerable.Empty<string>().ToList());

            //Act
            var result = await _clubAccessService.GetAllClubsIdAndName(new User());

            //Assert
            _mockUserManager.Verify(u => u.GetRolesAsync(It.IsAny<User>()));
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task HasAccessAsync_ReturnsTrue()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { ClubAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.ClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.ClubAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.ClubAdministration());
            _mockRepositoryWrapper.Setup(r => r.Club.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.Club> { new DatabaseEntities.Club() { ID = 1 } });
            _mockMapper.Setup(m => m.Map<IEnumerable<DatabaseEntities.Club>, IEnumerable<ClubDTO>>(It.IsAny<IEnumerable<DatabaseEntities.Club>>()))
                .Returns(new List<ClubDTO> { new ClubDTO() { ID = 1 } });

            // Act
            var result = await _clubAccessService.HasAccessAsync(new User(), 1);

            // Assert
            Assert.True(result);

        }

        [Test]
        public async Task HasAccessAsync_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { ClubAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.ClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.ClubAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.ClubAdministration());
            _mockRepositoryWrapper.Setup(r => r.Club.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.Club, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.Club> { new DatabaseEntities.Club() { ID = 1 } });
            _mockMapper.Setup(m => m.Map<IEnumerable<DatabaseEntities.Club>, IEnumerable<ClubDTO>>(It.IsAny<IEnumerable<DatabaseEntities.Club>>()))
                .Returns(new List<ClubDTO> { new ClubDTO() { ID = 1 } });
            //Act
            var result = await _clubAccessService.HasAccessAsync(new User(), 2);

            //Assert
            _mockUserManager.Verify();
            Assert.NotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task HasAccessAsync_TakesOneParametr_True()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { ClubAdminRoleName });

            // Act
            var result = await _clubAccessService.HasAccessAsync(new User());

            // Assert
            Assert.True(result);
        }

        [Test]
        public async Task HasAccessAsync_TakesOneParametr_False()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());

            // Act
            var result = await _clubAccessService.HasAccessAsync(new User());

            // Assert
            Assert.False(result);
        }
        

        private AdminType _adminType => new AdminType() { };
    }
}
