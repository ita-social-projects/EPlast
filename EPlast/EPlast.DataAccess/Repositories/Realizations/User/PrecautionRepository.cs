using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.DataAccess.Repositories
{
    class PrecautionRepository: RepositoryBase<Precaution>, IPrecautionRepository
    {
        public PrecautionRepository(EPlastDBContext dBContext)
            : base(dBContext)
        {
        }
    }
}
