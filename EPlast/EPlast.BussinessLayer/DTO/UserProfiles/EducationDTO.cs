using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO.UserProfiles
{
    public class EducationDTO
    {
        public int ID { get; set; }
        public string PlaceOfStudy { get; set; }
        public string Speciality { get; set; }
        public IEnumerable<UserProfileDTO> UsersProfiles { get; set; }
    }
}
