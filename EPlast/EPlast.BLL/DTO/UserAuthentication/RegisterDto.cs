using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.Account
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Поле електронна пошта є обов'язковим")]
        [EmailAddress(ErrorMessage = "Введене поле не є правильним для електронної пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле пароль є обов'язковим")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле повторення пароля є обов'язковим")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Поле ім'я є обов'язковим")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ']{1,20}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ']{1,20})*$",
            ErrorMessage = "Ім'я має містити тільки літери")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле прізвище є обов'язковим")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ']{1,20}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ']{1,20})*$", ErrorMessage = "Прізвище має містити тільки літери")]
        public string SurName { get; set; }
    }
}
