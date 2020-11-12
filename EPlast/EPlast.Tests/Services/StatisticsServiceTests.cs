using AutoMapper;
using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DTOs = EPlast.BLL.DTO.Statistics;
using Microsoft.EntityFrameworkCore.Query;
using EPlast.DataAccess.Entities;

namespace EPlast.Tests.Services
{
    [TestFixture]
    class StatisticsServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private Mock<StatisticsServiceSettings> settings;
        private StatisticsService statisticsService;
        
        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            settings = new Mock<StatisticsServiceSettings>();
            statisticsService = new StatisticsService(mockRepoWrapper.Object, settings.Object, mockMapper.Object);
        }

        [Test]
        public async Task GetCitiesStatisticsAsync_IsNotNull_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper.Setup(r => r.City.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                    .ReturnsAsync(new DataAccess.Entities.City());
            mockRepoWrapper.Setup(r => r.MembersStatistics.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<MembersStatistic, bool>>>(),
                It.IsAny<Func<IQueryable<MembersStatistic>, IIncludableQueryable<MembersStatistic, object>>>()))
                    .ReturnsAsync(new MembersStatistic());
            mockMapper.Setup(m => m.Map<DataAccess.Entities.City, DTOs.City>(It.IsAny<DataAccess.Entities.City>()))
                .Returns(cities.First());

            //Act
            var result = await statisticsService.GetCitiesStatisticsAsync(cityIds, years, indicators );

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityStatistics>>(result);
        }

        private readonly IEnumerable<int> cityIds = new List<int>() { 5, 19, 20, 28, 29 };
        private readonly IEnumerable<int> regionIds = new List<int>() { 3, 8, 9, 11 };
        private readonly IEnumerable<int> years = new List<int>() { 2019, 2020 };

        private readonly IEnumerable<DTOs.Region> regions = new List<DTOs.Region>
        {
            new DTOs.Region { ID = 1, RegionName = "Львівський" },
            new DTOs.Region { ID = 2, RegionName = "Тернопільський" }
        };

        private readonly IEnumerable<DTOs.City> cities = new List<DTOs.City>
        {
            new DTOs.City { ID = 1, Name = "Золочів" },
            new DTOs.City { ID = 2, Name = "Перемишляни" }
        };

        private readonly IEnumerable<StatisticsItemIndicator> indicators = new List<StatisticsItemIndicator>
        {
            StatisticsItemIndicator.NumberOfNovatstva, StatisticsItemIndicator.NumberOfUnatstva, StatisticsItemIndicator.NumberOfUnatstvaMembers,
            StatisticsItemIndicator.NumberOfUnatstvaSkobVirlyts, StatisticsItemIndicator.NumberOfSeigneurMembers, StatisticsItemIndicator.NumberOfSeigneurSupporters,
            StatisticsItemIndicator.NumberOfSenior
        };
    }
}
