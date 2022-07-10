using System.Collections.Generic;

namespace EPlast.BLL.DTO.Region
{
    public class RegionForAdministrationDto
    {
        public int ID { get; set; }
        public string RegionName { get; set; }
        public IEnumerable<int> YearsHasReport { get; set; }
        public bool IsActive { get; set; }
    }
}
