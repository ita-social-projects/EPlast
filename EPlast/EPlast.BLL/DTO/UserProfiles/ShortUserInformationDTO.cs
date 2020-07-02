using System;

namespace EPlast.BLL.DTO.UserProfiles
{
    public class ShortUserInformationDTO
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
