using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Club;

namespace EPlast.DataAccess.Repositories.Realizations.Club
{
    public class ClubReportAdminsRepository : RepositoryBase<ClubReportAdmins>, IClubReportAdminsRepository
    {
        public ClubReportAdminsRepository(EPlastDBContext dbContext):base(dbContext)
        {
        }
    }
}