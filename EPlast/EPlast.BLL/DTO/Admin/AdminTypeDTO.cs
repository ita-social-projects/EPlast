using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Club;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.Admin
{
    public class AdminTypeDTO
    {
        public int ID { get; set; }
        public string AdminTypeName { get; set; }
        public ICollection<CityAdministrationDTO> CityAdministration { get; set; }
        public ICollection<ClubAdministrationDTO> ClubAdministration { get; set; }
        public ICollection<RegionAdministrationDTO> RegionAdministration { get; set; }
    }
}
