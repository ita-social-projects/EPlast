using System.Collections.Generic;
using EPlast.BLL.DTO.City;

namespace EPlast.BLL.DTO.AnnualReport
{
    public class CityDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int RegionId { get; set; }
        public IEnumerable<CityMembersDTO> CityMembers;
        public RegionDTO Region { get; set; }
    }
}
