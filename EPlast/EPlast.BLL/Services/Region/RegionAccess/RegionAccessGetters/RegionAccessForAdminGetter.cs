using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Region.RegionAccess.RegionAccessGetters
{
    public class RegionAccessForAdminGetter: IRegionAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public RegionAccessForAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<DatabaseEntities.Region>> GetRegion(string userId)
        {
            return await _repositoryWrapper.Region.GetAllAsync(include: source => source.Include(c => c.Cities));
        }
    }
}
