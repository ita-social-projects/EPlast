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

namespace EPlast.Tests.Services
{
    class AnnualReportAccessServiceTests
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
                    It.IsAny<Expression<Func<EPlast.DataAccess.Entities.AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<EPlast.DataAccess.Entities.AnnualReport>, IIncludableQueryable<EPlast.DataAccess.Entities.AnnualReport, object>>>()))
                .ReturnsAsync(new EPlast.DataAccess.Entities.AnnualReport() { ID = 1 });

            //Act
            var result = await _annualReportAccessService.CanEditCityReportAsync(It.IsAny<string>(), It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

    }
}
