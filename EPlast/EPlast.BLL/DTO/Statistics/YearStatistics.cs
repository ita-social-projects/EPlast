using System.Collections.Generic;

namespace EPlast.BLL.DTO.Statistics
{
    public class YearStatistics
    {
        public int Year { get; set; }
        public IEnumerable<StatisticsItem> StatisticsItems { get; set; }
    }
}