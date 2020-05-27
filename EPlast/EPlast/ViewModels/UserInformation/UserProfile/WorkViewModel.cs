﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels.UserInformation.UserProfile
{
    public class WorkViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Місце праці")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31})*$",
            ErrorMessage = "Місце праці має містити тільки літери")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Місце праці повинне складати від 3 до 30 символів")]
        public string PlaceOfwork { get; set; }
        [Display(Name = "Посада")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31})*$",
            ErrorMessage = "Посада має містити тільки літери")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Посада повинна складати від 3 до 30 символів")]
        public string Position { get; set; }
        public IEnumerable<UserProfileViewModel> UserProfiles { get; set; }
    }
}
