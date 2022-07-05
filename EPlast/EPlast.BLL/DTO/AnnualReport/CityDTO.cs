using System.Collections.Generic;
using EPlast.BLL.DTO.City;

namespace EPlast.BLL.DTO.AnnualReport
{
    public class CityDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public IEnumerable<CityMembersDto> CityMembers { get; set; }
        public RegionDto Region { get; set; }
    }
}
