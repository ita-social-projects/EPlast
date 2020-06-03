using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.DataAccess.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
