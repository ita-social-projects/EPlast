using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BusinessLogicLayer.DTO.AnnualReport
{
    public class CityDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int RegionId { get; set; }
        public RegionDTO Region { get; set; }
    }
}
