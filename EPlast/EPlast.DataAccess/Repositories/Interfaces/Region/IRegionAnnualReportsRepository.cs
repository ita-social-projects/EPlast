using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Interfaces.Region
{
    public interface IRegionAnnualReportsRepository : IRepositoryBase<RegionAnnualReport>
    {
        Task<IEnumerable<RegionAnnualReportTableObject>> GetRegionAnnualReportsAsync(string searchdata, int page, int pageSize);
    }
}
