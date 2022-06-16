using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.Account
{
    public class FeedbackDto
    {
        [Required]
        [RegularExpression(@"[a-zA-Zа-яА-ЯІіЄєЇїҐґ' ]+", ErrorMessage = "Only space, latin and cyrillic symbols are allowed")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(50), MaxLength(2500)]
        public string FeedbackBody { get; set; }
    }
}
