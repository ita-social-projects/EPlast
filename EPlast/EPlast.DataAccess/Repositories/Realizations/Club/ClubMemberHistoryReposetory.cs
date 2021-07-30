using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class ClubMemberHistoryReposetory : RepositoryBase<ClubMemberHistory>, IClubMemberHistoryRepository
    {
        public ClubMemberHistoryReposetory(EPlastDBContext dbContext)
          : base(dbContext)
        {
        }
    }
}
