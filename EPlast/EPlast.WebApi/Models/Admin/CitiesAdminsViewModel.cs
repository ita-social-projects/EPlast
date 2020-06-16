using EPlast.BussinessLayer.DTO.City;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.Admin
{
    public class CitiesAdminsViewModel
    {
        public IEnumerable<CityDTO> Cities { get; set; }
        public string CityName { get; set; }
    }
}
