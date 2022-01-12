using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.AboutBase;

namespace EPlast.DataAccess.Repositories
{
    public class SubsectionPicturesRepository : RepositoryBase<SubsectionPictures>, ISubsectionPicturesRepository
    {
        public SubsectionPicturesRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}