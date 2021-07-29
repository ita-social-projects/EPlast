using System.Collections.Generic;

namespace EPlast.BLL.DTO.City
{
    public class CityProfileDTO
    {
        public CityDTO City { get; set; }
        public CityAdministrationDTO Head { get; set; }
        public CityAdministrationDTO HeadDeputy { get; set; }
        public List<CityAdministrationDTO> Admins { get; set; }
        public List<CityMembersDTO> Members { get; set; }
        public List<CityMembersDTO> Followers { get; set; }
        public IEnumerable<CityDocumentsDTO> Documents { get; set; }
    }
}
