using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Commands.City;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class CreateCityHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private CreateCityCommand _command;
        private CreateCityHandler _handler;

        private CityDTO _city;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _city = new CityDTO {ID = 1, Region = new RegionDTO {ID = 1, RegionName = "Region"}};
            _command = new CreateCityCommand(_city);
            _handler = new CreateCityHandler(_mockRepoWrapper.Object, _mockMapper.Object);
        }

        [Test]
        public async Task CreateCityHandlerTest_CreatesCity()
        {
            //Arrange
            var city = new DataAccess.Entities.City { ID = 1, Region = new Region { ID = 1, RegionName = "Region" } };
            var region = new Region {ID = 1, RegionName = "Region"};
            _mockMapper
                .Setup(m => m.Map<CityDTO, DataAccess.Entities.City>(It.IsAny<CityDTO>()))
                .Returns(city);
            _mockRepoWrapper
                .Setup(r => r.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                    It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(region);

            //Act
            var result = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DataAccess.Entities.City>(result);
        }
    }
}
