using EPlast.BLL.DTO.Statistics;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;

namespace EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces
{
    public interface IStatisticsItem
    {
        StatisticsItem GetValue(MembersStatistic membersStatistic);
        StatisticsItem GetValue(IEnumerable<MembersStatistic> membersStatistics);
    }
}