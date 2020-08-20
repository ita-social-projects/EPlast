using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.EducatorsStaff
{
    public class KadraVykhovnykivTypes
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(20, ErrorMessage = "Назва типу кадри має бути не довшою ніж 20 символів")]
        public string Name { get; set; }
        public ICollection<KadraVykhovnykiv> UsersKadras { get; set; }
    }
}
