using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Region;

namespace EPlast.DataAccess.Repositories.Realizations.Region
{
    public class RegionFollowerRepository : RepositoryBase<RegionFollowers>, IRegionFollowersRepository
    {
        public RegionFollowerRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
