using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.DataAccess.Repositories
{
    public class KadrasRepository: RepositoryBase<KadraVykhovnykiv>, IKadrassRepository
    {
        public KadrasRepository(EPlastDBContext dbContext)
       : base(dbContext)
        {
        }
    }
}
