using EPlast.BLL.DTO.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Statistics
{
    public interface ICityStatisticsService
    {
        Task<IEnumerable<CityStatistics>> GetCitiesStatisticsAsync(IEnumerable<int> CityIds, IEnumerable<int> years, IEnumerable<StatisticsItemIndicator> indicators);
    }
}