using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.Admin
{
    public class TableFilterParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<string> Cities { get; set; }
        public IEnumerable<string> Regions { get; set; }
        public IEnumerable<string> Clubs { get; set; }
        public IEnumerable<string> Degrees { get; set; }
    }

}
