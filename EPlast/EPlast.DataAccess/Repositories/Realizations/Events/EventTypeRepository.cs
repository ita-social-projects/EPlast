using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class EventTypeRepository : RepositoryBase<EventType>, IEventTypeRepository
    {
        public EventTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}