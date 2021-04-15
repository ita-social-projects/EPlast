using System;

namespace EPlast.DataAccess.Entities
{
    public class ClubAnnualReportTableObject
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public bool CanManage { get; set; }
    }
}
