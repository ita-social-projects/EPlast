using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class CityManagementsRepository : RepositoryBase<CityManagement>, ICityManagementsRepository
    {
        public CityManagementsRepository(EPlastDBContext dBContext)
            : base(dBContext)
        {

        }
    }
}