using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.UserProfiles
{
    public class NationalityDTO
    {
        public int ID { get; set; }

        [Display(Name = "Національність")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "Національність має містити тільки літери")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Національність повинна складати від 3 до 25 символів")]
        public string Name { get; set; }

        public IEnumerable<UserProfileDTO> UserProfiles { get; set; }
    }
}
