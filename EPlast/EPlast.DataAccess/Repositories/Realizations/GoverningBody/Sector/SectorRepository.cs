using EPlast.DataAccess.Repositories.Interfaces.GoverningBody.Sector;

namespace EPlast.DataAccess.Repositories.Realizations.GoverningBody.Sector
{
    public class SectorRepository : RepositoryBase<Entities.GoverningBody.Sector.Sector>, ISectorRepository
    {
        public SectorRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}