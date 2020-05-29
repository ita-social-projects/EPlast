using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class UserPlastDegreesRepository : RepositoryBase<UserPlastDegree>, IUserPlastDegreesRepository
    {
        public UserPlastDegreesRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}