using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class EducationRepository : RepositoryBase<Education>, IEducationRepository
    {
        public EducationRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
