using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.Admin
{
    public class FilterTableParametersByRole
    {
        public string Regions { get; set; }
        public string Cities { get; set; }
        public string Clubs { get; set; }
        public string AndClubs { get; set; }
    }
}
