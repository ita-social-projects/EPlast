﻿using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.Blank;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Blank
{
    [TestFixture]
    class BlankAchievementsServiceTest
    {
        private AchievementDocumentService _achievementDocumentService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IBlankAchievementBlobStorageRepository> _blankBlobRepository;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mock<IMapper>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _blankBlobRepository = new Mock<IBlankAchievementBlobStorageRepository>();
            _achievementDocumentService = new AchievementDocumentService(_repoWrapper.Object, _mapper.Object, _blankBlobRepository.Object);
        }

        [Test]
        public async Task AddDocumentAsync_ReturnBlankBiographyDocumentsDTO()
        {
            //Arrange
            _blankBlobRepository
                .Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _mapper
                .Setup(m => m.Map<AchievementDocumentsDTO, AchievementDocuments>(It.IsAny<AchievementDocumentsDTO>()))
                .Returns(AchievementDocuments);
            _repoWrapper
                .Setup(r => r.AchievementDocumentsRepository.Attach(It.IsAny<AchievementDocuments>()));
            _repoWrapper.Setup(rw => rw.AchievementDocumentsRepository.CreateAsync(It.IsAny<AchievementDocuments>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            var result = await _achievementDocumentService.AddDocumentAsync(AchievementDocumentsListDTO());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<AchievementDocumentsDTO>>(result);
        }

        [Test]
        public async Task DeleteFileAsync_ReturnStatusCode204NoContent()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.AchievementDocumentsRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<AchievementDocuments, bool>>>(),
                It.IsAny<Func<IQueryable<AchievementDocuments>,
                IIncludableQueryable<AchievementDocuments, object>>>()))
                .ReturnsAsync(AchievementDocuments);
            _blankBlobRepository
                .Setup(b => b.DeleteBlobAsync(AchievementDocuments.BlobName));
            _repoWrapper.Setup(rw => rw.AchievementDocumentsRepository.Delete(It.IsAny<AchievementDocuments>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            await _achievementDocumentService.DeleteFileAsync(AchievementDocuments.ID);

            //Assert
            _repoWrapper.Verify();
        }

        [Test]
        public async Task DownloadFileAsync_ReturnString()
        {
            //Arrange
            _blankBlobRepository
                .Setup(b => b.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(AchievementDocuments.FileName);

            //Act
            var result = await _achievementDocumentService.DownloadFileAsync(AchievementDocuments.FileName);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(AchievementDocuments.FileName, result);
        }

        private AchievementDocuments AchievementDocuments => new AchievementDocuments
        {
            ID = 1,
            BlobName = "newBlob,lastBlob",
            FileName = "FileName",
            UserId = "fgh123",
        };

        private AchievementDocumentsDTO AchievementDocumentsDTO => new AchievementDocumentsDTO
        {
            ID = 1,
            BlobName = "newBlob,LastBlob",
            FileName = "FileName",
            UserId = "fgh123",
        };

        public List<AchievementDocuments> GetTestAchievements()
        {
            return new List<AchievementDocuments>
            {
                new AchievementDocuments
                {
                      ID = 1,
                      BlobName = "newBlob,lastBlob",
                      FileName = "FileName",
                      UserId = "fgh123",
                },
                new AchievementDocuments
                {
                      ID = 2,
                      BlobName = "newBlob,lastBlob",
                      FileName = "FileName",
                      UserId = "fgh123",
                },
            };
        }

        private List<AchievementDocumentsDTO> AchievementDocumentsListDTO()
        {
            return new List<AchievementDocumentsDTO>
            {
                new AchievementDocumentsDTO
                {
                    ID = 1,
                    BlobName = "newBlob,LastBlob",
                    FileName = "FileName",
                    UserId = "fgh123",
                },
                 new AchievementDocumentsDTO
                 {
                     ID = 2,
                     BlobName = "newBlob1,LastBlob1",
                     FileName = "FileName1",
                     UserId = "fgh123",
                 }
            };
        }

        [Test]
        public async Task GetDocumentsByUserId_ReturnsListOfDocuments()
        {
            //Arrange
            _repoWrapper
                .Setup(b => b.AchievementDocumentsRepository.GetAllAsync(It.IsAny<Expression<Func<AchievementDocuments, bool>>>(),
                    It.IsAny<Func<IQueryable<AchievementDocuments>, IIncludableQueryable<AchievementDocuments, object>>>())) 
                .ReturnsAsync(GetTestAchievements());
            _mapper
                .Setup(m => m.Map<IEnumerable<AchievementDocuments>, IEnumerable<AchievementDocumentsDTO>>(It.IsAny<IEnumerable<AchievementDocuments>>()))
                .Returns(new List<AchievementDocumentsDTO>().AsEnumerable());

            //Act
            var result = await _achievementDocumentService.GetDocumentsByUserIdAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<AchievementDocumentsDTO>>(result);
        }

        [TestCase(1, 1, "someId")]
        public async Task GetPartOfAchievement_ReturnsObj(int pageNumber, int pageSize, string userId)
        {
            //Arrange
            _repoWrapper
               .Setup(b => b.AchievementDocumentsRepository.GetAllAsync(It.IsAny<Expression<Func<AchievementDocuments, bool>>>(),
                    It.IsAny<Func<IQueryable<AchievementDocuments>, IIncludableQueryable<AchievementDocuments, object>>>()))
                .ReturnsAsync(GetTestAchievements());

            //Act
            var result = await _achievementDocumentService.GetPartOfAchievementAsync(pageNumber, pageSize, userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<AchievementDocumentsDTO>>(result);
        }
    }
}
