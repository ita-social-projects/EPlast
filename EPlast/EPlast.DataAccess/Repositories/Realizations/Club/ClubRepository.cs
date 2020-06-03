using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class ClubRepository : RepositoryBase<Club>, IClubRepository
    {
        public ClubRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

    }
}
