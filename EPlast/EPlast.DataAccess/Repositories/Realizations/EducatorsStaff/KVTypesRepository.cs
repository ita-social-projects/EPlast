using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.DataAccess.Repositories.Realizations.EducatorsStaff
{
    public class KVTypesRepository: RepositoryBase<KVTypes>, IKVTypesRepository
    {
        public KVTypesRepository(EPlastDBContext dbContext)
       : base(dbContext)
        {
        }

    }
}
