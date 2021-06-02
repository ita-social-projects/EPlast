using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.WebApi.Models.City
{
    public class CitiesViewModel
    {
        public CitiesViewModel(int page, int pageSize, IEnumerable<CityDTO> cities,string cityName, bool isAdmin)
        {
            if (cityName == null)
            {
                Cities = cities.Skip((page - 1) * pageSize).Take(pageSize);
                Total = cities.Count();
                CanCreate = isAdmin;
            }
            else
            {
                Cities = from city in cities where city.Name.ToLower().Contains(cityName.ToLower()) select city;
                Total = Cities.Count();
                CanCreate = isAdmin;
            }
        }

        public IEnumerable<CityDTO> Cities { get; set; }
        public int Total { get; set; }
        public bool CanCreate { get; set; }
    }
}
