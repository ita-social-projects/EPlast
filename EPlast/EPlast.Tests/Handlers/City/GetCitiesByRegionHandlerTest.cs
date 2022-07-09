using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class GetCitiesByRegionHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetCitiesByRegionHandler _handler;
        private GetCitiesByRegionQuery _query;

        private const int RegionId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetCitiesByRegionHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetCitiesByRegionQuery(RegionId);
        }

        [Test]
        public async Task GetCitiesByRegionHandlerTest_ReturnsCities()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(GetCity());
            _mockRepoWrapper
                .Setup(r => r.CityMembers.GetAllAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new List<CityMembers>());
            _mockMapper
                .Setup(m => m.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DataAccess.Entities.City>>()))
                .Returns(new List<CityDto>());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityDto>>(result);
        }

        private static IEnumerable<DataAccess.Entities.City> GetCity()
        {
            return new List<DataAccess.Entities.City>
            {
                new DataAccess.Entities.City
                {
                    ID = 1,
                    Name = "Name1",
                    RegionId = 1
                },
                new DataAccess.Entities.City
                {
                    ID = 2,
                    Name = "Name2",
                    RegionId = 2
                }
            };
        }
    }
}
