using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.Club
{
   public class ClubReportCitiesDTO
    {
        public int ID { get; set; }
        public int ClubAnnualReportId { get; set; }
        public string UserId { get; set; }
        public int CityId { get; set; }
        public ClubReportCityDTO City { get; set; }
    }
}
