using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class PlastDegreeRepository: RepositoryBase<PlastDegree>, IPlastDegreeRepository
    {
        public PlastDegreeRepository(EPlastDBContext dbContext)
          : base(dbContext)
        {
        }
    }
}
