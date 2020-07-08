using System.Collections.Generic;

namespace EPlast.BLL.DTO.Statistics
{
    public class RegionStatistics
    {
        public Region Region { get; set; }
        public IEnumerable<YearStatistics> YearStatistics { get; set; }
    }
}