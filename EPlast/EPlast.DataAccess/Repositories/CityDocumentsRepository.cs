using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class CityDocumentsRepository : RepositoryBase<CityDocuments>, ICityDocumentsRepository
    {
        public CityDocumentsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
