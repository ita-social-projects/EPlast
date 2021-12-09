using EPlast.BLL.DTO.Statistics;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces
{
    public abstract class MajorStatisticsItem : IStatisticsItem
    {
        protected IEnumerable<StatisticsItemIndicator> minorStatisticsIndicators = Enumerable.Empty<StatisticsItemIndicator>();

        public abstract StatisticsItem GetValue(MembersStatistic membersStatistic);
        public abstract StatisticsItem GetValue(IEnumerable<MembersStatistic> membersStatistics);

        public void RemoveMinors(Dictionary<StatisticsItemIndicator, IStatisticsItem> minorStatisticsItems)
        {
            foreach (var statisticsItemIndicator in minorStatisticsIndicators.Where(x => minorStatisticsItems.ContainsKey(x)))
            {
                minorStatisticsItems.Remove(statisticsItemIndicator);
            }
        }
    }
}