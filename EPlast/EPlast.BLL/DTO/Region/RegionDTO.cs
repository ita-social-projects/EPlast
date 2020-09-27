using EPlast.BLL.DTO.City;
using System.Collections.Generic;


namespace EPlast.BLL.DTO.Region
{
    public class RegionDTO
    {
        public int ID { get; set; }
        public string RegionName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public IEnumerable<RegionAdministrationDTO> Administration { get; set; }
        public IEnumerable<CityDTO> Cities { get; set; }
    }
}
