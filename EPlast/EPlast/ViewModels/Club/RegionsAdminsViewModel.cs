using EPlast.ViewModels.City;
using System.Collections.Generic;

namespace EPlast.ViewModels
{
    public class RegionsAdminsViewModel
    {
        public IEnumerable<CityViewModel> Cities { get; set; }
        public string CityName { get; set; }
    }
}
