using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class ClubReportAdmins
    {
        public int ID { get; set; }
        public int ClubAnnualReportId { get; set; }
        public ClubAnnualReport ClubAnnualReport { get; set; }
        public int ClubAdministrationId { get; set; }
        public ClubAdministration ClubAdministration { get; set; }
    }
}
