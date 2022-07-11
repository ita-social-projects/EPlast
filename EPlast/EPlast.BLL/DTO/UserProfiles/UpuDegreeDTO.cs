using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.UserProfiles
{
    public class UpuDegreeDto
    {
        public int ID { get; set; }

        [Display(Name = "Ступінь в УПЮ")]
        [Required(ErrorMessage = "Поле cтупінь в УПЮ є обов'язковим")]
        public string Name { get; set; }
    }
}
