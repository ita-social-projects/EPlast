using System.Collections.Generic;

namespace EPlast.ViewModels
{
    public class RegionsAdminsViewModel
    {
        public IEnumerable<CityViewModel2> Cities { get; set; }
        public string CityName { get; set; }
    }
}
