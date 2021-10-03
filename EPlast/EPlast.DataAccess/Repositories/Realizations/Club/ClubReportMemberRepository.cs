using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Club;

namespace EPlast.DataAccess.Repositories.Realizations.Club
{
    public class ClubReportMemberRepository : RepositoryBase<ClubReportMember>, IClubReportMemberRepository
    {
        public ClubReportMemberRepository(EPlastDBContext dbContext):base(dbContext)
        {
        }
    }
}