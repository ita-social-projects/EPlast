using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Regions
{
    class RegionAnnualReportServiceTest
    {
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<IRegionAccessService> _mockRegionAccessService;
        private Mock<IMapper> _mockMapper;

        private RegionAnnualReportService service;

        [SetUp]
        public void SetUp()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockRegionAccessService = new Mock<IRegionAccessService>();
            _mockMapper = new Mock<IMapper>();
            service = new RegionAnnualReportService(_mockRepositoryWrapper.Object, _mockRegionAccessService.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetReportByIdAsync_ReturnsRegionAnnualReportDTO()
        {
            // Arrange
            int Id = 2, year = 2020;
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetFirstAsync(
                It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new RegionAnnualReport() {ID=2 });
            _mockMapper.Setup(x => x.Map<RegionAnnualReport, RegionAnnualReportDTO>(It.IsAny<RegionAnnualReport>()))
                .Returns(new RegionAnnualReportDTO() { ID = 2 });
            // Act
            var result = await service.GetReportByIdAsync(Id, year);
            // Assert
            Assert.IsInstanceOf<RegionAnnualReportDTO>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllRegionsReportsAsync()
        {
            //Arrange
            var report = new RegionAnnualReportTableObject() {Id = 1};
            _mockRepositoryWrapper
                .Setup(r => r.RegionAnnualReports.GetRegionAnnualReportsAsync(It.IsAny<string>(), It.IsAny<int>(),
                    It.IsAny<int>())).ReturnsAsync(new List<RegionAnnualReportTableObject>(){ report});

            //Act
            var result = await service.GetAllRegionsReportsAsync("", 1, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.ToList());
            Assert.True(result.ToList().Contains(report));
        }
    }
}
