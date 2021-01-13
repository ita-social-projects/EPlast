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
        [Required(ErrorMessage = "Поле дата народження є обов'язковим")]
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
        [Required(ErrorMessage = "Поле домашня адреса є обов'язковим")]
        public string Address { get; set; }
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31})*$",
            ErrorMessage = "Псевдо має містити тільки літери")]
        [MaxLength(30, ErrorMessage = "Псевдо не може перевищувати 30 символів")]
        public string Pseudo { get; set; }
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`0-9.-]{1,51}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`0-9.-]{1,51})*$",
            ErrorMessage = "Поле Громадська, політична діяльність має містити тільки літери та цифри")]
        [MaxLength(500, ErrorMessage = "Поле Громадська, політична діяльність не може перевищувати 500 символів")]
        public string PublicPoliticalActivity { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
    }
}
