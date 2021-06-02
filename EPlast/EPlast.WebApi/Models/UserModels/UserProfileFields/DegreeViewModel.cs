using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.UserModels.UserProfileFields
{
    public class DegreeViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Ступінь")]
        public string Name { get; set; }
    }
}
