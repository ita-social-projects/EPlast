using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BLL.Services.Statistics.StatisticsItems.MajorStatisticsItems
{
    public class UnatstvaStatisticsItem : MajorStatisticsItem
    {
        public UnatstvaStatisticsItem()
        {
            minorStatisticsIndicators = new List<StatisticsItemIndicator>
            {
                StatisticsItemIndicator.NumberOfUnatstvaMembers,
                StatisticsItemIndicator.NumberOfUnatstvaNoname,
                StatisticsItemIndicator.NumberOfUnatstvaProspectors,
                StatisticsItemIndicator.NumberOfUnatstvaSkobVirlyts,
                StatisticsItemIndicator.NumberOfUnatstvaSupporters
            };
        }

        public override StatisticsItem GetValue(MembersStatistic membersStatistic)
        {
            return new StatisticsItem
            {
                Indicator = StatisticsItemIndicator.NumberOfUnatstva,
                Value = (membersStatistic == null) ? 0 : membersStatistic.NumberOfUnatstvaMembers + membersStatistic.NumberOfUnatstvaNoname
                    + membersStatistic.NumberOfUnatstvaProspectors + membersStatistic.NumberOfUnatstvaSkobVirlyts + membersStatistic.NumberOfUnatstvaSupporters
            };
        }

        public override StatisticsItem GetValue(IEnumerable<MembersStatistic> membersStatistics)
        {
            int value = 0;
            foreach (var membersStatistic in membersStatistics ?? Enumerable.Empty<MembersStatistic>())
            {
                value += membersStatistic.NumberOfUnatstvaMembers + membersStatistic.NumberOfUnatstvaNoname + membersStatistic.NumberOfUnatstvaProspectors
                    + membersStatistic.NumberOfUnatstvaSkobVirlyts + membersStatistic.NumberOfUnatstvaSupporters;
            }
            return new StatisticsItem
            {
                Indicator = StatisticsItemIndicator.NumberOfUnatstva,
                Value = value
            };
        }
    }
}