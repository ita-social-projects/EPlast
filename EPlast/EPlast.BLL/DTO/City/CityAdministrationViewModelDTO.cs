
using System.Collections.Generic;

namespace EPlast.BLL.DTO.City
{
    public class CityAdministrationViewModelDTO
    {
        public IEnumerable<CityAdministrationDTO> Administration { get; set; }
        public CityAdministrationDTO Head { get; set; }
        public CityAdministrationDTO HeadDeputy { get; set; }
    }
}
