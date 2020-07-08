using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BLL.Services.Statistics.StatisticsItems.MajorStatisticsItems
{
    public class SeigneurStatisticsItem : MajorStatisticsItem
    {
        public SeigneurStatisticsItem()
        {
            minorStatisticsIndicators = new List<StatisticsItemIndicator>
            {
                StatisticsItemIndicator.NumberOfSeigneurMembers,
                StatisticsItemIndicator.NumberOfSeigneurSupporters
            };
        }

        public override StatisticsItem GetValue(MembersStatistic membersStatistic)
        {
            return new StatisticsItem
            {
                Indicator = StatisticsItemIndicator.NumberOfSeigneur,
                Value = (membersStatistic == null) ? 0 : membersStatistic.NumberOfSeigneurMembers + membersStatistic.NumberOfSeigneurSupporters
            };
        }

        public override StatisticsItem GetValue(IEnumerable<MembersStatistic> membersStatistics)
        {
            int value = 0;
            foreach (var membersStatistic in membersStatistics ?? Enumerable.Empty<MembersStatistic>())
            {
                value += membersStatistic.NumberOfSeigneurMembers + membersStatistic.NumberOfSeigneurSupporters;
            }
            return new StatisticsItem
            {
                Indicator = StatisticsItemIndicator.NumberOfSeigneur,
                Value = value
            };
        }
    }
}