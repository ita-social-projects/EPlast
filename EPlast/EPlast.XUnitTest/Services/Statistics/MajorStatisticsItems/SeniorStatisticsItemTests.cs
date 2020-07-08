using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces;
using EPlast.BLL.Services.Statistics.StatisticsItems.MajorStatisticsItems;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EPlast.XUnitTest.Services.Statistics.MajorStatisticsItems
{
    public class SeniorStatisticsItemTests
    {
        private readonly SeniorStatisticsItem _statisticsItem = new SeniorStatisticsItem();

        private readonly IEnumerable<MembersStatistic> _membersStatistics = new List<MembersStatistic>
        {
            new MembersStatistic
            {
                NumberOfSeniorPlastynMembers = 10,
                NumberOfSeniorPlastynSupporters = 20
            },
            new MembersStatistic
            {
                NumberOfSeniorPlastynMembers = 30,
                NumberOfSeniorPlastynSupporters = 40
            }
        };

        [Fact]
        public void GetValue()
        {
            // Act
            var result = _statisticsItem.GetValue(_membersStatistics.First());

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfSenior, result.Indicator);
            Assert.Equal(_membersStatistics.First().NumberOfSeniorPlastynMembers + _membersStatistics.First().NumberOfSeniorPlastynSupporters, result.Value);
        }

        [Fact]
        public void GetValueNull()
        {
            // Act
            var result = _statisticsItem.GetValue((MembersStatistic)null);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfSenior, result.Indicator);
            Assert.Equal(0, result.Value);
        }

        [Fact]
        public void GetValueList()
        {
            // Act
            var result = _statisticsItem.GetValue(_membersStatistics);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfSenior, result.Indicator);
            Assert.Equal(_membersStatistics.First().NumberOfSeniorPlastynMembers + _membersStatistics.First().NumberOfSeniorPlastynSupporters
                + _membersStatistics.Last().NumberOfSeniorPlastynMembers + _membersStatistics.Last().NumberOfSeniorPlastynSupporters, result.Value);
        }

        [Fact]
        public void GetValueListEmpty()
        {
            // Act
            var result = _statisticsItem.GetValue(Enumerable.Empty<MembersStatistic>());

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfSenior, result.Indicator);
            Assert.Equal(0, result.Value);
        }

        [Fact]
        public void GetValueListNull()
        {
            // Act
            var result = _statisticsItem.GetValue((IEnumerable<MembersStatistic>)null);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfSenior, result.Indicator);
            Assert.Equal(0, result.Value);
        }

        [Fact]
        public void RemoveMinors()
        {
            // Arrange
            IDictionary<StatisticsItemIndicator, IStatisticsItem> minorStatisticsItems = new Dictionary<StatisticsItemIndicator, IStatisticsItem>
            {
                { StatisticsItemIndicator.NumberOfPtashata, null },
                { StatisticsItemIndicator.NumberOfNovatstva, null },
                { StatisticsItemIndicator.NumberOfUnatstva, null },
                { StatisticsItemIndicator.NumberOfUnatstvaMembers, null },
                { StatisticsItemIndicator.NumberOfUnatstvaNoname, null },
                { StatisticsItemIndicator.NumberOfUnatstvaProspectors, null },
                { StatisticsItemIndicator.NumberOfUnatstvaSkobVirlyts, null },
                { StatisticsItemIndicator.NumberOfUnatstvaSupporters, null },
                { StatisticsItemIndicator.NumberOfSenior, null },
                { StatisticsItemIndicator.NumberOfSeniorPlastynMembers, null },
                { StatisticsItemIndicator.NumberOfSeniorPlastynSupporters, null },
                { StatisticsItemIndicator.NumberOfSeigneur, null },
                { StatisticsItemIndicator.NumberOfSeigneurMembers, null },
                { StatisticsItemIndicator.NumberOfSeigneurSupporters, null },
            };

            // Act
            _statisticsItem.RemoveMinors(minorStatisticsItems as Dictionary<StatisticsItemIndicator, IStatisticsItem>);

            // Assert
            Assert.Contains(StatisticsItemIndicator.NumberOfPtashata, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfNovatstva, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfUnatstva, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfUnatstvaMembers, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfUnatstvaNoname, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfUnatstvaProspectors, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfUnatstvaSkobVirlyts, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfUnatstvaSupporters, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfSenior, minorStatisticsItems);
            Assert.DoesNotContain(StatisticsItemIndicator.NumberOfSeniorPlastynMembers, minorStatisticsItems);
            Assert.DoesNotContain(StatisticsItemIndicator.NumberOfSeniorPlastynSupporters, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfSeigneur, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfSeigneurMembers, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfSeigneurSupporters, minorStatisticsItems);
        }
    }
}