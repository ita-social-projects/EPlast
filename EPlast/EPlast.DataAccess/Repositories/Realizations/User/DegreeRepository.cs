using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class DegreeRepository : RepositoryBase<Degree>, IDegreeRepository
    {
        public DegreeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
