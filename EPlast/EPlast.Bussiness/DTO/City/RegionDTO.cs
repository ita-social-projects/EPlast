using System.Collections.Generic;
using EPlast.BusinessLogicLayer.DTO.City;

namespace EPlast.BusinessLogicLayer.DTO
{
    public class RegionDTO
    {
        public int ID { get; set; }
        public string RegionName { get; set; }
        public string Description { get; set; }
        public ICollection<RegionAdministrationDTO> RegionAdministration { get; set; }
        public ICollection<CityDTO> Cities { get; set; }
    }
}
