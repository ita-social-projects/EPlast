using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class CityMembersRepository : RepositoryBase<CityMembers>, ICityMembersRepository
    {
        public CityMembersRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
