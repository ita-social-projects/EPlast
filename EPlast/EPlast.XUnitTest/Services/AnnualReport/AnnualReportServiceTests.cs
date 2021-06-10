using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Region;
using Xunit;
using CityDTOs = EPlast.BLL.DTO.City;

using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.XUnitTest.Services.AnnualReport
{
    public class AnnualReportServiceTests
    {
        private readonly IAnnualReportService _annualReportService;
        private readonly Mock<ICityAccessService> _cityAccessService = new Mock<ICityAccessService>();

        private readonly Mock<IRegionAnnualReportService> _regionAnnualReportService =
            new Mock<IRegionAnnualReportService>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IRepositoryWrapper> _repositoryWrapper = new Mock<IRepositoryWrapper>();

        public AnnualReportServiceTests()
        {
            _repositoryWrapper.Setup(r => r.AdminType.FindByCondition(It.IsAny<Expression<Func<DatabaseEntities.AdminType, bool>>>()))
                .Returns(new List<DatabaseEntities.AdminType> { new DatabaseEntities.AdminType() }.AsQueryable());
            _annualReportService = new AnnualReportService(_repositoryWrapper.Object, _cityAccessService.Object, _regionAnnualReportService.Object, _mapper.Object);
        }

        [Fact]
        public async Task Cancel_Correct_SaveChanges()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                        .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                            IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                .ReturnsAsync(new DatabaseEntities.AnnualReport(){City = new DatabaseEntities.City(){RegionId = 1}, Date = new DateTime()});
            _regionAnnualReportService.Setup(x => x.UpdateMembersInfo(It.IsAny<int>(), It.IsAny<int>()));

            // Act
            await _annualReportService.CancelAsync(It.IsAny<User>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Once);
            _regionAnnualReportService.Verify(x => x.UpdateMembersInfo(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

        }

        [Fact]
        public async Task Cancel_NotExist_NullReferenceException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.AnnualReport)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _annualReportService.CancelAsync(It.IsAny<User>(), It.IsAny<int>()));
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CheckCreatedFalse()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.City());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.AnnualReport)null);

            // Act
            var result = await _annualReportService.CheckCreated(It.IsAny<User>(), It.IsAny<int>());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckCreatedNullReferenceException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.City)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _annualReportService.CheckCreated(It.IsAny<User>(), It.IsAny<int>()));
        }

        [Fact]
        public async Task CheckCreatedTrue()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.City());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());

            // Act
            var result = await _annualReportService.CheckCreated(It.IsAny<User>(), It.IsAny<int>());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckCreatedUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
               .ReturnsAsync(new DatabaseEntities.City());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _annualReportService.CheckCreated(It.IsAny<User>(), It.IsAny<int>()));
        }

        [Fact]
        public async Task Confirm_Correct_SaveChanges()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                        .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                            IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                .ReturnsAsync(new DatabaseEntities.AnnualReport() { City = new DatabaseEntities.City() { RegionId = 1 }, Date = new DateTime() });
            _regionAnnualReportService.Setup(x => x.UpdateMembersInfo(It.IsAny<int>(), It.IsAny<int>()));

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Once);
            _regionAnnualReportService.Verify(x => x.UpdateMembersInfo(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

        }

        [Fact]
        public async Task Confirm_NotExist_NullReferenceException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.AnnualReport)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _annualReportService.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>()));
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsyncInvalidOperationException()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.City());
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(() => _annualReportService.CreateAsync(It.IsAny<User>(), new AnnualReportDTO()));

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.CreateAsync(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsyncUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.City());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _annualReportService.CreateAsync(It.IsAny<User>(), new AnnualReportDTO()));

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.CreateAsync(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateCityNullReferenceException()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.City)null);

            // Act
            await Assert.ThrowsAsync<NullReferenceException>(() => _annualReportService.CreateAsync(It.IsAny<User>(), new AnnualReportDTO()));

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsyncCorrect()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), It
                        .IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>,
                            IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                .ReturnsAsync(new DatabaseEntities.AnnualReport() { City = new DatabaseEntities.City() { RegionId = 1 }, Date = new DateTime() });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _regionAnnualReportService.Setup(x => x.UpdateMembersInfo(It.IsAny<int>(), It.IsAny<int>()));

            // Act
            await _annualReportService.DeleteAsync(It.IsAny<User>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Delete(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Once);
            _regionAnnualReportService.Verify(x => x.UpdateMembersInfo(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncNullReferenceException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.AnnualReport)null);

            // Act
            await Assert.ThrowsAsync<NullReferenceException>(() => _annualReportService.DeleteAsync(It.IsAny<User>(), It.IsAny<int>()));

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Delete(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task EditAsyncCorrect()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _annualReportService.EditAsync(It.IsAny<User>(), new AnnualReportDTO { Status = AnnualReportStatusDTO.Unconfirmed });

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
        }

        [Fact]
        public async Task EditAsyncInvalidOperationException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _annualReportService.EditAsync(It.IsAny<User>(), new AnnualReportDTO { Status = AnnualReportStatusDTO.Confirmed }));

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task EditAsyncNullReferenceException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.AnnualReport)null);

            // Act
            await Assert.ThrowsAsync<NullReferenceException>(() =>
                _annualReportService.EditAsync(It.IsAny<User>(), new AnnualReportDTO { Status = AnnualReportStatusDTO.Unconfirmed }));

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task EditAsyncUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _annualReportService.EditAsync(It.IsAny<User>(), new AnnualReportDTO { Status = AnnualReportStatusDTO.Unconfirmed }));

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public void GetAllAsync_Succeded()
        {
            // Arrange
            _cityAccessService.Setup(r => r.HasAccessAsync(It.IsAny<User>())).ReturnsAsync(true);
            _repositoryWrapper
                .Setup(r => r.AnnualReports.GetAnnualReportsAsync(It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<AnnualReportTableObject>());
            // Act
            var result= _annualReportService.GetAllAsync(It.IsAny<User>(), It.IsAny<bool>(),
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetAllAsync_UnauthorizedAccessException()
        {
            // Arrange
            _cityAccessService.Setup(r => r.HasAccessAsync(It.IsAny<User>())).ReturnsAsync(false);
           
            // Act
            // Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await _annualReportService.GetAllAsync(
                    It.IsAny<User>(), It.IsAny<bool>(),
                    It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
            });
        }

        [Fact]
        public async Task GetCityMembersAsync_Valid()
        {
            //Arrange
            _repositoryWrapper.Setup(x =>
                    x.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                        It.IsAny<Func<IQueryable<DatabaseEntities.City>,
                            IIncludableQueryable<DatabaseEntities.City, object>>>()))
                .ReturnsAsync(new DatabaseEntities.City()
                    {CityMembers = new List<CityMembers>() {new CityMembers() {IsApproved = true}}});
            
            //Act
            await _annualReportService.GetCityMembersAsync(1);

            //Assert
            _mapper.Verify(x=>x.Map<DatabaseEntities.City, CityDTO>(It.IsAny<DatabaseEntities.City>()), Times.Once);
        }

        [Fact]
        public async Task GetCityMembersAsync_ReturnsNull()
        {
            //Arrange
            _repositoryWrapper.Setup(x =>
                    x.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                        It.IsAny<Func<IQueryable<DatabaseEntities.City>,
                            IIncludableQueryable<DatabaseEntities.City, object>>>()))
                .ReturnsAsync(null as DatabaseEntities.City);

            //Act
            var result = await _annualReportService.GetCityMembersAsync(1);

            //Assert
            _mapper.Verify(x => x.Map<DatabaseEntities.City, CityDTO>(It.IsAny<DatabaseEntities.City>()), Times.Never);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetAllAsync(null,
                It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                    .ReturnsAsync(new List<DatabaseEntities.AnnualReport> { new DatabaseEntities.AnnualReport() });
            _cityAccessService.Setup(c => c.GetCitiesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<CityDTOs.CityDTO> { new CityDTOs.CityDTO() });

            // Act
            await _annualReportService.GetAllAsync(It.IsAny<User>());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.AnnualReport>, IEnumerable<AnnualReportDTO>>(It.IsAny<IEnumerable<DatabaseEntities.AnnualReport>>()));
        }

        [Fact]
        public async Task GetByIdAsyncCorrect()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            await _annualReportService.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>());

            // Assert
            _mapper.Verify(m => m.Map<DatabaseEntities.AnnualReport, AnnualReportDTO>(It.IsAny<DatabaseEntities.AnnualReport>()));
        }

        [Fact]
        public async Task GetByIdAsync_UnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                    .ReturnsAsync((DatabaseEntities.AnnualReport)null);
            _cityAccessService.Setup(r => r.HasAccessAsync(It.IsAny<User>())).ReturnsAsync(false);

            // Act
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _annualReportService.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()));

            // Assert
            _mapper.Verify(m => m.Map<DatabaseEntities.AnnualReport, AnnualReportDTO>(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsyncUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _annualReportService.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()));

            // Assert
            _mapper.Verify(m => m.Map<DatabaseEntities.AnnualReport, AnnualReportDTO>(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
        }

        [Fact]
        public async Task GetEditFormByIdAsync_Valid()
        {
            //Arrange
            _repositoryWrapper
                .Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            await _annualReportService.GetEditFormByIdAsync(new User(), 1);

            //Assert
            _mapper.Verify(x=>x.Map<DatabaseEntities.AnnualReport, AnnualReportDTO>(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Once);
        }

        [Fact]
        public void GetEditFormByIdAsync_UnauthorizedAccessException()
        {
            //Arrange
            _repositoryWrapper
                .Setup(r => r.AnnualReports.GetAnnualReportsAsync(It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<AnnualReportTableObject>());
            _cityAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);

            //Act
            //Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async ()=> await _annualReportService.GetEditFormByIdAsync(new User(), 1) );
            _mapper.Verify(x => x.Map<DatabaseEntities.AnnualReport, AnnualReportDTO>(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
        }
    }
}
