using EPlast.WebApi.Models.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.Region
{
    public class RegionProfileViewModel
    {
        public RegionViewModel Region { get; set; }
        public IEnumerable<CityViewModel> Cities { get; set; }
    }
}
