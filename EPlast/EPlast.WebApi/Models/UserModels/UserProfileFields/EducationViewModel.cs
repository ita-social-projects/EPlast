using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class EducationViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Місце навчання")]
        public string PlaceOfStudy { get; set; }

        [Display(Name = "Спеціальність")]
        public string Speciality { get; set; }
    }
}
