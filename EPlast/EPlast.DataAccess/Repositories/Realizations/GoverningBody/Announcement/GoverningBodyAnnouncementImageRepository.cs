using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody.Announcement;

namespace EPlast.DataAccess.Repositories.Realizations.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementImageRepository: RepositoryBase<GoverningBodyAnnouncementImage>, IGoverningBodyAnnouncementImageRepository
    {
        public GoverningBodyAnnouncementImageRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
