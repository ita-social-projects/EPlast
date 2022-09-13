using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Club
{
    [TestFixture]
    public class ClubDocumentsServiceTest
    {
        private protected ClubDocumentsService _clubDocumentsService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IClubFilesBlobStorageRepository> _clubFilesBlobStorage;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _clubFilesBlobStorage = new Mock<IClubFilesBlobStorageRepository>();
            _clubDocumentsService = new ClubDocumentsService(
                _repoWrapper.Object,
                _mapper.Object,
                _clubFilesBlobStorage.Object
            );
        }

        [Test]
        public async Task GetAllCityDocumentTypesAsync_ReturnsDocumentTypes()
        {
            // Arrange
            ClubDocumentsService clubDocumentsService = CreateClubDocumentsService();

            // Act
            var result = await clubDocumentsService.GetAllClubDocumentTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<ClubDocumentTypeDto>>(result);
        }

        [Test]
        public async Task AddDocumentAsync_ReturnsDocument()
        {
            // Arrange
            ClubDocumentsService clubDocumentsService = CreateClubDocumentsService();

            // Act
            var result = await clubDocumentsService.AddDocumentAsync(ClubDocumentsDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ClubDocumentsDto>(result);
        }

        [Test]
        public async Task DownloadFileAsync_ReturnsFileBase64()
        {
            // Arrange
            ClubDocumentsService clubDocumentsService = CreateClubDocumentsService();

            // Act
            var result = await clubDocumentsService.DownloadFileAsync(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<string>(result);
        }

        [Test]
        public async Task DeleteFileAsync()
        {
            // Arrange
            ClubDocumentsService clubDocumentsService = CreateClubDocumentsService();

            // Act
            await clubDocumentsService.DeleteFileAsync(fakeId);

            // Assert
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        private int fakeId => 1;
        private string fakeFile => "file";

        private ClubDocumentsService CreateClubDocumentsService()
        {
            _repoWrapper
                .Setup(r => r.ClubDocuments.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubDocuments, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubDocuments>, IIncludableQueryable<ClubDocuments, object>>>()))
                .ReturnsAsync(new ClubDocuments());
            _repoWrapper
                .Setup(r => r.ClubDocuments.CreateAsync(It.IsAny<ClubDocuments>()));
            _repoWrapper
                .Setup(r => r.ClubDocuments.Attach(It.IsAny<ClubDocuments>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());
            _repoWrapper
                .Setup(r => r.ClubDocumentType.GetAllAsync(It.IsAny<Expression<Func<ClubDocumentType, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubDocumentType>, IIncludableQueryable<ClubDocumentType, object>>>()))
                .ReturnsAsync(new List<ClubDocumentType> { new ClubDocumentType() { ID = 1 } });
            _mapper
                .Setup(m => m.Map<ClubDocumentsDto, ClubDocuments>(It.IsAny<ClubDocumentsDto>()))
                .Returns(new ClubDocuments());
            _mapper
                .Setup(m => m.Map<IEnumerable<ClubDocumentType>, IEnumerable<ClubDocumentTypeDto>>(It.IsAny<IEnumerable<ClubDocumentType>>()))
                .Returns(GetClubDocumentTypeDTOs());
            _clubFilesBlobStorage
                .Setup(c => c.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _clubFilesBlobStorage
                .Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _clubFilesBlobStorage
                .Setup(c => c.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(fakeFile);

            return new ClubDocumentsService(
                _repoWrapper.Object,
                _mapper.Object,
                _clubFilesBlobStorage.Object
            );
        }

        private ClubDocumentsDto ClubDocumentsDTO => new ClubDocumentsDto
        {
            ID = 1,
            BlobName = "newBlob,LastBlob",
            FileName = "FileName",
            ClubDocumentType = new ClubDocumentTypeDto()
            {
                ID = 1,
                Name = "DocumentTypeName"
            },
            ClubDocumentTypeId = 1,
            ClubId = 1,
            SubmitDate = DateTime.Now
        };

        private IEnumerable<ClubDocumentTypeDto> GetClubDocumentTypeDTOs()
        {
            return new List<ClubDocumentTypeDto>
            {
                new ClubDocumentTypeDto
                {
                    ID = 1,
                    Name = "DocumentTypeName"
                }
            }.AsEnumerable();
        }
    }
}
