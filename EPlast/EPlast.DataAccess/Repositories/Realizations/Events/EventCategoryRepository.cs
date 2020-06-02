using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.DataAccess.Repositories
{
    public class EventCategoryRepository : RepositoryBase<EventCategory>, IEventCategoryRepository
    {
        public EventCategoryRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}