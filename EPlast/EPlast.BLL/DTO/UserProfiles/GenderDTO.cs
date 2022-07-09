using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.UserProfiles
{
    public class GenderDto
    {
        public int ID { get; set; }

        [Display(Name = "Стать")]
        public string Name { get; set; }
    }
}
