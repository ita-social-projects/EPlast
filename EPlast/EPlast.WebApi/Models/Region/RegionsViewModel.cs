using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.WebApi.Models.Region
{
    public class RegionsViewModel
    {
        public RegionsViewModel(int page, int pageSize, IEnumerable<RegionDTO> regions, string regionName)
        {
            if (regionName == null)
            {
                Regions = regions.Skip((page - 1) * pageSize).Take(pageSize);
                Total = regions.Count();
            }
            else
            {
                Regions = from region in regions where region.RegionName.Contains(regionName) select region;
                Total = regions.Count();
            }
            
        }

        public IEnumerable<RegionDTO> Regions { get; set; }
        public int Total { get; set; }
    }
}
