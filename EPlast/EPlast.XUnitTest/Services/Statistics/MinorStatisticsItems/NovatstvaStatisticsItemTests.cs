using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.MinorStatisticsItems;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EPlast.XUnitTest.Services.Statistics.MinorStatisticsItems
{
    public class NovatstvaStatisticsItemTests
    {
        private readonly NovatstvaStatisticsItem _statisticsItem = new NovatstvaStatisticsItem();

        private readonly IEnumerable<MembersStatistic> _membersStatistics = new List<MembersStatistic>
        {
            new MembersStatistic { NumberOfNovatstva = 10 },
            new MembersStatistic { NumberOfNovatstva = 20 }
        };

        [Fact]
        public void GetValue()
        {
            // Act
            var result = _statisticsItem.GetValue(_membersStatistics.First());

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfNovatstva, result.Indicator);
            Assert.Equal(_membersStatistics.First().NumberOfNovatstva, result.Value);
        }

        [Fact]
        public void GetValueNull()
        {
            // Act
            var result = _statisticsItem.GetValue((MembersStatistic)null);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfNovatstva, result.Indicator);
            Assert.Equal(0, result.Value);
        }

        [Fact]
        public void GetValueList()
        {
            // Act
            var result = _statisticsItem.GetValue(_membersStatistics);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfNovatstva, result.Indicator);
            Assert.Equal(_membersStatistics.First().NumberOfNovatstva + _membersStatistics.Last().NumberOfNovatstva, result.Value);
        }

        [Fact]
        public void GetValueListEmpty()
        {
            // Act
            var result = _statisticsItem.GetValue(Enumerable.Empty<MembersStatistic>());

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfNovatstva, result.Indicator);
            Assert.Equal(0, result.Value);
        }

        [Fact]
        public void GetValueListNull()
        {
            // Act
            var result = _statisticsItem.GetValue((IEnumerable<MembersStatistic>)null);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfNovatstva, result.Indicator);
            Assert.Equal(0, result.Value);
        }
    }
}