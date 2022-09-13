using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO
{
    public class DecisionTargetDto
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Поле TargetName є обов'язковим")]
        public string TargetName { get; set; }
    }
}
