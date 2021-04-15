using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
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
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.Tests.Services.Club
{
    [TestFixture]
    public class ClubDocumentsServiceTest
    {
        private protected ClubDocumentsService _clubDocumentsService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IClubFilesBlobStorageRepository> _clubFilesBlobStorage;
        private Mock<IUniqueIdService> _uniqueId;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _clubFilesBlobStorage = new Mock<IClubFilesBlobStorageRepository>();
            _uniqueId = new Mock<IUniqueIdService>();
            _clubDocumentsService = new ClubDocumentsService(_repoWrapper.Object, _mapper.Object, _clubFilesBlobStorage.Object, _uniqueId.Object);
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
            Assert.IsAssignableFrom<List<ClubDocumentTypeDTO>>(result);
        }

        [Test]
        public async Task AddDocumentAsync_ReturnsDocument()
        {
            // Arrange
            ClubDocumentsService clubDocumentsService = CreateClubDocumentsService();

            // Act
            var result = await clubDocumentsService.AddDocumentAsync(clubDocumentsDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ClubDocumentsDTO>(result);
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
                .Setup(m => m.Map<ClubDocumentsDTO, ClubDocuments>(It.IsAny<ClubDocumentsDTO>()))
                .Returns(new ClubDocuments());
            _mapper
                .Setup(m => m.Map<IEnumerable<ClubDocumentType>, IEnumerable<ClubDocumentTypeDTO>>(It.IsAny<IEnumerable<ClubDocumentType>>()))
                .Returns(GetClubDocumentTypeDTOs());
            _clubFilesBlobStorage
                .Setup(c => c.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _clubFilesBlobStorage
                .Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _clubFilesBlobStorage
                .Setup(c => c.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(fakeFile);
            _uniqueId
                .Setup(u => u.GetUniqueId())
                .Returns(Guid.NewGuid());

            return new ClubDocumentsService(_repoWrapper.Object, _mapper.Object, _clubFilesBlobStorage.Object, _uniqueId.Object);
        }

        private ClubDocumentsDTO clubDocumentsDTO => new ClubDocumentsDTO
        {
            ID = 1,
            BlobName = "newBlob,LastBlob",
            FileName = "FileName",
            ClubDocumentType = new ClubDocumentTypeDTO()
            {
                ID = 1,
                Name = "DocumentTypeName"
            },
            ClubDocumentTypeId = 1,
            ClubId = 1,
            SubmitDate = DateTime.Now
        };

        private IEnumerable<ClubDocumentTypeDTO> GetClubDocumentTypeDTOs()
        {
            return new List<ClubDocumentTypeDTO>
            {
                new ClubDocumentTypeDTO
                {
                    ID = 1,
                    Name = "DocumentTypeName"
                }
            }.AsEnumerable();
        }
    }
}
