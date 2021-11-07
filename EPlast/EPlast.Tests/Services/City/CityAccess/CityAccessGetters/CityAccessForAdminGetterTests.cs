using EPlast.BLL.Services.City.CityAccess.CityAccessGetters;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Tests.Services.City.CityAccess.CityAccessGetters
{
    internal class CityAccessForAdminGetterTests
    {
        private IDistinctionAccessGetter _cityAccessForAdminGetter;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        [SetUp]
        public void SetUp()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _cityAccessForAdminGetter = new CityAccessForAdminGetter(_mockRepositoryWrapper.Object);
        }
        [Test]
        public async Task GetCitiesIdAndName_Valid()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                .ReturnsAsync(new List<DatabaseEntities.City>());

            //Act
            var result = await _cityAccessForAdminGetter.GetCitiesIdAndName("userId");

            //Assert
            Assert.IsInstanceOf<IEnumerable<Tuple<int, string>>>(result);
        }
    }
}
