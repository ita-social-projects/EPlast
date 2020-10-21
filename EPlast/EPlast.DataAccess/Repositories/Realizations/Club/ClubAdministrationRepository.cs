using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class ClubAdministrationRepository : RepositoryBase<ClubAdministration>, IClubAdministrationRepository
    {
        public ClubAdministrationRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
