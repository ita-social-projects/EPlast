using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace EPlast.BLL.DTO.AboutBase
{
    public class SectionDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public IEnumerable<SubsectionDto> SubsectionDTOs { get; set; }
    }
}
