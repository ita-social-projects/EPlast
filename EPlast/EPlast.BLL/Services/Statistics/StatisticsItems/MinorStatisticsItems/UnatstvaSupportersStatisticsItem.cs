using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BLL.Services.Statistics.StatisticsItems.MinorStatisticsItems
{
    public class UnatstvaSupportersStatisticsItem : IStatisticsItem
    {
        public StatisticsItem GetValue(MembersStatistic membersStatistic)
        {
            return new StatisticsItem
            {
                Indicator = StatisticsItemIndicator.NumberOfUnatstvaSupporters,
                Value = membersStatistic == null ? 0 : membersStatistic.NumberOfUnatstvaSupporters
            };
        }

        public StatisticsItem GetValue(IEnumerable<MembersStatistic> membersStatistics)
        {
            int value = 0;
            foreach (var membersStatistic in membersStatistics ?? Enumerable.Empty<MembersStatistic>())
            {
                value += membersStatistic.NumberOfUnatstvaSupporters;
            }
            return new StatisticsItem
            {
                Indicator = StatisticsItemIndicator.NumberOfUnatstvaSupporters,
                Value = value
            };
        }
    }
}