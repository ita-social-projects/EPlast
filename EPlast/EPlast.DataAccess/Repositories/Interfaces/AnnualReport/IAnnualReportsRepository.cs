using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface IAnnualReportsRepository : IRepositoryBase<AnnualReport>
    {
        Task<int> GetAnnualReportsCount();

        Task<IEnumerable<AnnualReportTableObject>> GetAnnualReports(string searchdata);
    }
}