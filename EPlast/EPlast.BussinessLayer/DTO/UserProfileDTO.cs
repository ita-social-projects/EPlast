﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BussinessLayer.DTO
{
    public class UserProfileDTO
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата народження")]
        public DateTime? Birthday { get; set; }
        public int? EducationId { get; set; }
        public EducationDTO Education { get; set; }
        public int? DegreeId { get; set; }
        public DegreeDTO Degree { get; set; }
        public int? NationalityId { get; set; }
        public NationalityDTO Nationality { get; set; }
        public int? ReligionId { get; set; }
        public ReligionDTO Religion { get; set; }
        public int? WorkId { get; set; }
        public WorkDTO Work { get; set; }
        public int? GenderID { get; set; }
        public GenderDTO Gender { get; set; }
        [Display(Name = "Домашня адреса")]
        [MaxLength(50, ErrorMessage = "Адреса не може перевищувати 50 символів")]
        public string Address { get; set; }
        public string UserID { get; set; }
        public UserDTO User { get; set; }
    }
}
