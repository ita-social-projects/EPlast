using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody.Sector;

namespace EPlast.DataAccess.Repositories.Realizations.GoverningBody.Sector
{
    public class SectorDocumentsRepository : RepositoryBase<SectorDocuments>, ISectorDocumentsRepository
    {
        public SectorDocumentsRepository(EPlastDBContext dbContext)
            :base(dbContext)
        {
        }
    }
}