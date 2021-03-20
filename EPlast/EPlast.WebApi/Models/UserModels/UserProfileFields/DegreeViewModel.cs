using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class DegreeViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Ступінь")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ()'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ()'.`]{1,31})*$",
            ErrorMessage = "Поле 'Ступінь' має містити тільки літери")]
        public string Name { get; set; }
    }
}
