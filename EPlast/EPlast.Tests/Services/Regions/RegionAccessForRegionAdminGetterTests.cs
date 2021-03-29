using EPlast.BLL.Services.Region.RegionAccess.RegionAccessGetters;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Regions
{
    class RegionAccessForRegionAdminGetterTests
    {
        private Mock<IRepositoryWrapper> _repositoryWrapper;
        private RegionAccessForRegionAdminGetter _regionAccessGetter;

        [SetUp]
        public void SetUp()
        {
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _repositoryWrapper
               .Setup(x => x.AdminType.GetFirstAsync(It.IsAny<Expression<Func<AdminType, bool>>>(),
               It.IsAny<Func<IQueryable<AdminType>, IIncludableQueryable<AdminType, object>>>()))
               .ReturnsAsync(new AdminType() { AdminTypeName = Roles.OkrugaHead, ID = 2 });
            _regionAccessGetter = new RegionAccessForRegionAdminGetter(_repositoryWrapper.Object);
        }

        [Test]
        public async Task GetRegionAsync_ReturnsIEnumerableRegions()
        {
            // Arrange
             _repositoryWrapper
                .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(new RegionAdministration() { ID=2});
            _repositoryWrapper
                .Setup(x => x.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new List<Region>() { new Region() { ID = 2 },new Region() { ID = 3 } });
            //Act
            var result = await _regionAccessGetter.GetRegionAsync(It.IsAny<string>());
            // Assert
            Assert.IsInstanceOf<IEnumerable<Region>>(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetRegionAsync_ReturnsEmptyIEnumerableRegions()
        {
            // Arrange
            RegionAdministration r = null;
            _repositoryWrapper
               .Setup(x => x.RegionAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
               It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
               .ReturnsAsync(r);
            _repositoryWrapper
                .Setup(x => x.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new List<Region>() { new Region() { ID = 2 }, new Region() { ID = 3 } });
            //Act
            var result = await _regionAccessGetter.GetRegionAsync(It.IsAny<string>());
            // Assert
            Assert.IsInstanceOf<IEnumerable<Region>>(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
