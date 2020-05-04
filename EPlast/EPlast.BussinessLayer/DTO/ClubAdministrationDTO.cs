using System;

namespace EPlast.DataAccess.DTO
{
    public class ClubAdministrationDTO
    {
        public int adminId { get; set; }

        public int clubIndex { get; set; }

        public DateTime? enddate { get; set; }
        public DateTime startdate { get; set; }
        public string AdminType { get; set; }
    }
}
