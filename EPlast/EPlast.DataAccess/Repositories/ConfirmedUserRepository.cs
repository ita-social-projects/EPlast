using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class ConfirmedUserRepository : RepositoryBase<ConfirmedUser>, IConfirmedUserRepository
    {
        public ConfirmedUserRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
