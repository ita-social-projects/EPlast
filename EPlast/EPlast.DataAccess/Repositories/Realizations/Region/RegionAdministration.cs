using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class RegionAdministrationRepository : RepositoryBase<RegionAdministration>, IRegionAdministrationRepository
    {
        public RegionAdministrationRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
