using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

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