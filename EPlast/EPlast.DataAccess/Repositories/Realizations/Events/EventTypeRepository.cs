using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

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