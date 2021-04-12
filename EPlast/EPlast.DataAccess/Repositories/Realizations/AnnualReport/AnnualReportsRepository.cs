using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories
{
    public class AnnualReportsRepository : RepositoryBase<AnnualReport>, IAnnualReportsRepository
    {
        public AnnualReportsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
        public Task<int> GetAnnualReportsCount()
        {
            return EPlastDBContext.AnnualReports.CountAsync();
        }

        public async Task<IEnumerable<AnnualReportTableObject>> GetAnnualReports(string searchdata)
        {
            var items = EPlastDBContext.Set<AnnualReportTableObject>().FromSqlRaw("dbo.getCityAnnualReportsInfo @searchData = {0}", searchdata).AsEnumerable();
            return items as IEnumerable<AnnualReportTableObject>;
        }
    }
}