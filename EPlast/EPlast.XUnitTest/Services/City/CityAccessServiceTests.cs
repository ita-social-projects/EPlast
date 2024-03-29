﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
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
using Xunit;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.XUnitTest.Services.City
{
    public class CityAccessServiceTests
    {
        private const string AdminRoleName = Roles.Admin;
        private const string RegionAdminRoleName = Roles.OkrugaHead;
        private const string CityAdminRoleName = Roles.CityHead;
        private const string CityAdminDeputyRoleName = Roles.CityHeadDeputy;
        private const string RegionAdminDeputyRoleName = Roles.OkrugaHeadDeputy;

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
            _cityAccessService = new CityAccessService(_repositoryWrapper.Object, cityAccessSettings, _userManager.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetCitiesAsyncAdmin()
        {
            // Arrange
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { AdminRoleName });
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null,
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(),
              null, null))
                .ReturnsAsync(new Tuple<IEnumerable<DatabaseEntities.City>, int>(GetTestCitiesForHandler(), 1));

            // Act
            await _cityAccessService.GetCitiesAsync(new User());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()));
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
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null,
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(),
              null, null))
                .ReturnsAsync(new Tuple<IEnumerable<DatabaseEntities.City>, int>(GetTestCitiesForHandler(), 1));

            // Act
            await _cityAccessService.GetCitiesAsync(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
              null,
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), null, null));

            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()));
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
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null,
               It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(),
               null, null))
                 .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));

            // Act
            await _cityAccessService.GetCitiesAsync( new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), It.IsAny<Expression<Func<DatabaseEntities.City, DatabaseEntities.City>>>(),
              null, It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(),
              null, null), Times.Never);

            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()));
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
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null,
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), null, null))
                .ReturnsAsync(new Tuple<IEnumerable<DatabaseEntities.City>, int>(GetTestCitiesForHandler(), 1));

            // Act
            await _cityAccessService.GetCitiesAsync(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null,
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), null, null));

            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()));
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
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(),
                null, null))
            .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));

            // Act
            await _cityAccessService.GetCitiesAsync(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), It.IsAny<Expression<Func<DatabaseEntities.City, DatabaseEntities.City>>>(),
             null, It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(),
             null, null), Times.Never);
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()));
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
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()), Times.Never);
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
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null,
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), null, null))
                .ReturnsAsync(new Tuple<IEnumerable<DatabaseEntities.City>, int>(GetTestCitiesForHandler(), 1));
            _mapper.Setup(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(new List<CityDto> { new CityDto() { ID = 1 } });
            
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
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null,
               It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), null, null))
                 .ReturnsAsync(new Tuple<IEnumerable<DatabaseEntities.City>, int>(GetTestCitiesForHandler(), 1));
            _mapper.Setup(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(new List<CityDto> { new CityDto() { ID = 1 } });

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
            _mapper.Setup(m => m.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(new List<CityDto> { new CityDto() { ID = 1 } });

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
            var expectedEmpty = Enumerable.Empty<CityForAdministrationDto>();
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});

            //Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(It.IsAny<User>());

            // Assert
            _userManager.Verify(u => u.GetRolesAsync(It.IsAny<User>()));
            Assert.Equal(result, expectedEmpty);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_CityAdminEmptyList()
        {
            //Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { CityAdminRoleName });

            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                   .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                 .ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((CityAdministration)null);
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(),
                null, null))
            .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));

            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);

            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), It.IsAny<Expression<Func<DatabaseEntities.City, DatabaseEntities.City>>>(),
               null, It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(),
               null, null), Times.Never);
            Assert.Equal(result, _expected);
        }



        [Fact]
        public async Task GetAllCitiesIdAndName_CityAdmin_Succeeded()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { CityAdminRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityAdministration());
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null,
             It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
             It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), null, null))
               .ReturnsAsync(new Tuple<IEnumerable<DatabaseEntities.City>, int>(GetTestCitiesForHandler(), 1));
            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>())).Returns(_expected);

            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null,
             It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
             It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), null, null));
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_CityAdminDeputyEmptyList()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { CityAdminDeputyRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((CityAdministration)null);
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                  It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
                  It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(),
                  null, null))
              .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));

            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);

            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                It.IsAny<Expression<Func<DatabaseEntities.City, DatabaseEntities.City>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            Assert.Equal(result, _expected);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_CityAdminDeputy_Succeeded()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { CityAdminDeputyRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityAdministration());
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                  It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
                  It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(), null, null))
              .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));

            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);

            // Act
            await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null,
               It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), null, null));
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_RegionAdminEmptyList()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(), null))
                .ReturnsAsync((RegionAdministration)null);
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                 It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
                 It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(),
                 null, null))
             .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));

            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);

            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                 It.IsAny<Expression<Func<DatabaseEntities.City, DatabaseEntities.City>>>(),
                 It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
                 It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            Assert.Equal(result, _expected);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_RegionAdmin_Succeeded()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.RegionAdministration());
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                 It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
                 It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(), null, null))
             .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));

            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);


            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            Assert.Equal(result, _expected);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_RegionAdminDeputyEmptyList()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminDeputyRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(), null))
                .ReturnsAsync((RegionAdministration)null);

            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                            It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
                            It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(),
                            null, null))
                  .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));

            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);

            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                 It.IsAny<Expression<Func<DatabaseEntities.City, DatabaseEntities.City>>>(),
                 It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
                 It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);

            Assert.Equal(result, _expected);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_RegionAdminDeputyAndCityAdminDeputy_Succeeded()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminDeputyRoleName, CityAdminDeputyRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.RegionAdministration());
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
              It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
              It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(),
              null, null))
            .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));
            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);

            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            Assert.Equal(result, _expected);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_RegionAdminDeputyAndCityAdminDeputy_EmptyList()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminDeputyRoleName, CityAdminDeputyRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.RegionAdministration());
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                 It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
                 It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(),
                 null, null))
             .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));
            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);

            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), It.IsAny<Expression<Func<DatabaseEntities.City, DatabaseEntities.City>>>(),
                                       null, It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(),null, null), Times.Never);
            Assert.Equal(result, _expected);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_RegionAdminAndCityAdmin_Succeeded()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName, CityAdminRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.RegionAdministration());
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(),
                null, null))
            .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));
            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);

            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            Assert.Equal(result, _expected);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_RegionAdminAndCityAdmin_EmptyList()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName, CityAdminRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>
                    >>())).ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.RegionAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.RegionAdministration());

            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(),
                null, null))
            .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));
            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);

            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
              It.IsAny<Expression<Func<DatabaseEntities.City, DatabaseEntities.City>>>(),
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            Assert.Equal(result, _expected);
        }

        [Fact]
        public async Task GetAllCitiesIdAndName_Admin_Passed()
        {
            // Arrange
            _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { AdminRoleName });
            _repositoryWrapper.Setup(x => x.AnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                        IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                 .ReturnsAsync(new List<DatabaseEntities.AnnualReport>()
                {new DatabaseEntities.AnnualReport() {CityId = 1}});
            _repositoryWrapper.Setup(x => x.City.GetRangeAsync(null, null,
               It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IQueryable<DataAccess.Entities.City>>>(),
               It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>(),
               null, null))
           .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));
            _mapper.Setup(x =>
                    x.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                        It.IsAny<IEnumerable<DatabaseEntities.City>>()))
                .Returns(_expected);


            // Act
            var result = await _cityAccessService.GetAllCitiesIdAndName(new User());

            // Assert
            _repositoryWrapper.Verify(x => x.City.GetRangeAsync(null, null,
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IQueryable<DatabaseEntities.City>>>(),
              It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>(), null, null), Times.Once);
        }

        private IEnumerable<DataAccess.Entities.City> GetTestCitiesForHandler()
        {
            return new List<DataAccess.Entities.City>
            {
                new DataAccess.Entities.City{ID = 1, Name = "Львів"},
                new DataAccess.Entities.City{ID = 2, Name = "Харків"},
                new DataAccess.Entities.City{ID = 3, Name = "Вінниця"}
            }.AsEnumerable();
        }


        private List<CityForAdministrationDto> _expected = new List<CityForAdministrationDto>
            {new CityForAdministrationDto {ID = 1, Name = "TestCityName", HasReport = true, IsActive = true}};
    }
}