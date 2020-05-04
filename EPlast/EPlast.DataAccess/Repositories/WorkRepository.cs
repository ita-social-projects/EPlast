using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class WorkRepository : RepositoryBase<Work>, IWorkRepository
    {
        public WorkRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
