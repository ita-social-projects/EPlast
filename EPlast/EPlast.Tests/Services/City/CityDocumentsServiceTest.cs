using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using DataAccessCity = EPlast.DataAccess.Entities;


namespace EPlast.Tests.Services.City
{
    [TestFixture]
    public class CityDocumentsServiceTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<ICityFilesBlobStorageRepository> _cityFilesBlobStorage;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _cityFilesBlobStorage = new Mock<ICityFilesBlobStorageRepository>();
        }

        [Test]
        public async Task GetAllCityDocumentTypesAsync_ReturnsDocumentTypes()
        {
            // Arrange
            CityDocumentsService cityDocumentsService = CreateCityDocumentsService();

            // Act
            var result = await cityDocumentsService.GetAllCityDocumentTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<CityDocumentTypeDTO>>(result);
        }

        [Test]
        public async Task AddDocumentAsync_ReturnsDocument()
        {
            // Arrange
            CityDocumentsService cityDocumentsService = CreateCityDocumentsService();

            // Act
            var result = await cityDocumentsService.AddDocumentAsync(cityDocumentsDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<CityDocumentsDTO>(result);
        }

        [Test]
        public async Task DownloadFileAsync_ReturnsFileBase64()
        {
            // Arrange
            CityDocumentsService cityDocumentsService = CreateCityDocumentsService();

            // Act
            var result = await cityDocumentsService.DownloadFileAsync(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<string>(result);
        }

        [Test]
        public async Task DeleteFileAsync()
        {
            // Arrange
            CityDocumentsService cityDocumentsService = CreateCityDocumentsService();

            // Act
            await cityDocumentsService.DeleteFileAsync(fakeId);

            // Assert
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        private int fakeId => 1;
        private string fakeFile => "file";

        private CityDocumentsService CreateCityDocumentsService()
        {
            _repoWrapper
                .Setup(r => r.CityDocuments.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessCity.CityDocuments, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccessCity.CityDocuments>, IIncludableQueryable<DataAccessCity.CityDocuments, object>>>()))
                .ReturnsAsync(new DataAccessCity.CityDocuments());
            _repoWrapper
                .Setup(r => r.CityDocuments.CreateAsync(It.IsAny<DataAccessCity.CityDocuments>()));
            _repoWrapper
                .Setup(r => r.CityDocuments.Attach(It.IsAny<DataAccessCity.CityDocuments>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());
            _repoWrapper
                .Setup(r => r.CityDocumentType.GetAllAsync(It.IsAny<Expression<Func<DataAccessCity.CityDocumentType, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccessCity.CityDocumentType>, IIncludableQueryable<DataAccessCity.CityDocumentType, object>>>()))
                .ReturnsAsync(new List<DataAccessCity.CityDocumentType> { new DataAccessCity.CityDocumentType() { ID = 1 } });
            _mapper
                .Setup(m => m.Map<CityDocumentsDTO, DataAccessCity.CityDocuments>(It.IsAny<CityDocumentsDTO>()))
                .Returns(new DataAccessCity.CityDocuments());
            _mapper
                .Setup(m => m.Map<IEnumerable<CityDocumentType>, IEnumerable<CityDocumentTypeDTO>>(It.IsAny<IEnumerable<CityDocumentType>>()))
                .Returns(GetCityDocumentTypeDTOs());
            _cityFilesBlobStorage
                .Setup(c => c.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _cityFilesBlobStorage
                .Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _cityFilesBlobStorage
                .Setup(c => c.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(fakeFile);

            return new CityDocumentsService(
                _repoWrapper.Object,
                _mapper.Object,
                _cityFilesBlobStorage.Object
            );
        }

        private CityDocumentsDTO cityDocumentsDTO => new CityDocumentsDTO
        {
            ID = 1,
            BlobName = "newBlob,LastBlob",
            FileName = "FileName",
            CityDocumentType = new CityDocumentTypeDTO() 
            { 
                ID = 1, 
                Name = "DocumentTypeName"
            },
            CityDocumentTypeId = 1,
            CityId = 1,
            SubmitDate = DateTime.Now
        };

        private IEnumerable<CityDocumentTypeDTO> GetCityDocumentTypeDTOs()
        {
            return new List<CityDocumentTypeDTO>
            {
                new CityDocumentTypeDTO
                {
                    ID = 1,
                    Name = "DocumentTypeName"
                }
            }.AsEnumerable();
        }
    }
}
