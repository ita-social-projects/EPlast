using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Region;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Realizations.Region
{
    public class RegionAnnualReportRepository : RepositoryBase<RegionAnnualReport>, IRegionAnnualReportsRepository
    {
        public RegionAnnualReportRepository(EPlastDBContext dbContext)
           : base(dbContext)
        {

        }

        public async Task<IEnumerable<RegionAnnualReportTableObject>> GetRegionAnnualReportsAsync(string searchdata,
            int page, int pageSize, int sortKey)
        {
            var items = EPlastDBContext.Set<RegionAnnualReportTableObject>().FromSqlRaw(
                "dbo.getRegionAnnualReportsInfo @searchData = {0}, @PageIndex ={1}, @PageSize={2}, @sort={3}",
                searchdata, page, pageSize, sortKey);
            return items;
        }
    }
}
