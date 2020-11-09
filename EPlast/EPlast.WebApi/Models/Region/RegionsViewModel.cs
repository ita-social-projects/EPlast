using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.WebApi.Models.Region
{
    public class RegionsViewModel
    {
        public RegionsViewModel(int page, int pageSize, IEnumerable<RegionDTO> regions, string regionName, bool isAdmin)
        {
            if (regionName == null)
            {
                Regions = regions.Skip((page - 1) * pageSize).Take(pageSize);
                Total = regions.Count();
                CanCreate = isAdmin;
            }
            else
            {
                Regions = from region in regions where region.RegionName.Contains(regionName) select region;
                Total = regions.Count();
                CanCreate = isAdmin;
            }
            
        }

        public IEnumerable<RegionDTO> Regions { get; set; }
        public int Total { get; set; }
        public bool CanCreate { get; set; }
    }
}
