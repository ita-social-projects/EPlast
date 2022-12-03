using System.Collections.Generic;

namespace EPlast.BLL.DTO.Admin
{
    public class TableFilterParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRow { get; set; }
        public string Tab { get; set; }
        public IEnumerable<int> Cities { get; set; }
        public IEnumerable<int> Regions { get; set; }
        public IEnumerable<int> Clubs { get; set; }
        public IEnumerable<int> Degrees { get; set; }
        public int SortKey { get; set; }
        public IEnumerable<string> FilterRoles { get; set; }
        public IEnumerable<string> FilterKadras { get; set; }
        public string SearchData { get; set; }
    }
}
