using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.AboutBase;

namespace EPlast.DataAccess.Repositories
{
    public class PicturesRepository : RepositoryBase<Pictures>, IPicturesRepository
    {
        public PicturesRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}