using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.City;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class UploadCityPhotoHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<ICityBlobStorageRepository> _mockCityBlobStorage;
        private UploadCityPhotoCommand _command;
        private UploadCityPhotoHandler _handler;

        private CityDTO _city;
        
        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockCityBlobStorage = new Mock<ICityBlobStorageRepository>();
            _city = new CityDTO {ID = 1, Logo = "Img.png/Pic.png,New.png/Logo.png"};
            _command = new UploadCityPhotoCommand(_city);
            _handler = new UploadCityPhotoHandler(
                _mockRepoWrapper.Object,
                _mockCityBlobStorage.Object
            );
        }

        [Test]
        public async Task UploadCityPhotoHandlerTest_UploadsPhoto()
        {
            //Arrange
            var city = new DataAccess.Entities.City { ID = 1, Logo = "OldLogo.png" };
            _mockRepoWrapper
                .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                        IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(city);
            _mockCityBlobStorage
                .Setup(b => b.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _mockCityBlobStorage
                .Setup(b => b.DeleteBlobAsync(It.IsAny<string>()));

            //Act
            var result = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Unit>(result);
        }
    }
}
