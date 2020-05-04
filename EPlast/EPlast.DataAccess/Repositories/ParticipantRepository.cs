using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class ParticipantRepository: RepositoryBase<Participant>, IParticipantRepository
    {
        public ParticipantRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
