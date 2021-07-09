using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody.Sector;

namespace EPlast.DataAccess.Repositories.Realizations.GoverningBody.Sector
{
    public class SectorAdministrationRepository : RepositoryBase<SectorAdministration>, ISectorAdministrationRepository
    {
        public SectorAdministrationRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}