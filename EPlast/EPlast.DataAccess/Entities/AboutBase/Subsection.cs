using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class Subsection
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int SectionId { get; set; }
        public Section Section { get; set; }

        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
