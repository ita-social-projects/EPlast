﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Work
    {
        public int ID { get; set; }
        [Display(Name = "Місце праці")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31})*$",
            ErrorMessage = "Місце праці має містити тільки літери")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Місце праці повинне складати від 3 до 50 символів")]
        public string PlaceOfwork { get; set; }
        [Display(Name = "Посада")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31})*$",
            ErrorMessage = "Посада має містити тільки літери")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Посада повинна складати від 3 до 50 символів")]
        public string Position { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}
