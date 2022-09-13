using System.Collections.Generic;

namespace EPlast.BLL.DTO.City
{
    public class CityProfileDto
    {
        public CityDto City { get; set; }
        public CityAdministrationDto Head { get; set; }
        public CityAdministrationDto HeadDeputy { get; set; }
        public List<CityAdministrationDto> Admins { get; set; }
        public List<CityMembersDto> Members { get; set; }
        public List<CityMembersDto> Followers { get; set; }
        public List<CityDocumentsDto> Documents { get; set; }
    }
}
