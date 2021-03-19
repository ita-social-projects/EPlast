using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class OrganizationRepository : RepositoryBase<GoverningBody>, IOrganizationRepository
    {
        public OrganizationRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }
    }
}