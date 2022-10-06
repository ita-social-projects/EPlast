using EPlast.BLL.Services;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services
{
    internal class AnnualReportAccessServiceTests
    {
        private IAnnualReportAccessService _annualReportAccessService;
        private Mock<IRepositoryWrapper> _repositoryWrapper;
        private readonly Mock<UserManager<User>> _userManager;

        [SetUp]
        public void SetUp()
        {
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _annualReportAccessService = new AnnualReportAccessService(_repositoryWrapper.Object, _userManager.Object);
        }

        [Test]
        public async Task CanViewCityReportDetailsAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<AnnualReport>, IIncludableQueryable<AnnualReport, object>>>()))
                .ReturnsAsync(new AnnualReport {CityId = 1});
            _repositoryWrapper
                .Setup(x => x.CityAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration());
            _repositoryWrapper
                .Setup(x => x.City.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.City());
            _repositoryWrapper
                .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAdministration>,
                        IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(new RegionAdministration());

            //Act
            var result =
                await _annualReportAccessService.CanViewReportDetailsAsync(It.IsAny<User>(), false, ReportType.City,
                    It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanViewCityUndefinedReportDetailsAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<AnnualReport>, IIncludableQueryable<AnnualReport, object>>>()))
                .ReturnsAsync((AnnualReport) null);
            _repositoryWrapper
                .Setup(x => x.CityAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration());
            _repositoryWrapper
                .Setup(x => x.RegionFollowers.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionFollowers, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionFollowers>, IIncludableQueryable<RegionFollowers, object>>>()))
                .ReturnsAsync((RegionFollowers) null);
            _repositoryWrapper
                .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAdministration>,
                        IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync((RegionAdministration) null);

            //Act
            var result =
                await _annualReportAccessService.CanViewReportDetailsAsync(It.IsAny<User>(), false, ReportType.City,
                    It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanViewClubReportDetailsAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAnnualReport>, IIncludableQueryable<ClubAnnualReport, object>>>()))
                .ReturnsAsync(new ClubAnnualReport {ClubId = 1});
            _repositoryWrapper
                .Setup(x => x.ClubAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAdministration>, IIncludableQueryable<ClubAdministration, object>>>()))
                .ReturnsAsync(new ClubAdministration());
            //Act
            var result = await _annualReportAccessService.CanViewReportDetailsAsync(new User {Id = "1"}, false,
                ReportType.Club, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanViewClubUndefinedReportDetailsAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAnnualReport>, IIncludableQueryable<ClubAnnualReport, object>>>()))
                .ReturnsAsync((ClubAnnualReport) null);
            _repositoryWrapper
                .Setup(x => x.ClubAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAdministration>, IIncludableQueryable<ClubAdministration, object>>>()))
                .ReturnsAsync(new ClubAdministration());
            //Act
            var result =
                await _annualReportAccessService.CanViewReportDetailsAsync(new User {Id = "1"}, false, ReportType.Club,
                    It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanViewRegionReportDetailsAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new RegionAnnualReport {ID = 1});
            _repositoryWrapper
                .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAdministration>,
                        IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(new RegionAdministration());
            //Act
            var result = await _annualReportAccessService.CanViewReportDetailsAsync(new User {Id = "1"}, false,
                ReportType.Region, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanViewRegionUndefinedReportDetailsAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync((RegionAnnualReport) null);
            _repositoryWrapper
                .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAdministration>,
                        IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(new RegionAdministration());
            //Act
            var result = await _annualReportAccessService.CanViewReportDetailsAsync(new User {Id = "1"}, false,
                ReportType.Region, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanViewUndefinedTypeReportDetailsAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new RegionAnnualReport {ID = 1});

            //Act
            var result = await _annualReportAccessService.CanViewReportDetailsAsync(It.IsAny<User>(), false,
                ReportType.City - 1, It.IsAny<int>());

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task CanViewUndefinedTypeUndefinedIdReportDetailsAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new RegionAnnualReport {ID = 1});

            //Act
            var result =
                await _annualReportAccessService.CanViewReportDetailsAsync(It.IsAny<User>(), false, null, null);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task CanEditCityReportAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<AnnualReport>, IIncludableQueryable<AnnualReport, object>>>()))
                .ReturnsAsync(new AnnualReport {CityId = 1});
            _repositoryWrapper
                .Setup(x => x.CityAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration());

            //Act
            var result =
                await _annualReportAccessService.CanEditReportAsync(It.IsAny<User>(), false, ReportType.City,
                    It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanEditCityUndefinedReportAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<AnnualReport>, IIncludableQueryable<AnnualReport, object>>>()))
                .ReturnsAsync((AnnualReport) null);
            _repositoryWrapper
                .Setup(x => x.CityAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration());

            //Act
            var result =
                await _annualReportAccessService.CanEditReportAsync(It.IsAny<User>(), false, ReportType.City,
                    It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanEditClubReportAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAnnualReport>, IIncludableQueryable<ClubAnnualReport, object>>>()))
                .ReturnsAsync(new ClubAnnualReport {ClubId = 1});
            _repositoryWrapper
                .Setup(x => x.ClubAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAdministration>, IIncludableQueryable<ClubAdministration, object>>>()))
                .ReturnsAsync(new ClubAdministration());
            //Act
            var result = await _annualReportAccessService.CanEditReportAsync(new User {Id = "1"}, false,
                ReportType.Club, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanEditClubUndefinedReportAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAnnualReport>, IIncludableQueryable<ClubAnnualReport, object>>>()))
                .ReturnsAsync((ClubAnnualReport) null);
            _repositoryWrapper
                .Setup(x => x.ClubAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAdministration>, IIncludableQueryable<ClubAdministration, object>>>()))
                .ReturnsAsync(new ClubAdministration());
            //Act
            var result =
                await _annualReportAccessService.CanEditReportAsync(new User {Id = "1"}, false, ReportType.Club,
                    It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanEditRegionReportAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new RegionAnnualReport {ID = 1});
            _repositoryWrapper
                .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAdministration>,
                        IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(new RegionAdministration());
            //Act
            var result = await _annualReportAccessService.CanEditReportAsync(new User {Id = "1"}, false,
                ReportType.Region, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanEditRegionUndefinedReportAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync((RegionAnnualReport) null);
            _repositoryWrapper
                .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAdministration>,
                        IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(new RegionAdministration());
            //Act
            var result = await _annualReportAccessService.CanEditReportAsync(new User {Id = "1"}, false,
                ReportType.Region, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task CanEditUndefinedTypeReportAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new RegionAnnualReport {ID = 1});

            //Act
            var result = await _annualReportAccessService.CanEditReportAsync(It.IsAny<User>(), false,
                ReportType.City - 1, It.IsAny<int>());

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task CanEditUndefinedTypeUndefinedIdReportAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new RegionAnnualReport {ID = 1});

            //Act
            var result = await _annualReportAccessService.CanEditReportAsync(It.IsAny<User>(), false,
                null, null);

            //Assert
            Assert.AreEqual(false, result);
        }
    }
}