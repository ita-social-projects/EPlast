using System.Collections.Generic;
using System.Linq;
using EPlast.BLL.DTO.Region;

namespace EPlast.WebApi.Models.Region
{
    public class RegionsViewModel
    {
        public RegionsViewModel(int page, int pageSize, IEnumerable<RegionDto> regions, string regionName, bool isAdmin)
        {
            if (regionName == null)
            {
                Regions = regions.Skip((page - 1) * pageSize).Take(pageSize);
                Total = regions.Count();
                CanCreate = isAdmin;
            }
            else
            {
                Regions = from region in regions where region.RegionName.ToLower().Contains(regionName.ToLower()) select region;
                Total = Regions.Count();
                CanCreate = isAdmin;
            }

        }

        public IEnumerable<RegionDto> Regions { get; set; }
        public int Total { get; set; }
        public bool CanCreate { get; set; }
    }
}
