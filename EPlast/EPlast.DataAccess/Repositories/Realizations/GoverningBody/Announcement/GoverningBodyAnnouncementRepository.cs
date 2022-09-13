using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody.Announcement;

namespace EPlast.DataAccess.Repositories.Realizations.GoverningBody
{
    public class GoverningBodyAnnouncementRepository : RepositoryBase<GoverningBodyAnnouncement>, IGoverningBodyAnnouncementRepository
    {
        public GoverningBodyAnnouncementRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
