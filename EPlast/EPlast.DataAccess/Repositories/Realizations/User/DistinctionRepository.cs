using EPlast.DataAccess.Entities.UserEntities;


namespace EPlast.DataAccess.Repositories
{
    public class DistinctionRepository : RepositoryBase<Distinction>, IDistinctionRepository
    {
        public DistinctionRepository(EPlastDBContext dBContext) 
            : base(dBContext)
        {

        }
    }
}
