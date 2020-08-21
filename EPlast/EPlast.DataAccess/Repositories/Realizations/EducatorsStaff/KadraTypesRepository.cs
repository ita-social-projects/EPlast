using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.DataAccess.Repositories.Realizations.EducatorsStaff
{
    public class KadraTypesRepository: RepositoryBase<KadraVykhovnykivTypes>, IKadraTypesRepository
    {
        public KadraTypesRepository(EPlastDBContext dbContext)
       : base(dbContext)
        {
        }

    }
}
