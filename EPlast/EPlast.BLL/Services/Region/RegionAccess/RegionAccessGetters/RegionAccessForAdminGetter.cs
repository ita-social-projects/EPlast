using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Region.RegionAccess.RegionAccessGetters
{
    public class RegionAccessForAdminGetter : IRegionAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public RegionAccessForAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<DatabaseEntities.Region>> GetRegionAsync(string userId)
        {
            var regionRange = await _repositoryWrapper.Region.GetRangeAsync(
                  null, null, с => с.OrderBy(x => x.RegionName), source => source.Include(c => c.Cities), null, null);

            return regionRange.Item1;
        }
    }
}
