using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class DecesionRepository : RepositoryBase<Decesion>, IDecesionRepository
    {
        public DecesionRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }
    }
}