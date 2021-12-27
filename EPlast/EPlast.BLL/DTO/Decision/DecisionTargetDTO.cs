using System.ComponentModel.DataAnnotations;
namespace EPlast.BLL.DTO
{
    public class DecisionTargetDTO
    {
        public int ID { get; set; }
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
           ErrorMessage = "Поле має містити текст")]
        [Required(ErrorMessage = "Поле TargetName є обов'язковим")]
        public string TargetName { get; set; }
    }
}