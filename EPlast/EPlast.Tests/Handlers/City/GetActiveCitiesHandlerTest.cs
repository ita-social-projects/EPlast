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
    public class GetActiveCitiesHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetActiveCitiesQuery _query;
        private GetActiveCitiesHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _query = new GetActiveCitiesQuery();
            _handler = new GetActiveCitiesHandler(_mockRepoWrapper.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetActiveCitiesHandlerTest_ReturnsActiveCities()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.City.GetRangeAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(), null,
                    It.IsAny<Func<IQueryable<DataAccess.Entities.City>,
                        IQueryable<DataAccess.Entities.City>>>(), null, null, null))
                .ReturnsAsync(new Tuple<IEnumerable<DataAccess.Entities.City>, int>(GetTestCitiesForHandler(), 1));

            _mockMapper.Setup(m => m.Map<IEnumerable<CityForAdministrationDto>>(It.IsAny<IEnumerable<DataAccess.Entities.City>>()))
                .Returns(new List<CityForAdministrationDto>());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityForAdministrationDto>>(result);
        }

        private IEnumerable<DataAccess.Entities.City> GetTestCitiesForHandler()
        {
            return new List<DataAccess.Entities.City>
            {
                new DataAccess.Entities.City{ID = 1, Name = "Старший пластун прихильник / старша пластунка прихильниця"},
                new DataAccess.Entities.City{ID = 2, Name = "Старший пластун / старша пластунка"},
                new DataAccess.Entities.City{ID = 3, Name = "Старший пластун скоб/ старша пластунка скоб"}
            }.AsEnumerable();
        }
    }
}
