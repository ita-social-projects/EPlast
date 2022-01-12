using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class GetCityByIdHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetCityByIdHandler _handler;
        private GetCityByIdQuery _query;

        private const int CityId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetCityByIdHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetCityByIdQuery(CityId);
        }

        [Test]
        public async Task GetCityByIdHandlerTest_ReturnsCity()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.City());
            _mockMapper
                .Setup(m => m.Map<DataAccess.Entities.City, CityDTO>(It.IsAny<DataAccess.Entities.City>()))
                .Returns(new CityDTO());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityDTO>(result);
        }
    }
}
