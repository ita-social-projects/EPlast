using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class NationalityRepository : RepositoryBase<Nationality>, INationalityRepository
    {
        public NationalityRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}