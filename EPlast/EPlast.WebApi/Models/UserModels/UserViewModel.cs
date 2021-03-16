using EPlast.WebApi.Models.UserModels.UserProfileFields;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels
{
    public class UserViewModel
    {
        public string ID { get; set; }
        public string Email { get; set; }
        [Display(Name = "Ім'я")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{1,26})*$",
            ErrorMessage = "Ім'я має містити тільки літери")]
        [Required(ErrorMessage = "Поле ім'я є обов'язковим")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Ім'я повинне складати від 2 до 25 символів")]
        public string FirstName { get; set; }
        [Display(Name = "Прізвище")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{1,26})*$",
            ErrorMessage = "Прізвище має містити тільки літери")]
        [Required(ErrorMessage = "Поле прізвище є обов'язковим")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Прізвище повинне складати від 2 до 25 символів")]
        public string LastName { get; set; }
        [Display(Name = "По-батькові")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ()'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ()'.`]{1,26})*$",
            ErrorMessage = "По-батькові має містити тільки літери")]
        //[StringLength(25, MinimumLength = 2, ErrorMessage = "Поле по-батькові повинне складати від 2 до 25 символів")]
        public string FatherName { get; set; }
        //[StringLength(18, MinimumLength = 10, ErrorMessage = "Номер телефону повинен містити 10 цифр")]
        [Required(ErrorMessage = "Поле номер телефону є обов'язковим")]
        public string PhoneNumber { get; set; }
        public DateTime RegistredOn { get; set; }
        public DateTime EmailSendedOnRegister { get; set; }
        public DateTime EmailSendedOnForgotPassword { get; set; }
        public string ImagePath { get; set; }
        public bool SocialNetworking { get; set; }
        public string UserProfileID { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата народження")]
        [Required(ErrorMessage = "Поле дата народження є обов'язковим")]
        public DateTime Birthday { get; set; }
        public EducationViewModel Education { get; set; }
        public DegreeViewModel Degree { get; set; }
        public NationalityViewModel Nationality { get; set; }
        public ReligionViewModel Religion { get; set; }
        public WorkViewModel Work { get; set; }
        public GenderViewModel Gender { get; set; }
        [Display(Name = "Ступінь в УПЮ")]
        [Required(ErrorMessage = "Поле ступінь в УПЮ є обов'язковим")]
        public UpuDegreeViewModel UpuDegree { get; set; }
        [Display(Name = "Домашня адреса")]
        [MaxLength(50, ErrorMessage = "Адреса не може перевищувати 50 символів")]
        [Required(ErrorMessage = "Поле домашня адреса є обов'язковим")]
        public string Address { get; set; }
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{1,31})*$",
            ErrorMessage = "Псевдо має містити тільки літери")]
        [MaxLength(30, ErrorMessage = "Псевдо не може перевищувати 30 символів")]
        public string Pseudo { get; set; }
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`0-9.()-]{1,51}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`0-9.()-]{1,51})*$",
            ErrorMessage = "Поле Громадська, політична діяльність має містити тільки літери та цифри")]
        [MaxLength(50, ErrorMessage = "Поле Громадська, політична діяльність не може перевищувати 50 символів")]
        public string PublicPoliticalActivity { get; set; }
        public string City { get; set; }
        public string Club { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string InstagramLink { get; set; }
    }
}
