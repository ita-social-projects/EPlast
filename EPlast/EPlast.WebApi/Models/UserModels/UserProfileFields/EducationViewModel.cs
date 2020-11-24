using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class EducationViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Місце навчання")]
        public string PlaceOfStudy { get; set; }

        [Display(Name = "Спеціальність")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'\(\)" + "\"" + @".`]{1,51}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'\(\)" + "\"" + @".`]{1,51})*$",
            ErrorMessage = "Спеціальність має містити тільки літери")]
        public string Speciality { get; set; }
    }
}
