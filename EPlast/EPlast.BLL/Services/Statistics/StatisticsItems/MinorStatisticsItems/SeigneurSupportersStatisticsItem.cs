﻿using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BLL.Services.Statistics.StatisticsItems.MinorStatisticsItems
{
    public class SeigneurSupportersStatisticsItem : IStatisticsItem
    {
        public StatisticsItem GetValue(MembersStatistic membersStatistic)
        {
            return new StatisticsItem
            {
                Indicator = StatisticsItemIndicator.NumberOfSeigneurSupporters,
                Value = (membersStatistic == null) ? 0 : membersStatistic.NumberOfSeigneurSupporters
            };
        }

        public StatisticsItem GetValue(IEnumerable<MembersStatistic> membersStatistics)
        {
            int value = 0;
            foreach (var membersStatistic in membersStatistics ?? Enumerable.Empty<MembersStatistic>())
            {
                value += membersStatistic.NumberOfSeigneurSupporters;
            }
            return new StatisticsItem
            {
                Indicator = StatisticsItemIndicator.NumberOfSeigneurSupporters,
                Value = value
            };
        }
    }
}