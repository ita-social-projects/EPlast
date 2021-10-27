using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EPlast.DataAccess.Repositories
{
    public class RegionRepository : RepositoryBase<Region>, IRegionRepository
    {
        public RegionRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
        public async Task<Tuple<IEnumerable<RegionObject>, int>> GetRegionsObjects(int pageNum, int pageSize,  string searchData, bool isArchive)
        {
            var items = EPlastDBContext.Set<RegionObject>().FromSqlRaw("dbo.sp_GetRegions @PageIndex = {0}, @PageSize = {1}, @IsArhivated = {2}, @searchData = {3}", pageNum, pageSize, isArchive, searchData);
            var num = items.Select(u => u.Count).ToList();
            int rowCount = num.Count > 0 ? num[0] : 0;
            return new Tuple<IEnumerable<RegionObject>, int>(items, rowCount);        
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
