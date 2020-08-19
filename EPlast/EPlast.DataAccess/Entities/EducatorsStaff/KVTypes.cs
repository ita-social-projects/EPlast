using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.EducatorsStaff
{
    public  class KVTypes
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(20, ErrorMessage = "Назва типу кадри має бути не довшою ніж 20 символів")]
        public string KVTypeName { get; set; }
    }
}
