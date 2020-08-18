using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.DataAccess.Repositories
{
    public class KVsRepository: RepositoryBase<KVs>, IKVsRepository
    {
        public KVsRepository(EPlastDBContext dbContext)
       : base(dbContext)
        {
        }
    }
}
