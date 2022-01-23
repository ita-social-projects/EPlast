using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class UnArchiveCityHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private UnArchiveCityCommand _command;
        private UnArchiveCityHandler _handler;

        private const int CityId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _command = new UnArchiveCityCommand(CityId);
            _handler = new UnArchiveCityHandler(_mockRepoWrapper.Object);
        }

        [Test]
        public async Task UnArchiveCityHandlerTest_UnArchivesCity()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                        IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(GetCityToUnArchive());
            _mockRepoWrapper
                .Setup(r => r.City.Update(It.IsAny<DataAccess.Entities.City>()));
            _mockRepoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Unit>(result);
        }

        private static DataAccess.Entities.City GetCityToUnArchive()
        {
            return new DataAccess.Entities.City {ID = 1, IsActive = false};
        }
    }
}
