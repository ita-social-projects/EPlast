using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class UserPlastDegreeRepository : RepositoryBase<UserPlastDegree>, IUserPlastDegreeRepository
    {
        public UserPlastDegreeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}