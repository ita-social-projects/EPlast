using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }
    }
}
