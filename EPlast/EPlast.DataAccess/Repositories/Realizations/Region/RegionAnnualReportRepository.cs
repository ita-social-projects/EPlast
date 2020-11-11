using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Region;

namespace EPlast.DataAccess.Repositories.Realizations.Region
{
    public class RegionAnnualReportRepository : RepositoryBase<RegionAnnualReport>, IRegionAnnualReportsRepository
    {
        public RegionAnnualReportRepository(EPlastDBContext dbContext)
           : base(dbContext)
        {

        }
    }
}
