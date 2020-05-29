using EPlast.DataAccess.Entities;

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