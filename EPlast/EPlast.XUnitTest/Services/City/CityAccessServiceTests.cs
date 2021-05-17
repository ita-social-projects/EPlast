using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.City.CityAccess;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.XUnitTest.Services.City
{
    public class CityAccessServiceTests
    {
        private const string AdminRoleName = Roles.Admin;
        private const string RegionAdminRoleName = Roles.OkrugaHead;
        private const string CityAdminRoleName = Roles.CityHead;

        private readonly Mock<IRepositoryWrapper> _repositoryWrapper = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<UserManager<User>> _userManager;
        private readonly ICityAccessService _cityAccessService;

        public CityAccessServiceTests()
        {
            var userStore = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            _repositoryWrapper.Setup(r => r.AdminType.GetFirstAsync(It.IsAny<Expression<Func<AdminType, bool>>>(), null))
                .ReturnsAsync(new AdminType());
            var cityAccessSettings = new CityAccessSettings(_repositoryWrapper.Object);
            _cityAccessService = new CityAccessService(cityAccessSettings, _userManager.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetCitiesAsyncAdmin()
        {
            // Arrange
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { AdminRoleName });
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(null, null))
                .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() });

            // Act
            await _cityAccessService.GetCitiesAsync(new User());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()));
        }

        [Fact]
        public async Task GetCitiesAsyncRegionAdmin()
        {
            // Arrange
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.RegionAdministration>, IIncludableQueryable<DatabaseEntities.RegionAdministration, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.RegionAdministration());
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                        .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() });

            // Act
            await _cityAccessService.GetCitiesAsync(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()));
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()));
        }

        [Fact]
        public async Task GetCitiesAsyncRegionAdminEmptyList()
        {
            // Arrange
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.RegionAdministration>, IIncludableQueryable<DatabaseEntities.RegionAdministration, object>>>()))
                    .ReturnsAsync((DatabaseEntities.RegionAdministration)null);
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() });

            // Act
            await _cityAccessService.GetCitiesAsync( new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null), Times.Never);
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()));
        }

        [Fact]
        public async Task GetCitiesAsyncCityAdmin()
        {
            // Arrange
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { CityAdminRoleName });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                    .ReturnsAsync(new DatabaseEntities.CityAdministration());
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                        .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() });

            // Act
            await _cityAccessService.GetCitiesAsync(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()));
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()));
        }

        [Fact]
        public async Task GetCitiesAsyncCityAdminEmptyList()
        {
            // Arrange
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { CityAdminRoleName });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                    .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() });

            // Act
            await _cityAccessService.GetCitiesAsync(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null), Times.Never);
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()));
        }

        [Fact]
        public async Task GetCitiesAsyncNoRoles()
        {
            // Arrange
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(Enumerable.Empty<string>().ToList());

            // Act
            await _cityAccessService.GetCitiesAsync(It.IsAny<User>());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()), Times.Never);
        }

        [Fact]
        public async Task HasAccessAsyncTrue()
        {
            // Arrange
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { CityAdminRoleName });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                    .ReturnsAsync(new DatabaseEntities.CityAdministration());
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() { ID = 1 } });
            _mapper.Setup(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(new List<CityDTO> { new CityDTO() { ID = 1 } });

            // Act
            var result = await _cityAccessService.HasAccessAsync(new User(), 1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task HasAccessAsyncFalse()
        {
            // Arrange
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { CityAdminRoleName });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                    .ReturnsAsync(new DatabaseEntities.CityAdministration());
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() { ID = 1 } });
            _mapper.Setup(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(new List<CityDTO> { new CityDTO() { ID = 1 } });

            // Act
            var result = await _cityAccessService.HasAccessAsync(new User(), 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task HasAccessAsync_NoRoles_ReturnsFalse()
        {
            // Arrange
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string>() { "...", });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityAdministration());
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() { ID = 1 } });
            _mapper.Setup(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(new List<CityDTO> { new CityDTO() { ID = 1 } });

            // Act
            var result = await _cityAccessService.HasAccessAsync(new User());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task HasAccessAsync_TakesOneParametr_True()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { CityAdminRoleName });

            // Act
            var result = await _cityAccessService.HasAccessAsync(new User());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task HasAccessAsync_TakesOneParametr_False()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());

            // Act
            var result = await _cityAccessService.HasAccessAsync(new User());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_NoRoles()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());
            
            //Act
            await _cityAccessService.GetAllCitiesIdAndName(It.IsAny<User>());

            // Assert
            _userManager.Verify(u => u.GetRolesAsync(It.IsAny<User>()));
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_CityAdminEmptyList()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { CityAdminRoleName });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((CityAdministration)null);
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() });

            // Act
            await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_CityAdmin_Succeeded()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { CityAdminRoleName });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityAdministration());
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() });

            // Act
            await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()));
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_RegionAdmin_EmptyList()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.RegionAdministration>, IIncludableQueryable<DatabaseEntities.RegionAdministration, object>>>()))
                .ReturnsAsync((DatabaseEntities.RegionAdministration)null);
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() });

            // Act
            await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_RegionAdmin_Passed()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<DatabaseEntities.User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.RegionAdministration>, IIncludableQueryable<DatabaseEntities.RegionAdministration, object>>>()))
                .ReturnsAsync(new DatabaseEntities.RegionAdministration());
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                .ReturnsAsync(new List<DatabaseEntities.City> { new DatabaseEntities.City() });

            // Act
            await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_Admin_Passed()
        {
            var expected = new DatabaseEntities.City() {Name = "TestCityName"};
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { AdminRoleName });
            _repositoryWrapper.Setup(r => r.City.GetAllAsync(null,
                    It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>
                    >>()))
                .ReturnsAsync(new List<DatabaseEntities.City> {expected,});


            // Act
            await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetAllAsync(null,
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>
                >>()), Times.Once);
        }
    }
}