using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.UserEntities
{
    public class Precaution
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int MonthsPeriod { get; set; }
        public ICollection<UserPrecaution> UserPrecautions { get; set; }
    }
}
