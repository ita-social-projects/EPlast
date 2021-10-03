using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
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
        private Mock<UserManager<User>> _userManager;
        private Mock<IRepositoryWrapper> _repositoryWrapper;
        private Mock<IClubAccessService> _clubAccessService;
        private Mock<IMapper> _mapper;
        private ClubAnnualReportService _service;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _clubAccessService = new Mock<IClubAccessService>();
            _mapper = new Mock<IMapper>();
            _service = new ClubAnnualReportService(
                _userManager.Object,
                _repositoryWrapper.Object,
                _clubAccessService.Object,
                _mapper.Object
            );
        }

        [Test]
        public async Task GetByIdAsync_ReturnsClubAnnualReportDTO_ClubAdmin()
        {
            // Arrange
            ClubAnnualReport report = new ClubAnnualReport();
            _clubAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);

            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.KurinHead });

            _repositoryWrapper.Setup(r => r.ClubReportMember.GetAllAsync(It.IsAny<Expression<Func<ClubReportMember, bool>>>(),
                                It.IsAny<Func<IQueryable<ClubReportMember>, IIncludableQueryable<ClubReportMember, IQueryable<User>>>>()))
                              .ReturnsAsync(new List<ClubReportMember>());

            _mapper.Setup(m => m.Map<IEnumerable<ClubMemberHistory>, IEnumerable<ClubMemberHistoryDTO>>
                      (It.IsAny<IEnumerable<ClubMemberHistory>>()))
                   .Returns(new List<ClubMemberHistoryDTO>());
 
            _repositoryWrapper.Setup(r => r.ClubReportAdmins.GetAllAsync(It.IsAny<Expression<Func<ClubReportAdmins, bool>>>(),
                 It.IsAny<Func<IQueryable<ClubReportAdmins>, IIncludableQueryable<ClubReportAdmins, IQueryable<User>>>>()))
           .ReturnsAsync(new List<ClubReportAdmins>());

            _mapper.Setup(m => m.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubReportAdministrationDTO>>
                      (It.IsAny<IEnumerable<ClubAdministration>>()))
                .Returns(new List<ClubReportAdministrationDTO>());


            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(report);
            _mapper
                .Setup(x => x.Map<ClubAnnualReport, ClubAnnualReportDTO>(report)).Returns(new ClubAnnualReportDTO());

            User testUser = new User();

              // Act
              var result = await _service.GetByIdAsync(testUser, It.IsAny<int>());
            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubAnnualReportDTO>(result);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsClubAnnualReportDTO_Admin()
        {
            // Arrange
            ClubAnnualReport report = new ClubAnnualReport();

            _repositoryWrapper.Setup(r => r.ClubReportMember.GetAllAsync(It.IsAny<Expression<Func<ClubReportMember, bool>>>(),
                                     It.IsAny<Func<IQueryable<ClubReportMember>, IIncludableQueryable<ClubReportMember, IQueryable<User>>>>()))
                               .ReturnsAsync(new List<ClubReportMember>());

            _mapper.Setup(m => m.Map<IEnumerable<ClubMemberHistory>, IEnumerable<ClubMemberHistoryDTO>>
                      (It.IsAny<IEnumerable<ClubMemberHistory>>()))
                   .Returns(new List<ClubMemberHistoryDTO>());


            _repositoryWrapper.Setup(r => r.ClubReportAdmins.GetAllAsync(It.IsAny<Expression<Func<ClubReportAdmins, bool>>>(),
                 It.IsAny<Func<IQueryable<ClubReportAdmins>, IIncludableQueryable<ClubReportAdmins, IQueryable<User>>>>()))
           .ReturnsAsync(new List<ClubReportAdmins>());

            _mapper.Setup(m => m.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubReportAdministrationDTO>>
                      (It.IsAny<IEnumerable<ClubAdministration>>()))
                .Returns(new List<ClubReportAdministrationDTO>());

            _repositoryWrapper.Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                                                                                     It.IsAny<Func<IQueryable<ClubAnnualReport>,
                                                                                     IIncludableQueryable<ClubAnnualReport, object>>>()))
             .ReturnsAsync(new ClubAnnualReport());

            _clubAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);

            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAnnualReport>,
                        IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(report);
            _mapper
                .Setup(x => x.Map<ClubAnnualReport, ClubAnnualReportDTO>(report)).Returns(new ClubAnnualReportDTO());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin });

            // Act
            var result = await _service.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>());
            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubAnnualReportDTO>(result);
        }

        [Test]
        public void GetByIdAsync_UnauthorizedAccessException()
        {
            // Arrange
            ClubAnnualReport report = new ClubAnnualReport();

            _repositoryWrapper
                .Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubAnnualReport>,
                        IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(report);
            _clubAccessService
                .Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);
            _mapper
                .Setup(x => x.Map<ClubAnnualReport, ClubAnnualReportDTO>(report)).Returns(new ClubAnnualReportDTO());
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
            
            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.GetByIdAsync(It.IsAny<User>(), It.IsAny<int>()));
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
        public async Task GetAllAsync_TakesParameters_Valid()
        {
            //Arrange
            ClubAnnualReportTableObject report = new ClubAnnualReportTableObject() {Id = 1};
            _clubAccessService.Setup(c => c.HasAccessAsync(It.IsAny<User>())).ReturnsAsync(true);
            _repositoryWrapper.Setup(r => r.ClubAnnualReports.GetClubAnnualReportsAsync(It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<ClubAnnualReportTableObject>() { report});

            //Act
            var result = await _service.GetAllAsync(new User(), true, "", 1, 1,1,false);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.ToList());
            Assert.True(result.ToList().Contains(report));
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
                .Setup(x => x.UserPlastDegree.GetAllAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
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
        public void CreateAsync_DegreeNull_ReturnsCorrect()
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
                .Setup(x => x.UserPlastDegree.GetAllAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync( new List<UserPlastDegree>());
            _mapper
                .Setup(x => x.Map<ClubAnnualReportDTO, ClubAnnualReport>(It.IsAny<ClubAnnualReportDTO>()))
                .Returns(new ClubAnnualReport());
            _repositoryWrapper.Setup(x => x.CityMembers.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<CityMembers, bool>>>(), It.IsAny<Func<IQueryable<CityMembers>,
                    IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(null as CityMembers);

            // Act  
            var result = _service.CreateAsync(It.IsAny<User>(), reportDto);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CreateAsync_Tests()
        {
            // Arrange
            ClubAnnualReport report = null;
            ClubAnnualReportDTO reportDto = new ClubAnnualReportDTO { Members = new List<ClubMemberHistoryDTO>()
                                                                        ,Followers=new List<ClubMemberHistoryDTO>()
                                                                        ,Admins=new List<ClubReportAdministrationDTO>()
                                                                     };
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
                .Setup(x => x.UserPlastDegree.GetAllAsync(It.IsAny<Expression<Func<UserPlastDegree, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPlastDegree>, IIncludableQueryable<UserPlastDegree, object>>>()))
                .ReturnsAsync(new List<UserPlastDegree>());
            _mapper
                .Setup(x => x.Map<ClubAnnualReportDTO, ClubAnnualReport>(It.IsAny<ClubAnnualReportDTO>()))
                .Returns(new ClubAnnualReport());

            _repositoryWrapper.Setup(x => x.CityMembers.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<CityMembers, bool>>>(), It.IsAny<Func<IQueryable<CityMembers>,
                    IIncludableQueryable<CityMembers, object>>>())).ReturnsAsync(null as CityMembers);
    
            // Act  
            var result = _service.CreateAsync(It.IsAny<User>(), reportDto);

            // Assert
            Assert.IsNotNull(result);
            _repositoryWrapper.Verify(r => r.SaveAsync(), Times.AtLeastOnce());
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

        [Test]
        public void CheckCreated_UnauthorizedAccessException()
        {
            //Arrange
            _clubAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);
            

            //Act
            //Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.CheckCreated(new User(), 1));
        }

        [Test]
        public async Task CheckCreated_ReturnsTrue()
        {
            //Arrange
            _clubAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _repositoryWrapper.Setup(x=>x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                    IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(new ClubAnnualReport());

            //Act
            var result = await _service.CheckCreated(new User(), 1);

            //Assert
            Assert.True(result);
        }

        [Test]
        public async Task CheckCreated_ReturnsFalse()
        {
            //Arrange
            _clubAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _repositoryWrapper.Setup(x => x.ClubAnnualReports.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAnnualReport>,
                    IIncludableQueryable<ClubAnnualReport, object>>>())).ReturnsAsync(null as ClubAnnualReport);

            //Act
            var result = await _service.CheckCreated(new User(), 1);

            //Assert
            Assert.False(result);
        }

        [Test]
        public async Task GetClubReportMembersAsync_ReturnDTO_Test()
        {
            //Arrange
            int ID = 1;

            // Arrange
            _repositoryWrapper
                .Setup(r => r.ClubReportMember.GetAllAsync(It.IsAny<Expression<Func<ClubReportMember, bool>>>(),
                      It.IsAny<Func<IQueryable<ClubReportMember>, IIncludableQueryable<ClubReportMember, IQueryable<User>>>>()))
                .ReturnsAsync(new List<ClubReportMember>());

            _mapper.Setup(m => m.Map<IEnumerable<ClubMemberHistory>, IEnumerable<ClubMemberHistoryDTO>>
                      (It.IsAny<IEnumerable<ClubMemberHistory>>()))
                .Returns(new List<ClubMemberHistoryDTO>());

            //Act
            var result = await _service.GetClubReportMembersAsync(ID);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<ClubMemberHistoryDTO>>(result);
        }

        [Test]
        public async Task GetClubReportMembersAsync_ReturnNull_Test()
        {
            //Arrange
            int ID = 1;

            // Arrange
            _repositoryWrapper
                .Setup(r => r.ClubReportMember.GetAllAsync(It.IsAny<Expression<Func<ClubReportMember, bool>>>(),
                      It.IsAny<Func<IQueryable<ClubReportMember>, IIncludableQueryable<ClubReportMember, IQueryable<User>>>>()))
                .ReturnsAsync(new List<ClubReportMember>());

            _mapper.Setup(m => m.Map<IEnumerable<ClubMemberHistory>, IEnumerable<ClubMemberHistoryDTO>>
                      (It.IsAny<IEnumerable<ClubMemberHistory>>()))
                .Returns(null as List<ClubMemberHistoryDTO>);

            //Act
            var result = await _service.GetClubReportMembersAsync(ID);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetClubReportAdminsAsync_ReturnDTO_Test()
        {
            //Arrange
            int ID = 1;
            // Arrange
            _repositoryWrapper
              .Setup(r => r.ClubReportAdmins.GetAllAsync(It.IsAny<Expression<Func<ClubReportAdmins, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubReportAdmins>, IIncludableQueryable<ClubReportAdmins, IQueryable<User>>>>()))
              .ReturnsAsync(new List<ClubReportAdmins>());

            _mapper.Setup(m => m.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubReportAdministrationDTO>>
                      (It.IsAny<IEnumerable<ClubAdministration>>()))
                .Returns(new List<ClubReportAdministrationDTO>());

            //Act
            var result = await _service.GetClubReportAdminsAsync(ID);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<ClubReportAdministrationDTO>>(result);
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
