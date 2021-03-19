﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPlast.BLL;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;


namespace EPlast.Tests.Services.PDF
{
    [TestFixture]
    public class PdfServiceTests
    {
        private PdfService _pdfService;
        private static Mock<IRepositoryWrapper> _repository;
        private static Mock<ILoggerService<PdfService>> _logger;
        private static Mock<IDecisionBlobStorageRepository> _decisionBlobStorage;
       
        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _logger = new Mock<ILoggerService<PdfService>>();
            _decisionBlobStorage = new Mock<IDecisionBlobStorageRepository>();
            _repository.Setup(rep => rep.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(GetTestUsersQueryable());
            _repository.Setup(rep => rep.UserProfile.FindByCondition(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(GetTestUserProfilesQueryable());
            _repository.Setup(rep => rep.CityMembers.FindByCondition(It.IsAny<Expression<Func<CityMembers, bool>>>()))
                .Returns(GetTestCityMembersQueryable());
            _repository.Setup(rep => rep.ClubMembers.FindByCondition(It.IsAny<Expression<Func<ClubMembers, bool>>>()))
                .Returns(GetTestClubMembersQueryable());
            _logger.Setup(log => log.LogError(It.IsAny<string>()));
            _pdfService = new PdfService(_repository.Object,_logger.Object,_decisionBlobStorage.Object);
        }

        [TestCase(1)]
        [TestCase(546546)]
        public void DecisionCreatePDFAsync_ReturnsByteArray_Test(int decisionId)
        {

            _repository.Setup(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(Decesions.FirstOrDefault());
            _decisionBlobStorage.Setup(blob => blob.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync("Blank");
           
            var actualReturn = _pdfService.DecisionCreatePDFAsync(decisionId);

            _repository.Verify(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()), Times.Once);
            Assert.IsInstanceOf<byte[]>(actualReturn.Result);
        }
        [TestCase("1")] 
        [TestCase("546546")]
        public void BlankCreatePdfAsync_ReturnsByteArray_Test(string userId)
        {
            _repository.Setup(repo => repo.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(new User() { Id=userId });
            var actualReturn = _pdfService.BlankCreatePDFAsync(userId);
            _repository.Verify();
            Assert.IsInstanceOf<byte[]>(actualReturn.Result);
        }

        [TestCase("1")]
        [TestCase("546546")]
        public void BlankCreatePdfAsync_ReturnsNull_Test(string userId)
        {
            _repository.Setup(rep => rep.CityMembers.FindByCondition(It.IsAny<Expression<Func<CityMembers, bool>>>()))
                .Returns(new List<CityMembers>().AsQueryable());

            var actualReturn = _pdfService.BlankCreatePDFAsync(userId);
            _logger.Verify();
            Assert.Null(actualReturn.Result);
        }

        [TestCase(1)]
        [TestCase(546546)]
        public void DecisionCreatePDFAsync_ReturnsNull_Test(int decisionId)
        {
            var decisionRepository = new Mock<IDecesionRepository>();
            _repository.Setup(rep => rep.Decesion).Returns(decisionRepository.Object);
            var actualReturn = _pdfService.DecisionCreatePDFAsync(decisionId);
            _repository.Verify(rep => rep.Decesion, Times.Once);
            _logger.Verify();
            Assert.Null(actualReturn.Result);
        }
        private static IQueryable<User> GetTestUsersQueryable()
        {
            return new List<User>
            {
                new User(){Id = "1",Approvers = new List<Approver>(){new Approver(){User =new User(),UserID = "1",ConfirmedUser = new ConfirmedUser()}}},
                new User(){Id = "546546",Approvers = new List<Approver>(){new Approver(){User =new User(), UserID = "546546", ConfirmedUser = new ConfirmedUser()}}}
            }.AsQueryable();
        }
        private static IQueryable<UserProfile> GetTestUserProfilesQueryable()
        {
            return new List<UserProfile>
            {
                new UserProfile(){ UserID ="1"},
                new UserProfile(){ UserID ="546546"}
            }.AsQueryable();
        }
        private static IQueryable<CityMembers> GetTestCityMembersQueryable()
        {
            return new List<CityMembers>
            {
                new CityMembers(){ UserId ="1",CityId=3,City = new DataAccess.Entities.City()},
                new CityMembers(){ UserId ="546546",CityId=3,City = new DataAccess.Entities.City()}
            }.AsQueryable();
        }
        private static IQueryable<ClubMembers> GetTestClubMembersQueryable()
        {
            return new List<ClubMembers>
            {
                new ClubMembers(){ UserId ="1",Club = new DataAccess.Entities.Club()},
                new ClubMembers(){ UserId ="546546", Club = new DataAccess.Entities.Club()}
            }.AsQueryable();
        }

        private IQueryable<Decesion> Decesions => new List<Decesion>()
        {
            new Decesion()
            {
                ID = 1, DecesionStatusType = DecesionStatusType.Confirmed, Date = new DateTime(), Description = "FDS",
                Name = "FS", FileName = "dsf", DecesionTarget = new DecesionTarget(), Organization = new Organization()
            },
            new Decesion()
            {
                ID = 546546, DecesionStatusType = DecesionStatusType.Confirmed, Date = new DateTime(),
                Description = "FDS", Name = "FS", FileName = "dsf", DecesionTarget = new DecesionTarget(),
                Organization = new Organization()
            }
        }.AsQueryable();
    }
}
