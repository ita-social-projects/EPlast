using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class ArchiveCityHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private ArchiveCityCommand _command;
        private ArchiveCityHandler _handler;

        private const int CityId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _command = new ArchiveCityCommand(CityId);
            _handler = new ArchiveCityHandler(_mockRepoWrapper.Object);
        }

        [Test]
        public void ArchiveCityHandlerTest_ThrowsException()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                        IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(GetCityWthMembersToArchive());
            _mockRepoWrapper
                .Setup(r => r.City.Update(It.IsAny<DataAccess.Entities.City>()));
            _mockRepoWrapper
                .Setup(r => r.SaveAsync());

            //Act //Assert
            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _handler.Handle(_command, It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ArchiveCityHandlerTest_ArchivesCity()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                        IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(GetCityToArchive());
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

        private static DataAccess.Entities.City GetCityWthMembersToArchive()
        {
            return new DataAccess.Entities.City
            {
                ID = 1,
                IsActive = true,
                CityMembers = new List<CityMembers>
                {
                    new CityMembers
                    {
                        ID = 1
                    },
                    new CityMembers
                    {
                        ID = 2
                    }
                }
            };
        }

        private static DataAccess.Entities.City GetCityToArchive()
        {
            return new DataAccess.Entities.City
            {
                ID = 1,
                IsActive = true
            };
        }
    }
}
