using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories
{
    public class AnnualReportsRepository : RepositoryBase<AnnualReport>, IAnnualReportsRepository
    {
        public AnnualReportsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<AnnualReportTableObject>> GetAnnualReportsAsync(string userId, bool isAdmin, string searchdata, int page, int pageSize, int sortKey, bool auth)
        {
            var items = await Task.Run(() => EPlastDBContext.Set<AnnualReportTableObject>().FromSqlRaw(
                "dbo.getCityAnnualReportsInfo @userId={0}, @AdminRole={1}, @searchData = {2}, @PageIndex ={3}, @PageSize={4}, @sort={5}, @auth={6}",
                userId, isAdmin ? 1 : 0, searchdata, page, pageSize, sortKey, auth ? 1 : 0));
            return items;
        }
    }
}
