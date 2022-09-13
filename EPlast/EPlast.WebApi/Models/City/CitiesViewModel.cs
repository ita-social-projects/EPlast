using System.Collections.Generic;
using System.Linq;
using EPlast.BLL.DTO.City;

namespace EPlast.WebApi.Models.City
{
    public class CitiesViewModel
    {
        public CitiesViewModel(int page, int pageSize, IEnumerable<CityDto> cities, bool isAdmin)
        {
            Cities = cities.Skip((page - 1) * pageSize).Take(pageSize);
            Total = cities.Count();
            CanCreate = isAdmin;
        }

        public IEnumerable<CityDto> Cities { get; set; }
        public int Total { get; set; }
        public bool CanCreate { get; set; }
    }
}
