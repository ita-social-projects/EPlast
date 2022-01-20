using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class GetCityIdByUserIdHendlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private GetCityIdByUserIdHandler _handler;
        private GetCityIdByUserIdQuery _query;

        private const string UserId = "UserId";

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _handler = new GetCityIdByUserIdHandler(_mockRepoWrapper.Object);
            _query = new GetCityIdByUserIdQuery(UserId);
        }

        [Test]
        public async Task GetCityIdByUserIdHendlerTest_ReturnsInt()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(x => x.CityMembers.GetFirstAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(), null))
                .ReturnsAsync(new CityMembers());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<int>(result);
        }
    }
}
