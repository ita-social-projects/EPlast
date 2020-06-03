using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class ClubMembersRepository : RepositoryBase<ClubMembers>, IClubMembersRepository
    {
        public ClubMembersRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
