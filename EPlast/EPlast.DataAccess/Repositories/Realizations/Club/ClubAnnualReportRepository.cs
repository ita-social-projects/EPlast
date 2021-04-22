using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Club;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories.Realizations.Club
{
    public class ClubAnnualReportRepository: RepositoryBase<ClubAnnualReport>, IClubAnnualReportsRepository
    {
        public ClubAnnualReportRepository(EPlastDBContext dbContext)
            : base(dbContext)
        { }

        public async Task<IEnumerable<ClubAnnualReportTableObject>> GetClubAnnualReportsAsync(string userId,
            bool isAdmin, string searchdata, int page, int pageSize, int sortKey, bool auth)
        {
            var items = EPlastDBContext.Set<ClubAnnualReportTableObject>().FromSqlRaw(
                "dbo.getClubAnnualReportsInfo @UserId={0}, @AdminRole={1}, @searchData = {2}, @PageIndex ={3}, @PageSize={4}, @sort={5}, @auth={6}",
                userId, isAdmin ? 1 : 0, searchdata, page, pageSize, sortKey, auth ? 1 : 0);
            return items;
        }
    }
}
