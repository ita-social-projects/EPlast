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
    class AnnualReportServiceTests
    {
        private IAnnualReportService _annualReportService;
        private Mock<IDistinctionAccessService> _cityAccessService;
        private Mock<IRegionAnnualReportService> _regionAnnualReportService;
        private Mock<IRepositoryWrapper> _repositoryWrapper;
        private Mapper _mapper;
        private User _user;
        private AnnualReportDTO _annualReportDTO;

        [SetUp]
        public void SetUp()
        {
            _cityAccessService = new Mock<IDistinctionAccessService>();
            _regionAnnualReportService = new Mock<IRegionAnnualReportService>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AnnualReportProfile>()));
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _annualReportService = new AnnualReportService(
                _repositoryWrapper.Object,
                _cityAccessService.Object,
                _regionAnnualReportService.Object,
                _mapper);
            _user = new User() { Id = "1" };
            _annualReportDTO = new AnnualReportDTO() { ID = 0 };
        }
        
        [Test]
        public void CreateAsync_AnnualReportExists_ThrowsInvalidOperationException()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.City.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.City() { ID = 1 });

            _cityAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            _repositoryWrapper
                .Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<EPlast.DataAccess.Entities.AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<EPlast.DataAccess.Entities.AnnualReport>, IIncludableQueryable<EPlast.DataAccess.Entities.AnnualReport, object>>>()))
                .ReturnsAsync(new EPlast.DataAccess.Entities.AnnualReport() { ID = 1 });

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _annualReportService.CreateAsync(_user, _annualReportDTO));
        }

        [Test]
        public void CreateAsync_UserHasNoAccess_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.City.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.City() { ID = 1 });

            _cityAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            _repositoryWrapper
                .Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<EPlast.DataAccess.Entities.AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<EPlast.DataAccess.Entities.AnnualReport>, IIncludableQueryable<EPlast.DataAccess.Entities.AnnualReport, object>>>()))
                .ReturnsAsync(new EPlast.DataAccess.Entities.AnnualReport() { ID = 1 });

            //Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _annualReportService.CreateAsync(_user, _annualReportDTO));
        }

        [Test]
        public void CreateAsync_CreatesAnnualReport()
        {
            //Arrange
            _repositoryWrapper
                .Setup(x => x.City.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.City() { ID = 1 });

            _cityAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            _repositoryWrapper
                .Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<EPlast.DataAccess.Entities.AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<EPlast.DataAccess.Entities.AnnualReport>, IIncludableQueryable<EPlast.DataAccess.Entities.AnnualReport, object>>>()))
                .ReturnsAsync(() => null);

            //Act
            _annualReportService.CreateAsync(_user, _annualReportDTO);

            //Assert
            _repositoryWrapper.Verify(x => x.AnnualReports.CreateAsync(It.IsAny<EPlast.DataAccess.Entities.AnnualReport>()), Times.Once);
            _repositoryWrapper.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void EditAsync_EditsAnnualReport()
        {
            //Arrange
            _repositoryWrapper
                   .Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                       It.IsAny<Expression<Func<DataAccess.Entities.AnnualReport, bool>>>(),
                       It.IsAny<Func<IQueryable<DataAccess.Entities.AnnualReport>, IIncludableQueryable<EPlast.DataAccess.Entities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DataAccess.Entities.AnnualReport() { Status = AnnualReportStatus.Confirmed});

            _cityAccessService
               .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
               .ReturnsAsync(true);

            //Act
            _annualReportService.EditAsync(new User(), new AnnualReportDTO());

            //Assert
            _repositoryWrapper.Verify(x => x.AnnualReports.Update(It.IsAny<EPlast.DataAccess.Entities.AnnualReport>()), Times.Once);
            _repositoryWrapper.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        async public Task GetCityMembersAsync_CityIsNull_ReturnsNull()
        {
            //Arrange
            _repositoryWrapper
                  .Setup(x => x.City.GetFirstOrDefaultAsync(
                      It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                      It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                  .ReturnsAsync(null as DataAccess.Entities.City);
            //Act
            var res = await _annualReportService.GetCityMembersAsync(0);

            //Assert
            Assert.IsNull(res);
        }
    }
}
