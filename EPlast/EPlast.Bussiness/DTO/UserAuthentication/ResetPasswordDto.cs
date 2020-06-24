using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPlast.Bussiness.DTO.Account
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Поле електронна пошта є обов'язковим")]
        [EmailAddress(ErrorMessage = "Введене поле не є правильним для електронної пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль має вміщати мінімум 8 символів")]
        [StringLength(100, ErrorMessage = "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле повторення пароля є обов'язковим")]
        [DataType(DataType.Password)] 
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
