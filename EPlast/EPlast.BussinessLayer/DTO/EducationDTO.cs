﻿using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO
{
    public class EducationDTO
    {
        public int ID { get; set; }
        public string PlaceOfStudy { get; set; }
        public string Speciality { get; set; }
        public ICollection<UserProfileDTO> UsersProfiles { get; set; }
    }
}
