using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class GallaryRepository : RepositoryBase<Gallary>, IGallaryRepository
    {
        public GallaryRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
