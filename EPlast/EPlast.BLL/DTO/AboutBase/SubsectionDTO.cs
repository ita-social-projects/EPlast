using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.AboutBase
{
    public class SubsectionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int SectionId { get; set; }
        public SectionDTO Section { get; set; }

        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
