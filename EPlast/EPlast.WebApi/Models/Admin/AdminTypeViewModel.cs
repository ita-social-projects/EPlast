using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.Admin
{
    public class AdminTypeViewModel
    {
        public int ID { get; set; }
        [Required]
        public string AdminTypeName { get; set; }
    }
}