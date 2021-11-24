using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Mapping.AnnualReport;
using EPlast.BLL.Services;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.UserProfiles;
using Microsoft.AspNetCore.Identity;

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
                .ReturnsAsync(new AnnualReport() {ID = 1});

            //Act
            var result = await _annualReportAccessService.CanEditReportAsync(It.IsAny<User>(), 0, It.IsAny<int>());

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
                .ReturnsAsync(new ClubAnnualReport() {ClubId = 1});
            _repositoryWrapper
                .Setup(x => x.ClubAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAdministration>, IIncludableQueryable<ClubAdministration, object>>>()))
                .ReturnsAsync(new ClubAdministration());
            //Act
            var result = await _annualReportAccessService.CanEditReportAsync(new User {Id = "1"}, 1, It.IsAny<int>());

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
                .ReturnsAsync(new RegionAnnualReport() { ID = 1 });
            _repositoryWrapper
                .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(new RegionAdministration());
            //Act
            var result = await _annualReportAccessService.CanEditReportAsync(new User { Id = "1" }, 2, It.IsAny<int>());

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
                .ReturnsAsync(new RegionAnnualReport() {ID = 1});
           
            //Act
            var result = await _annualReportAccessService.CanEditReportAsync(It.IsAny<User>(), 3, It.IsAny<int>());

            //Assert
            Assert.AreEqual(result, false);
        }
    }
}