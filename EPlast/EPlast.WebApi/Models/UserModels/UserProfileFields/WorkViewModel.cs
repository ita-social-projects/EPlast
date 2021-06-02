using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class WorkViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Місце праці")]
        public string PlaceOfwork { get; set; }
        [Display(Name = "Посада")]
        public string Position { get; set; }
    }
}
