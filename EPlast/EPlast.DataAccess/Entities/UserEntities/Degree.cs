using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Degree
    {
        public int ID { get; set; }

        [Display(Name = "Ступінь")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31})*$",
            ErrorMessage = "Ступінь має містити тільки літери")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Ступінь повинна складати від 3 до 50 символів")]
        public string Name { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}
