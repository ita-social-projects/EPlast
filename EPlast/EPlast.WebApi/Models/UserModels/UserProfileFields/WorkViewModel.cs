using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class WorkViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Місце праці")]
        public string PlaceOfwork { get; set; }
        [Display(Name = "Посада")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'().`]{1,31})*$",
            ErrorMessage = "Поле 'Посада' має містити тільки літери")]
        public string Position { get; set; }
    }
}
