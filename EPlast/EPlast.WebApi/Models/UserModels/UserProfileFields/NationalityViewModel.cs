using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class NationalityViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Національність")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{0,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{0,26})*$",
            ErrorMessage = "Національність має містити тільки літери")]
        public string Name { get; set; }
    }
}
