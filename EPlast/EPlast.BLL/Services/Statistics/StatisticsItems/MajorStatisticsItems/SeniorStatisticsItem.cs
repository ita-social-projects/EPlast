using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BLL.Services.Statistics.StatisticsItems.MajorStatisticsItems
{
    public class SeniorStatisticsItem : MajorStatisticsItem
    {
        public SeniorStatisticsItem()
        {
            minorStatisticsIndicators = new List<StatisticsItemIndicator>
            {
                StatisticsItemIndicator.NumberOfSeniorPlastynMembers,
                StatisticsItemIndicator.NumberOfSeniorPlastynSupporters
            };
        }

        public override StatisticsItem GetValue(MembersStatistic membersStatistic)
        {
            return new StatisticsItem
            {
                Indicator = StatisticsItemIndicator.NumberOfSenior,
                Value = (membersStatistic == null) ? 0 : membersStatistic.NumberOfSeniorPlastynMembers + membersStatistic.NumberOfSeniorPlastynSupporters
            };
        }

        public override StatisticsItem GetValue(IEnumerable<MembersStatistic> membersStatistics)
        {
            int value = 0;
            foreach (var membersStatistic in membersStatistics ?? Enumerable.Empty<MembersStatistic>())
            {
                value += membersStatistic.NumberOfSeniorPlastynMembers + membersStatistic.NumberOfSeniorPlastynSupporters;
            }
            return new StatisticsItem
            {
                Indicator = StatisticsItemIndicator.NumberOfSenior,
                Value = value
            };
        }
    }
}