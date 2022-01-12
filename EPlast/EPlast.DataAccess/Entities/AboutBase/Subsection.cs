using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.AboutBase
{
    public class Subsection
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int SectionId { get; set; }
        public Section Section { get; set; }

        public string Description { get; set; }
        public string ImagePath { get; set; }

        public ICollection<SubsectionPictures> SubsectionsPictures { get; set; }
    }
}
