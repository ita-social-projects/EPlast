using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Gender
    {
        public int ID { get; set; }
        [Display(Name = "Стать")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Стать повинна складати від 2 до 25 символів")]
        public string Name { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}
