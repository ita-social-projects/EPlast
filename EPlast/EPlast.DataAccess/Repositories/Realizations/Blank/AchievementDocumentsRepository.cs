using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories.Interfaces.Blank;

namespace EPlast.DataAccess.Repositories.Realizations.Blank
{
    public class AchievementDocumentsRepository:RepositoryBase<AchievementDocuments>,IAchievementDocumentsRepository
    {
        public AchievementDocumentsRepository(EPlastDBContext dbContext)
           : base(dbContext)
        {
        }
    }
}
