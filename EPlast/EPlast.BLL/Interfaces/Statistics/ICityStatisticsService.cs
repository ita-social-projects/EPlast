using EPlast.BLL.DTO.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Statistics
{
    public interface ICityStatisticsService
    {
        Task<CityStatistics> GetCityStatisticsAsync(int cityId, int year, IEnumerable<StatisticsItemIndicator> indicators);
        Task<CityStatistics> GetCityStatisticsAsync(int cityId, int minYear, int maxYear, IEnumerable<StatisticsItemIndicator> indicators);
        Task<IEnumerable<CityStatistics>> GetCityStatisticsAsync(IEnumerable<int> citiesIds, int year, IEnumerable<StatisticsItemIndicator> indicators);
        Task<IEnumerable<CityStatistics>> GetCityStatisticsAsync(IEnumerable<int> citiesIds, int minYear, int maxYear, IEnumerable<StatisticsItemIndicator> indicators);
        Task<IEnumerable<CityStatistics>> GetAllCitiesStatisticsAsync();
    }
}