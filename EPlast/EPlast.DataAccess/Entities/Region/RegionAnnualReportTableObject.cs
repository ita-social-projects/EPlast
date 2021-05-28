using System;

namespace EPlast.DataAccess.Entities
{
    public class RegionAnnualReportTableObject
    {
        public int Id { get; set; }
        public string RegionName { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public int Status { get; set; }
        public int Total { get; set; }
    }
}
