using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class UserProfile
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата народження")]
        public DateTime? Birthday { get; set; }
        public int? EducationId { get; set; }
        public Education Education { get; set; }
        public int? DegreeId { get; set; }
        public Degree Degree { get; set; }
        public int? NationalityId { get; set; }
        public Nationality Nationality { get; set; }
        public int? ReligionId { get; set; }
        public Religion Religion { get; set; }
        public int? WorkId { get; set; }
        public Work Work { get; set; }
        public int? GenderID { get; set; }
        public Gender Gender { get; set; }
        [Display(Name = "Домашня адреса")]
        [MaxLength(50,ErrorMessage = "Адреса не може перевищувати 50 символів")]
        public string Address { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
    }
}
