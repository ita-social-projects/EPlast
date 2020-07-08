using System.Collections.Generic;

namespace EPlast.BLL.DTO.Statistics
{
    public class CityStatistics
    {
        public City City { get; set; }
        public IEnumerable<YearStatistics> YearStatistics { get; set; }
    }
}