using EPlast.DataAccess.Entities.GoverningBody.Sector;

namespace EPlast.DataAccess.Repositories.Realizations.Base
{
    internal class SectorAnnouncementsRepository : RepositoryBase<SectorAnnouncement>, ISectorAnnouncementsRepository
    {
        public SectorAnnouncementsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}