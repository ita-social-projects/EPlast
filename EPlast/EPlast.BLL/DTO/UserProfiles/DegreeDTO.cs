using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.UserProfiles
{
    public class DegreeDto
    {
        public int ID { get; set; }

        [Display(Name = "Ступінь")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31})*$",
            ErrorMessage = "Ступінь має містити тільки літери")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Ступінь повинна складати від 3 до 30 символів")]
        public string Name { get; set; }

        public IEnumerable<UserProfileDto> UserProfiles { get; set; }
    }
}
