using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface IAnnualReportsRepository : IRepositoryBase<AnnualReport>
    {
        Task<IEnumerable<AnnualReportTableObject>> GetAnnualReportsAsync(string userId, bool isAdmin, string searchdata, int page, int pageSize);
    }
}
