using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class DocumentTemplateRepository : RepositoryBase<DocumentTemplate>, IDocumentTemplateRepository
    {
        public DocumentTemplateRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }
    }
}