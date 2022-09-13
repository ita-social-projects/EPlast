using System;
using System.Linq;
using System.Linq.Expressions;
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
    public class GetCityByIdWthFullInfoHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetCityByIdWthFullInfoHandler _handler;
        private GetCityByIdWthFullInfoQuery _query;

        private const int CityId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetCityByIdWthFullInfoHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetCityByIdWthFullInfoQuery(CityId);
        }

        [Test]
        public async Task GetCityByIdWthFullInfoHandlerTest_ReturnsCity()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.City());
            _mockMapper
                .Setup(m => m.Map<DataAccess.Entities.City, CityDto>(It.IsAny<DataAccess.Entities.City>()))
                .Returns(new CityDto());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityDto>(result);
        }
    }
}
