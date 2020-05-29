using EPlast.DataAccess.Entities;

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
