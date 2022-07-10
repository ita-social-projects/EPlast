using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Handlers.RegionHandlers;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Queries.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.Regions
{
    public class GetAllRegionsByPageAndIsArchiveHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;
        private Mock<IRegionBlobStorageRepository> _mockBlobStorage;
        private GetAllRegionsByPageAndIsArchiveQuery _query;
        private GetAllRegionsByPageAndIsArchiveHandler _handler;

        private static int number => 1;
        private static int page => 3;
        private static string name => "name";
        private static bool isArchived => true;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockBlobStorage = new Mock<IRegionBlobStorageRepository>();
            _query = new GetAllRegionsByPageAndIsArchiveQuery(number, page, name, isArchived);
            _handler = new GetAllRegionsByPageAndIsArchiveHandler(_mockRepoWrapper.Object, _mockMapper.Object, _mockBlobStorage.Object);
        }

        [Test]
        public async Task GetAllRegionByPageAndIsArchiveHandlerTest_ReturnsRegionWithoutException()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(x => x.Region.GetRangeAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                    It.IsAny<Expression<Func<Region, Region>>>(), It.IsAny<Func<IQueryable<Region>, IQueryable<Region>>>(),
                    It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(CreateTuple);
            _mockMapper
                .Setup(m =>
                    m.Map<IEnumerable<Region>, IEnumerable<RegionObject>>(It.IsAny<IEnumerable<Region>>())).Returns(RegionObjectsModel);
            _mockBlobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>()));
            _mockMapper
                .Setup(m =>
                    m.Map<IEnumerable<RegionObject>, IEnumerable<RegionObjectsDto>>(It.IsAny<IEnumerable<RegionObject>>()));

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<RegionObjectsDto>, int>>(result);
        }

        [Test]
        public async Task GetAllRegionByPageAndIsArchiveHandlerTest_ReturnsCityObjectsWithException()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(x => x.Region.GetRangeAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                    It.IsAny<Expression<Func<Region, Region>>>(), It.IsAny<Func<IQueryable<Region>, IQueryable<Region>>>(),
                    It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(CreateTuple);
            _mockMapper
                .Setup(m =>
                    m.Map<IEnumerable<Region>, IEnumerable<RegionObject>>(It.IsAny<IEnumerable<Region>>())).Returns(RegionObjectsModel);
            _mockBlobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>())).ThrowsAsync(new Exception());
            _mockMapper
                .Setup(m =>
                    m.Map<IEnumerable<RegionObject>, IEnumerable<RegionObjectsDto>>(It.IsAny<IEnumerable<RegionObject>>()));

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<RegionObjectsDto>, int>>(result);
        }

        private List<Region> GetRegionsByPage()
        {
            return new List<Region>()
            {
                new Region() {Logo = "logo.png"},
                new Region() {Logo = "logco.png"},
                new Region() {Logo = "locgo.png"}
            };
        }

        private List<RegionObject> GetRegionsObjectByPage()
        {
            return new List<RegionObject>()
            {
                new RegionObject() {Logo = "logo.png"},
                new RegionObject() {Logo = "logco.png"},
                new RegionObject() {Logo = "locgo.png"}
            };
        }

        private Tuple<IEnumerable<Region>, int> CreateTuple => new Tuple<IEnumerable<Region>, int>(GetRegionsByPage(), 100);
        private IEnumerable<RegionObject> RegionObjectsModel => new List<RegionObject>(GetRegionsObjectByPage());
    }
}