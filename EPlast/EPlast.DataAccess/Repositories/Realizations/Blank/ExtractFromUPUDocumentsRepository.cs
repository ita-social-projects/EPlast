using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories.Interfaces.Blank;

namespace EPlast.DataAccess.Repositories.Realizations.Blank
{
    public class ExtractFromUPUDocumentsRepository : RepositoryBase<ExtractFromUPUDocuments>, IExtractFromUPUDocumentsRepository
    {
        public ExtractFromUPUDocumentsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {

        }
    }
}
