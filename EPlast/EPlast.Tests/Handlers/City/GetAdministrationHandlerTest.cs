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
    public class GetAdministrationHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private GetAdministrationHandler _handler;
        private GetAdministrationQuery _query;

        private const int CityId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAdministrationHandler(_mockRepoWrapper.Object, _mockMapper.Object);
            _query = new GetAdministrationQuery(CityId);
        }

        [Test]
        public async Task GetAdministrationHandlerTest_ReturnsCityAdministration()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration>());
            _mockMapper
                .Setup(m => m.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationGetDto>>(It.IsAny<IEnumerable<CityAdministration>>()))
                .Returns(new List<CityAdministrationGetDto>());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityAdministrationGetDto>>(result);
        }
    }
}
