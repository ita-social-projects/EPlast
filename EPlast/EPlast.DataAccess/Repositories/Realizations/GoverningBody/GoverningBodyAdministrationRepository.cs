using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody;

namespace EPlast.DataAccess.Repositories.Realizations.GoverningBody
{
    public class GoverningBodyAdministrationRepository : RepositoryBase<GoverningBodyAdministration>, IGoverningBodyAdministrationRepository
    {
        public GoverningBodyAdministrationRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
