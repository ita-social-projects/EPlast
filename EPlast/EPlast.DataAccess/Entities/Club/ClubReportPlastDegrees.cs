using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class ClubReportPlastDegrees
    {
        public int ID { get; set; }
        public int ClubAnnualReportId { get; set; }
        public ClubAnnualReport ClubAnnualReport { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int PlastDegreeId { get; set; }
        public PlastDegree PlastDegree { get; set; }
    }
}
