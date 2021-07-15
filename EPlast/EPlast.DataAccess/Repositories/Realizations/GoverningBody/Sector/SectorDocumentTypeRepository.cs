using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody.Sector;

namespace EPlast.DataAccess.Repositories.Realizations.GoverningBody.Sector
{
    public class SectorDocumentTypeRepository : RepositoryBase<SectorDocumentType>, ISectorDocumentTypeRepository
    {
        public SectorDocumentTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}