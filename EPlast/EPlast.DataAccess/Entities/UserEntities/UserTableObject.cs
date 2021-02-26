﻿using System;

namespace EPlast.DataAccess.Entities
{
    public class UserTableObject
    {
        public string ID { get; set; }
        public long Index { get; set; }
        //public int UserProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ClubName { get; set; }
        public string PlastDegree { get; set; }
        public string Roles { get; set; }
        public string Email { get; set; }
        public string UPUDegree { get; set; }
    }
}