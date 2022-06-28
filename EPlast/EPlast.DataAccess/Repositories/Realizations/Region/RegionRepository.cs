#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories
{
    public class RegionRepository : RepositoryBase<Region>, IRegionRepository
    {
        public RegionRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
        public async Task<Tuple<IEnumerable<RegionObject>, int>> GetRegionsObjects(int pageNum, int pageSize, string? searchData, bool isArchive)
        {
            searchData = searchData?.ToLower();

            IQueryable<Region> found = EPlastDBContext.Set<Region>()
                .Where(r => r.IsActive ^ isArchive)
                .Where(r =>
                    string.IsNullOrWhiteSpace(searchData)
                    || r.RegionName.ToLower().Contains(searchData)
                );

            var result = await found
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .Select(r => new RegionObject()
                {
                    ID = r.ID,
                    RegionName = r.RegionName,
                    Logo = r.Logo,
                    Count = found.Count()
                })
                .ToListAsync();

            return new Tuple<IEnumerable<RegionObject>, int>(result, result.FirstOrDefault()?.Count ?? 0);
        }

        public IQueryable<RegionNamesObject> GetActiveRegionsNames()
        {
            var regions = EPlastDBContext.Regions
                .Where(x => x.IsActive && x.Status != RegionsStatusType.RegionBoard)
                .Select(s =>new RegionNamesObject(){ ID = s.ID, RegionName = s.RegionName });
            return regions; 
        }
    }
}
