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
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class GetAllCitiesOrByNameHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetAllCitiesOrByNameHandler _handler;
        private GetAllCitiesOrByNameQuery _query;
        
        private const string CityName = "Name";
        
        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllCitiesOrByNameHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetAllCitiesOrByNameQuery(CityName);
        }

        [Test]
        public async Task GetAllCitiesOrByNameHandlerTest_ReturnsCities()
        {
            //Arrange
           _mockRepoWrapper
                .Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(GetCity());
           var cities = GetCity().Where(c => c.Name.ToLower().Contains(CityName.ToLower()));
           _mockMapper
                .Setup(m => m.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDto>>(cities))
                .Returns(GetExpectedCity());

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
                    Name = "Name"
                },
                new DataAccess.Entities.City
                {
                    Name = "City"
                }
            };
        }

        private static IEnumerable<CityDto> GetExpectedCity()
        {
            return new List<CityDto>
            {
                new CityDto
                {
                    Name = "Name"
                }
            };
        }
    }
}
