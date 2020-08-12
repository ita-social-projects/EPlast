using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EPlast.DataAccess.Entities.UserEntities
{
    public class Distinction
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<UserDistinction> UserDistinctions { get; set; }


    }
}
