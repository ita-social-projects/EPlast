using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO.Club
{
    public class ClubDTO
    {
        public int ID { get; set; }
        public string ClubName { get; set; }
        public string ClubURL { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public IEnumerable<ClubMembersDTO> ClubMembers { get; set; }
        public IEnumerable<ClubAdministrationDTO> ClubAdministration { get; set; }
    }
}