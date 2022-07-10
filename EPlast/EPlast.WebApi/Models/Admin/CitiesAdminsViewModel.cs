using System.Collections.Generic;
using EPlast.BLL.DTO.City;

namespace EPlast.WebApi.Models.Admin
{
    public class CitiesAdminsViewModel
    {
        public IEnumerable<CityDto> Cities { get; set; }
        public string CityName { get; set; }
    }
}
