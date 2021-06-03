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
                .Setup(r => r.RegionAnnualReports.GetRegionAnnualReportsAsync(It.IsAny<string>(), It.IsAny<bool>(),It.IsAny<string>(), It.IsAny<int>(),
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(new List<RegionAnnualReportTableObject>(){ report});

            //Act
            var result = await service.GetAllRegionsReportsAsync(new User(), true, "", 1, 1,1, true);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.ToList());
            Assert.True(result.ToList().Contains(report));
        }

        [Test]
        public async Task GetRegionsNameThatUserHasAccessTo_Succeeded()
        {
            // Arrange
            _mockRegionAccessService.Setup(x => x.GetRegionsAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<RegionDTO>(){new RegionDTO(){ID = 1, RegionName = "RegionName"}});

            // Act
            var result = await service.GetAllRegionsIdAndName(new User());

            // Assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task GetRegionMembersInfo_MemBersListCount2()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.City.GetAllAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                    IIncludableQueryable<DataAccess.Entities.City, object>>>())).ReturnsAsync(
                new List<DataAccess.Entities.City>()
                {
                    new DataAccess.Entities.City()
                    {
                        ID = 1,
                        Name = "CityName"
                    }
                });
            _mockRepositoryWrapper.Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.AnnualReport>,
                        IIncludableQueryable<DataAccess.Entities.AnnualReport, object>>>()))
                .ReturnsAsync(FakeAnnualReportConfirmed());
            _mockRepositoryWrapper
                .Setup(x => x.MembersStatistics.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<MembersStatistic, bool>>>(), null)).ReturnsAsync(FakeMembersStatistic());

            //Act
            var result = await service.GetRegionMembersInfo(1, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.ToList());
            Assert.True(result.ToList().Count==2);
        }

        [Test]
        public async Task GetRegionMembersInfo_MemBersListCount1()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.City.GetAllAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                    IIncludableQueryable<DataAccess.Entities.City, object>>>())).ReturnsAsync(
                new List<DataAccess.Entities.City>()
                {
                    new DataAccess.Entities.City()
                    {
                        ID = 1,
                        Name = "CityName"
                    }
                });
            _mockRepositoryWrapper.Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.AnnualReport>,
                        IIncludableQueryable<DataAccess.Entities.AnnualReport, object>>>()))
                .ReturnsAsync(FakeAnnualReportUnconfirmed());
            _mockRepositoryWrapper
                .Setup(x => x.MembersStatistics.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<MembersStatistic, bool>>>(), null)).ReturnsAsync(FakeMembersStatistic());

            //Act
            var result = await service.GetRegionMembersInfo(1, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.ToList());
            Assert.True(result.ToList().Count == 1);
        }

        [Test]
        public async Task GetRegionMembersInfo_CityReportNull()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.City.GetAllAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                    IIncludableQueryable<DataAccess.Entities.City, object>>>())).ReturnsAsync(
                new List<DataAccess.Entities.City>()
                {
                    new DataAccess.Entities.City()
                    {
                        ID = 1,
                        Name = "CityName"
                    }
                });
            _mockRepositoryWrapper.Setup(x => x.AnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.AnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.AnnualReport>,
                        IIncludableQueryable<DataAccess.Entities.AnnualReport, object>>>()))
                .ReturnsAsync(null as AnnualReport);
            _mockRepositoryWrapper
                .Setup(x => x.MembersStatistics.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<MembersStatistic, bool>>>(), null)).ReturnsAsync(FakeMembersStatistic());

            //Act
            var result = await service.GetRegionMembersInfo(1, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.ToList());
            Assert.True(result.ToList().Count == 1);
            _mockRepositoryWrapper.Verify(x => x.AnnualReports.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.AnnualReport, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.AnnualReport>,
                    IIncludableQueryable<DataAccess.Entities.AnnualReport, object>>>()));
        }

        [Test]
        public async Task GetRegionMembersInfo_CityEmpty()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.City.GetAllAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                    IIncludableQueryable<DataAccess.Entities.City, object>>>())).ReturnsAsync(new List<DataAccess.Entities.City>());

            //Act
            var result = await service.GetRegionMembersInfo(1, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.ToList());
            _mockRepositoryWrapper.Verify(x => x.City.GetAllAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                    IIncludableQueryable<DataAccess.Entities.City, object>>>()));
        }
        

        [Test]
        public async Task CancelAsync()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync(new RegionAnnualReport() { RegionId = 1, Status = AnnualReportStatus.Unconfirmed });


            //Act
            await service.ConfirmAsync(1);
            await service.CancelAsync(1);
            await service.DeleteAsync(1);

            //Assert
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.RegionAnnualReport, bool>>>(), null));
        }

        [Test]
        public async Task EditAsync()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync(new RegionAnnualReport() { RegionId = 1, Status = AnnualReportStatus.Unconfirmed });


            //Act
            await service.EditAsync(1, fakeRegionAnnualReportQuestions());

            //Assert
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.RegionAnnualReport, bool>>>(), null));
        }


        private AnnualReport FakeAnnualReportConfirmed()
        {
            return new AnnualReport()
            {
                ID = 1,
                Status = AnnualReportStatus.Confirmed,
                NumberOfSeatsPtashat = 1,
                NumberOfAdministrators = 1,
                NumberOfBeneficiaries = 1,
                NumberOfClubs = 1,
                NumberOfHonoraryMembers = 1,
                NumberOfIndependentGroups = 1,
                NumberOfIndependentRiy = 1,
                NumberOfPlastpryiatMembers = 1,
                NumberOfTeacherAdministrators = 1,
                NumberOfTeachers = 1,
                MembersStatistic = FakeMembersStatistic()
            };
        }

        private AnnualReport FakeAnnualReportUnconfirmed()
        {
            return new AnnualReport()
            {
                ID = 1,
                Status = AnnualReportStatus.Unconfirmed,
                NumberOfSeatsPtashat = 1,
                NumberOfAdministrators = 1,
                NumberOfBeneficiaries = 1,
                NumberOfClubs = 1,
                NumberOfHonoraryMembers = 1,
                NumberOfIndependentGroups = 1,
                NumberOfIndependentRiy = 1,
                NumberOfPlastpryiatMembers = 1,
                NumberOfTeacherAdministrators = 1,
                NumberOfTeachers = 1,
                MembersStatistic = FakeMembersStatistic()
            };
        }

        private MembersStatistic FakeMembersStatistic()
        {
            return new MembersStatistic()
            {
                Id = 1,
                AnnualReportId = 1,
                NumberOfNovatstva = 1,
                NumberOfPtashata = 1,
                NumberOfSeigneurMembers = 1,
                NumberOfSeigneurSupporters = 1,
                NumberOfSeniorPlastynMembers = 1,
                NumberOfSeniorPlastynSupporters = 1,
                NumberOfUnatstvaMembers = 1,
                NumberOfUnatstvaNoname = 1,
                NumberOfUnatstvaProspectors = 1,
                NumberOfUnatstvaSkobVirlyts = 1,
                NumberOfUnatstvaSupporters = 1
            };
        }

        private RegionAnnualReportQuestions fakeRegionAnnualReportQuestions()
        {
            return new RegionAnnualReportQuestions()
            {
                StateOfPreparation = " ",

                Characteristic = " ",

                ChurchCooperation = " ",

                InvolvementOfVolunteers = " ",

                ImportantNeeds = " ",

                SocialProjects = " ",

                StatusOfStrategy = " ",

                SuccessStories = " ",

                ProblemSituations = " ",

                TrainedNeeds = " ",

                PublicFunding = " ",

                Fundraising = " ",
            };
        }
    }
}
