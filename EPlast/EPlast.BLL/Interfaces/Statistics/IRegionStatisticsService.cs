using EPlast.BLL.DTO.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Statistics
{
    public interface IRegionStatisticsService
    {
        Task<IEnumerable<RegionStatistics>> GetRegionsStatisticsAsync(IEnumerable<int> regionsIds, IEnumerable<int> years, IEnumerable<StatisticsItemIndicator> indicators);
    }
}