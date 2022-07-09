using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories.Interfaces.Blank;

namespace EPlast.DataAccess.Repositories.Realizations.Blank
{
    public class ExtractFromUpuDocumentsRepository : RepositoryBase<ExtractFromUpuDocuments>, IExtractFromUpuDocumentsRepository
    {
        public ExtractFromUpuDocumentsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {

        }
    }
}
