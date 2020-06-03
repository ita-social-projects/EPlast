using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

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
