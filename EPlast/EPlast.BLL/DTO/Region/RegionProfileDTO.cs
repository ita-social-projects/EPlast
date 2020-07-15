using EPlast.BLL.DTO.City;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.Region
{
    public class RegionProfileDTO
    {
        public int ID { get; set; }
        public string RegionName { get; set; }
        public string Description { get; set; }
        public IEnumerable<RegionAdministrationDTO> Administration { get; set; }
        public IEnumerable<CityDTO> Cities { get; set; }
    }
}
