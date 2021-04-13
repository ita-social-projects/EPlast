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

        public async Task<IEnumerable<ClubAnnualReportTableObject>> GetClubAnnualReportsAsync(string searchdata, int page, int pageSize)
        {
            var items = EPlastDBContext.Set<ClubAnnualReportTableObject>().FromSqlRaw("dbo.getClubAnnualReportsInfo @searchData = {0}, @PageIndex ={1}, @PageSize={2}", searchdata, page, pageSize);
            return items;
        }
    }
}
