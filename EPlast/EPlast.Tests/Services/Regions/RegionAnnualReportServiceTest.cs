﻿using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Regions
{
    class RegionAnnualReportServiceTest
    {
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<IRegionAccessService> _mockRegionAccessService;
        private Mock<IMapper> _mockMapper;

        private RegionAnnualReportService _service;

        [SetUp]
        public void SetUp()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockRegionAccessService = new Mock<IRegionAccessService>();
            _mockMapper = new Mock<IMapper>();
            _service = new RegionAnnualReportService(_mockRepositoryWrapper.Object, _mockRegionAccessService.Object, _mockMapper.Object);
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
            var result = await _service.GetReportByIdAsync(Id, year);
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
            var result = await _service.GetAllRegionsReportsAsync(new User(), true, "", 1, 1,1, true);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.ToList());
            Assert.True(result.ToList().Contains(report));
        }


        [Test]
        public async Task GetRegionsNameThatUserHasAccessTo_Succeeded()
        {
            // Arrange
            _mockRegionAccessService.Setup(x => x.GetAllRegionsIdAndName(It.IsAny<User>()))
                .ReturnsAsync(new List<RegionForAdministrationDTO>(){new RegionForAdministrationDTO(){ID = 1, RegionName = "RegionName"}});

            // Act
            var result = await _service.GetAllRegionsIdAndName(new User());

            // Assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task GetRegionMembersInfo_ReturnsExpexted()
        {
            //Arrange
            var expected = new List<RegionMembersInfoTableObject>() {_fakeMembersInfoTableObject()};
            _mockRepositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetRegionMembersInfoAsync(It.IsAny<int>(), It.IsAny<int>(), false,
                    It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expected);

            //Act
            var result = await _service.GetRegionMembersInfoAsync(1, 1, 1, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.ToList());
            Assert.AreEqual(expected, result);
        }


        [Test]
        public async Task CancelAsync()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync(new RegionAnnualReport() { RegionId = 1, Status = AnnualReportStatus.Unconfirmed });


            //Act
            await _service.ConfirmAsync(1);
            await _service.CancelAsync(1);
            await _service.DeleteAsync(1);

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
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetRegionMembersInfoAsync(It.IsAny<int>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<RegionMembersInfoTableObject>() { _fakeMembersInfoTableObject() });


            //Act
            await _service.EditAsync(1, _fakeRegionAnnualReportQuestions());

            //Assert
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.RegionAnnualReport, bool>>>(), null));
        }

        [Test]
        public void EditAsync_InvalidOperationException()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync(new RegionAnnualReport() { Status = AnnualReportStatus.Saved });
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetRegionMembersInfoAsync(It.IsAny<int>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<RegionMembersInfoTableObject>() { _fakeMembersInfoTableObject() });

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EditAsync(1, _fakeRegionAnnualReportQuestions()));
        }

        [Test]
        public async Task GetAllRegionsReportsAsync_NoParams()
        {
            //Arrange
            var expected = new List<RegionAnnualReportDTO> {new RegionAnnualReportDTO() {RegionId = 1}};
            _mockRepositoryWrapper
                .Setup(r => r.RegionAnnualReports.FindAll()).Returns(new List<RegionAnnualReport>{ new RegionAnnualReport()}.AsQueryable());
            _mockMapper.Setup(x =>
                x.Map<IEnumerable<RegionAnnualReport>, IEnumerable<RegionAnnualReportDTO>>(
                    It.IsAny<List<RegionAnnualReport>>())).Returns(expected);

            //Act
            var result = await _service.GetAllRegionsReportsAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.ToList());
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsExpected()
        {
            //Arrange
            var expected = new List<RegionAnnualReportDTO> {_fakeRegionAnnualReport()};
            _mockRepositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetAllAsync(It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new List<RegionAnnualReport>());
            _mockMapper.Setup(x =>
                x.Map<IEnumerable<RegionAnnualReport>, IEnumerable<RegionAnnualReportDTO>>(
                    It.IsAny<IEnumerable<RegionAnnualReport>>())).Returns(expected);

            //Act
            var result= await _service.GetAllAsync(new User());

            //Assert
            _mockMapper.Verify(x =>
                x.Map<IEnumerable<RegionAnnualReport>, IEnumerable<RegionAnnualReportDTO>>(
                    It.IsAny<IEnumerable<RegionAnnualReport>>()), Times.Once);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateAsync_InvalidOperationException()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x =>
                    x.Region.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Region, bool>>>(), null))
                .ReturnsAsync(new Region() { ID=1});
            _mockRegionAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _mockRepositoryWrapper.Setup(x =>
                    x.RegionAnnualReports.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync(new RegionAnnualReport());

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateAsync(new User(), new RegionAnnualReportDTO()));
        }

        [Test]
        public void CreateAsync_UnauthorizedAccessException()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x =>
                    x.Region.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Region, bool>>>(), null))
                .ReturnsAsync(new Region() { ID = 1 });
            _mockRegionAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);
            _mockRepositoryWrapper.Setup(x =>
                    x.RegionAnnualReports.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync((RegionAnnualReport) null);

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.CreateAsync(new User(), new RegionAnnualReportDTO()));
        }

        [Test]
        public async Task CreateAsync_Succeeded()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x =>
                    x.Region.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Region, bool>>>(), null))
                .ReturnsAsync(new Region() { RegionName = "TestRegionName" });
            _mockRegionAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _mockRepositoryWrapper.Setup(x =>
                    x.RegionAnnualReports.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync((RegionAnnualReport)null);
            _mockMapper.Setup(x => x.Map<RegionAnnualReportDTO, RegionAnnualReport>(It.IsAny<RegionAnnualReportDTO>()))
                .Returns(new RegionAnnualReport());

            //Act
            await _service.CreateAsync(new User(), new RegionAnnualReportDTO());

            //Assert
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.CreateAsync(It.IsAny<RegionAnnualReport>()),
                Times.Once);
            _mockRepositoryWrapper.Verify(x => x.SaveAsync(), Times.Once);
            _mockMapper.Verify(x => x.Map<RegionAnnualReportDTO, RegionAnnualReport>(It.IsAny<RegionAnnualReportDTO>()), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_ReturnsRegionAnnualReportDTOIEnumerable()
        {
            //Arrange
            _mockRepositoryWrapper
                .Setup(x => x.RegionAnnualReports.GetAllAsync(
                    It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAnnualReport>, IIncludableQueryable<RegionAnnualReport, object>>>()))
                .ReturnsAsync(new List<RegionAnnualReport>() { new RegionAnnualReport() });

            //Act
            var result = await _service.GetAllAsync(new User());

            //Assert
            Assert.IsInstanceOf<IEnumerable<RegionAnnualReportDTO>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateMembersInfo_Succeeded()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x =>
                    x.RegionAnnualReports.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync(new RegionAnnualReport());
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetRegionMembersInfoAsync(It.IsAny<int>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<RegionMembersInfoTableObject>() {_fakeMembersInfoTableObject(),});

            //Act
            await _service.UpdateMembersInfo(1, 1);

            //Assert
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.RegionAnnualReport, bool>>>(), null), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.Update(It.IsAny<RegionAnnualReport>()));
            _mockRepositoryWrapper.Verify(x=>x.SaveAsync());
        }

        [Test]
        public async Task UpdateMembersInfo_ReportNotFound()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x =>
                    x.RegionAnnualReports.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync((RegionAnnualReport)null);

            //Act
            await _service.UpdateMembersInfo(1, 1);

            //Assert
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DataAccess.Entities.RegionAnnualReport, bool>>>(), null), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.GetRegionMembersInfoAsync(It.IsAny<int>(),
                It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.Update(It.IsAny<RegionAnnualReport>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.SaveAsync(), Times.Never);

        }

        [Test]
        public void CreateByNameAsync_UnauthorizedAccessException()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x =>
                    x.Region.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Region, bool>>>(), null))
                .ReturnsAsync(new Region());
            _mockRegionAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.CreateByNameAsync(new User(), 1, 1, new RegionAnnualReportQuestions()));
        }

        [Test]
        public void CreateByNameAsync_InvalidOperationException()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x =>
                    x.Region.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Region, bool>>>(), null))
                .ReturnsAsync(new Region());
            _mockRegionAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _mockRepositoryWrapper.Setup(x =>
                    x.RegionAnnualReports.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync(new RegionAnnualReport());

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateByNameAsync(new User(), 1, 1, new RegionAnnualReportQuestions()));
        }

        [Test]
        public async Task CreateByNameAsync_Succeeded()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x =>
                    x.Region.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Region, bool>>>(), null))
                .ReturnsAsync(new Region() { RegionName = "TestRegionName"});
            _mockRegionAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _mockRepositoryWrapper.Setup(x =>
                    x.RegionAnnualReports.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync((RegionAnnualReport)null);
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetRegionMembersInfoAsync(It.IsAny<int>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<RegionMembersInfoTableObject>() { _fakeMembersInfoTableObject(), });

            //Act
            var result = await _service.CreateByNameAsync(new User(), 1, 1, _fakeRegionAnnualReportQuestions());

            //Assert
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.Create(It.IsAny<RegionAnnualReport>()),
                Times.Once);
            _mockRepositoryWrapper.Verify(x=>x.SaveAsync(), Times.Once);
            _mockMapper.Verify(x => x.Map<RegionAnnualReportDTO>(It.IsAny<RegionAnnualReport>()), Times.Once);
        }

        [Test]
        public void EditAsync_ThrowsException()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DataAccess.Entities.RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync(new RegionAnnualReport() { RegionId = 1, Status = AnnualReportStatus.Confirmed });

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EditAsync(1, _fakeRegionAnnualReportQuestions()));
        }

        [Test]
        public async Task CreateByNameAsync_Succeeded_ReturnsExpected()
        {
            //Arrange
            RegionAnnualReportDTO expected = _fakeRegionAnnualReport();
            _mockRepositoryWrapper.Setup(x =>
                    x.Region.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Region, bool>>>(), null))
                .ReturnsAsync(new Region() { RegionName = "TestRegionName" });
            _mockRegionAccessService.Setup(x => x.HasAccessAsync(It.IsAny<User>(), It.IsAny<int>())).ReturnsAsync(true);
            _mockRepositoryWrapper.Setup(x =>
                    x.RegionAnnualReports.GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<RegionAnnualReport, bool>>>(), null))
                .ReturnsAsync((RegionAnnualReport)null);
            _mockRepositoryWrapper.Setup(x => x.RegionAnnualReports.GetRegionMembersInfoAsync(It.IsAny<int>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<RegionMembersInfoTableObject>() { _fakeMembersInfoTableObject(), });
            _mockMapper.Setup(x => x.Map<RegionAnnualReportDTO>(It.IsAny<RegionAnnualReport>()))
                .Returns(expected);

            //Act
            var result = await _service.CreateByNameAsync(new User(), 1, DateTime.Now.Year, _fakeRegionAnnualReportQuestions());

            //Assert
            _mockRepositoryWrapper.Verify(x => x.RegionAnnualReports.Create(It.IsAny<RegionAnnualReport>()),
                Times.Once);
            _mockRepositoryWrapper.Verify(x => x.SaveAsync(), Times.Once);
            _mockMapper.Verify(x => x.Map<RegionAnnualReportDTO>(It.IsAny<RegionAnnualReport>()), Times.Once);
            Assert.AreEqual(expected, result);
        }

        private AnnualReport _fakeAnnualReportConfirmed()
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
                MembersStatistic = _fakeMembersStatistic()
            };
        }

        private AnnualReport _fakeAnnualReportUnconfirmed()
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
                MembersStatistic = _fakeMembersStatistic()
            };
        }

        private RegionMembersInfoTableObject _fakeMembersInfoTableObject()
        {
            return new RegionMembersInfoTableObject()
            {
                CityAnnualReportId = 1,
                CityId = 1,
                CityName = "",
                NumberOfAdministrators = 1,
                NumberOfBeneficiaries = 1,
                NumberOfClubs = 1,
                NumberOfHonoraryMembers = 1,
                NumberOfIndependentGroups = 1,
                NumberOfIndependentRiy = 1,
                NumberOfNovatstva = 1,
                NumberOfPlastpryiatMembers = 1,
                NumberOfPtashata = 1,
                NumberOfSeatsPtashat = 1,
                NumberOfUnatstvaNoname = 1,
                NumberOfUnatstvaSupporters = 1,
                NumberOfUnatstvaMembers = 1,
                NumberOfUnatstvaProspectors = 1,
                NumberOfUnatstvaSkobVirlyts = 1,
                NumberOfSeniorPlastynSupporters = 1,
                NumberOfSeniorPlastynMembers = 1,
                NumberOfSeigneurSupporters = null,
                NumberOfSeigneurMembers = null,
                NumberOfTeacherAdministrators = 1,
                NumberOfTeachers = 1
            };
        }

        private MembersStatistic _fakeMembersStatistic()
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

        private RegionAnnualReportQuestions _fakeRegionAnnualReportQuestions()
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

        private RegionAnnualReportDTO _fakeRegionAnnualReport()
        {
            return new RegionAnnualReportDTO()
            {
                ID = 1,
                RegionName = "TestRegionName",
            };
        }
    }
}
