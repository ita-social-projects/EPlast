using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class GenderViewModel
    {
        private string gender;
        public int ID { get; set; }

        [Required(ErrorMessage = "Поле стать є обов'язковим")]
        public string Name 
        {
            get
            {
                return gender;
            }
            set
            {
                if (value == "Чоловік" || value == "Жінка" || value == "Інша") 
                {
                    gender = value;
                }
            }
        }
    }
}
