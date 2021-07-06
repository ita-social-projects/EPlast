using EPlast.WebApi.Models.UserModels.UserProfileFields;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels
{
    public class UserShortViewModel
    {
        public string ID { get; set; }
        [Display(Name = "Ім'я")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "Ім'я має містити тільки літери")]
        [Required(ErrorMessage = "Поле ім'я є обов'язковим")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Ім'я повинне складати від 2 до 25 символів")]
        public string FirstName { get; set; }
        [Display(Name = "Прізвище")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "Прізвище має містити тільки літери")]
        [Required(ErrorMessage = "Поле прізвище є обов'язковим")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Прізвище повинне складати від 2 до 25 символів")]
        public string LastName { get; set; }
        [Display(Name = "По-батькові")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "По-батькові має містити тільки літери")]
        public string FatherName { get; set; }
        
        public DateTime RegistredOn { get; set; }
        public DateTime EmailSendedOnRegister { get; set; }
        public DateTime EmailSendedOnForgotPassword { get; set; }
        public string ImagePath { get; set; }
        public bool SocialNetworking { get; set; }
        public string UserProfileID { get; set; }
        [Display(Name = "Ступінь в УПЮ")]
        [Required(ErrorMessage = "Поле ступінь в УПЮ є обов'язковим")]
        public UpuDegreeViewModel UpuDegree { get; set; }       
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31})*$",
            ErrorMessage = "Псевдо має містити тільки літери")]
        [MaxLength(30, ErrorMessage = "Псевдо не може перевищувати 30 символів")]
        public string Pseudo { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public string Club { get; set; }
        public int ClubId { get; set; }
        public int RegionId { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string InstagramLink { get; set; }
    }
}
