using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.AboutBase
{
    public class Pictures
    {
        public int ID { get; set; }
        [Required]
        public string PictureFileName { get; set; }
        public ICollection<SubsectionPictures> Subsections { get; set; }
    }
}
