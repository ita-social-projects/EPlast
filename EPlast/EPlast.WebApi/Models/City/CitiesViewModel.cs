using EPlast.BLL.DTO.City;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.City
{
    public class CitiesViewModel
    {
        public IEnumerable<CityDTO> Cities { get; set; }
        public int Total { get; set; }
        public PageViewModel Page { get; set; }
        public bool CanCreate { get; set; }
    }
}
