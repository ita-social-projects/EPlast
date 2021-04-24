using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class ReligionViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Віровизнання")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{1,26})*$",
            ErrorMessage = "Віровизнання має містити тільки літери")]
        public string Name { get; set; }
    }
}
