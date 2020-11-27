using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class GenderViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Поле стать є обов'язковим")]
        public string Name { get; set; }
    }
}
