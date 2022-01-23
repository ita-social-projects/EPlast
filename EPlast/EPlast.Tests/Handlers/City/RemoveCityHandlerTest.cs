using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class RemoveCityHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<ICityBlobStorageRepository> _mockCityBlobStorage;
        private RemoveCityCommand _command;
        private RemoveCityHandler _handler;

        private const int CityId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockCityBlobStorage = new Mock<ICityBlobStorageRepository>();
            _command = new RemoveCityCommand(CityId);
            _handler = new RemoveCityHandler(_mockRepoWrapper.Object, _mockCityBlobStorage.Object);
        }

        [Test]
        public async Task RemoveCityHandlerTest_RemovesCity()
        {
            //Arrange
            var city = new DataAccess.Entities.City{Logo = "Logo"};
            _mockRepoWrapper
                .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                        IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(city);
            _mockCityBlobStorage
                .Setup(b => b.DeleteBlobAsync(It.IsAny<string>()));
            _mockRepoWrapper
                .Setup(r => r.City.Delete(It.IsAny<DataAccess.Entities.City>()));
            _mockRepoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Unit>(result);
        }
    }
}
