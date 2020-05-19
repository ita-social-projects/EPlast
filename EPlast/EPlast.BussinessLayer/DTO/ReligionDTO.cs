using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.DTO
{
    public class ReligionDTO
    {
        public int ID { get; set; }
        [Display(Name = "Віровизнання")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "Віровизнання має містити тільки літери")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Віровизнання повинне складати від 3 до 25 символів")]
        public string Name { get; set; }
        public ICollection<UserProfileDTO> UserProfiles { get; set; }
    }
}
