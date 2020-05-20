using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.DTO
{
    public class DegreeDTO
    {
        public int ID { get; set; }

        [Display(Name = "Ступінь")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31})*$",
            ErrorMessage = "Ступінь має містити тільки літери")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Ступінь повинна складати від 3 до 30 символів")]
        public string Name { get; set; }
        public ICollection<UserProfileDTO> UserProfiles { get; set; }
    }
}
