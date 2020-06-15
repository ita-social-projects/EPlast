using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.User
{
    public class ContactsDTO
    {
        [Required(ErrorMessage = "Поле ім'я є обов'язковим")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЇїЄєҐґ']{1,20}((\s+|-)[a-zA-Zа-яА-ЯІіЇїЄєҐґ']{1,20})*$",
            ErrorMessage = "Ім'я має містити тільки літери")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле електронна пошта є обов'язковим")]
        [EmailAddress(ErrorMessage = "Введене поле не є правильним для електронної пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле номер телефону є обов'язковим")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Поле текст є обов'язковим")]
        public string FeedBackDescription { get; set; }
    }
}
