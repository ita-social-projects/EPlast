using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories.Interfaces.Blank;

namespace EPlast.DataAccess.Repositories.Realizations.Blank
{
    public class BlankBiographyDocumentsRepository : RepositoryBase<BlankBiographyDocuments>, IBlankBiographyDocumentsRepository
    {
        public BlankBiographyDocumentsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
