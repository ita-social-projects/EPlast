using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories
{
    public class AnnualReportsRepository : RepositoryBase<AnnualReport>, IAnnualReportsRepository
    {
        public AnnualReportsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<AnnualReportTableObject>> GetAnnualReportsAsync(string searchdata, int page, int pageSize)
        {
            var items = EPlastDBContext.Set<AnnualReportTableObject>().FromSqlRaw("dbo.getCityAnnualReportsInfo @searchData = {0}, @PageIndex ={1}, @PageSize={2}", searchdata, page, pageSize);
            return items;
        }
    }
}