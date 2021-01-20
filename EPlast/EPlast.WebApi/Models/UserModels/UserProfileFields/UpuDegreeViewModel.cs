using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class UpuDegreeViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Поле ступінь в УПЮ є обов'язковим")]
        public string Name { get; set; }
    }
}
