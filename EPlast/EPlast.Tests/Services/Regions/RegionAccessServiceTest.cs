using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Services.Region.RegionAccess;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Regions
{
    class RegionAccessServiceTest
    {
        private RegionAccessService _regionAccessService;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private RegionAccessSettings _regionAccessSettings;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        private const string AdminRoleName = Roles.Admin;
        private const string RegionAdminRoleName = Roles.OkrugaHead;

        [SetUp]
        public void SetUp()
        {
            Mock<IUserStore<User>> _mockStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(_mockStore.Object, null, null, null, null, null, null, null, null);

            _mockMapper = new Mock<IMapper>();

            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            _mockRepositoryWrapper.Setup(x => x.AdminType.GetFirstAsync(
                    It.IsAny<Expression<Func<AdminType, bool>>>(),
                    It.IsAny<Func<IQueryable<AdminType>, IIncludableQueryable<AdminType, object>>>()))
                .ReturnsAsync(_adminType);

            _regionAccessSettings = new RegionAccessSettings(_mockRepositoryWrapper.Object);


            _regionAccessService = new RegionAccessService(_regionAccessSettings, _mockUserManager.Object, _mockMapper.Object, _mockRepositoryWrapper.Object);
        }

        [Test]
        public async Task GetRegionsAsync_Admin_Passed()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { AdminRoleName });
            _mockRepositoryWrapper
               .Setup(x => x.Region.GetRangeAsync(null, null,
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IQueryable<DataAccess.Entities.Region>>>(),
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>(),
                                 null, null))
               .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.Region>, int>(GetTestRegionsForHandler(), 1));

            // Act
            await _regionAccessService.GetRegionsAsync(new User());

            // Assert
            _mockMapper.Verify(x =>
                x.Map<IEnumerable<Region>, IEnumerable<RegionDto>>(
                    It.IsAny<IEnumerable<Region>>()));
        }

        [Test]
        public async Task GetRegionsAsync_RegionAdmin_Passed()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(), null))
                .ReturnsAsync(new RegionAdministration());
            _mockRepositoryWrapper.Setup(x => x.Region.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(), null,
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IQueryable<DataAccess.Entities.Region>>>(),
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>(),
                                 null, null))
               .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.Region>, int>(GetTestRegionsForHandler(), 1));

            // Act
            await _regionAccessService.GetRegionsAsync(new User());

            // Assert
            _mockMapper.Verify(x =>
                x.Map<IEnumerable<Region>, IEnumerable<RegionDto>>(
                    It.IsAny<IEnumerable<Region>>()));
        }

        [Test]
        public async Task GetRegionsAsync_RegionAdmin_EmptyList()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(), null))
                .ReturnsAsync((RegionAdministration)null);
            _mockRepositoryWrapper.Setup(r => r.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                    It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new List<Region> { new Region() });

            // Act
            await _regionAccessService.GetRegionsAsync(new User());

            // Assert
            _mockMapper.Verify(x =>
                x.Map<IEnumerable<Region>, IEnumerable<RegionDto>>(
                    It.IsAny<IEnumerable<Region>>()));
        }

        [Test]
        public async Task GetRegionsAsync_NoRoles()
        {
            //Arrange
            _mockUserManager.Setup(x =>
                x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(Enumerable.Empty<string>().ToList());

            //Act
            var result = await _regionAccessService.GetRegionsAsync(new User());

            //Assert
            _mockUserManager.Verify(x =>
                x.GetRolesAsync(It.IsAny<User>()));
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllRegionsIdAndName_Admin_Passed()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { AdminRoleName });
            _mockRepositoryWrapper
               .Setup(x => x.Region.GetRangeAsync(null, null,
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IQueryable<DataAccess.Entities.Region>>>(),
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>(),
                                 null, null))
               .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.Region>, int>(GetTestRegionsForHandler(), 1));
            _mockRepositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetAllAsync(null, null)).ReturnsAsync(new List<RegionAnnualReport>()
                    {new RegionAnnualReport() {RegionId = 1, Date = DateTime.Now}});

            _mockMapper.Setup(x =>
                x.Map<IEnumerable<Region>, IEnumerable<RegionForAdministrationDto>>(
                    It.IsAny<IEnumerable<Region>>())).Returns(_expected);

            // Act
            var result = await _regionAccessService.GetAllRegionsIdAndName(new User());

            // Assert
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.GetAllAsync(
                    It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()),
                Times.Once);
            Assert.AreEqual(result, _expected);
        }

        [Test]
        public async Task GetAllRegionsIdAndName_RegionAdmin_Passed()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(), null))
                .ReturnsAsync(new RegionAdministration());
            _mockRepositoryWrapper
               .Setup(x => x.Region.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(), null, 
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IQueryable<DataAccess.Entities.Region>>>(),
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>(), null, null))
               .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.Region>, int>(GetTestRegionsForHandler(), 1));
            _mockRepositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetAllAsync(It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new List<RegionAnnualReport>() { new RegionAnnualReport() { RegionId = 1 } });

            _mockMapper.Setup(x =>
                x.Map<IEnumerable<Region>, IEnumerable<RegionForAdministrationDto>>(
                    It.IsAny<IEnumerable<Region>>())).Returns(_expected);


            // Act
            var result = await _regionAccessService.GetAllRegionsIdAndName(new User());

            // Assert
            _mockRepositoryWrapper.Verify(x => x.Region.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(), null, 
                It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IQueryable<DataAccess.Entities.Region>>>(), 
                It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, 
                IIncludableQueryable<DataAccess.Entities.Region, object>>>(), null, null), Times.Once);
            Assert.AreEqual(result, _expected);
        }

        [Test]
        public async Task GetAllRegionsIdAndName_RegionAdmin_EmptyList()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _mockRepositoryWrapper.Setup(r =>
                    r.RegionAdministration.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<RegionAdministration, bool>>>(), null))
                .ReturnsAsync((RegionAdministration) null);
            _mockRepositoryWrapper.Setup(r => r.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                    It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new List<Region> { new Region() });

            _mockRepositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetAllAsync(It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new List<RegionAnnualReport>() {new RegionAnnualReport() {RegionId = 1}});
            _mockMapper.Setup(x =>
                    x.Map<IEnumerable<Region>, IEnumerable<RegionForAdministrationDto>>(
                        It.IsAny<IEnumerable<Region>>()))
                .Returns(_expected);


            // Act
            var result = await _regionAccessService.GetAllRegionsIdAndName(new User());

            // Assert
            _mockRepositoryWrapper.Verify(r => r.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()), Times.Never);
            Assert.AreEqual(result, _expected);
        }

        [Test]
        public async Task GetAllRegionsIdAndName_NoRoles()
        {
            //Arrange
            var expectedEmpty = Enumerable.Empty<RegionForAdministrationDto>();
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetAllAsync(
                It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(), It
                    .IsAny<Func<IQueryable<RegionAnnualReport>,
                        IIncludableQueryable<RegionAnnualReport, object>
                    >>())).ReturnsAsync(new List<RegionAnnualReport>()
                {new RegionAnnualReport() {RegionId = 1}});

            //Act
            var result = await _regionAccessService.GetAllRegionsIdAndName(new User());

            // Assert
            _mockUserManager.Verify(u => u.GetRolesAsync(It.IsAny<User>()));
            Assert.AreEqual(result, expectedEmpty);
        }

        [Test]
        public async Task HasAccessAsync_ReturnsTrue()
        {
            // Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(), null))
                .ReturnsAsync(new RegionAdministration());
            _mockRepositoryWrapper
               .Setup(x => x.Region.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(), null,
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IQueryable<DataAccess.Entities.Region>>>(),
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>(), null, null))
               .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.Region>, int>(GetTestRegionsForHandler(), 1));
            _mockMapper.Setup(m => m.Map<IEnumerable<Region>, IEnumerable<RegionDto>>(It.IsAny<IEnumerable<Region>>()))
                .Returns(new List<RegionDto> { new RegionDto() { ID = 1 } });

            // Act
            var result = await _regionAccessService.HasAccessAsync(new User(), 1);

            // Assert
            Assert.True(result);

        }

        [Test]
        public async Task HasAccessAsync_ReturnsFalse()
        {
            //Arrange
            _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { RegionAdminRoleName });
            _mockRepositoryWrapper.Setup(r => r.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(), null))
                .ReturnsAsync(new RegionAdministration());
            _mockRepositoryWrapper.Setup(x => x.Region.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(), null,
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IQueryable<DataAccess.Entities.Region>>>(),
                                 It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>(),
                                 null, null))
               .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.Region>, int>(GetTestRegionsForHandler(), 1));
            _mockMapper.Setup(m => m.Map<IEnumerable<Region>, IEnumerable<RegionDto>>(It.IsAny<IEnumerable<Region>>()))
                .Returns(new List<RegionDto> { new RegionDto() { ID = 1 } });
            //Act
            var result = await _regionAccessService.HasAccessAsync(new User(), 2);

            //Assert
            _mockUserManager.Verify();
            Assert.NotNull(result);
            Assert.IsFalse(result);
        }
        private IEnumerable<DataAccess.Entities.Region> GetTestRegionsForHandler()
        {
            return new List<DataAccess.Entities.Region>
            {
                new DataAccess.Entities.Region() { ID = 2, RegionName = "Lviv" },
                new DataAccess.Entities.Region() { ID = 3, RegionName = "Kharkiv" }
            }.AsEnumerable();
        }

        private AdminType _adminType => new AdminType() { };

        private List<RegionForAdministrationDto> _expected = new List<RegionForAdministrationDto>
        {
            new RegionForAdministrationDto
                {ID = 1, RegionName = "TestRegionName", YearsHasReport = new List<int> {DateTime.Now.Year}, IsActive = true}
        };
    }
}
