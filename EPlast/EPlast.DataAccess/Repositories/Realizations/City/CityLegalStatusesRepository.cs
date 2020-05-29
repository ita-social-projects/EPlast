using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class CityLegalStatusesRepository : RepositoryBase<CityLegalStatus>, ICityLegalStatusesRepository
    {
        public CityLegalStatusesRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
