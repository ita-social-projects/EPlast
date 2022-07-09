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
    class BlankExtractOfUPUServiceTest
    {
        private BlankExtractFromUpuDocumentService _blankExtractOfUPUService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IBlankExtractFromUpuBlobStorageRepository> _blankBlobRepository;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mock<IMapper>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _blankBlobRepository = new Mock<IBlankExtractFromUpuBlobStorageRepository>();
            _blankExtractOfUPUService = new BlankExtractFromUpuDocumentService(_repoWrapper.Object, _mapper.Object, _blankBlobRepository.Object);
        }

        [Test]
        public async Task AddDocumentAsync_ReturnsExtractFromUPUDocumentsDTO()
        {
            //Arrange
            _blankBlobRepository
                .Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _mapper
                .Setup(m => m.Map<ExtractFromUpuDocumentsDto, ExtractFromUpuDocuments>(It.IsAny<ExtractFromUpuDocumentsDto>()))
                .Returns(ExtractFromUPUDocuments);
            _repoWrapper
                .Setup(r => r.ExtractFromUPUDocumentsRepository.Attach(It.IsAny<ExtractFromUpuDocuments>()));
            _repoWrapper.Setup(rw => rw.ExtractFromUPUDocumentsRepository.CreateAsync(It.IsAny<ExtractFromUpuDocuments>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            var result = await _blankExtractOfUPUService.AddDocumentAsync(ExtractFromUPUDocumentsDTO);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ExtractFromUpuDocumentsDto>(result);
        }

        [Test]
        public async Task DeleteFileAsync()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.ExtractFromUPUDocumentsRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ExtractFromUpuDocuments, bool>>>(),
                It.IsAny<Func<IQueryable<ExtractFromUpuDocuments>,
                IIncludableQueryable<ExtractFromUpuDocuments, object>>>()))
                .ReturnsAsync(ExtractFromUPUDocuments);
            _blankBlobRepository
                .Setup(b => b.DeleteBlobAsync(ExtractFromUPUDocuments.BlobName));

            _repoWrapper.Setup(rw => rw.ExtractFromUPUDocumentsRepository.Delete(It.IsAny<ExtractFromUpuDocuments>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            await _blankExtractOfUPUService.DeleteFileAsync(ExtractFromUPUDocuments.ID);
            //Assert
            _repoWrapper.Verify();
        }

        [Test]
        public async Task DownloadFileAsync_ReturnString()
        {
            //Arrange
            _blankBlobRepository
                .Setup(b => b.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(ExtractFromUPUDocuments.FileName);

            //Act
            var result = await _blankExtractOfUPUService.DownloadFileAsync(ExtractFromUPUDocuments.FileName);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ExtractFromUPUDocuments.FileName, result);
        }

        [Test]
        public async Task GetDocumentByUserId_ReturnListOfBlankBiographyDocumentsDTO()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.ExtractFromUPUDocumentsRepository.FindByCondition(It.IsAny<Expression<Func<ExtractFromUpuDocuments, bool>>>()))
                .Returns(GetTestExtract);
            _mapper
                .Setup(m => m.Map<ExtractFromUpuDocuments, ExtractFromUpuDocumentsDto>(It.IsAny<ExtractFromUpuDocuments>()))
                .Returns(ExtractFromUPUDocumentsDTO);
            //Act
            var result = await _blankExtractOfUPUService.GetDocumentByUserId(new string("1"));

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ExtractFromUpuDocumentsDto>(result);
        }
        private ExtractFromUpuDocuments ExtractFromUPUDocuments => new ExtractFromUpuDocuments
        {
            ID = 1,
            BlobName = "newBlob,lastBlob",
            FileName = "FileName",
            UserId = "fgh123",
        };

        private ExtractFromUpuDocumentsDto ExtractFromUPUDocumentsDTO => new ExtractFromUpuDocumentsDto
        {
            ID = 1,
            BlobName = "newBlob,LastBlob",
            FileName = "FileName",
            UserId = "fgh123",
        };

        public IQueryable<ExtractFromUpuDocuments> GetTestExtract()
        {
            return new List<ExtractFromUpuDocuments>
            {
                new ExtractFromUpuDocuments
                {
                      ID = 1,
                      BlobName ="BlobName",
                      FileName = "FileName",
                      UserId = "fgh123",
                },
            }.AsQueryable();

        }
    }
}
