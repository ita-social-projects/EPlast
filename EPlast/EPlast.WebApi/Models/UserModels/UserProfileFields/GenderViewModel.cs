using EPlast.Resources;
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
            get => gender;
            set
            {
                if (value == UserGenders.Male || value == UserGenders.Female || value == UserGenders.Undefined)
                {
                    gender = value;
                }
            }
        }
    }
}
