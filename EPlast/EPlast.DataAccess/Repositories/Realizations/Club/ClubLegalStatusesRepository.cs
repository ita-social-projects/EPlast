using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Club;

namespace EPlast.DataAccess.Repositories
{
    public class ClubLegalStatusesRepository : RepositoryBase<ClubLegalStatus>, IClubLegalStatusesRepository
    {
        public ClubLegalStatusesRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
