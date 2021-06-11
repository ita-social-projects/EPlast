using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody;

namespace EPlast.DataAccess.Repositories.Realizations.GoverningBody
{
    public class GoverningBodyDocumentTypeRepository : RepositoryBase<GoverningBodyDocumentType>, IGoverningBodyDocumentTypeRepository
    {
        public GoverningBodyDocumentTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
