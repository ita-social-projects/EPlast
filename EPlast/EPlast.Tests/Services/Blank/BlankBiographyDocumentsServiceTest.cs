using AutoMapper;
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
                .Setup(m => m.Map<BlankBiographyDocumentsDTO, BlankBiographyDocuments>(It.IsAny<BlankBiographyDocumentsDTO>()))
                .Returns(BlankBiographyDocuments);
            _repoWrapper
                .Setup(r => r.BiographyDocumentsRepository.Attach(It.IsAny<BlankBiographyDocuments>()));
            _repoWrapper.Setup(rw => rw.BiographyDocumentsRepository.CreateAsync(It.IsAny<BlankBiographyDocuments>()));
            _repoWrapper.Setup(rw => rw.SaveAsync());

            //Act
            var result = await _blankBiographyService.AddDocumentAsync(BlankBiographyDocumentsDTO);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BlankBiographyDocumentsDTO>(result);
        }

        [Test]
        public async Task DeleteFileAsync_ReturnStatusCode200OK()
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
            var result = await _blankBiographyService.DeleteFileAsync(BlankBiographyDocuments.ID);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result);

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
                .Setup(m => m.Map<BlankBiographyDocuments, BlankBiographyDocumentsDTO>(It.IsAny<BlankBiographyDocuments>()))
                .Returns(BlankBiographyDocumentsDTO);
            //Act
            var result = await _blankBiographyService.GetDocumentByUserId(new string("1"));

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BlankBiographyDocumentsDTO>(result);
        }

        private string BlobNameF => Guid.NewGuid().ToString();
        private BlankBiographyDocuments BlankBiographyDocuments => new BlankBiographyDocuments
        {
            ID = 1,
            BlobName = BlobNameF,
            FileName = "FileName",
            UserId = "fgh123",
        };

        private BlankBiographyDocumentsDTO BlankBiographyDocumentsDTO => new BlankBiographyDocumentsDTO
        {
            ID = 1,
            BlobName = BlobNameF,
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
