using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class AdminTypeRepository : RepositoryBase<AdminType>, IAdminTypeRepository
    {
        public AdminTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
