using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPlast.DataAccess.Entities.EducatorsStaff
{
   public class KVs
    {
        [Key]
        public int ID { get; set; }

        public User User { get; set; }

        public KVTypes KVType { get; set; }

        public DateTime DateOfGranting { get; set; }

        public int NumberInRegister { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Опис причини вручення не має перевищувати 100 символів")]
        public string BasisOfGranting { get; set; }

        public string Link { get; set; }

    }
}
