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

        public async Task<IEnumerable<RegionAnnualReportTableObject>> GetRegionAnnualReportsAsync(string userId,
            bool isAdmin, string searchdata, int page, int pageSize, int sortKey, bool auth)
        {
            var items = await Task.Run(() => EPlastDBContext.Set<RegionAnnualReportTableObject>().FromSqlRaw(
                "dbo.getRegionAnnualReportsInfo @UserId={0}, @AdminRole={1}, @searchData = {2}, @PageIndex ={3}, @PageSize={4}, @sort={5}, @auth={6}",
                userId, isAdmin ? 1 : 0, searchdata, page, pageSize, sortKey, auth ? 1 : 0));
            return items;
        }

        public async Task<IEnumerable<RegionMembersInfoTableObject>> GetRegionMembersInfoAsync(int regionId, int year, bool? getGeneral,
            int? page, int? pageSize)
        {
            var items = await Task.Run(() => EPlastDBContext.Set<RegionMembersInfoTableObject>().FromSqlRaw(
                "dbo.GetRegionMembersInfo @regionId={0}, @year={1}, @GetGeneral={2}, @PageIndex ={3}, @PageSize={4}",
                regionId, year, getGeneral, page, pageSize));
            return items;
        }
    }
}
