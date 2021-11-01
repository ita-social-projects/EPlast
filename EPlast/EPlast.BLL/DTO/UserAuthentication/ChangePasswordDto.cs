using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.Account
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Поле поточний пароль є обов'язковим")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,}$",ErrorMessage = "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Поле новий пароль є обов'язковим")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,}$", ErrorMessage = "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Поле введіть новий пароль ще раз є обов'язковим")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Новий пароль не співпадає")]
        [StringLength(100, ErrorMessage = "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,}$", ErrorMessage = "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8")]
        public string ConfirmPassword { get; set; }
    }
}
