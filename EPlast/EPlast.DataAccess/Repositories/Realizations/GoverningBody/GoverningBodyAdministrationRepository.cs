using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody;

namespace EPlast.DataAccess.Repositories
{
    public class GoverningBodyAdministrationRepository: RepositoryBase<GoverningBodyAdministration>, IGoverningBodyAdministrationRepository
    {
        public GoverningBodyAdministrationRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
