using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.GoverningBodies;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.GoverningBody
{
    public class GoverningBodyDocumentsServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IGoverningBodyFilesBlobStorageRepository> _governingBodyFilesBlobStorage;
        private GoverningBodyDocumentsService _service;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _governingBodyFilesBlobStorage = new Mock<IGoverningBodyFilesBlobStorageRepository>();
            _service = new GoverningBodyDocumentsService(
                _repoWrapper.Object,
                _mapper.Object,
                _governingBodyFilesBlobStorage.Object
            );
        }

        [Test]
        public async Task GetAllGoverningBodyDocumentTypesAsync_ReturnsDocumentTypes()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.GoverningBodyDocumentType.GetAllAsync(
                    It.IsAny<Expression<Func<GoverningBodyDocumentType, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyDocumentType>, IIncludableQueryable<GoverningBodyDocumentType, object>>>()))
                .ReturnsAsync(new List<GoverningBodyDocumentType> { new GoverningBodyDocumentType { Id = 1 } });

            // Act
            var result = await _service.GetAllGoverningBodyDocumentTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyDocumentTypeDTO>>(result);
        }

        [Test]
        public async Task AddGoverningBodyDocumentAsync_ReturnsDocument()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.GoverningBodyDocumentType.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyDocumentType, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyDocumentType>, IIncludableQueryable<GoverningBodyDocumentType, object>>>()))
                .ReturnsAsync(new List<GoverningBodyDocumentType> { new GoverningBodyDocumentType { Id = 1 } });
            _repoWrapper
                .Setup(r => r.GoverningBodyDocuments.Attach(It.IsAny<GoverningBodyDocuments>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());
            _governingBodyFilesBlobStorage
                .Setup(c => c.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _mapper
                .Setup(m => m.Map<GoverningBodyDocumentsDTO, GoverningBodyDocuments>(It.IsAny<GoverningBodyDocumentsDTO>()))
                .Returns(new GoverningBodyDocuments());
            _mapper
                .Setup(m => m.Map<IEnumerable<GoverningBodyDocumentType>, IEnumerable<GoverningBodyDocumentTypeDTO>>(It.IsAny<IEnumerable<GoverningBodyDocumentType>>()))
                .Returns(GetGoverningBodyDocumentTypeDtoS);

            // Act
            var result = await _service.AddGoverningBodyDocumentAsync(GetGoverningBodyDocumentsDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<GoverningBodyDocumentsDTO>(result);
        }

        [Test]
        public async Task DownloadFileAsync_ReturnsFileBase64()
        {
            // Arrange
            _governingBodyFilesBlobStorage
                .Setup(c => c.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(FakeFile);
            _repoWrapper
                .Setup(r => r.SaveAsync());

            // Act
            var result = await _service.DownloadGoverningBodyDocumentAsync(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<string>(result);
        }

        [Test]
        public async Task DeleteFileAsync()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.GoverningBodyDocuments.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyDocuments, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyDocuments>, IIncludableQueryable<GoverningBodyDocuments, object>>>()))
                .ReturnsAsync(new GoverningBodyDocuments());
            _repoWrapper
                .Setup(r => r.GoverningBodyDocuments.Delete(It.IsAny<GoverningBodyDocuments>()));
            _governingBodyFilesBlobStorage
                .Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));

            // Act
            await _service.DeleteGoverningBodyDocumentAsync(FakeId);

            // Assert
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
            _repoWrapper.Verify(r => r.GoverningBodyDocuments.Delete(It.IsAny<GoverningBodyDocuments>()), Times.Once);
        }

        private static int FakeId => 1;
        private static string FakeFile => "file";

        private static GoverningBodyDocumentsDTO GetGoverningBodyDocumentsDto => new GoverningBodyDocumentsDTO
        {
            Id = 1,
            BlobName = "newBlob,LastBlob",
            FileName = "FileName",
            GoverningBodyDocumentType = new GoverningBodyDocumentTypeDTO
            {
                Id = 1,
                Name = "DocumentTypeName"
            },
            GoverningBodyDocumentTypeId = 1,
            GoverningBodyId = 1,
            SubmitDate = DateTime.Now
        };

        private static IEnumerable<GoverningBodyDocumentTypeDTO> GetGoverningBodyDocumentTypeDtoS => new List<GoverningBodyDocumentTypeDTO>
        {
            new GoverningBodyDocumentTypeDTO
                {
                    Id = 1,
                    Name = "DocumentTypeName"
                }
        };
    }
}
