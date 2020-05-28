using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO.City
{
    public class CityProfileDTO
    {
        public CityDTO City { get; set; }
        public CityAdministrationDTO CityHead { get; set; }
        public List<CityAdministrationDTO> CityAdmins { get; set; }
        public List<CityMembersDTO> Members { get; set; }
        public List<CityMembersDTO> Followers { get; set; }
        public List<CityDocumentsDTO> CityDoc { get; set; }
    }
}
