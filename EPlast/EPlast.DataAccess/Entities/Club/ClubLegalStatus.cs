using System;

namespace EPlast.DataAccess.Entities
{
    public class ClubLegalStatus
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public ClubLegalStatusType LegalStatusType { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
    }
}