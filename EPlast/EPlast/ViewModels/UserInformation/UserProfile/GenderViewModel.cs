using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels.UserInformation.UserProfile
{
    public class GenderViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Стать")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Стать повинна складати від 2 до 10 символів")]
        public string Name { get; set; }
        public ICollection<UserProfileViewModel> UserProfiles { get; set; }
    }
}
