using EPlast.DataAccess.Entities.GoverningBody.Announcement;

namespace EPlast.DataAccess.Repositories.Realizations.Base
{
    internal class SectorAnnouncementImageRepository : RepositoryBase<SectorAnnouncementImage>, ISectorAnnouncementImageRepository
    {
        public SectorAnnouncementImageRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}