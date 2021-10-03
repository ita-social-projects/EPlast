using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Club;

namespace EPlast.DataAccess.Repositories.Realizations.Club
{
    public class ClubReportPlastDegreesRepository : RepositoryBase<ClubReportPlastDegrees>, IClubReportPlastDegreesRepository
    {
        public ClubReportPlastDegreesRepository(EPlastDBContext dbContext):base(dbContext)
        {
        }
    }
}
