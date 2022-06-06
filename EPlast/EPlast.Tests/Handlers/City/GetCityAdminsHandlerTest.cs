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
using EPlast.Resources;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class GetCityAdminsHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetCityAdminsHandler _handler;
        private GetCityAdminsQuery _query;

        private const int CityId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetCityAdminsHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetCityAdminsQuery(CityId);
        }

        [Test]
        public async Task GetCityAdminsHandlerTest_ReturnsCityProfile()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(x => x.CityAdministration.GetAllAsync(
                    It.IsAny<Expression<Func<CityAdministration,CityAdministration>>>(),
                    It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()
                    )).ReturnsAsync(GetCity());
            _mockMapper
                    .Setup(m => m.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DataAccess.Entities.City>>()))
                    .Returns(new List<CityDTO>());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityAdministrationViewModelDTO>(result);
        }

        [Test]
        public async Task GetCityAdminsHandlerTest_ReturnsNull()
        {
            //Arrange
            _mockRepoWrapper
                 .Setup(x => x.CityAdministration.GetAllAsync(
                     It.IsAny<Expression<Func<CityAdministration, CityAdministration>>>(),
                     It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                     It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()
                     )).ReturnsAsync(() => null);

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNull(result);
        }

        private static List<CityAdministration> GetCity()
        {
            return new List<CityAdministration>
                {
                    new CityAdministration
                    {

                        Status = true
                    }
                };
        }
    }
}
