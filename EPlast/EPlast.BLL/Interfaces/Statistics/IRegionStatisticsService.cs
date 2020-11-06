using EPlast.BLL.DTO.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Statistics
{
    public interface IRegionStatisticsService
    {
        Task<RegionStatistics> GetRegionStatisticsAsync(int regionId, int year, IEnumerable<StatisticsItemIndicator> indicators);
        Task<RegionStatistics> GetRegionStatisticsAsync(int regionId, int minYear, int maxYear, IEnumerable<StatisticsItemIndicator> indicators);
        Task<IEnumerable<RegionStatistics>> GetRegionStatisticsAsync(IEnumerable<int> regionsIds, int year, IEnumerable<StatisticsItemIndicator> indicators);
        Task<IEnumerable<RegionStatistics>> GetRegionStatisticsAsync(IEnumerable<int> regionsIds, int minYear, int maxYear, IEnumerable<StatisticsItemIndicator> indicators);
        Task<IEnumerable<RegionStatistics>> GetAllRegionsStatisticsAsync();
        Task<IEnumerable<RegionStatistics>> GetRegionStatisticsAsync(IEnumerable<int> regionsIds, IEnumerable<int> years, IEnumerable<StatisticsItemIndicator> indicators);
    }
}