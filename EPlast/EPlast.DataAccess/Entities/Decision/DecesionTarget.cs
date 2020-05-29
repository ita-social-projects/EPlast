using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class DecesionTarget
    {
        public int ID { get; set; }

        [DisplayName("Тематика рішення не заповнена.")]
        [Required, MaxLength(50, ErrorMessage = "Тематика рішення має бути не довшою ніж 50 символів")]
        public string TargetName { get; set; }
    }
}