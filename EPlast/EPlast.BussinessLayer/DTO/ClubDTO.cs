using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO
{
    public class ClubDTO
    {
        public int ID { get; set; }
        public string ClubName { get; set; }
        public string ClubURL { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public ICollection<ClubMembersDTO> ClubMembers { get; set; }
        public ICollection<ClubAdministrationDTO> ClubAdministration { get; set; }
    }
}