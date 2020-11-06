using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.Statistics
{
    public class StatisticsParameters
    {
        public IEnumerable<int> CitiesId { get; set; }
        public IEnumerable<int> Years { get; set; }
        public IEnumerable<StatisticsItemIndicator> Indicators { get; set; }
    }
}
