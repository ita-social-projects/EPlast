﻿using System;

namespace EPlast.BLL.DTO.UserProfiles
{
    public class ShortUserInformationDTO
    {
        public string ID { get; set; }
        public int UserProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public GenderDTO Gender { get; set; }
        public string Email { get; set; }
    }
}
