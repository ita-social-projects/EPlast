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
    internal class CityAccessForRegionAdminGetterTests
    {
        private IDistinctionAccessGetter _cityAccessForRegionAdminGetter;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        [SetUp]
        public void SetUp()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
           _mockRepositoryWrapper.Setup(r => r.AdminType.GetFirstAsync(It.IsAny<Expression<Func<AdminType, bool>>>(), null))
                .ReturnsAsync(new AdminType());
            _cityAccessForRegionAdminGetter = new CItyAccessForRegionAdminGetter(_mockRepositoryWrapper.Object);
        }
        [Test]
        public async Task GetCitiesIdAndName_Valid()
        {
            //Arrange
            _mockRepositoryWrapper.Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<RegionAdministration>,
                        IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(new RegionAdministration());
            _mockRepositoryWrapper.Setup(r => r.City.GetAllAsync(It.IsAny<Expression<Func<DatabaseEntities.City, bool>>>(),
                    It.IsAny<Func<IQueryable<DatabaseEntities.City>, IIncludableQueryable<DatabaseEntities.City, object>>>()))
                .ReturnsAsync(new List<DatabaseEntities.City>());

            //Act
            var result = await _cityAccessForRegionAdminGetter.GetCitiesIdAndName("userId");

            //Assert
            Assert.IsNotNull(result);
        }
    }
}