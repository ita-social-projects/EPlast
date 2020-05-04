using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class GenderRepository : RepositoryBase<Gender>, IGenderRepository
    {
        public GenderRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
