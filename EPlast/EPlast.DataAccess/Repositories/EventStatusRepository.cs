using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class EventStatusRepository : RepositoryBase<EventStatus>, IEventStatusRepository
    {
        public EventStatusRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}