using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class MethodicDocumentRepository : RepositoryBase<MethodicDocument>, IMethodicDocumentRepository
    {
        public MethodicDocumentRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }
    }
}
