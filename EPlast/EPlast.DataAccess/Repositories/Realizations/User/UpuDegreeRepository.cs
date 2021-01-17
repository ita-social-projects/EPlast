using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class UpuDegreeRepository : RepositoryBase<UpuDegree>, IUpuDegreeRepository
    {
        public UpuDegreeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
