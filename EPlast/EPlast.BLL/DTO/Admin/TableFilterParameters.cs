using System.Collections.Generic;

namespace EPlast.BLL.DTO.Admin
{
    public class TableFilterParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRow { get; set; }
        public string Tab { get; set; }
        public IEnumerable<string> Cities { get; set; }
        public IEnumerable<string> Regions { get; set; }
        public IEnumerable<string> Clubs { get; set; }
        public IEnumerable<string> Degrees { get; set; }
    }
}
