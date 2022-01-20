using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.Distinction
{
    public class DistictionTableSettings
    {
        public IEnumerable<string> SortByOrder { get; set; }

        public string SearchedData { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
