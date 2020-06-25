using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.UserProfiles
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
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`0-9.-]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`0-9.-]{1,31})*$",
            ErrorMessage = "Домашня адреса має містити тільки літери та цифри")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Домашня адреса повинна складати від 3 до 30 символів")]
        public string Address { get; set; }
        public string UserID { get; set; }
        public UserDTO User { get; set; }
    }
}
