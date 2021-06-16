using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.Services.City.CityAccess.CityAccessGetters;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Tests.Services.City.CityAccess.CityAccessGetters
{
    internal class CityAccessForCityAdminGetterTests
    {
        private ICItyAccessGetter _cityAccessForCityAdminGetter;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        [SetUp]
        public void SetUp()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockRepositoryWrapper.Setup(r => r.AdminType.GetFirstAsync(It.IsAny<Expression<Func<AdminType, bool>>>(), null))
                .ReturnsAsync(new AdminType());
            _cityAccessForCityAdminGetter = new CityAccessForCityAdminGetter(_mockRepositoryWrapper.Object);
        }

        [Test]
        public async Task GetCitiesIdAndName_Valid()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.CityAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>,
                        IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration());
            _mockRepositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                .ReturnsAsync(new List<DatabaseEntities.City>());

            //Act
            var result = await _cityAccessForCityAdminGetter.GetCitiesIdAndName("userId");

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<Tuple<int, string>>>(result);
        }
    }
}
