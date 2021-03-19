using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class GoverningBody
    {
        public int ID { get; set; }

        [MaxLength(255, ErrorMessage = "Назва повинна містити не більше 255 символів")]
        public string GoverningBodyName { get; set; }

        [MaxLength(1000, ErrorMessage = "Історія станиці не має перевищувати 1024 символів")]
        public string Description { get; set; }

        [StringLength(18, ErrorMessage = "Контактний номер керівного органу повинен містити 12 цифр")]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Logo { get; set; }
    }
}