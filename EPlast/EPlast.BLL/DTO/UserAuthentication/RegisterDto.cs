using System;
using System.ComponentModel.DataAnnotations;
using EPlast.Resources;

namespace EPlast.BLL.DTO.Account
{
    public class RegisterDto
    {
        [Required]
        [RegularExpression(@"[а-яА-ЯІіЄєЇїҐґ' ]+", ErrorMessage = "Only cyrillic symbols and space are allowed")]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"[а-яА-ЯІіЄєЇїҐґ' ]+", ErrorMessage = "Only cyrillic symbols and space are allowed")]
        [MaxLength(20)]
        public string LastName { get; set; }

        [RegularExpression(@"[а-яА-ЯІіЄєЇїҐґ' ]+", ErrorMessage = "Only cyrillic symbols and space are allowed")]
        [MaxLength(20)]
        public string FatherName { get; set; }

        [Range(1, 7, ErrorMessage = "Gender field is invalid")]
        public int GenderId { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Потрібно вказати адресу проживання")]
        public string Address { get; set; }

        [Required, Range(1, int.MaxValue)]
        public UkraineOblasts Oblast { get; set; }

        public int? CityId { get; set; }

        [Url]
        public string FacebookLink { get; set; }

        [Url]
        public string TwitterLink { get; set; }

        [Url]
        public string InstagramLink { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
