using System;
using System.Collections.Generic;

namespace EPlast.DataAccess.Entities
{
    public class CityLegalStatus
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public CityLegalStatusType LegalStatusType { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public IEnumerable<CityManagement> CityManagements { get; set; }
    }
}