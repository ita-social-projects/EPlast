using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class ClubDocumentsRepository : RepositoryBase<ClubDocuments>, IClubDocumentsRepository
    {
        public ClubDocumentsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
