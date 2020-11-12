using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Statistics;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    class StatisticsControllerTests
    {
        private Mock<ICityStatisticsService> cityStatisticsService;
        private Mock<IRegionStatisticsService> regionStatisticsService;
        private Mock<ILoggerService<StatisticsController>> loggerService;
        private StatisticsController statisticsController;

        [SetUp]
        public void SetUp()
        {
            cityStatisticsService = new Mock<ICityStatisticsService>();
            regionStatisticsService = new Mock<IRegionStatisticsService>();
            loggerService = new Mock<ILoggerService<StatisticsController>>();
            statisticsController = new StatisticsController(cityStatisticsService.Object, regionStatisticsService.Object, loggerService.Object);
        }

        [Test]
        public async Task GetCitiesStatistics_IsNotNull_IsInstanceOf()
        {            
            // Act
            var result = await statisticsController.GetCitiesStatistics(citiesStatisticsParameters);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
        }


        private readonly CitiesStatisticsParameters citiesStatisticsParameters = new CitiesStatisticsParameters()
        {
            CityIds = new List<int>() { 5, 19, 20, 28, 29 },
            Years = new List<int>() { 2019, 2020 },
            Indicators = indicators
        };

        private static readonly IEnumerable<StatisticsItemIndicator> indicators = new List<StatisticsItemIndicator>
        {
            StatisticsItemIndicator.NumberOfNovatstva, StatisticsItemIndicator.NumberOfUnatstva, StatisticsItemIndicator.NumberOfUnatstvaMembers,
            StatisticsItemIndicator.NumberOfUnatstvaSkobVirlyts, StatisticsItemIndicator.NumberOfSeigneurMembers, StatisticsItemIndicator.NumberOfSeigneurSupporters,
            StatisticsItemIndicator.NumberOfSenior
        };
    }
}
