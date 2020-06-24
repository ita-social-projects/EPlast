using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.Account
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Поле електронна пошта є обов'язковим")]
        [EmailAddress(ErrorMessage = "Введене поле не є правильним для електронної пошти")]
        public string Email { get; set; }
    }
}
