using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class ReligionRepository : RepositoryBase<Religion>, IReligionRepository
    {
        public ReligionRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
