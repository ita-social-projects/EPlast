
using System.Collections.Generic;

namespace EPlast.BLL.DTO.City
{
    public class CityAdministrationViewModelDto
    {
        public IEnumerable<CityAdministrationDto> Administration { get; set; }
        public CityAdministrationDto Head { get; set; }
        public CityAdministrationDto HeadDeputy { get; set; }
    }
}
