using System.Collections.Generic;
using EPlast.BLL.DTO.City;

namespace EPlast.BLL.DTO
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
