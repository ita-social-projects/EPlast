using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface IRegionRepository : IRepositoryBase<Region>
    {
        Task<Tuple<IEnumerable<RegionObject>, int>> GetRegionsObjects(int pageNum, int pageSize, string searchData, bool isArchive);
        IQueryable<RegionNamesObject> GetActiveRegionsNames();
    }
}
