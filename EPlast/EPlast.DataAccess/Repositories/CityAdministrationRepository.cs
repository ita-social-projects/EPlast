using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class CityAdministrationRepository : RepositoryBase<CityAdministration>, ICityAdministrationRepository
    {
        public CityAdministrationRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
