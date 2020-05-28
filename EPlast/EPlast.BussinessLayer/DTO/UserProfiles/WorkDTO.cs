using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO.UserProfiles
{
    public class WorkDTO
    {
        public int ID { get; set; }
        public string PlaceOfwork { get; set; }
        public string Position { get; set; }
        public ICollection<UserProfileDTO> UserProfiles { get; set; }
    }
}
