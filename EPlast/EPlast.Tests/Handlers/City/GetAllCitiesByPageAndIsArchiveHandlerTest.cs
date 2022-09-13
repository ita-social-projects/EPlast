using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class GetAllCitiesByPageAndIsArchiveHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<ICityBlobStorageRepository> _mockBlobStorage;
        private Mock<IMapper> _mockMapper;
        private GetAllCitiesByPageAndIsArchiveQuery _query;
        private GetAllCitiesByPageAndIsArchiveHandler _handler;

        private const int Page = 1;
        private const int PageSize = 10;
        private const string Name = "Name";
        private const bool IsArchive = false;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockBlobStorage = new Mock<ICityBlobStorageRepository>();
            _mockMapper = new Mock<IMapper>();
            _query = new GetAllCitiesByPageAndIsArchiveQuery(Page, PageSize, Name, IsArchive);
            _handler = new GetAllCitiesByPageAndIsArchiveHandler(_mockRepoWrapper.Object,
                _mockBlobStorage.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetAllCitiesByPageAndIsArchiveHandlerTest_ReturnsCityObjects()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.City.GetCitiesObjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<UkraineOblasts>()))
                .ReturnsAsync(GetCityObjects());
            _mockBlobStorage
                .Setup(b => b.GetBlobBase64Async(It.IsAny<string>()));
            _mockMapper
                .Setup(m =>
                    m.Map<IEnumerable<CityObject>, IEnumerable<CityObjectDto>>(It.IsAny<IEnumerable<CityObject>>()));

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<CityObjectDto>, int>>(result);
        }

        [Test]
        public async Task GetAllCitiesByPageAndIsArchiveHandlerTest_ReturnsCityObjectsWthException()
        {
            //Arrange
            _mockRepoWrapper
                .Setup(r => r.City.GetCitiesObjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<UkraineOblasts>()))
                .ReturnsAsync(GetCityObjects());
            _mockBlobStorage
                .Setup(b => b.GetBlobBase64Async(It.IsAny<string>()))
                .ThrowsAsync(new Exception());
            _mockMapper
                .Setup(m =>
                    m.Map<IEnumerable<CityObject>, IEnumerable<CityObjectDto>>(It.IsAny<IEnumerable<CityObject>>()));

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<CityObjectDto>, int>>(result);
        }

        private static Tuple<IEnumerable<CityObject>, int> GetCityObjects()
        {
            return new Tuple<IEnumerable<CityObject>, int>(CreateCityObjects(), 2);
        }

        private static IEnumerable<CityObject> CreateCityObjects()
        {
            return new List<CityObject>
            {
                new CityObject {Logo = "logo.png"},
                new CityObject {Logo = "NewLogo.png"}
            };
        }
    }
}
