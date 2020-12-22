using EPlast.BLL.Services.Region.RegionAccess.RegionAccessGetters;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Regions
{
    class RegionAccessForAdminGetterTests
    {
        private Mock<IRepositoryWrapper> _repositoryWrapper;
        private RegionAccessForAdminGetter _regionAccessGetter;

        [SetUp]
        public void SetUp()
        {
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _regionAccessGetter = new RegionAccessForAdminGetter(_repositoryWrapper.Object);
        }

        [Test]
        public async Task GetRegionAsync_ReturnsIEnumerableRegions()
        {
            // Arrange
            _repositoryWrapper
                .Setup(x => x.Region.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>()))
                .ReturnsAsync(new List<DataAccess.Entities.Region>() { new DataAccess.Entities.Region() { ID=2} });
            //Act
            var result = await _regionAccessGetter.GetRegionAsync(It.IsAny<string>());
            // Assert
            Assert.IsInstanceOf<IEnumerable<DataAccess.Entities.Region>>(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}
