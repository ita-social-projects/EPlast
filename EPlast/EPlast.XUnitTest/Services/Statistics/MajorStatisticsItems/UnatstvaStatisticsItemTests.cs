using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces;
using EPlast.BLL.Services.Statistics.StatisticsItems.MajorStatisticsItems;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EPlast.XUnitTest.Services.Statistics.MajorStatisticsItems
{
    public class UnatstvaStatisticsItemTests
    {
        private readonly UnatstvaStatisticsItem _statisticsItem = new UnatstvaStatisticsItem();

        private readonly IEnumerable<MembersStatistic> _membersStatistics = new List<MembersStatistic>
        {
            new MembersStatistic
            {
                NumberOfUnatstvaMembers = 10,
                NumberOfUnatstvaNoname = 20,
                NumberOfUnatstvaProspectors = 30,
                NumberOfUnatstvaSkobVirlyts = 40,
                NumberOfUnatstvaSupporters = 50
            },
            new MembersStatistic
            {
                NumberOfUnatstvaMembers = 60,
                NumberOfUnatstvaNoname = 70,
                NumberOfUnatstvaProspectors = 80,
                NumberOfUnatstvaSkobVirlyts = 90,
                NumberOfUnatstvaSupporters = 100
            }
        };

        [Fact]
        public void GetValue()
        {
            // Act
            var result = _statisticsItem.GetValue(_membersStatistics.First());

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfUnatstva, result.Indicator);
            Assert.Equal(_membersStatistics.First().NumberOfUnatstvaMembers + _membersStatistics.First().NumberOfUnatstvaNoname
                + _membersStatistics.First().NumberOfUnatstvaProspectors + _membersStatistics.First().NumberOfUnatstvaSkobVirlyts
                + _membersStatistics.First().NumberOfUnatstvaSupporters, result.Value);
        }

        [Fact]
        public void GetValueNull()
        {
            // Act
            var result = _statisticsItem.GetValue((MembersStatistic)null);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfUnatstva, result.Indicator);
            Assert.Equal(0, result.Value);
        }

        [Fact]
        public void GetValueList()
        {
            // Act
            var result = _statisticsItem.GetValue(_membersStatistics);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfUnatstva, result.Indicator);
            Assert.Equal(_membersStatistics.First().NumberOfUnatstvaMembers + _membersStatistics.First().NumberOfUnatstvaNoname
                + _membersStatistics.First().NumberOfUnatstvaProspectors + _membersStatistics.First().NumberOfUnatstvaSkobVirlyts
                + _membersStatistics.First().NumberOfUnatstvaSupporters + _membersStatistics.Last().NumberOfUnatstvaMembers
                + _membersStatistics.Last().NumberOfUnatstvaNoname + _membersStatistics.Last().NumberOfUnatstvaProspectors
                + _membersStatistics.Last().NumberOfUnatstvaSkobVirlyts + _membersStatistics.Last().NumberOfUnatstvaSupporters, result.Value);
        }

        [Fact]
        public void GetValueListEmpty()
        {
            // Act
            var result = _statisticsItem.GetValue(Enumerable.Empty<MembersStatistic>());

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfUnatstva, result.Indicator);
            Assert.Equal(0, result.Value);
        }

        [Fact]
        public void GetValueListNull()
        {
            // Act
            var result = _statisticsItem.GetValue((IEnumerable<MembersStatistic>)null);

            // Assert
            Assert.Equal(StatisticsItemIndicator.NumberOfUnatstva, result.Indicator);
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
            Assert.DoesNotContain(StatisticsItemIndicator.NumberOfUnatstvaMembers, minorStatisticsItems);
            Assert.DoesNotContain(StatisticsItemIndicator.NumberOfUnatstvaNoname, minorStatisticsItems);
            Assert.DoesNotContain(StatisticsItemIndicator.NumberOfUnatstvaProspectors, minorStatisticsItems);
            Assert.DoesNotContain(StatisticsItemIndicator.NumberOfUnatstvaSkobVirlyts, minorStatisticsItems);
            Assert.DoesNotContain(StatisticsItemIndicator.NumberOfUnatstvaSupporters, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfSenior, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfSeniorPlastynMembers, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfSeniorPlastynSupporters, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfSeigneur, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfSeigneurMembers, minorStatisticsItems);
            Assert.Contains(StatisticsItemIndicator.NumberOfSeigneurSupporters, minorStatisticsItems);
        }
    }
}