using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody;

namespace EPlast.DataAccess.Repositories.Realizations.GoverningBody
{
    public class GoverningBodyDocumentsRepository : RepositoryBase<GoverningBodyDocuments>, IGoverningBodyDocumentsRepository
    {
        public GoverningBodyDocumentsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
