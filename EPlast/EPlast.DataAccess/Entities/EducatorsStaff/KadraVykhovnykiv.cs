using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.EducatorsStaff
{
    public class KadraVykhovnykiv
    {
        [Key]
        public int ID { get; set; }

        public string UserId { get; set; }
       public User User { get; set; }
        public int KVTypesID { get; set; }
        public KadraVykhovnykivTypes KadraVykhovnykivTypes { get; set; }
        
        public DateTime DateOfGranting { get; set; }

        public int NumberInRegister { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Опис причини вручення не має перевищувати 100 символів")]
        public string BasisOfGranting { get; set; }

        public string Link { get; set; }

    }
}
