using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Club
{
    public class ClubAnnualReportServiceTests
    {
        private Mock<IRepositoryWrapper> _repositoryWrapper;
        private Mock<IClubAccessService> _clubAccessService;
        private Mock<IMapper> _mapper;
        private ClubAnnualReportService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _clubAccessService = new Mock<IClubAccessService>();
            _mapper = new Mock<IMapper>();
            _service = new ClubAnnualReportService(
                _repositoryWrapper.Object,
                _clubAccessService.Object,
                _mapper.Object
            );
        }

        [Test]
        public async Task GetByIdAsync_ReturnsClubAnnualReportDTO()
        {
            // Arrange
            ClubAnnualReport report = new ClubAnnualReport();
            
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(report);
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _mapper
                .Setup(x => x.Map<ClubAnnualReport, ClubAnnualReportDTO>(report)).Returns(new ClubAnnualReportDTO());

            // Act
            var result = await _service.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>());
            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubAnnualReportDTO>(result);
        }

        [Test]
        public void GetByIdAsync_ReturnsExeption()
        {
            // Arrange
            ClubAnnualReport report = new ClubAnnualReport();

            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(report);
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);
            // Act           
            // Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async ()=>await _service.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()));
        }

        [Test]
        public async Task GetAllAsync_ReturnsIEnumerableClubAnnualReportDTO()
        {
            // Arrange
             _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetAllAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(GetReports());
            _mapper
                .Setup(x => x.Map<IEnumerable<ClubAnnualReport>, IEnumerable<ClubAnnualReportDTO>>(GetReports()))
                .Returns(new List<ClubAnnualReportDTO>());
            // Act  
            var result = await _service.GetAllAsync(It.IsAny<User>());
            // Assert
            Assert.IsInstanceOf<IEnumerable<ClubAnnualReportDTO>>(result);
        }

        [Test]
        public void CreateAsync_ReturnsInvalidOperationException()
        {
            // Arrange
            ClubAnnualReport report = new ClubAnnualReport(); 
            _repositoryWrapper
               .Setup(x => x.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.Club, bool>>>(),
               It.IsAny<Func<IQueryable<DataAccess.Entities.Club>,
               IIncludableQueryable<DataAccess.Entities.Club, object>>>())).ReturnsAsync(new DataAccess.Entities.Club() { ID=2});
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(report); 
            // Act  
            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.CreateAsync(It.IsAny<User>(), It.IsAny<ClubAnnualReportDTO>()));
        }

        [Test]
        public void CreateAsync_ReturnsUnauthorizedAccessException()
        {
            // Arrange
            ClubAnnualReport report = null;
            _repositoryWrapper
               .Setup(x => x.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.Club, bool>>>(),
               It.IsAny<Func<IQueryable<DataAccess.Entities.Club>,
               IIncludableQueryable<DataAccess.Entities.Club, object>>>())).ReturnsAsync(new DataAccess.Entities.Club() { ID = 2 });
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(report);
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);

            // Act  
            // Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _service.CreateAsync(It.IsAny<User>(), It.IsAny<ClubAnnualReportDTO>()));
        }


        [Test]
        public void CreateAsync_ReturnsCorrect()
        {
            // Arrange
            ClubAnnualReport report = null;
            ClubAnnualReportDTO reportDto = new ClubAnnualReportDTO();
            _repositoryWrapper
               .Setup(x => x.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.Club, bool>>>(),
               It.IsAny<Func<IQueryable<DataAccess.Entities.Club>,
               IIncludableQueryable<DataAccess.Entities.Club, object>>>())).ReturnsAsync(GetClub());
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(report);
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _repositoryWrapper
                .Setup(x => x.UserPlastDegrees.GetAllAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(GetDegree());
            _mapper
                .Setup(x=>x.Map<ClubAnnualReportDTO, ClubAnnualReport>(It.IsAny<ClubAnnualReportDTO>()))
                .Returns(new ClubAnnualReport());
            _repositoryWrapper.Setup(x => x.CityMembers.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<CityMembers, bool>>>(), It.IsAny<Func<IQueryable<CityMembers>,
                    IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(GetCityMembers());

            // Act  
            var result = _service.CreateAsync(It.IsAny<User>(), reportDto);
            
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void ConfirmAsync_ReturnsUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(new ClubAnnualReport());
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);
            // Act  
            // Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _service.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>()));
        }

        [Test]
        public void ConfirmAsync_ReturnsCorrect()
        {
            // Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(new ClubAnnualReport());
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _repositoryWrapper
                .Setup(x=>x.ClubAnnualReports.Update(It.IsAny<ClubAnnualReport>()));
            // Act  
            var result = _service.ConfirmAsync(It.IsAny<User>(), It.IsAny<int>());
            // Assert
            _repositoryWrapper.Verify(x=>x.SaveAsync());
            Assert.IsNotNull(result);
        }

        [Test]
        public void CancelAsync_ReturnsUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(new ClubAnnualReport());
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);
            // Act  
            // Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _service.CancelAsync(It.IsAny<User>(), It.IsAny<int>()));
        }

        [Test]
        public void CancelAsync_ReturnsCorrect()
        {
            // Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(new ClubAnnualReport());
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.Update(It.IsAny<ClubAnnualReport>()));
            // Act  
            var result = _service.CancelAsync(It.IsAny<User>(), It.IsAny<int>());
            // Assert
            _repositoryWrapper.Verify(x => x.SaveAsync());
            Assert.IsNotNull(result);
        }


        [Test]
        public void DeleteClubReportAsync_ReturnsUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(new ClubAnnualReport());
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);
            // Act  
            // Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _service.DeleteClubReportAsync(It.IsAny<User>(), It.IsAny<int>()));
        }

        [Test]
        public void DeleteClubReportAsync_ReturnsCorrect()
        {
            // Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(new ClubAnnualReport());
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.Delete(It.IsAny<ClubAnnualReport>()));
            // Act  
            var result = _service.DeleteClubReportAsync(It.IsAny<User>(), It.IsAny<int>());
            // Assert
            _repositoryWrapper.Verify(x => x.SaveAsync());
            Assert.IsNotNull(result);
        }

        [Test]
        public void EditClubReportAsync_ReturnsInvalidOperationException()
        {
            // Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(new ClubAnnualReport() { Status=AnnualReportStatus.Confirmed});
           
            // Act  
            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _service
                .EditClubReportAsync(It.IsAny<User>(), new ClubAnnualReportDTO(){Status = AnnualReportStatus.Confirmed }));
        }

        [Test]
        public void EditClubReportAsync_ReturnsUnauthorizedAccessException()
        {
            // Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(new ClubAnnualReport() { Status = AnnualReportStatus.Confirmed });
            _clubAccessService
               .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);
            // Act  
            // Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _service
                .EditClubReportAsync(It.IsAny<User>(), new ClubAnnualReportDTO() { Status = AnnualReportStatus.Unconfirmed }));
        }

        [Test]
        public void EditClubReportAsync_ReturnsCorrect()
        {
            // Arrange
            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(new ClubAnnualReport() { Status = AnnualReportStatus.Confirmed });
            _clubAccessService
               .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _mapper
                .Setup(x=>x.Map<ClubAnnualReportDTO, ClubAnnualReport>(It.IsAny<ClubAnnualReportDTO>())).Returns(new ClubAnnualReport());
            // Act  
            var result = _service.EditClubReportAsync(It.IsAny<User>(), new ClubAnnualReportDTO() { Status = AnnualReportStatus.Unconfirmed });
            // Assert
            Assert.IsNotNull(result);
        }

        private List<ClubAnnualReport> GetReports() {
            return new List<ClubAnnualReport>()
            {
                new ClubAnnualReport()
            };
        }

        private DataAccess.Entities.Club GetClub()
        {
            return new DataAccess.Entities.Club()
            {
                ID = 2,
                ClubMembers = new List<ClubMembers>()
                {
                    new ClubMembers()
                    {
                        User = new User()
                        {
                            FirstName = "",
                            LastName = "",
                        }
                    },
                    new ClubMembers()
                    {
                        UserId = "1",
                        User = new User()
                        {
                            FirstName = "",
                            LastName = "",
                        }
                    }
                },
                ClubAdministration = new List<ClubAdministration>()
                {
                    new ClubAdministration()
                    {
                        AdminTypeId = 69,
                        User = new User()
                        {
                            FirstName = "",
                            LastName = "",
                            Email = "",
                            PhoneNumber = "",
                        }
                    },
                    new ClubAdministration()
                    {
                        UserId = "1",
                        AdminTypeId = 69,
                        User = new User()
                        {
                            FirstName = "",
                            LastName = "",
                            Email = "",
                            PhoneNumber = "",
                        }
                    },
                }
            };
        }

        private CityMembers GetCityMembers()
        {
            return new CityMembers()
            {
                City = new DataAccess.Entities.City()
                {
                    Name = "",
                }
            };
        }

        private List<UserPlastDegree> GetDegree()
        {
            return new List<UserPlastDegree>()
            {
                new UserPlastDegree()
                {
                    PlastDegree = new PlastDegree()
                    {
                        Name = "",
                    },
                },
                new UserPlastDegree()
                {
                    UserId = "1",
                    PlastDegree = new PlastDegree()
                    {
                        Name = "",
                    },
                }
            };
        }


    }
}
