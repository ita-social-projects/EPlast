using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.DTO
{
    public class GenderDTO
    {
        public int ID { get; set; }
        [Display(Name = "Стать")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Стать повинна складати від 2 до 10 символів")]
        public string Name { get; set; }
        public ICollection<UserProfileDTO> UserProfiles { get; set; }
    }
}
