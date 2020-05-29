using EPlast.DataAccess.Entities;
namespace EPlast.DataAccess.Repositories
{
    public class ParticipantStatusRepository : RepositoryBase<ParticipantStatus>, IParticipantStatusRepository
    {
        public ParticipantStatusRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
