using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.PrecautionsDTO
{
    public class PrecautionTableSettings
    {
        public IEnumerable<string> SortByOrder { get; set; }
        public IEnumerable<string> StatusSorter { get; set; }
        public IEnumerable<string> PrecautionNameSorter { get; set; }
        public IEnumerable<string> DateSorter { get; set; }

        public string SearchedData { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
