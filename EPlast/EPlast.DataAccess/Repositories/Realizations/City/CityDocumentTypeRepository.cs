using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class CityDocumentTypeRepository : RepositoryBase<CityDocumentType>, ICityDocumentTypeRepository
    {
        public CityDocumentTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
