using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.MinorStatisticsItems;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EPlast.XUnitTest.Services.Statistics.MinorStatisticsItems
{
    public class UnatstvaNonameStatisticsItemTests
    {
        private readonly UnatstvaNonameStatisticsItem _statisticsItem = new UnatstvaNonameStatisticsItem();

        private readonly IEnumerable<MembersStatistic> _membersStatistics = new List<MembersStatistic>
        {
            new MembersStatistic { NumberOfUnatstvaNoname = 10 },
            new MembersStatistic { NumberOfUnatstvaNoname = 20 }
        };

        [Fact]
        public void GetValue()
        {
            // Act
            var result = _statisticsItem.GetValue(_membersStatistics.First());

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfUnatstvaNoname, result.Indicator);
            Assert.Equal(_membersStatistics.First().NumberOfUnatstvaNoname, result.Value);
        }

        [Fact]
        public void GetValueNull()
        {
            // Act
            var result = _statisticsItem.GetValue((MembersStatistic)null);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfUnatstvaNoname, result.Indicator);
            Assert.Equal(0, result.Value);
        }

        [Fact]
        public void GetValueList()
        {
            // Act
            var result = _statisticsItem.GetValue(_membersStatistics);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfUnatstvaNoname, result.Indicator);
            Assert.Equal(_membersStatistics.First().NumberOfUnatstvaNoname + _membersStatistics.Last().NumberOfUnatstvaNoname, result.Value);
        }

        [Fact]
        public void GetValueListEmpty()
        {
            // Act
            var result = _statisticsItem.GetValue(Enumerable.Empty<MembersStatistic>());

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfUnatstvaNoname, result.Indicator);
            Assert.Equal(0, result.Value);
        }

        [Fact]
        public void GetValueListNull()
        {
            // Act
            var result = _statisticsItem.GetValue((IEnumerable<MembersStatistic>)null);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfUnatstvaNoname, result.Indicator);
            Assert.Equal(0, result.Value);
        }
    }
}