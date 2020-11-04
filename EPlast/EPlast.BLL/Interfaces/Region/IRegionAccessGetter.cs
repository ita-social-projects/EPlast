using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Region.RegionAccess.RegionAccessGetters
{
    public interface IRegionAccessGetter
    {
        Task<IEnumerable<DatabaseEntities.Region>> GetRegion(string userId);
    }
}
