using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class AnnualReportTableObject
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
    }
}
