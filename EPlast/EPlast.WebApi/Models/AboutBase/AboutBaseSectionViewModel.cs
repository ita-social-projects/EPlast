using System.Collections.Generic;
using EPlast.BLL.DTO.AboutBase;

namespace EPlast.WebApi.Models.AboutBase
{
    public class AboutBaseSectionViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }

        public IEnumerable<SubsectionDto> SubsectionDTOs { get; set; }
    }
}
