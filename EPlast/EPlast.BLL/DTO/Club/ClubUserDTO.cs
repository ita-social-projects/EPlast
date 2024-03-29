﻿using EPlast.DataAccess.Entities;
namespace EPlast.BLL.DTO.Club
{
    public class ClubUserDto
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CityName { get; set; }
        public PlastDegree PlastDegree { get; set; }
    }
}