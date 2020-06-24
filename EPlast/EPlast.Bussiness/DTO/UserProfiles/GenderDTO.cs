using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BusinessLogicLayer.DTO.UserProfiles
{
    public class GenderDTO
    {
        public int ID { get; set; }
        [Display(Name = "Стать")]
        public string Name { get; set; }
        public IEnumerable<UserProfileDTO> UserProfiles { get; set; }
    }
}
