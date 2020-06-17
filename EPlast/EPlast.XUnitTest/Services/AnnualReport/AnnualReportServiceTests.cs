using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.Services;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Repositories;
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

namespace EPlast.XUnitTest.Services.AnnualReport
{
    public class AnnualReportServiceTests
    {
        private const string ErrorMessageNoAccess = "Ви не маєте доступу до даного звіту!";
        private const string ErrorMessageHasCreated = "Річний звіт для даної станиці вже створений!";
        private const string ErrorMessageHasUnconfirmed = "Станиця має непідтверджені звіти!";
        private const string ErrorMessageEditFailed = "Не вдалося редагувати річний звіт!";
        private const string ErrorMessageNotFound = "Не вдалося знайти річний звіт!";

        private readonly Mock<IRepositoryWrapper> _repositoryWrapper = new Mock<IRepositoryWrapper>();
        private readonly Mock<ICityAccessService> _cityAccessService = new Mock<ICityAccessService>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<UserManager<DatabaseEntities.User>> _userManager;
        private readonly IAnnualReportService _annualReportService;

        public AnnualReportServiceTests()
        {
            var userStore = new Mock<IUserStore<DatabaseEntities.User>>();
            _userManager = new Mock<UserManager<DatabaseEntities.User>>(userStore.Object, null, null, null, null, null, null, null, null);
            _repositoryWrapper.Setup(r => r.AdminType.FindByCondition(It.IsAny<Expression<Func<DatabaseEntities.AdminType, bool>>>()))
                .Returns(new List<DatabaseEntities.AdminType> { new DatabaseEntities.AdminType() }.AsQueryable());
            _annualReportService = new AnnualReportService(_repositoryWrapper.Object, _userManager.Object, _cityAccessService.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetByIdAsyncCorrect()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                    .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _annualReportService.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _mapper.Verify(m => m.Map<DatabaseEntities.AnnualReport, AnnualReportDTO>(It.IsAny<DatabaseEntities.AnnualReport>()));
        }

        [Fact]
        public async Task GetByIdAsyncNullReferenceException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                    .ReturnsAsync((DatabaseEntities.AnnualReport)null);

            // Act
            await Assert.ThrowsAsync<NullReferenceException>(() => _annualReportService.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));

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
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _annualReportService.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));

            // Assert
            Assert.Equal(ErrorMessageNoAccess, result.Message);
            _mapper.Verify(m => m.Map<DatabaseEntities.AnnualReport, AnnualReportDTO>(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetAllAsync(null,
                It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                    .ReturnsAsync(new List<DatabaseEntities.AnnualReport> { new DatabaseEntities.AnnualReport() });
            _cityAccessService.Setup(c => c.GetCitiesAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new List<CityDTO> { new CityDTO() });

            // Act
            await _annualReportService.GetAllAsync(It.IsAny<ClaimsPrincipal>());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DatabaseEntities.AnnualReport>, IEnumerable<AnnualReportDTO>>(It.IsAny<IEnumerable<DatabaseEntities.AnnualReport>>()));
        }

        [Fact]
        public async Task CreateAsyncCorrect()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.AnnualReport)null);
            _mapper.Setup(m => m.Map<AnnualReportDTO, DatabaseEntities.AnnualReport>(It.IsAny<AnnualReportDTO>()))
                .Returns(new DatabaseEntities.AnnualReport());
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new DatabaseEntities.User());

            // Act
            await _annualReportService.CreateAsync(It.IsAny<ClaimsPrincipal>(), new AnnualReportDTO());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.CreateAsync(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
        }

        [Fact]
        public async Task CreateAsyncUnauthorizedAccessException()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _annualReportService.CreateAsync(It.IsAny<ClaimsPrincipal>(), new AnnualReportDTO()));

            // Assert
            Assert.Equal(ErrorMessageNoAccess, result.Message);
            _repositoryWrapper.Verify(r => r.AnnualReports.CreateAsync(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsyncHasCreated()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());

            // Act
            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => _annualReportService.CreateAsync(It.IsAny<ClaimsPrincipal>(), new AnnualReportDTO()));

            // Assert
            Assert.Equal(ErrorMessageHasCreated, result.Message);
            _repositoryWrapper.Verify(r => r.AnnualReports.CreateAsync(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsyncHasUnconfirmed()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.SetupSequence(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.AnnualReport)null)
                .ReturnsAsync(new DatabaseEntities.AnnualReport());

            // Act
            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => _annualReportService.CreateAsync(It.IsAny<ClaimsPrincipal>(), new AnnualReportDTO()));

            // Assert
            Assert.Equal(ErrorMessageHasUnconfirmed, result.Message);
            _repositoryWrapper.Verify(r => r.AnnualReports.CreateAsync(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task EditAsyncCorrect()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _annualReportService.EditAsync(It.IsAny<ClaimsPrincipal>(), new AnnualReportDTO { Status = AnnualReportStatusDTO.Unconfirmed });

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
        }

        [Fact]
        public async Task EditAsyncNullReferenceException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.AnnualReport)null);

            // Act
            var result = await Assert.ThrowsAsync<NullReferenceException>(() => 
                _annualReportService.EditAsync(It.IsAny<ClaimsPrincipal>(), new AnnualReportDTO { Status = AnnualReportStatusDTO.Unconfirmed }));

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task EditAsyncInvalidOperationException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());

            // Act
            var result = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _annualReportService.EditAsync(It.IsAny<ClaimsPrincipal>(), new AnnualReportDTO { Status = AnnualReportStatusDTO.Confirmed }));

            // Assert
            Assert.Equal(ErrorMessageEditFailed, result.Message);
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task EditAsyncUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _annualReportService.EditAsync(It.IsAny<ClaimsPrincipal>(), new AnnualReportDTO { Status = AnnualReportStatusDTO.Unconfirmed }));

            // Assert
            Assert.Equal(ErrorMessageNoAccess, result.Message);
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task ConfirmAsyncCorrectWithoutAdmins()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement() });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityAdministration.Update(It.IsAny<DatabaseEntities.CityAdministration>()), Times.Never);
            _userManager.Verify(u => u.RemoveFromRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()), Times.Never);
            _repositoryWrapper.Verify(r => r.CityAdministration.CreateAsync(It.IsAny<DatabaseEntities.CityAdministration>()), Times.Never);
            _userManager.Verify(u => u.AddToRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmAsyncCorrectOldAdmin()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement() });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityAdministration());
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityAdministration.Update(It.IsAny<DatabaseEntities.CityAdministration>()), Times.Never);
            _userManager.Verify(u => u.RemoveFromRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()), Times.Never);
            _repositoryWrapper.Verify(r => r.CityAdministration.CreateAsync(It.IsAny<DatabaseEntities.CityAdministration>()), Times.Never);
            _userManager.Verify(u => u.AddToRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmAsyncCorrectNewAdmin()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement { UserId = "1" } });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _userManager.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityAdministration.Update(It.IsAny<DatabaseEntities.CityAdministration>()), Times.Never);
            _userManager.Verify(u => u.RemoveFromRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()), Times.Never);
            _repositoryWrapper.Verify(r => r.CityAdministration.CreateAsync(It.IsAny<DatabaseEntities.CityAdministration>()));
            _userManager.Verify(u => u.AddToRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task ConfirmAsyncCorrectOldAdminEqualNewAdmin()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement { UserId = "1" } });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityAdministration { UserId = "1" });
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityAdministration.Update(It.IsAny<DatabaseEntities.CityAdministration>()), Times.Never);
            _userManager.Verify(u => u.RemoveFromRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()), Times.Never);
            _repositoryWrapper.Verify(r => r.CityAdministration.CreateAsync(It.IsAny<DatabaseEntities.CityAdministration>()), Times.Never);
            _userManager.Verify(u => u.AddToRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmAsyncCorrectOldAdminNotEqualNewAdmin()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement { UserId = "1" } });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityAdministration { UserId = "2" });
            _userManager.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityAdministration.Update(It.IsAny<DatabaseEntities.CityAdministration>()));
            _userManager.Verify(u => u.RemoveFromRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()));
            _repositoryWrapper.Verify(r => r.CityAdministration.CreateAsync(It.IsAny<DatabaseEntities.CityAdministration>()));
            _userManager.Verify(u => u.AddToRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task ConfirmAsyncCorrectWithoutOldStatus()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement() });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityLegalStatuses.Update(It.IsAny<DatabaseEntities.CityLegalStatus>()), Times.Never);
            _repositoryWrapper.Verify(r => r.CityLegalStatuses.CreateAsync(It.IsAny<DatabaseEntities.CityLegalStatus>()));
        }

        [Fact]
        public async Task ConfirmAsyncCorrectOldStatusEqualNewStatus()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport
                   {
                       CityManagement = new DatabaseEntities.CityManagement { CityLegalStatusNew = DatabaseEntities.CityLegalStatusType.InTheProcessOfLegalization }
                   });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityLegalStatus { LegalStatusType = DatabaseEntities.CityLegalStatusType.InTheProcessOfLegalization });

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityLegalStatuses.Update(It.IsAny<DatabaseEntities.CityLegalStatus>()), Times.Never);
            _repositoryWrapper.Verify(r => r.CityLegalStatuses.CreateAsync(It.IsAny<DatabaseEntities.CityLegalStatus>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmAsyncCorrectOldStatusNotEqualNewStatus()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport
                   {
                       CityManagement = new DatabaseEntities.CityManagement { CityLegalStatusNew = DatabaseEntities.CityLegalStatusType.LegalizedByMessage }
                   });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityLegalStatus { LegalStatusType = DatabaseEntities.CityLegalStatusType.InTheProcessOfLegalization });

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityLegalStatuses.Update(It.IsAny<DatabaseEntities.CityLegalStatus>()));
            _repositoryWrapper.Verify(r => r.CityLegalStatuses.CreateAsync(It.IsAny<DatabaseEntities.CityLegalStatus>()));
        }

        [Fact]
        public async Task ConfirmAsyncCorrectSaveLastConfirmed()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement() });
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Exactly(2));
            _repositoryWrapper.Verify(r => r.SaveAsync());
        }

        [Fact]
        public async Task ConfirmAsyncCorrectLastConfirmedNotFound()
        {
            // Arrange
            _repositoryWrapper.SetupSequence(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement() })
                   .ReturnsAsync(default(DatabaseEntities.AnnualReport));
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Update(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Once);
            _repositoryWrapper.Verify(r => r.SaveAsync());
        }

        [Fact]
        public async Task ConfirmAsyncUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));

            // Assert
            Assert.Equal(ErrorMessageNoAccess, result.Message);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task ConfirmAsyncNullReferenceException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync((DatabaseEntities.AnnualReport)null);

            // Act
            await Assert.ThrowsAsync<NullReferenceException>(() => _annualReportService.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));

            // Assert
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CancelAsyncCorrectAdminRevertPointNull()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement() });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.CityAdministration> { new DatabaseEntities.CityAdministration() });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.CityLegalStatus> { new DatabaseEntities.CityLegalStatus() });
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.CancelAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityAdministration.Update(It.IsAny<DatabaseEntities.CityAdministration>()), Times.Never);
            _userManager.Verify(u => u.AddToRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CancelAsyncCorrectAdminRevertPointNotNull()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement() });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.CityAdministration> { new DatabaseEntities.CityAdministration() });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityAdministration());
            _userManager.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new DatabaseEntities.User());
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.CityLegalStatus> { new DatabaseEntities.CityLegalStatus() });
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.CancelAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityAdministration.Update(It.IsAny<DatabaseEntities.CityAdministration>()));
            _userManager.Verify(u => u.AddToRoleAsync(It.IsAny<DatabaseEntities.User>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task CancelAsyncCorrectStatusRevertPointNull()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement() });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.CityAdministration> { new DatabaseEntities.CityAdministration() });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.CityLegalStatus> { new DatabaseEntities.CityLegalStatus() });
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityLegalStatus)null);

            // Act
            await _annualReportService.CancelAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityLegalStatuses.Update(It.IsAny<DatabaseEntities.CityLegalStatus>()), Times.Never);
        }

        [Fact]
        public async Task CancelAsyncCorrectStatusRevertPointNotNull()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport { CityManagement = new DatabaseEntities.CityManagement() });
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.CityAdministration> { new DatabaseEntities.CityAdministration() });
            _repositoryWrapper.Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityAdministration, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.CityAdministration)null);
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync(new List<DatabaseEntities.CityLegalStatus> { new DatabaseEntities.CityLegalStatus() });
            _repositoryWrapper.Setup(r => r.CityLegalStatuses.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.CityLegalStatus, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.CityLegalStatus());

            // Act
            await _annualReportService.CancelAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.SaveAsync());
            _repositoryWrapper.Verify(r => r.CityLegalStatuses.Update(It.IsAny<DatabaseEntities.CityLegalStatus>()));
        }

        [Fact]
        public async Task CancelAsyncUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _annualReportService.CancelAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));

            // Assert
            Assert.Equal(ErrorMessageNoAccess, result.Message);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CancelAsyncNullReferenceException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(),
               It.IsAny<Func<IQueryable<DatabaseEntities.AnnualReport>, IIncludableQueryable<DatabaseEntities.AnnualReport, object>>>()))
                   .ReturnsAsync((DatabaseEntities.AnnualReport)null);

            // Act
            await Assert.ThrowsAsync<NullReferenceException>(() => _annualReportService.CancelAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));

            // Assert
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsyncCorrect()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _annualReportService.DeleteAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>());

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Delete(It.IsAny<DatabaseEntities.AnnualReport>()));
            _repositoryWrapper.Verify(r => r.SaveAsync());
        }

        [Fact]
        public async Task DeleteAsyncNullReferenceException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync((DatabaseEntities.AnnualReport)null);

            // Act
            await Assert.ThrowsAsync<NullReferenceException>(() => _annualReportService.DeleteAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));

            // Assert
            _repositoryWrapper.Verify(r => r.AnnualReports.Delete(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsyncUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper.Setup(r => r.AnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DatabaseEntities.AnnualReport, bool>>>(), null))
                .ReturnsAsync(new DatabaseEntities.AnnualReport());
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _annualReportService.DeleteAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));

            // Assert
            Assert.Equal(ErrorMessageNoAccess, result.Message);
            _repositoryWrapper.Verify(r => r.AnnualReports.Delete(It.IsAny<DatabaseEntities.AnnualReport>()), Times.Never);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.Never);
        }
    }
}