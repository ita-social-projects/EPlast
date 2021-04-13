using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Interfaces.Club
{
    public interface IClubAnnualReportsRepository : IRepositoryBase<ClubAnnualReport>
    {
        Task<IEnumerable<ClubAnnualReportTableObject>> GetClubAnnualReportsAsync(string searchdata, int page, int pageSize);
    }
}
