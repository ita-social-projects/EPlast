using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.WebApi.Models.City
{
    public class CitiesViewModel
    {
        public CitiesViewModel(int page, int pageSize, IEnumerable<CityDTO> cities, bool isAdmin)
        {
            Cities = cities.Skip((page - 1) * pageSize).Take(pageSize);
            Total = cities.Count();
            CanCreate = page == 1 && isAdmin;
        }

        public IEnumerable<CityDTO> Cities { get; set; }
        public int Total { get; set; }
        public bool CanCreate { get; set; }
    }
}
