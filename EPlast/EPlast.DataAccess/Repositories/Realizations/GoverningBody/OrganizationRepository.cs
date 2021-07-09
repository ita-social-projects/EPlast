using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody;

namespace EPlast.DataAccess.Repositories.Realizations.GoverningBody
{
    public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }
    }
}
