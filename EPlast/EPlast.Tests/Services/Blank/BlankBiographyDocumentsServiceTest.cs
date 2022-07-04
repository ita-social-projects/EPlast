using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.Blank;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Blank
{
    [TestFixture]
    class BlankBiographyDocumentsServiceTest
    {
        private BlankBiographyDocumentsService _blankBiographyService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IBlankFilesBlobStorageRepository> _blankBlobRepository;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mock<IMapper>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _blankBlobRepository = new Mock<IBlankFilesBlobStorageRepository>();
            _blankBiographyService = new BlankBiographyDocumentsService(_repoWrapper.Object, _mapper.Object, _blankBlobRepository.Object);
        }

        [Test]
        public async Task AddDocumentAsync_ReturnBlankBiographyDocumentsDTO()
        {
            //Arrange
            _blankBlobRepository
                .Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _mapper
                .Setup(m => m.Map<BlankBiographyDocumentsDto, BlankBiographyDocuments>(It.IsAny<BlankBiographyDocumentsDto>()))
                .Returns(BlankBiographyDocuments);
            _repoWrapper
                .Setup(r => r.BiographyDocumentsRepository.Attach(It.IsAny<BlankBiographyDocuments>()));
            _repoWrapper.Setup(rw => rw.BiographyDocumentsRepository.CreateAsync(It.IsAny<BlankBiographyDocuments>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            var result = await _blankBiographyService.AddDocumentAsync(BlankBiographyDocumentsDTO);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BlankBiographyDocumentsDto>(result);
        }

        [Test]
        public async Task DeleteFileAsync_ReturnStatusCode204NoContent()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.BiographyDocumentsRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<BlankBiographyDocuments, bool>>>(),
                It.IsAny<Func<IQueryable<BlankBiographyDocuments>,
                IIncludableQueryable<BlankBiographyDocuments, object>>>()))
                .ReturnsAsync(BlankBiographyDocuments);
            _blankBlobRepository
                .Setup(b => b.DeleteBlobAsync(BlankBiographyDocuments.BlobName));

            _repoWrapper.Setup(rw => rw.BiographyDocumentsRepository.Delete(It.IsAny<BlankBiographyDocuments>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            await _blankBiographyService.DeleteFileAsync(BlankBiographyDocuments.ID);
            //Assert
            _repoWrapper.Verify();
        }

        [Test]
        public async Task DownloadFileAsync_ReturnString()
        {
            //Arrange
            _blankBlobRepository
                .Setup(b => b.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(BlankBiographyDocuments.FileName);

            //Act
            var result = await _blankBiographyService.DownloadFileAsync(BlankBiographyDocuments.FileName);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(BlankBiographyDocuments.FileName, result);
        }

        [Test]
        public async Task GetDocumentByUserId_ReturnListOfBlankBiographyDocumentsDTO()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.BiographyDocumentsRepository.FindByCondition(It.IsAny<Expression<Func<BlankBiographyDocuments, bool>>>()))
                .Returns(GetTestBiography());
            _mapper
                .Setup(m => m.Map<BlankBiographyDocuments, BlankBiographyDocumentsDto>(It.IsAny<BlankBiographyDocuments>()))
                .Returns(BlankBiographyDocumentsDTO);
            //Act
            var result = await _blankBiographyService.GetDocumentByUserId(new string("1"));

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BlankBiographyDocumentsDto>(result);
        }

        private string BlobNameF => Guid.NewGuid().ToString();
        private BlankBiographyDocuments BlankBiographyDocuments => new BlankBiographyDocuments
        {
            ID = 1,
            BlobName = "newBlob,lastBlob",
            FileName = "FileName",
            UserId = "fgh123",
        };

        private BlankBiographyDocumentsDto BlankBiographyDocumentsDTO => new BlankBiographyDocumentsDto
        {
            ID = 1,
            BlobName = "newBlob,LastBlob",
            FileName = "FileName",
            UserId = "fgh123",
        };

        public IQueryable<BlankBiographyDocuments> GetTestBiography()
        {
            return new List<BlankBiographyDocuments>
            {
                new BlankBiographyDocuments
                {
                      ID = 1,
                      BlobName = BlobNameF,
                      FileName = "FileName",
                      UserId = "fgh123",
                },
            }.AsQueryable();

        }

    }
}
