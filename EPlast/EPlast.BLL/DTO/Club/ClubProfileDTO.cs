using System.Collections.Generic;

namespace EPlast.BLL.DTO.Club
{
    public class ClubProfileDto
    {
        public ClubDto Club { get; set; }
        public ClubAdministrationDto Head { get; set; }
        public ClubAdministrationDto HeadDeputy { get; set; }
        public List<ClubAdministrationDto> Admins { get; set; }
        public List<ClubMembersDto> Members { get; set; }
        public List<ClubMembersDto> Followers { get; set; }
        public List<ClubDocumentsDto> Documents { get; set; }
    }
}