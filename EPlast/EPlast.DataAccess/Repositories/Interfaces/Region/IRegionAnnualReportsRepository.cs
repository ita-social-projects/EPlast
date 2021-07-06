using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Interfaces.Region
{
    public interface IRegionAnnualReportsRepository : IRepositoryBase<RegionAnnualReport>
    {
        Task<IEnumerable<RegionAnnualReportTableObject>> GetRegionAnnualReportsAsync(string userId,
            bool isAdmin, string searchdata, int page, int pageSize, int sortKey, bool auth);

        Task<IEnumerable<RegionMembersInfoTableObject>> GetRegionMembersInfoAsync(int regionId, int year,
            bool? getGeneral,
            int? page, int? pageSize);
    }
}
