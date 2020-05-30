using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class DecesionTargetRepository : RepositoryBase<DecesionTarget>, IDecesionTargetRepository
    {
        public DecesionTargetRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }
    }
}