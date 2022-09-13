using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AboutBase;

namespace EPlast.WebApi.Models.AboutBase
{
    public class AboutBaseSubsectionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        

        public int SectionId { get; set; }
        public SectionDto Section { get; set; }

        public string Description { get; set; }

        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
    }
}
