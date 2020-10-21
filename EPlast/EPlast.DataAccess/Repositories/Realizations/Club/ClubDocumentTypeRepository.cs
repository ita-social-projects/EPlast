using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class ClubDocumentTypeRepository : RepositoryBase<ClubDocumentType>, IClubDocumentTypeRepository
    {
        public ClubDocumentTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
