﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels.UserInformation.UserProfile
{
    public class EducationViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Місце навчання")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,51}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,51})*$",
            ErrorMessage = "Місце навчання має містити тільки літери")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Місце навчання повинне складати від 2 до 50 символів")]

        public string PlaceOfStudy { get; set; }
        [Display(Name = "Спеціальність")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,51}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,51})*$",
            ErrorMessage = "Спеціальність має містити тільки літери")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Спеціальність повинна складати від 3 до 50 символів")]
        public string Speciality { get; set; }
        public IEnumerable<UserProfileViewModel> UsersProfiles { get; set; }
    }
}
