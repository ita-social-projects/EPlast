﻿using EPlast.BLL.Services;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
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

        [SetUp]
        public void SetUp()
        {
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _annualReportAccessService = new AnnualReportAccessService(_repositoryWrapper.Object);
        }

        [Test]
        public async Task CanEditCityReportAsync_ReturnsBool()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<AnnualReport>, IIncludableQueryable<AnnualReport, object>>>()))
                .ReturnsAsync(new AnnualReport {ID = 1});

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
                .ReturnsAsync(new RegionAnnualReport { ID = 1 });

            //Act
            var result = await _annualReportAccessService.CanEditReportAsync(It.IsAny<User>(), false,
                null, null);

            //Assert
            Assert.AreEqual(false, result);
        }
    }
}