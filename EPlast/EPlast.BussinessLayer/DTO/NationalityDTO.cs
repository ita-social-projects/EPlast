using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO
{
    public class NationalityDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<UserProfileDTO> UserProfiles { get; set; }
    }
}
